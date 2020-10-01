using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Monitoring;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Maximus.Library.SCOM.Editors
{
  public delegate void ObjectChangedEventHandler(object sender, ObjectChangedEventArgs e);

  /// <summary>
  /// Inherit from this class to create editable structure representing properties of a particular SCOM class.
  /// Use <seealso cref="SCOMClassPropertyAttribute"/> to mark properties from SCOM Class definition.
  /// </summary>
  public abstract class SCOMClassInstanceAdapter : INotifyPropertyChanged, IEditableObject
  {
    private string _DisplayName;
    protected MonitoringObject _ActionPoint, BackupActionPoint;
    protected InstanceCommitStatus _CommitStatus, BackupCommitStatus;
    protected bool isEditing = false;
    private readonly Dictionary<Guid, PropertyInfo> myProperties;
    private Dictionary<Guid, object> BackupValues;
    /// <summary>
    /// Existing SCOM Class Instance, if the current object was loaded from it.
    /// </summary>
    public EnterpriseManagementObject SourceInstance { get; set; }

    /// <summary>
    /// Allow or Deny Action Point to be edited.
    /// </summary>
    public bool ActionPointProtected { get; private set; }

    public event PropertyChangedEventHandler PropertyChanged;
    public event ObjectChangedEventHandler ObjectChanged;

    /// <summary>
    /// Call this method to notify <seealso cref="INotifyPropertyChanged"/> consumers about property change.
    /// </summary>
    /// <param name="propertyName"></param>
    protected void PropertyChangedInvoke([CallerMemberName]string propertyName = "")
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      if (propertyName != nameof(CommitStatus)) // to avoid infinite loop and commit status change should not change commit status
        if (CommitStatus == InstanceCommitStatus.Committed)
          CommitStatus = InstanceCommitStatus.Modified;
    }

    /// <summary>
    /// Built-in SCOM Class Property (exists in all classes).
    /// </summary>
    [SCOMClassProperty("afb4f9e6-bf48-1737-76ad-c9b3ec325b97")]
    public string DisplayName
    {
      get => _DisplayName;
      set { _DisplayName = value; PropertyChangedInvoke(); }
    }

    /// <summary>
    /// Existing Class Instance action point.
    /// </summary>
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

    /// <summary>
    /// Existing Class Instance action point wrapped in <seealso cref="BindableMonitoringObject"/> class.
    /// </summary>
    public BindableMonitoringObject BindableActionPoint
    {
      get => ActionPoint is null ? null : new BindableMonitoringObject(ActionPoint);
      set { ActionPoint = value.MonitoringObject; } // no notification is over here, as it's just a proxy to ActionPoint
    }

    /// <summary>
    /// Current object editing state.
    /// </summary>
    public InstanceCommitStatus CommitStatus
    {
      get => _CommitStatus;
      set
      {
        _CommitStatus = value;
        PropertyChangedInvoke();
      }
    }

    /// <summary>
    /// Load property values from existing SCOM Class Instance.
    /// </summary>
    /// <param name="instance">SCOM Class Instance</param>
    /// <param name="actionPoint">SCOM Class Instance representing an Action Point (for example Health Service)</param>
    /// <param name="actionPointProtected">Enable or Disable action point editing</param>
    public void ParseClassInstance(EnterpriseManagementObject instance, MonitoringObject actionPoint, bool actionPointProtected)
    {
      foreach (KeyValuePair<Guid, PropertyInfo> myProperty in myProperties)
        myProperty.Value.SetValue(this, instance[myProperty.Key].Value);
      SourceInstance = instance;
      ActionPoint = actionPoint;
      ActionPointProtected = actionPointProtected;
      CommitStatus = InstanceCommitStatus.Committed;
    }

    /// <summary>
    /// Access SCOM Class properties via indexer.
    /// </summary>
    /// <param name="propertyIndex"></param>
    /// <returns></returns>
    public object this[Guid propertyIndex]
    {
      get => GetClassInstanceProperty(propertyIndex);
      set { myProperties[propertyIndex].SetValue(this, value); }
    }

    /// <summary>
    /// Returns property value.
    /// </summary>
    /// <param name="propertyId">SCOM Class Property Id as defined after management pack import.</param>
    /// <returns>Property value</returns>
    public object GetClassInstanceProperty(Guid propertyId)
    {
      return myProperties[propertyId].GetValue(this);
    }

    /// <summary>
    /// Implements IEditableObject.BeginEdit(). Start editing object properties.
    /// </summary>
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

    /// <summary>
    /// Implements IEditableObject.EndEdit(). Commits all changes made since IEditableObject.BeginEdit() or object initialization.
    /// </summary>
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

    /// <summary>
    /// Implements IEditableObject.CancelEdit(). Discard all changes made since IEditableObject.BeginEdit() or object initialization.
    /// </summary>
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

    /// <summary>
    /// Default constructor. Create a list of own properties corresponding SCOM Class definition.
    /// </summary>
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
