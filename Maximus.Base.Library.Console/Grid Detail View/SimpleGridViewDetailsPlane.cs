using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EnterpriseManagement.Mom.Internal.UI;
using Microsoft.EnterpriseManagement.Monitoring;
using Microsoft.EnterpriseManagement.Mom.Internal.UI.Common;

namespace Maximus.Base.Library.Console.Grid_Detail_View
{
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
        ShowStatusMessage("Select an Item to show Control Panel.", false);
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
  }
}
