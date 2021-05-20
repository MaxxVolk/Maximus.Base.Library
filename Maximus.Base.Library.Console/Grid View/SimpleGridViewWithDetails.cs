using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

using Maximus.Base.Library.Console;
using Maximus.Library.SCOMId;

using Microsoft.EnterpriseManagement.Common;
using Microsoft.EnterpriseManagement.Configuration;
using Microsoft.EnterpriseManagement.ConsoleFramework;
using Microsoft.EnterpriseManagement.Mom.Internal.UI;
using Microsoft.EnterpriseManagement.Mom.Internal.UI.Cache;
using Microsoft.EnterpriseManagement.Mom.Internal.UI.Controls;
using Microsoft.EnterpriseManagement.Mom.UI;
using Microsoft.EnterpriseManagement.Monitoring;


namespace Maximus.Library.GridView
{
  /// <summary>
  /// Base class to create object Grid View with details plane. No support for roles/hosted child object.
  /// </summary>
  [TypeDescriptionProvider(typeof(AbstractControlDescriptionProvider<SimpleGridViewWithDetails, GridViewBase<InstanceState, StateQuery>>))]
  public abstract class SimpleGridViewWithDetails : GridViewBase<InstanceState, StateQuery>, IParentView
  {
    private readonly Dictionary<HealthState, Image> HealthStateImages;
    private readonly Dictionary<HealthState, string> HealthStrings;

    /// <summary>
    /// Resource manager to access embedded console resources.
    /// </summary>
    protected readonly ResourceManager ConsoleResources;
    /// <summary>
    /// Current Culture.
    /// </summary>
    protected readonly CultureInfo CurrentCulture = Thread.CurrentThread.CurrentUICulture;

    public SimpleGridViewWithDetails() : base(new ManagedEntityTargetParser())
    {
      UseRowContextMenu = false; // row context menu is recalculated for each selected item's context
      //InitializeComponent();
      ConsoleResources = new ResourceManager("Microsoft.EnterpriseManagement.Mom.Internal.UI.Views.SharedResources", typeof(UrlView).Assembly);
      HealthStateImages = new Dictionary<HealthState, Image>()
      {
        { HealthState.Success,       (Bitmap)ConsoleResources.GetObject("StateGreen1Image", CurrentCulture) },
        { HealthState.Warning,       (Bitmap)ConsoleResources.GetObject("StateYellow1Image", CurrentCulture) },
        { HealthState.Error,         (Bitmap)ConsoleResources.GetObject("StateRed1Image", CurrentCulture) },
        { HealthState.Uninitialized, (Bitmap)ConsoleResources.GetObject("StateGray1Image", CurrentCulture) }
      };
    HealthStrings = new Dictionary<HealthState, string>()
      {
        { HealthState.Success,       ConsoleResources.GetString("StateGreen1Text", CurrentCulture) },
        { HealthState.Warning,       ConsoleResources.GetString("StateYellow1Text", CurrentCulture) },
        { HealthState.Error,         ConsoleResources.GetString("StateRed1Text", CurrentCulture) },
        { HealthState.Uninitialized, ConsoleResources.GetString("StateGray1Text", CurrentCulture) }
    };
  }

