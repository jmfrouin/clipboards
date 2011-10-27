using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using DragDropLib;

namespace Clipboards.Components
{
  public partial class ClipsListBox : ListBox
  {
    #region Members data
    public List<ClipItem> fClips = new List<ClipItem>();
    private bool fMousePressed;
    private bool fDragnDrop;
    MainForm fMainForm;
    #endregion

    #region Constructor
    public ClipsListBox()
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

    private void PasteClips()
    {
      int Index = SelectedIndex;
      ClipItem Clip = fClips[Index];
      fMainForm.Paste(Clip);
    }

    public void MoveUp()
    {
      int Index = SelectedIndex;
      if (Index > 0)
      {
        ClipItem Clip = fClips[Index];
        fClips.RemoveAt(Index);
        fClips.Insert(Index - 1, Clip);
        SelectedIndex = Index - 1;
      }
      Refresh();
    }

    public void MoveDown()
    {
      int Index = SelectedIndex;
      if (Index != -1 && Index < (Items.Count - 1))
      {
        ClipItem Clip = fClips[Index];
        fClips.RemoveAt(Index);
        fClips.Insert(Index + 1, Clip);
        SelectedIndex = Index + 1;
      }
      Refresh();
    }
    #endregion

    #region Globals callbacks
    private void ClipsMeasureItem(object sender, MeasureItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fClips.Count)
      {
        ClipItem Clip = fClips[e.Index];
        int W = e.ItemWidth, H = e.ItemHeight;
        Clip.Measure(e, Font, ref W, ref H);
        e.ItemWidth = W;
        e.ItemHeight = H;
      }
    }

    private void ClipsDrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fClips.Count)
      {
        Graphics g = e.Graphics;
        ClipItem Clip = fClips[e.Index];
        Clip.Draw(g, e.Bounds, e.State, e.Font);
        g.Dispose();
      }
    }

    private void ClipsKeyPress(object sender, KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      if (c == 13)
      {
        PasteClips();
      }
    }

    private void ClipsDoubleClick(object sender, EventArgs e)
    {
      /*IntPtr Temp = (IntPtr)462908;
      int nProcessID = Process.GetCurrentProcess().Id;*/

      //SetForegroundWindowInternal(Temp); //fExtApp);
      SendKeys.Send("^v");
      //PasteClips();
    }
    #endregion

    #region Drag & Drop callbacks
    private void ClipsDragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat)) e.Effect = DragDropEffects.Copy;
      else e.Effect = DragDropEffects.None;

#if ADVANCED_DRAG_AND_DROP
      Point p = Cursor.Position;
      Win32Point wp;
      wp.x = p.X;
      wp.y = p.Y;
      IDropTargetHelper dropHelper = (IDropTargetHelper)new DragDropHelper();
      dropHelper.DragEnter(IntPtr.Zero, (ComIDataObject)e.Data, ref wp, (int)e.Effect);
#endif
    }

    private void ClipsDragLeave(object sender, EventArgs e)
    {
      IDropTargetHelper dropHelper = (IDropTargetHelper)new DragDropHelper();
      dropHelper.DragLeave();
    }

    private void ClipsDragOver(object sender, DragEventArgs e)
    {
      /*fIndexOfItemUnderMouseToDrop = listBoxClips.IndexFromPoint(listBoxClips.PointToClient(new Point(e.X, e.Y)));

      if (fIndexOfItemUnderMouseToDrop != ListBox.NoMatches)
      {
        listBoxClips.SelectedIndex = fIndexOfItemUnderMouseToDrop;
      }
      else
      {*/
      SelectedIndex = 0;
      //}

      /*if (e.Effect == DragDropEffects.Move)
        listBoxClips.Items.Remove((string)e.Data.GetData(DataFormats.Text));*/

#if ADVANCED_DRAG_AND_DROP
      Point p = Cursor.Position;
      Win32Point wp;
      wp.x = p.X;
      wp.y = p.Y;
      IDropTargetHelper dropHelper = (IDropTargetHelper)new DragDropHelper();
      dropHelper.DragOver(ref wp, (int)e.Effect);
#endif
    }
    
    private void ClipsQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
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

    private void ClipsDragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        ClipItem Clip = new ClipItem((string)(e.Data.GetData(DataFormats.Text)), true);
        // add the selected string to bottom of list
        fClips.Add(Clip);
        Items.Add(e.Data.GetData(DataFormats.Text));

        /*if (fFromFavorites)
        {
          if (fIndexOfItemUnderMouseToDrop >= 0 && fIndexOfItemUnderMouseToDrop < listBoxFavorites.Items.Count)
          {
            int Index = 0;
            try
            {
              Index = int.Parse((string)e.Data.GetData(DataFormats.Text));
            }
            catch (Exception)
            {
              Console.WriteLine("Erreur de parsing");
            }

            ClipItem Clip = fFavorites[Index];
            fClips.Add(Clip);
            listBoxClips.Items.Add(fClips.Count.ToString());
          }
          fFromFavorites = false;
        }*/

        //Refresh
        Refresh();
      }

#if ADVANCED_DRAG_AND_DROP
      Point p = Cursor.Position;
      Win32Point wp;
      wp.x = p.X;
      wp.y = p.Y;
      IDropTargetHelper dropHelper = (IDropTargetHelper)new DragDropHelper();
      dropHelper.Drop((ComIDataObject)e.Data, ref wp, (int)e.Effect);
#endif
    }
    #endregion

    #region Mouse callbacks
    private void ClipsMouseClick(object sender, MouseEventArgs e)
    {
      int Index = SelectedIndex;
      if (Index != -1 && Index < Items.Count)
      {
        fMainForm.DisplayClip(fClips[Index]);
      }
    }

    private void ClipsMouseDown(object sender, MouseEventArgs e)
    {
      fMousePressed = true;
      fDragnDrop = false;
      fMainForm.fIndexToDragFromClips = IndexFromPoint(e.X, e.Y);
    }

    private void ClipsMouseUp(object sender, MouseEventArgs e)
    {
      fMousePressed = false;
      fMainForm.fIndexToDragFromClips = -1;
    }

    private void ClipsMouseMove(object sender, MouseEventArgs e)
    {
      if (fMousePressed)
      {
        if (!fDragnDrop)
        {
          fDragnDrop = true;
          int Index = fMainForm.fIndexToDragFromClips;
          if (Index >= 0 && Index < Items.Count)
          {
            fMainForm.fFromClips = true;
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
    #endregion
  }
}
