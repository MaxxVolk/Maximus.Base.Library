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
using Maximus.Library.SCOMId;

namespace Maximus.Library.SCOM.Editors
{
  public enum SCOMDiscoveryType { Insert, Update, Delete }

  public class SCOMClassInstanceEditor<T> : SCOMInstanceBindingList<T> where T : SCOMClassInstanceAdapter, new()
  {
    // internal variables
    protected ManagementPackClass mySeedClass, myActionPointClass = null;
    protected ManagementPackRelationship relMAPShouldManageEntity = null, relMAPManagesEntity = null, relHosting = null, relHSShouldManageEntiry = null;
    protected ManagementPackRelationship relToClearMAP = null, relToClearHS = null;
    protected MonitoringConnector myConnector;
    protected List<MonitoringObject> myActionPoints;
    private MonitoringObject _DefaultActionPoint;
    protected ManagementGroup myMG;
    protected bool ignorePropertyChangeNotification = false;
    protected bool ignoreListChangeNotification = false;

    // properties
    public ManagementGroup ManagementGroup { get { return myMG; } }

    public IList<MonitoringObject> ActionPoints { get { return myActionPoints; } }

    public MonitoringObject DefaultActionPoint
    {
      get
      {
        if (ActionPoints == null)
          return null;
        if (_DefaultActionPoint == null)
          return null;
        if (ActionPoints.Contains(_DefaultActionPoint))
          return _DefaultActionPoint;
        else
          return null;
      }
      set { _DefaultActionPoint = value; }
    }

    // constructors
    /// <summary>
    /// This constructor is for unhosted objects managed by All Management Server resource pool.
    /// </summary>
    /// <param name="managementGroup"></param>
    /// <param name="seedClass"></param>
    /// <param name="insertConnector"></param>
    public SCOMClassInstanceEditor(ManagementGroup managementGroup, ManagementPackClass seedClass, MonitoringConnector insertConnector)
    {
      DefaultConstructor(managementGroup, seedClass, insertConnector);
    }

    /// <summary>
    /// This constructor is for unhosted objects managed by specific instance of defined Action Point class OR for hosted objects, which are hosted on defined Action Point class.
    /// </summary>
    /// <param name="managementGroup"></param>
    /// <param name="seedClass"></param>
    /// <param name="actionPointClassName"></param>
    /// <param name="insertConnector"></param>
    public SCOMClassInstanceEditor(ManagementGroup managementGroup, ManagementPackClass seedClass, ManagementPackClass actionPointClass, MonitoringConnector insertConnector, ManagementPackRelationship hostingRelationship = null)
    {
      myActionPointClass = actionPointClass;
      if (seedClass.Hosted)
      {
        relMAPShouldManageEntity = null;
        relMAPManagesEntity = null;
        relHosting = hostingRelationship ?? managementGroup.EntityTypes.GetRelationshipClass(SystemId.HostingRelationshipId);
      }
      else
      {
        relHosting = null;
        if (actionPointClass.IsSubtypeOf(managementGroup.EntityTypes.GetClass(SystemCenterId.ManagementActionPointClassId)) || actionPointClass.Id == SystemCenterId.ManagementActionPointClassId)
          relMAPShouldManageEntity = managementGroup.EntityTypes.GetRelationshipClass(SystemCenterId.ManagementActionPointShouldManageEntityRelationshipId);
        else if (actionPointClass.IsSubtypeOf(managementGroup.EntityTypes.GetClass(SystemCenterId.HealthServiceClassId)) || actionPointClass.Id == SystemCenterId.HealthServiceClassId)
          relHSShouldManageEntiry = managementGroup.EntityTypes.GetRelationshipClass(SystemCenterId.HealthServiceShouldManageEntityRelationshipId);
        else
          throw new NotSupportedException("For unhosted scenario, action point class should be either Microsoft.SystemCenter.ManagementActionPoint or Microsoft.SystemCenter.HealthService, or inherited from these classes.");
        relMAPManagesEntity = managementGroup.EntityTypes.GetRelationshipClass(SystemCenterId.ManagementActionPointManagesEntityRelationshipId);
      }
      myActionPoints = new List<MonitoringObject>();
      myActionPoints.AddRange(managementGroup.EntityObjects.GetObjectReader<MonitoringObject>(myActionPointClass, ObjectQueryOptions.Default));
      DefaultConstructor(managementGroup, seedClass, insertConnector);
    }

    private void DefaultConstructor(ManagementGroup managementGroup, ManagementPackClass seedClass, MonitoringConnector insertConnector)
    {
      mySeedClass = seedClass;
      myConnector = insertConnector;
      myMG = managementGroup;
      relToClearHS = managementGroup.EntityTypes.GetRelationshipClass(SystemCenterId.HealthServiceShouldManageEntityRelationshipId);
      relToClearMAP = managementGroup.EntityTypes.GetRelationshipClass(SystemCenterId.ManagementActionPointShouldManageEntityRelationshipId);
      // load initial data
      Refresh();
    }

