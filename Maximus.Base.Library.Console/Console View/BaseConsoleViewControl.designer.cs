namespace Maximus.Library.ConsoleView
{
  partial class BaseConsoleViewControl
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.pPadding = new System.Windows.Forms.Panel();
      this.pHeader = new System.Windows.Forms.Panel();
      this.lHeader = new System.Windows.Forms.Label();
      this.lVersion = new System.Windows.Forms.Label();
      this.lLogo = new System.Windows.Forms.Label();
      this.pPadding.SuspendLayout();
      this.pHeader.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
      this.SuspendLayout();
      // 
      // pPadding
      // 
      this.pPadding.Controls.Add(this.pHeader);
      this.pPadding.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pPadding.Location = new System.Drawing.Point(0, 0);
      this.pPadding.Name = "pPadding";
      this.pPadding.Padding = new System.Windows.Forms.Padding(3);
      this.pPadding.Size = new System.Drawing.Size(992, 605);
      this.pPadding.TabIndex = 1;
      // 
      // pHeader
      // 
      this.pHeader.Controls.Add(this.lLogo);
      this.pHeader.Controls.Add(this.lHeader);
      this.pHeader.Controls.Add(this.lVersion);
      this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
      this.pHeader.Location = new System.Drawing.Point(3, 3);
      this.pHeader.Margin = new System.Windows.Forms.Padding(0);
      this.pHeader.Name = "pHeader";
      this.pHeader.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
      this.pHeader.Size = new System.Drawing.Size(986, 98);
      this.pHeader.TabIndex = 1;
      // 
      // lHeader
      // 
      this.lHeader.AutoSize = true;
      this.lHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lHeader.Location = new System.Drawing.Point(137, 11);
      this.lHeader.Name = "lHeader";
      this.lHeader.Size = new System.Drawing.Size(103, 31);
      this.lHeader.TabIndex = 2;
      this.lHeader.Text = "Header";
      // 
      // lVersion
      // 
      this.lVersion.AutoSize = true;
      this.lVersion.Location = new System.Drawing.Point(141, 48);
      this.lVersion.Name = "lVersion";
      this.lVersion.Size = new System.Drawing.Size(40, 13);
      this.lVersion.TabIndex = 1;
      this.lVersion.Text = "0.0.0.0";
      // 
      // lLogo
      // 
      this.lLogo.AutoSize = true;
      this.lLogo.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lLogo.Location = new System.Drawing.Point(3, 3);
      this.lLogo.Name = "lLogo";
      this.lLogo.Size = new System.Drawing.Size(85, 73);
      this.lLogo.TabIndex = 3;
      this.lLogo.Text = "m";
      // 
      // BaseConsoleViewControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.pPadding);
      this.Name = "BaseConsoleViewControl";
      this.Size = new System.Drawing.Size(992, 605);
      this.pPadding.ResumeLayout(false);
      this.pHeader.ResumeLayout(false);
      this.pHeader.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private System.Windows.Forms.Panel pHeader;
    private System.Windows.Forms.Label lHeader;
    private System.Windows.Forms.Label lVersion;
    protected System.Windows.Forms.Panel pPadding;
    private System.Windows.Forms.Label lLogo;
  }
}
