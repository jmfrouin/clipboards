using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DragDropLib;

namespace Clipboards.Components
{
  public partial class MFUListBox : ListBox
  {
    #region Members data
    public List<ClipItem> fMFU = new List<ClipItem>();
    private bool fMousePressed;
    private bool fDragnDrop;
    MainForm fMainForm;
    #endregion

    #region Constructor
    public MFUListBox()
    {
      fMousePressed = false;
      fDragnDrop = false;
      InitializeComponent();
    }
    #endregion

    #region Methods
    public void SetMainForm(MainForm form)
    {
      fMainForm = form;
    }

    private void PasteMFU()
    {
      int Index = SelectedIndex;
      ClipItem Clip = fMFU[Index];
      fMainForm.Paste(Clip);
    }

    public void MoveUp()
    {
      int Index = SelectedIndex;
      if (Index > 0)
      {
        ClipItem Clip = fMFU[Index];
        fMFU.RemoveAt(Index);
        fMFU.Insert(Index - 1, Clip);
        SelectedIndex = Index - 1;
      }
      Refresh();
    }

    public void MoveDown()
    {
      int Index = SelectedIndex;
      if (Index != -1 && Index < (Items.Count - 1))
      {
        ClipItem Clip = fMFU[Index];
        fMFU.RemoveAt(Index);
        fMFU.Insert(Index + 1, Clip);
        SelectedIndex = Index + 1;
      }
      Refresh();
    }
    #endregion

    #region Global callbacks
    private void MFUDrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fMFU.Count)
      {
        Graphics g = e.Graphics;
        ClipItem Clip = fMFU[e.Index];
        Clip.Draw(g, e.Bounds, e.State, e.Font);
        g.Dispose();
      }
    }

    private void MFUKeyPress(object sender, KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      if (c == 13)
      {
        PasteMFU();
      }
    }

    private void MFUDoubleClick(object sender, EventArgs e)
    {
      PasteMFU();
    }

    private void MFUMeasureItem(object sender, MeasureItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fMFU.Count)
      {
        ClipItem Clip = fMFU[e.Index];
        int W = e.ItemWidth, H = e.ItemHeight;
        Clip.Measure(e, Font, ref W, ref H);
        e.ItemWidth = W;
        e.ItemHeight = H;
      }
    }
    #endregion

    #region Drag & Drop callbacks
    private void MFUDragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        ClipItem Clip = new ClipItem((string)(e.Data.GetData(DataFormats.Text)), true);
        // add the selected string to bottom of list
        fMFU.Add(Clip);
        Items.Add(e.Data.GetData(DataFormats.Text));

        //Refresh
        Refresh();
      }
    }

    private void MFUDragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void MFUDragLeave(object sender, EventArgs e)
    {
      IDropTargetHelper dropHelper = (IDropTargetHelper)new DragDropHelper();
      dropHelper.DragLeave();
    }

    private void MFUDragOver(object sender, DragEventArgs e)
    {
      /*fIndexOfItemUnderMouseToDrop = IndexFromPoint(PointToClient(new Point(e.X, e.Y)));

      if (fIndexOfItemUnderMouseToDrop != ListBox.NoMatches)
      {
        SelectedIndex = fIndexOfItemUnderMouseToDrop;
      }*/
      /*else
      {
        int Count = Items.Count;
        SelectedIndex = (Count == 0) ? 1 : Count;
      }*/

      /*if (e.Effect == DragDropEffects.Move)
        Items.Remove((string)e.Data.GetData(DataFormats.Text));*/
    }

    private void MFUQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      Point ScreenOffset = SystemInformation.WorkingArea.Location;

      ListBox lb = sender as ListBox;

      if (lb != null)
      {
        Form f = lb.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((Control.MousePosition.X - ScreenOffset.X) < f.DesktopBounds.Left) ||
          ((Control.MousePosition.X - ScreenOffset.X) > f.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - ScreenOffset.Y) < f.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - ScreenOffset.Y) > f.DesktopBounds.Bottom))
        {
          e.Action = DragAction.Cancel;
        }
      }
    }
    #endregion

    #region Mouse callbacks
    private void MFUMouseDown(object sender, MouseEventArgs e)
    {
      fMousePressed = true;
      fMainForm.fIndexToDragFromMFU = IndexFromPoint(e.X, e.Y);
    }

    private void MFUMouseClick(object sender, MouseEventArgs e)
    {
      int Index = SelectedIndex;
      if (Index != -1 && Index < Items.Count)
      {
        fMainForm.DisplayClip(fMFU[Index]);
      }
    }

    private void MFUMouseMove(object sender, MouseEventArgs e)
    {
      if (fMousePressed)
      {
        if (!fDragnDrop)
        {
          fDragnDrop = true;
          int Index = fMainForm.fIndexToDragFromMFU;
          if (Index >= 0 && Index < Items.Count)
          {
            fMainForm.fFromMFU = true;
            DoDragDrop(Index.ToString(), DragDropEffects.Copy);
          }
#if ADVANCED_DRAG_AND_DROP
      Bitmap bmp = new Bitmap(100, 100, PixelFormat.Format32bppArgb);
      using (Graphics g = Graphics.FromImage(bmp))
      {
        g.Clear(Color.Magenta);
        using (Pen pen = new Pen(Color.Blue, 4f))
        {
          g.DrawEllipse(pen, 20, 20, 60, 60);
        }
      }

      DataObject data = new DataObject(new DragDropLib.DataObject());

      ShDragImage shdi = new ShDragImage();
      Win32Size size;
      size.cx = bmp.Width;
      size.cy = bmp.Height;
      shdi.sizeDragImage = size;
      Point p = e.Location;
      Win32Point wpt;
      wpt.x = p.X;
      wpt.y = p.Y;
      shdi.ptOffset = wpt;
      shdi.hbmpDragImage = bmp.GetHbitmap();
      shdi.crColorKey = Color.Magenta.ToArgb();

      IDragSourceHelper sourceHelper = (IDragSourceHelper)new DragDropHelper();
      sourceHelper.InitializeFromBitmap(ref shdi, data);

      DoDragDrop(data, DragDropEffects.Copy);
#endif
        }
      }
    }

    private void MFUMouseUp(object sender, MouseEventArgs e)
    {
      fMousePressed = false;
      fMainForm.fIndexToDragFromMFU = -1;
    }
    #endregion
  }
}