    // methods & overrides

    public void RefreshActionPoints()
    {
      if (myActionPointClass == null)
        throw new InvalidOperationException("This editor instance wasn't initialized to use Action Points.");
      if (myActionPoints == null)
        myActionPoints = new List<MonitoringObject>();
      else
        myActionPoints.Clear();
      myActionPoints.AddRange(myMG.EntityObjects.GetObjectReader<MonitoringObject>(myActionPointClass, ObjectQueryOptions.Default));
    }

    public void Refresh(string criteria = null)
    {
      // stop monitoring collection change
      ignorePropertyChangeNotification = true;
      ignoreListChangeNotification = true;
      try
      {
        // load existing class instances
        ObjectQueryOptions queryOptions = new ObjectQueryOptions(ObjectPropertyRetrievalBehavior.All) { ObjectRetrievalMode = ObjectRetrievalOptions.Buffered };
        IObjectReader<MonitoringObject> allSCOMClassInstances = null;
        if (string.IsNullOrWhiteSpace(criteria))
          allSCOMClassInstances = myMG.EntityObjects.GetObjectReader<MonitoringObject>(mySeedClass, queryOptions);
        else
        {
          EnterpriseManagementObjectCriteria searchExpression = new EnterpriseManagementObjectCriteria(criteria, mySeedClass);
          allSCOMClassInstances = myMG.EntityObjects.GetObjectReader<MonitoringObject>(searchExpression, queryOptions);
        }
        Items.Clear();
        OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
        foreach (MonitoringObject classInstance in allSCOMClassInstances)
        {
          T itemToAdd = AddNew();
          itemToAdd.ParseClassInstance(classInstance, FindActionPoint(classInstance), mySeedClass.Hosted); 
        }
      }
      finally
      {
        ignorePropertyChangeNotification = false;
        ignoreListChangeNotification = false;
        RemoveSortCore();
      }
    }

    protected override void OnAddingNew(AddingNewEventArgs e)
    {
      if (e.NewObject == null)
        e.NewObject = new T();
      ((T)e.NewObject).ObjectChanged += OnObjectChanged;
      ((T)e.NewObject).ActionPoint = DefaultActionPoint;
      base.OnAddingNew(e);
    }

    private void OnObjectChanged(object sender, ObjectChangedEventArgs e)
    {
      T commitedItem = (T)sender;
      if (e.CommitStatus == InstanceCommitStatus.New)
        if (HasAllRequiredProperties(commitedItem))
        {
          PerformSCOMDiscovery(SCOMDiscoveryType.Insert, new List<T>() { commitedItem });
          e.CommitStatus = InstanceCommitStatus.Committed;
        }
      if (e.CommitStatus == InstanceCommitStatus.Modified)
        if (HasAllRequiredProperties(commitedItem))
        {
          PerformSCOMDiscovery(SCOMDiscoveryType.Update, new List<T>() { commitedItem });
          e.CommitStatus = InstanceCommitStatus.Committed;
        }
    }

    protected override void RemoveItem(int index)
    {
      PerformSCOMDiscovery(SCOMDiscoveryType.Delete, new List<T>() { this[index] });
      base.RemoveItem(index);
    }

    protected override void OnListChanged(ListChangedEventArgs e)
    {
      if (e.ListChangedType == ListChangedType.ItemChanged && !ignorePropertyChangeNotification)
      {

      }
      base.OnListChanged(e);
    }

