using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EnterpriseManagement.Mom.Internal.UI;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Mom.Internal.UI.Common;

namespace Maximus.Library.GridView
{
  /// <summary>
  /// Base class to implement details plane for <seealso cref="SimpleGridViewWithDetails"/>
  /// </summary>
  public abstract partial class SimpleGridViewDetailsPlane : CachedDetailView<PartialMonitoringObject>, IDisposable
  {
    public SimpleGridViewDetailsPlane() : base()
    {
      InitializeComponent();
      RefreshColors();
    }

    /// <summary>
    /// Override this method to set branded colors to plane controls.
    /// </summary>
    protected virtual void RefreshColors()
    {
      if (SystemInformation.HighContrast)
      {
        BackColor = SystemColors.Control;
      }
      else
      {
        BackColor = BrandedColors.OverviewBackgroundColor;
      }
    }

    protected override void OnSystemColorsChanged(EventArgs e)
    {
      RefreshColors();
    }

    protected override void OnForeColorChanged(EventArgs e)
    {
      base.OnForeColorChanged(e);
      RefreshColors();
    }

    /// <summary>
    /// Detail Panel caption as shown in UI.
    /// </summary>
    public override string ViewName
    {
      get
      {
        return GetViewName();
      }
    }

    /// <summary>
    /// Override this method to set view name.
    /// </summary>
    /// <returns></returns>
    protected abstract string GetViewName();

    public override void OnCacheUpdated(PartialMonitoringObject monitoringObjectContext)
    {
      if (monitoringObjectContext == null)
      {
        ShowStatusMessage(NoItemSelectedMessage, false);
        return;
      }
      HideStatusMessage();
      OnMasterViewSelectedObjectChange(monitoringObjectContext);
    }

    /// <summary>
    /// Override this method to handle selection change in the master grid  view.
    /// </summary>
    /// <param name="monitoringObjectContext">Selected object</param>
    protected abstract void OnMasterViewSelectedObjectChange(PartialMonitoringObject monitoringObjectContext);

    /// <summary>
    /// Status message shown when no item in the parent view selected. Override to change.
    /// </summary>
    protected string NoItemSelectedMessage => "Select an Item to show Details Panel.";
  }
}
