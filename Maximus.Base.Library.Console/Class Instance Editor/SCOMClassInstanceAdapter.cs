using Microsoft.EnterpriseManagement;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections;
using System.ComponentModel;
using Microsoft.EnterpriseManagement.ConnectorFramework;
using Microsoft.EnterpriseManagement.Configuration;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Monitoring;
using System.Runtime.Serialization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maximus.Library.SCOM.Editors
{
  public delegate void ObjectChangedEventHandler(object sender, ObjectChangedEventArgs e);

  public abstract class SCOMClassInstanceAdapter : INotifyPropertyChanged, IEditableObject
  {
    protected string _DisplayName;
    protected MonitoringObject _ActionPoint, BackupActionPoint;
    protected InstanceCommitStatus _CommitStatus, BackupCommitStatus;
    protected bool isEditing = false;
    private Dictionary<Guid, PropertyInfo> myProperties;
    private Dictionary<Guid, object> BackupValues;
    public EnterpriseManagementObject SourceInstance { get; set; }
    public bool ActionPointProtected { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public event ObjectChangedEventHandler ObjectChanged;

    protected void PropertyChangedInvoke([CallerMemberName]string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      if (propertyName != nameof(CommitStatus)) // to avoid infinite loop and commit status change should not change commit status
        if (CommitStatus == InstanceCommitStatus.Committed)
          CommitStatus = InstanceCommitStatus.Modified;
    }

    [SCOMClassProperty("afb4f9e6-bf48-1737-76ad-c9b3ec325b97")]
    public string DisplayName
    {
      get => _DisplayName;
      set { _DisplayName = value; PropertyChangedInvoke(); }
    }

    public MonitoringObject ActionPoint
    {
      get => _ActionPoint;
      set
      {
        if (ActionPointProtected)
          throw new SCOMClassInstanceEditorException("Cannot change Action Point. Delete and re-create instance.");
        _ActionPoint = value;
        PropertyChangedInvoke();
      }
    }

    public BindableMonitoringObject BindableActionPoint
    {
      get => ActionPoint is null ? null : new BindableMonitoringObject(ActionPoint);
      set { ActionPoint = value.MonitoringObject; } // no notification is over here, as it's just a proxy to ActionPoint
    }

    public InstanceCommitStatus CommitStatus
    {
      get => _CommitStatus;
      set
      {
        _CommitStatus = value;
        PropertyChangedInvoke();
      }
    }

    public void ParseClassInstance(EnterpriseManagementObject instance, MonitoringObject actionPoint, bool actionPointProtected)
    {
      foreach (KeyValuePair<Guid, PropertyInfo> myProperty in myProperties)
        myProperty.Value.SetValue(this, instance[myProperty.Key].Value);
      SourceInstance = instance;
      ActionPoint = actionPoint;
      ActionPointProtected = actionPointProtected;
      CommitStatus = InstanceCommitStatus.Committed;
    }

    public object this[Guid propertyIndex]
    {
      get => GetClassInstanceProperty(propertyIndex);
      set { myProperties[propertyIndex].SetValue(this, value); }
    }

    public object GetClassInstanceProperty(Guid propertyId)
    {
      return myProperties[propertyId].GetValue(this);
    }

    public void BeginEdit()
    {
      if (!isEditing)
      {
        foreach (KeyValuePair<Guid, PropertyInfo> myProperty in myProperties)
          BackupValues[myProperty.Key] = GetClassInstanceProperty(myProperty.Key);
        BackupActionPoint = ActionPoint;
        BackupCommitStatus = CommitStatus;
        isEditing = true;
      }
    }

    public void EndEdit()
    {
      if (isEditing)
      {
        isEditing = false;
        if (CommitStatus == InstanceCommitStatus.Modified || CommitStatus == InstanceCommitStatus.New)
        {
          ObjectChangedEventArgs objectChangedEventArgs = new ObjectChangedEventArgs(CommitStatus);
          ObjectChanged?.Invoke(this, objectChangedEventArgs);
          CommitStatus = objectChangedEventArgs.CommitStatus;
        }
      }
    }

    public void CancelEdit()
    {
      if (isEditing)
      {
        if (CommitStatus != InstanceCommitStatus.Committed)
        {
          foreach (KeyValuePair<Guid, PropertyInfo> myProperty in myProperties)
            this[myProperty.Key] = BackupValues[myProperty.Key];
          ActionPoint = BackupActionPoint;
          CommitStatus = BackupCommitStatus;
        }
        isEditing = false;
      }
    }

    protected SCOMClassInstanceAdapter()
    {
      CommitStatus = InstanceCommitStatus.New;
      myProperties = new Dictionary<Guid, PropertyInfo>();
      BackupValues = new Dictionary<Guid, object>();
      foreach (PropertyInfo myProperty in GetType().GetProperties())
      {
        CustomAttributeData scomAttribure = myProperty.CustomAttributes.Where(x => x.AttributeType == typeof(SCOMClassPropertyAttribute)).FirstOrDefault();
        if (scomAttribure != null)
        {
          myProperties.Add(Guid.Parse((string)scomAttribure.ConstructorArguments[0].Value), myProperty);
          BackupValues.Add(Guid.Parse((string)scomAttribure.ConstructorArguments[0].Value), null);
        }
      }
    }
  }
}