    /// <summary>
    /// Create grid columns for all properties of the target class and common columns for health state, maintenance mode, etc.
    /// </summary>
    protected override void AddColumns()
    {
      // Health
      Grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
      InstanceStateProperty propertyTranslator = PropertyTranslator as InstanceStateProperty;
      GridControlImageTextColumn stateColumn = new GridControlImageTextColumn();
      stateColumn.CellContentsRequested += new EventHandler<GridControlImageTextColumn.ImageTextEventArgs>(StateCellContentsRequested);
      string str = $"{propertyTranslator.TargetType.Name}-*-{propertyTranslator.TargetType.Id}-*-Health";
      AddColumn(stateColumn, ConsoleResources.GetString("State", CurrentCulture) ?? "State", new Field("Health", typeof(int), str, Field.SortInfos.Sortable | Field.SortInfos.Sort, 0), false);

      // Maintenance Mode
      DataGridViewColumn MaintenanceModeColumn = AddColumn(new GridControlImageTextColumn(
        new Dictionary<string, Image>
        {
          { true.ToString(), (Image)ConsoleResources.GetObject("MaintenanceModeImage", CurrentCulture) },
          { false.ToString(), new Bitmap(16, 16) }
        },
        new Dictionary<string, string>
        {
          { true.ToString(), ConsoleResources.GetString("InMaintenanceMode", CurrentCulture) },
          { false.ToString(), ConsoleResources.GetString("NotInMaintenanceMode", CurrentCulture) }
        })
      {
        DefaultKey = string.Empty,
        Width = 22,
        ShowText = false,
        DefaultHeaderCellType = typeof(GridControlImageColumnHeaderCell)
      }, typeof(bool), ConsoleResources.GetString("MaintenanceModeText", CurrentCulture), "InMaintenanceMode", false, false);

      GridControlImageColumnHeaderCell headerCell = (GridControlImageColumnHeaderCell)MaintenanceModeColumn.HeaderCell;
      headerCell.Image = (Image)ConsoleResources.GetObject("MaintenanceModeImage", CurrentCulture);
      headerCell.ImagePadding = new Padding(2);

      AddColumn(new GridControlTextColumn(), typeof(string), "Name", "DisplayName", false, true);

      foreach (ManagementPackProperty property in GetViewClass().GetProperties()) // Microsoft.Windows.Computer)
      {
        Type type = property.SystemType;
        Type contentType = null;
        if (type == typeof(string) || type == typeof(int))
          contentType = type;
        string headerText = property.DisplayName ?? property.Name;
        if (contentType == null)
        {
          if (type == typeof(Enum))
            type = typeof(string);
          Field sortField = new Field(null, type, property, 0);
          DataGridViewColumn dataGridViewColumn = AddColumn(new GridControlTextColumn(), headerText, sortField, true);
          dataGridViewColumn.Visible = true;
          dataGridViewColumn.FillWeight = 1f;
        }
        else
        {
          DataGridViewColumn dataGridViewColumn = AddColumn(contentType, headerText, property, true);
          dataGridViewColumn.Visible = true;
          dataGridViewColumn.FillWeight = 1f;
        }
      }
    }


    private void StateCellContentsRequested(object sender, GridControlImageTextColumn.ImageTextEventArgs e)
    {
      if (e == null || e.DataItem == null || (e.DataItem.DataItem == null || !(e.DataItem.DataItem is InstanceState dataItem)))
        return;
      bool availability = InstanceState.GetAvailability(dataItem.Health);
      HealthState health = InstanceState.GetHealth(dataItem.Health);
      e.Icon = !availability ? HealthStateImages[HealthState.Uninitialized] : HealthStateImages[health];
      e.Text = HealthStrings[health];
    }

    protected override void OnQueryCreated()
    {
      // set Class Type for the query
      QueryCache.Query.ViewTargetType = GetViewClass();
    }

    /// <summary>
    /// Override this method to define Management Pack Class, instances of which are to be shown in this Grid View.
    /// </summary>
    /// <returns><seealso cref="ManagementPackClass"/> to show in the view</returns>
    protected abstract ManagementPackClass GetViewClass();

    #region Surrounding UI Support
    /// <summary>
    /// Set context menu for right-click at an item in the grid.
    /// </summary>
    /// <param name="contextMenu"></param>
    protected override void AddContextMenu(ContextMenuHelper contextMenu)
    {
      if (contextMenu != null)
      {
        AddUserContextMenu(contextMenu);
        // these methods, OnCopyCommand and UpdateCopyCommandStatus, are defined in GridViewBase
        contextMenu.AddContextMenuItem(StandardCommands.Copy, OnCopyCommand, UpdateCopyCommandStatus);
        contextMenu.AddContextMenuSeparator();
        AddContextMenu_Personalization(contextMenu);
        contextMenu.AddContextMenuItem(ViewCommands.Refresh, null); // use default handler
        contextMenu.AddContextMenuSeparator();
        AddContextMenu_InstanceProperties(contextMenu);
      }
    }

