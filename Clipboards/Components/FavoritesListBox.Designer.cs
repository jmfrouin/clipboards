namespace Clipboards.Components
{
  partial class FavoritesListBox
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
      this.AllowDrop = true;
      this.BackColor = System.Drawing.Color.White;
      this.Dock = System.Windows.Forms.DockStyle.Fill;
      this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.FormattingEnabled = true;
      this.Location = new System.Drawing.Point(0, 0);
      this.Name = "listBoxFavorites";
      this.Size = new System.Drawing.Size(242, 264);
      this.TabIndex = 0;
      this.MouseClick += new System.Windows.Forms.MouseEventHandler(FavMouseClick);
      this.DrawItem += new System.Windows.Forms.DrawItemEventHandler(FavDrawItem);
      this.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(FavMeasureItem);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(FavDragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(FavDragEnter);
      this.DragOver += new System.Windows.Forms.DragEventHandler(FavDragOver);
      this.DragLeave += new System.EventHandler(FavDragLeave);
      this.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(FavQueryContinueDrag);
      this.DoubleClick += new System.EventHandler(FavDoubleClick);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FavKeyPress);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(FavMouseDown);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(FavMouseMove);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(FavMouseUp);

    }

    #endregion
  }
}