    // internal methods
    private MonitoringObject FindActionPoint(MonitoringObject servedInstance)
    {
      if (mySeedClass.Hosted)
      {
        // if it is hosted, then myActionPoints contains lists of all hosts, so instead of querying SCOM, let's search in this list
        int lastPathPortionIndex = servedInstance.Path.LastIndexOf(';');
        MonitoringObject result = null;
        if (lastPathPortionIndex > 0) // ??
        {
          string hostName = servedInstance.Path.Substring(lastPathPortionIndex + 1);
          string hostPath = servedInstance.Path.Substring(0, lastPathPortionIndex);
          result = myActionPoints.Where(x => x.Name == hostName && x.Path == hostPath).FirstOrDefault();
        }
        else
          result = myActionPoints.Where(x => x.Name == servedInstance.Path).FirstOrDefault();
        if (result != null)
          return result;
        else
          // backup way if myActionPointClass defined incorrectly
          return myMG.EntityObjects.GetRelationshipObjectsWhereTarget<MonitoringObject>(servedInstance.Id, relHosting, DerivedClassTraversalDepth.Recursive, TraversalDepth.OneLevel, ObjectQueryOptions.Default).Where(x => !x.IsDeleted).First().SourceObject;
      }
      else
      {
        if (relMAPManagesEntity == null && relHSShouldManageEntiry == null)
          return null; // don't try to find Action Point for truly unhosted class
        IEnumerable<EnterpriseManagementRelationshipObject<MonitoringObject>> allMAPs = servedInstance.ManagementGroup.EntityObjects.GetRelationshipObjectsWhereTarget<MonitoringObject>(servedInstance.Id,
        relMAPManagesEntity, DerivedClassTraversalDepth.Recursive, TraversalDepth.OneLevel, ObjectQueryOptions.Default).Where(x => !x.IsDeleted);
        if (allMAPs.Count() == 1)
        {
          MonitoringObject draftResult = allMAPs.First().SourceObject;
          if (draftResult.LeastDerivedNonAbstractManagementPackClassId == SystemCenterId.ManagementServiceClassId)
            return myMG.EntityObjects.GetObject<MonitoringObject>((Guid)draftResult[SystemCenterId.ManagementServiceClassProperties.HealthServiceIdPropertyId].Value, ObjectQueryOptions.Default);
          else
            return draftResult;
        }
        else if (allMAPs.Count() == 0)
          return null;
        else
          throw new SCOMClassInstanceEditorException("Unexpected result from GetRelationshipObjectsWhereTarget");
      }
      throw new SCOMClassInstanceEditorException("This exception must never happen.");
    }

    protected virtual IncrementalDiscoveryData PrepareSCOMDiscoveryData(SCOMDiscoveryType direction, IList<T> objects)
    {
      IncrementalDiscoveryData result = new IncrementalDiscoveryData();
      // create object for both incremental and full discovery
      bool hasData = false;
      foreach (T newObject in objects)
      {
        if (!HasAllRequiredProperties(newObject))
          continue;
        hasData = true;
        CreatableEnterpriseManagementObject newSeedInstance = new CreatableEnterpriseManagementObject(myMG, mySeedClass);
        CreatableEnterpriseManagementRelationshipObject newSomethingShouldManageInstance = null;
        foreach (var classProperty in mySeedClass.PropertyCollection)
        {
          try
          {
            newSeedInstance[classProperty].Value = newObject[classProperty.Id];
          }
          catch (KeyNotFoundException) // ignore situations, when non-key or non-required fields are not available
          {
            if (classProperty.Required || classProperty.Key)
              throw;
          }
        }
        // also try to set DisplayName if available
        try
        {
          newSeedInstance[SystemId.EntityClassProperties.DisplayNamePropertyId].Value = newObject.GetClassInstanceProperty(SystemId.EntityClassProperties.DisplayNamePropertyId);
        }
        catch (KeyNotFoundException) { } // ignore

        // Unhosted, with specific action point
        if (!mySeedClass.Hosted && myActionPointClass != null && newObject.ActionPoint != null)
        {
          if (relMAPShouldManageEntity != null)
          {
            newSomethingShouldManageInstance = new CreatableEnterpriseManagementRelationshipObject(myMG, relMAPShouldManageEntity);
            newSomethingShouldManageInstance.SetTarget(newSeedInstance);
            newSomethingShouldManageInstance.SetSource(newObject.ActionPoint);
          }
          // sometimes when specific Health Service is deleted, relationship may revert to All Management Server Pool
          if (relHSShouldManageEntiry != null && !newObject.ActionPoint.IsInstanceOf(myMG.EntityTypes.GetClass(SystemCenterId.ManagementServicePoolClassId)))
          {
            newSomethingShouldManageInstance = new CreatableEnterpriseManagementRelationshipObject(myMG, relHSShouldManageEntiry);
            newSomethingShouldManageInstance.SetTarget(newSeedInstance);
            newSomethingShouldManageInstance.SetSource(newObject.ActionPoint);
          }
          if (relMAPShouldManageEntity == null && relHSShouldManageEntiry == null)
            throw new NotSupportedException("Scenario not supported.");
        }
        // Hosted, in this case myActionPointClass is the hosing class
        if (mySeedClass.Hosted && myActionPointClass != null && newObject.ActionPoint != null)
        {
          ManagementPackClass hostClass = myActionPointClass;
          while (hostClass != null)
          {
            foreach (ManagementPackProperty hostProperty in hostClass.PropertyCollection)
              if (hostProperty.Key)
                newSeedInstance[hostProperty].Value = newObject.ActionPoint[hostProperty].Value;
            hostClass = hostClass.FindHostClass();
          }
        }
        if (hasData)
          switch (direction)
          {
            case SCOMDiscoveryType.Insert:
            case SCOMDiscoveryType.Update:
              result.Add(newSeedInstance);
              if (newSomethingShouldManageInstance != null)
                result.Add(newSomethingShouldManageInstance);
              // don't need Hosted==True check, newSomethingShouldManageInstance will be null for hosted classes
              if (newSomethingShouldManageInstance != null && direction == SCOMDiscoveryType.Update)
                PerformRelationshipCleanup(mySeedClass, newSeedInstance, newObject.ActionPoint);
              if (mySeedClass.Hosted) { }
              break;
            case SCOMDiscoveryType.Delete:
              result.Remove(newSeedInstance);
              if (newSomethingShouldManageInstance != null)
                result.Remove(newSomethingShouldManageInstance);
              break;
          }
      }
      return result;
    }

