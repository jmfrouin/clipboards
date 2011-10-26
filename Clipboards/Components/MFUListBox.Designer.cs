namespace Clipboards.Components
{
  partial class MFUListBox
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
      components = new System.ComponentModel.Container();
      // 
      // listBoxMFU
      // 
      this.AllowDrop = true;
      this.BackColor = System.Drawing.Color.White;
      this.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.FormattingEnabled = true;
      this.Location = new System.Drawing.Point(0, 0);
      this.Name = "listBoxMFU";
      this.Size = new System.Drawing.Size(207, 264);
      this.TabIndex = 0;
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(MFUMouseClick);
      this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(MFUDrawItem);
      this.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(MFUMeasureItem);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(MFUDragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(MFUDragEnter);
      this.DragOver += new System.Windows.Forms.DragEventHandler(MFUDragOver);
      this.DragLeave += new System.EventHandler(MFUDragLeave);
      this.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(MFUQueryContinueDrag);
      this.DoubleClick += new System.EventHandler(MFUDoubleClick);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(MFUKeyPress);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(MFUMouseDown);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(MFUMouseMove);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(MFUMouseUp);
    }

    #endregion
  }
}