    /// <summary>
    /// Override this method to add more context menu items. Leave the body empty if no items to add.
    /// </summary>
    /// <param name="contextMenu">Contect menu instance to add items to</param>
    protected abstract void AddUserContextMenu(ContextMenuHelper contextMenu);

    /// <summary>
    /// Set context plane actions for the view.
    /// </summary>
    protected override void AddActions()
    {
      AddUserActions();
      AddActions_Personalization();
      base.AddActions();
    }

    /// <summary>
    /// Override this method to add context plane action. Leave the body empty if no actions to add.
    /// </summary>
    protected abstract void AddUserActions();
    #endregion

    #region IParentView Implementation
    /// <summary>
    /// Implements IParentView.ChildViewType. Returns component type, which implements detail view.
    /// </summary>
    public Type ChildViewType => GetDetailedViewControlType();

    /// <summary>
    /// Override this method to return type of a control, which will be attached to this Grid View as a detail plane.
    /// The detail view control should be inherited from <seealso cref="CachedDetailView{PartialMonitoringObject}"/>.
    /// </summary>
    /// <returns></returns>
    protected abstract Type GetDetailedViewControlType();

    /// <summary>
    /// This method is called from <seealso cref="CachedDetailView{PartialMonitoringObject}"/> ParentSelectionChanged method, where the returned object is casted as T.
    /// </summary>
    public object SelectedItem
    {
      get
      {
        if (Grid == null || Grid.SelectedRows == null || Grid.Rows == null || Grid.Rows.Count == 0 || Grid.SelectedRows[0] == null)
          return null;

        if (Grid.SelectedRows[0].Cells[0].Tag is GridDataItem tag)
        {
          InstanceState selectedCellContents = (InstanceState)tag.DataItem;
          return selectedCellContents.GetPartialMonitoringObject(ManagementGroup);
        }
        return null;
      }
    }

    /// <summary>
    /// Implements ChildViewType.GetSelection(). Returns a collection of currently selected items.
    /// </summary>
    /// <returns></returns>
    public ICollection GetSelection()
    {
      if (Grid.SelectedCells.Count == 0)
        return null;
      List<object> result = new List<object>();
      foreach (DataGridViewRow selectedRow in Grid.SelectedRows)
      {
        if (selectedRow.Cells[0].Tag != null)
        {
          InstanceState dataItem = (InstanceState)(selectedRow.Cells[0].Tag as GridDataItem).DataItem;
          if (QueryCache.Query is StateQuery query && query.ManagementGroup != null)
          {
            PartialMonitoringObject monitoringObject = dataItem.GetPartialMonitoringObject(query.ManagementGroup);
            if (monitoringObject != null)
              result.Add(monitoringObject);
          }
        }
      }
      return new ReadOnlyCollection<object>(result);
    }
    #endregion

    #region Personalization Support
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddContextMenu_Personalization(ContextMenuHelper contextMenu)
    {
      contextMenu.AddContextMenuItem(ViewCommands.Personalize, new EventHandler<CommandEventArgs>(OnShowPersonalization), new EventHandler<CommandStatusEventArgs>(OnPersonalizationStatus));

      // Ensure that the component is registered to receive commands and command statuses updates.
      RegisteredCommand registeredCommand = ((ICommandService)Site.GetService(typeof(ICommandService))).Find(ViewCommands.ViewProperties);
      if (!registeredCommand.ComponentIsRegistered(this))
      {
        // Methods OnViewPropertiesCommand and UpdateViewPropertiesCommandStatus are defined in the MomViewBase class
        registeredCommand.Register(new EventHandler<CommandEventArgs>(OnShowViewProperties));
        registeredCommand.RegisterStatusNotification(new EventHandler<CommandStatusEventArgs>(OnViewPropertiesStatus));
      }
    }

    private void OnViewPropertiesStatus(object sender, CommandStatusEventArgs e)
    {
      UpdateViewPropertiesCommandStatus(sender, e);
    }

