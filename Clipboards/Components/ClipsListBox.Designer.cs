namespace Clipboards.Components
{
  partial class ClipsListBox
  {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.AllowDrop = true;
      this.BackColor = System.Drawing.Color.White;
      this.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.FormattingEnabled = true;
      this.Location = new System.Drawing.Point(0, 0);
      this.Name = "listBoxClips";
      this.Size = new System.Drawing.Size(239, 264);
      this.TabIndex = 0;
      this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(ClipsDrawItem);
      this.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(ClipsMeasureItem);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(ClipsDragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(ClipsDragEnter);
      this.DragOver += new System.Windows.Forms.DragEventHandler(ClipsDragOver);
      this.DragLeave += new System.EventHandler(ClipsDragLeave);
      this.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(ClipsQueryContinueDrag);
      this.DoubleClick += new System.EventHandler(ClipsDoubleClick);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ClipsKeyPress);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(ClipsMouseDown);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(ClipsMouseMove);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(ClipsMouseUp);
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(ClipsMouseClick);
    }
    #endregion
  }
}