    private void PerformRelationshipCleanup(ManagementPackClass mySeedClass, CreatableEnterpriseManagementObject newSeedInstance, MonitoringObject monitoringObject)
    {
      // it's so complex, because the new instance may not exist
      EnterpriseManagementObject realSeedInstance;
      string criteriaString = "";
      foreach (ManagementPackProperty property in newSeedInstance.GetProperties())
        if (property.Key)
          if (criteriaString == "")
            criteriaString = property.Name + "='" + newSeedInstance[property] + "'";
          else
            criteriaString += " AND " + property.Name + "='" + newSeedInstance[property] + "'";
      EnterpriseManagementObjectCriteria searchQuery = new EnterpriseManagementObjectCriteria(criteriaString, mySeedClass);
      IObjectReader<EnterpriseManagementObject> realSeedInstanceList = myMG.EntityObjects.GetObjectReader<EnterpriseManagementObject>(searchQuery, ObjectQueryOptions.Default);
      if (realSeedInstanceList.Count == 0)
        return;
      else
        realSeedInstance = realSeedInstanceList.First();

      IncrementalDiscoveryData RemovalDiscovery = new IncrementalDiscoveryData();
      // Management Point
      bool commitOverwrite = false;
      IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> allMAPRelations = myMG.EntityObjects.GetRelationshipObjectsWhereTarget<EnterpriseManagementObject>(realSeedInstance.Id, relToClearMAP, DerivedClassTraversalDepth.Recursive, TraversalDepth.OneLevel, ObjectQueryOptions.Default).Where(x => !x.IsDeleted);
      IEnumerable<EnterpriseManagementRelationshipObject<EnterpriseManagementObject>> allHSRelations = myMG.EntityObjects.GetRelationshipObjectsWhereTarget<EnterpriseManagementObject>(realSeedInstance.Id, relToClearHS, DerivedClassTraversalDepth.Recursive, TraversalDepth.OneLevel, ObjectQueryOptions.Default).Where(x => !x.IsDeleted);
      foreach (EnterpriseManagementRelationshipObject<EnterpriseManagementObject> rel in allMAPRelations)
      {
        if (rel.SourceObject.Id != monitoringObject.Id)
        {
          // remove this relationship
          RemovalDiscovery.Remove(rel);
          commitOverwrite = true;
        }
      }
      foreach (EnterpriseManagementRelationshipObject<EnterpriseManagementObject> rel in allHSRelations)
      {
        if (rel.SourceObject.Id != monitoringObject.Id)
        {
          // remove this relationship
          RemovalDiscovery.Remove(rel);
          commitOverwrite = true;
        }
      }
      if (commitOverwrite)
        RemovalDiscovery.Overwrite(myConnector);
    }

    private void PerformSCOMDiscovery(SCOMDiscoveryType direction, IList<T> objects)
    {
      IncrementalDiscoveryData discoveryDataIncremental = PrepareSCOMDiscoveryData(direction, objects);
      switch (direction)
      {
        case SCOMDiscoveryType.Insert:
          discoveryDataIncremental.Commit(myConnector);
          break;
        case SCOMDiscoveryType.Update:
          discoveryDataIncremental.Overwrite(myConnector);
          break;
        case SCOMDiscoveryType.Delete:
          discoveryDataIncremental.Commit(myConnector);
          break;
      }
      // after successful operation set all items to committed state
      foreach (var newObject in objects)
      {
        newObject.CommitStatus = InstanceCommitStatus.Committed;
      }
    }

    private bool HasAllRequiredProperties(T testRecord)
    {
      // test if all key fields have values
      bool AllKeys = true;
      bool AllRequired = true;
      foreach (var classProperty in mySeedClass.PropertyCollection)
      {
        if (classProperty.Key)
          if (testRecord.GetClassInstanceProperty(classProperty.Id) == null)
            AllKeys = false;
        if (classProperty.Required)
          if (testRecord.GetClassInstanceProperty(classProperty.Id) == null)
            AllRequired = false;
      }
      // in some cases Action Point must have value too
      if (mySeedClass.Hosted || myActionPointClass != null)
        if (testRecord.ActionPoint == null)
          AllRequired = false;
      return AllKeys & AllRequired;
    }
  }
}