    private void OnShowViewProperties(object sender, CommandEventArgs e)
    {
      OnViewPropertiesCommand(sender, e);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void AddActions_Personalization()
    {
      AddTaskItem(TaskCommands.ActionsTaskGroup, ViewCommands.Personalize);
    }

    private void OnPersonalizationStatus(object sender, CommandStatusEventArgs e)
    {
      e.CommandStatus.Enabled = true;
    }

    private void OnShowPersonalization(object sender, CommandEventArgs e)
    {
      ShowPersonalization(sender, e);
    }

    protected override void ApplyPersonalization()
    {
      if (ColumnCollection != null)
      {
        ColumnCollection.Apply(Grid);
      }
      else
      {
        if (Configuration == null)
          return;
        ColumnCollection = ViewSupport.XmlToColumnInfoCollection(Configuration.Presentation);
        ColumnCollection.Apply(Grid);
      }
    }

    private void UpdatePersonalizeCommandStatus(object sender, CommandStatusEventArgs e)
    {
      e.CommandStatus.Enabled = true;
    }

    protected override void ShowPersonalization(object sender, CommandEventArgs e)
    {
      string defaultPersonalization = null;
      if (Configuration != null && Configuration.Presentation != null)
        defaultPersonalization = Configuration.Presentation;

      using (ColumnPickerDialog columnPickerDialog = new ColumnPickerDialog(defaultPersonalization))
      {
        columnPickerDialog.Grid = Grid;
        columnPickerDialog.Groupable = false;
        if (columnPickerDialog.ShowDialog(ParentWindow) != DialogResult.OK)
          return;
        ColumnCollection = new ColumnInfoCollection(columnPickerDialog.GetColumns());
        UpdateFields(true);
        SavePersonalization(this);
      }
    }

    protected override void UpdateViewPropertiesCommandStatus(object sender, CommandStatusEventArgs args)
    {
      args.CommandStatus.Enabled = true;
      args.CommandStatus.Visible = true;
    }

    // Properties of the view itself. I.e. when right-clicked at the view node in the navigation tree.
    protected override void OnViewPropertiesCommand(object sender, CommandEventArgs args)
    {
      ViewConfiguration viewConfiguration = ViewSupport.ViewEditDialog(Site, View, SystemView.State);
      Tag = viewConfiguration;
      if (viewConfiguration == null)
        return;
      Configuration.Configuration = viewConfiguration.Configuration;
      Personalization = ViewSupport.XmlToColumnInfoCollection(Configuration.Presentation);
      SavePersonalization(this);
      if (QueryCache == null)
        return;
      QueryCache.SetCriteria(GetQueryCriteria());
      UpdateCache(UpdateReason.Refresh);
    }
    #endregion

    #region Entity Properties Support
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void AddContextMenu_InstanceProperties(ContextMenuHelper contextMenu)
    {
      contextMenu.AddContextMenuItem(ViewCommands.Properties, new EventHandler<CommandEventArgs>(OnShowInstanceProperties), new EventHandler<CommandStatusEventArgs>(OnInstancePropertiesStatus));
    }

    private void OnInstancePropertiesStatus(object sender, CommandStatusEventArgs e)
    {
      if (SelectedItem is PartialMonitoringObject)
        e.CommandStatus.Enabled = true;
    }

    private void OnShowInstanceProperties(object sender, CommandEventArgs e)
    {
      using (InstancePropertiesDialog propertiesDialog = new InstancePropertiesDialog())
      {
        Site.Container.Add(propertiesDialog);
        if (SelectedItem is PartialMonitoringObject pmo)
        {
          var mo = ManagementGroup.EntityObjects.GetObject<MonitoringObject>(pmo.Id, ObjectQueryOptions.Default);
          propertiesDialog.Entity = mo;
          propertiesDialog.Type = mo.GetMostDerivedClasses().FirstOrDefault() ?? ManagementGroup.EntityTypes.GetClass(SystemId.EntityClassId);
          propertiesDialog.ShowDialog();
        }
      }
    }
    #endregion
  }
}
