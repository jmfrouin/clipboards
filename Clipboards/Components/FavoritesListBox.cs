using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Clipboards.Components
{
  public partial class FavoritesListBox : ListBox
  {
    #region Members data
    public List<ClipItem> fFavorites = new List<ClipItem>();
    private bool fMousePressed;
    private bool fDragnDrop;
    MainForm fMainForm;
    #endregion

    public FavoritesListBox(MainForm form)
    {
      fMainForm = form;
      fMousePressed = false;
      fDragnDrop = false;
      InitializeComponent();
    }

    private void FavMeasureItem(object sender, MeasureItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fFavorites.Count)
      {
        ClipItem Clip = fFavorites[e.Index];
        int W = e.ItemWidth, H = e.ItemHeight;
        Clip.Measure(e, Font, ref W, ref H);
        e.ItemWidth = W;
        e.ItemHeight = H;
      }
    }

    private void FavDrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fFavorites.Count)
      {
        Graphics g = e.Graphics;
        ClipItem Clip = fFavorites[e.Index];
        Clip.Draw(g, e.Bounds, e.State, e.Font);
        g.Dispose();
      }
    }
    private void PasteFavorites()
    {
      int Index = SelectedIndex;
      ClipItem Clip = fFavorites[Index];
      fMainForm.Paste(Clip);
    }

    private void FavKeyPress(object sender, KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      if (c == 13)
      {
        PasteFavorites();
      }
    }

    private void FavDoubleClick(object sender, EventArgs e)
    {
      PasteFavorites();
    }

    private void FavDragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {

        ClipItem Clip = new ClipItem((string)(e.Data.GetData(DataFormats.Text)), true);
        // add the selected string to bottom of list
        fFavorites.Add(Clip);
        Items.Add(e.Data.GetData(DataFormats.Text));

        /*if (fFromClips)
        {
          if (fIndexToDragFromClips >= 0 && fIndexToDragFromClips < listBoxClips.Items.Count)
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

            ClipItem Clip = listBoxClips.fClips[Index];
            fFavorites.Add(Clip);
            listBoxFavorites.Items.Add(fFavorites.Count.ToString());
          }
          fFromClips = false;
        }*/

        /*if (fFromFavorites)
        {
          if (fIndexToDragFromClips >= 0 && fIndexToDragFromFavorites < listBoxFavorites.Items.Count)
          {
          }
          fFromFavorites = false;
        }*/

        //Refresh
        Refresh();
      }
    }

    private void FavDragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void FavDragLeave(object sender, EventArgs e)
    {

    }

    private void FavDragOver(object sender, DragEventArgs e)
    {
      /*fIndexOfItemUnderMouseToDrop = listBoxFavorites.IndexFromPoint(listBoxFavorites.PointToClient(new Point(e.X, e.Y)));

      if (fIndexOfItemUnderMouseToDrop != ListBox.NoMatches)
      {
        listBoxFavorites.SelectedIndex = fIndexOfItemUnderMouseToDrop;
      }*/
      /*else
      {
        int Count = listBoxFavorites.Items.Count;
        listBoxFavorites.SelectedIndex = (Count == 0) ? 1 : Count;
      }*/

      /*if (e.Effect == DragDropEffects.Move)
        listBoxFavorites.Items.Remove((string)e.Data.GetData(DataFormats.Text));*/
    }

    private void FavMouseDown(object sender, MouseEventArgs e)
    {
      fMousePressed = true;
      fMainForm.fIndexToDragFromFavorites = IndexFromPoint(e.X, e.Y);
    }

    private void FavMouseClick(object sender, MouseEventArgs e)
    {
      int Index = SelectedIndex;
      if (Index != -1 && Index < Items.Count)
      {
        fMainForm.DisplayClip(fFavorites[Index]);
      }
    }

    private void FavMouseMove(object sender, MouseEventArgs e)
    {
      if (fMousePressed)
      {
        if (!fDragnDrop)
        {
          fDragnDrop = true;
          int Index = fMainForm.fIndexToDragFromFavorites;
          if (Index >= 0 && Index < Items.Count)
          {
            fMainForm.fFromFavorites = true;
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

    private void FavMouseUp(object sender, MouseEventArgs e)
    {
      fMousePressed = false;
      fMainForm.fIndexToDragFromFavorites = -1;
    }

    private void FavQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
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


  }
}
