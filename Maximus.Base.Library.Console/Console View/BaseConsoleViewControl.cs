using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.EnterpriseManagement.Mom.Internal.UI;
using Microsoft.EnterpriseManagement.Mom.Internal.UI.Common;
using System.Reflection;

namespace Maximus.Library.ConsoleView
{
  public partial class BaseConsoleViewControl : MomViewBase
  {
    public BaseConsoleViewControl() : base(null)
    {
      InitializeComponent();
      Init();
    }

    public BaseConsoleViewControl(IViewTargetParser parser) : base(parser)
    {
      InitializeComponent();
      Init();
    }

    public BaseConsoleViewControl(Guid containerId, IViewTargetParser parser) : base(containerId, parser)
    {
      InitializeComponent();
      Init();
    }

    public BaseConsoleViewControl(System.ComponentModel.IContainer parentContainer) : base(null)
    {
      ConnectToSite(parentContainer);
      InitializeComponent();
      Init();
    }

    public override string ViewName { get { return GetType().FullName; } }

    protected override void Initialize()
    {
      if (DesignMode)
        return;
      base.Initialize();
    }

    private void Init()
    {
      RefreshColors();
      lVersion.Text = string.Format("Version: {0}", Assembly.GetAssembly(GetType()).GetName().Version);
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

    public string ControlHeader
    {
      get => lHeader.Text;
      set => lHeader.Text = value;
    }
  }
}
