using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using System.Text;

namespace Clipboards
{
  public partial class MainForm : Form
  {
    #region Members data
    //Used when registering/unregistering the clipboard viewer!
    private IntPtr ClipboardViewerNext;

    //Liste de Clips
    List<ClipItem> fClips = new List<ClipItem>();
    //Liste de Clips Favoris
    List<ClipItem> fFavorites = new List<ClipItem>();

    //Pour gérer le cas où Clipboards, lui même, push quelque chose dans le ClipBoard system.
    private bool fLocalCopy;
    private bool fCallFromHotkey;

    private string fSettingsFolder;

    //To avoid catching system content @ launch
    private bool fInitialRun;

    IntPtr fExtApp;

    //Drag & Drop stuff
    private int fIndexOfItemUnderMouseToDrop;
    private int fIndexOfItemUnderMouseToDrag;
    private Point fScreenOffset;
    private bool fFromClips;
    private bool fFromFavorites;
    #endregion

    #region Ctor / Dtor
    public MainForm()
    {
      InitializeComponent();
      fLocalCopy = false;
      fCallFromHotkey = false;
      fInitialRun = true;
      fExtApp = IntPtr.Zero;
      fFromClips = false;
      fFromFavorites = false;

      //Callbacks
      this.Load += new System.EventHandler(this.MainForm_Load);
      this.Closed += new System.EventHandler(this.MainForm_Closed);
      this.Activated += new System.EventHandler(this.MainForm_Activated);
      this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);

      //Settings, to be put, later, in a dedicated DB
      Screen screen = Screen.PrimaryScreen;
      int ScreenW = screen.Bounds.Width;
      int ScreenH = screen.Bounds.Height;
      int AppW = this.Size.Width;
      int AppH = this.Size.Height;
      int TaskbarH = Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom;
      this.Location = new Point(ScreenW - AppW, ScreenH - AppH - TaskbarH);
      this.ShowInTaskbar = false;

      //Create settings folder i/a
      fSettingsFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Application.ProductName;
      if (!System.IO.File.Exists(fSettingsFolder))
      {
        System.IO.Directory.CreateDirectory(fSettingsFolder);
      }

      //Hooks
      RegisterClipboardViewer();

      //Only one type of pan should be visibler at startup
      splitContainerPreviewPan.Panel1Collapsed = false;
      splitContainerPreviewPan.Panel1.Show();
      splitContainerPreviewPan.Panel2Collapsed = true;
      splitContainerPreviewPan.Panel2.Hide();

      applySettingsToUI();

      //AutoRun
      // The path to the key where Windows looks for startup applications
      RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

      if (rkApp.GetValue("Clipboards") == null)
      {
        // The value doesn't exist, the application is not set to run at startup
        Properties.Settings.Default.AutoRun = false;
      }
      else
      {
        // The value exists, the application is set to run at startup
        Properties.Settings.Default.AutoRun = true;
      }

      //Give focus 
      ActiveControl = listBoxClips;
    }
    #endregion

    #region Callbacks !
    private void MainForm_Activated(object sender, System.EventArgs e)
    {
      //this.Opacity = 1;
      //this.Show();
    }

    private void MainForm_Deactivate(object sender, System.EventArgs e)
    {
      //this.Opacity = 0.3;
      //this.Hide();
    }

    private void MainForm_Load(object sender, System.EventArgs e)
    {
      RestoreClips();
      //Register CTRL+SHIFT+D & CTRL+E
      //http://msdn.microsoft.com/en-us/library/windows/desktop/ms646279(v=vs.85).aspx
      //APIFuncs.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), APIFuncs.Constants.CTRL + APIFuncs.Constants.SHIFT, (int)'V');
      APIFuncs.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), APIFuncs.Constants.CTRL, (int)'E');
    }

    private void MainForm_Closed(object sender, System.EventArgs e)
    {
      APIFuncs.UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
      UnregisterClipboardViewer();
      SaveClips();
    }

    protected override void WndProc(ref Message m)
    {
      switch ((APIFuncs.Msgs)m.Msg)
      {
        case APIFuncs.Msgs.WM_SETFOCUS:
          {
            fExtApp = APIFuncs.GetActiveWindow();
            break;
          }
        case APIFuncs.Msgs.WM_HOTKEY:
          {
            Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
            int Modifiers = ((int)m.LParam & 0xFFFF);
            switch (key)
            {
              case Keys.E:
                {
                  if (Modifiers == APIFuncs.Constants.CTRL)
                  {
                    listBoxClips.SelectedIndex = listBoxClips.Items.Count - 1;
                    Show();
                    WindowState = FormWindowState.Normal;
                    fCallFromHotkey = true;
                  }
                  break;
                }
              case Keys.V:
                {
                  if ((Modifiers == (APIFuncs.Constants.CTRL + APIFuncs.Constants.SHIFT)))
                  {
                    QuickPaste qp = new QuickPaste();
                    qp.Show();
                    fCallFromHotkey = true;
                  }
                  break;
                }
              default:
                break;

            }
            break;
          }
        case APIFuncs.Msgs.WM_DRAWCLIPBOARD:
          {
            if (!fInitialRun)
            {
              InsertClip();
              if (ClipboardViewerNext != System.IntPtr.Zero)
                APIFuncs.SendMessage(ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
            }
            else
            {
              fInitialRun = false;
            }
            break;
          }
        case APIFuncs.Msgs.WM_CHANGECBCHAIN:
          {
            if (m.WParam == ClipboardViewerNext)
            {
              ClipboardViewerNext = m.LParam;
            }
            else
            {
              if (ClipboardViewerNext != System.IntPtr.Zero)
                APIFuncs.SendMessage(ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
            }
            break;
          }
        default:
          {
            base.WndProc(ref m);
            break;
          }
      }
    }

    private void InsertClip()
    {
      IDataObject iData = Clipboard.GetDataObject();
      if (iData != null)
      {
        //Handle TXT clipboard
        if (iData.GetDataPresent(DataFormats.Text))
        {
          IntPtr hwnd = APIFuncs.GetForegroundWindow();
          Int32 pid = APIFuncs.GetWindowProcessID(hwnd);
          Process p = Process.GetProcessById(pid);

          ClipItem Item = new ClipItem(p.MainModule.FileName);
          Item.Content = iData.GetData(DataFormats.StringFormat).ToString();
          Item.Type = ClipItem.EType.eText;
          if (fLocalCopy == false)
          {
            fClips.Add(Item);
            listBoxClips.Items.Add(fClips.Count.ToString());
            trayIcon.ShowBalloonTip(500, Properties.Resources.ClipOfTypeText, Item.Content, ToolTipIcon.Info);
          }
          else
          {
            fLocalCopy = false;
          }
        }

        //Handle Bitmap element
        if (iData.GetDataPresent(DataFormats.Bitmap))
        {
          IntPtr hwnd = APIFuncs.GetForegroundWindow();
          Int32 pid = APIFuncs.GetWindowProcessID(hwnd);
          Process p = Process.GetProcessById(pid);

          ClipItem Item = new ClipItem(p.MainModule.FileName);
          Item.Image = (Bitmap)iData.GetData(DataFormats.Bitmap);
          Item.Type = ClipItem.EType.eImage;
          if (fLocalCopy == false)
          {
            fClips.Add(Item);
            listBoxClips.Items.Add(fClips.Count.ToString());
            trayIcon.ShowBalloonTip(500, Properties.Resources.ClipOfTypeImage, Item.Image.Size.ToString(), ToolTipIcon.Info);
          }
          else
          {
            fLocalCopy = false;
          }
        }

#warning Handle files element
        /*
                string[] type = iData.GetFormats();
                int a = type.Length;
                if (a == 3)
                {
                    int b = 3;
                }
                */
      }
    }
    #endregion

    #region Customized listboxes !
    private void listBoxClips_MeasureItem(object sender, MeasureItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fClips.Count)
      {
        ClipItem Clip = fClips[e.Index];
        int W = e.ItemWidth, H = e.ItemHeight;
        Clip.Measure(e, listBoxClips.Font, ref W, ref H);
        e.ItemWidth = W;
        e.ItemHeight = H;
      }
    }

    private void listBoxClips_DrawItem(object sender, DrawItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fClips.Count)
      {
        Graphics g = e.Graphics;
        ClipItem Clip = fClips[e.Index];
        Clip.Draw(g, e.Bounds, e.State, e.Font);
        g.Dispose();
      }
    }

    private void listBoxFavorites_FavMeasureItem(object sender, MeasureItemEventArgs e)
    {
      if (e.Index != -1 && e.Index < fFavorites.Count)
      {
        ClipItem Clip = fFavorites[e.Index];
        int W = e.ItemWidth, H = e.ItemHeight;
        Clip.Measure(e, listBoxFavorites.Font, ref W, ref H);
        e.ItemWidth = W;
        e.ItemHeight = H;
      }
    }

    private void listBoxFavorites_FavDrawItem(object sender, DrawItemEventArgs e)
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
      int Index = listBoxFavorites.SelectedIndex;
      ClipItem Clip = fFavorites[Index];
      switch (Clip.Type)
      {
        case ClipItem.EType.eText:
          {
            fLocalCopy = true;
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, Clip.Content);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
            if (fCallFromHotkey)
            {
              fCallFromHotkey = false;
              Hide();
              SendKeys.SendWait("^v");
            }
            break;
          }
        case ClipItem.EType.eImage:
          {
            fLocalCopy = true;
            DataObject data = new DataObject();
            data.SetData(DataFormats.Bitmap, Clip.Image);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
            if (fCallFromHotkey)
            {
              fCallFromHotkey = false;
              Hide();
              SendKeys.SendWait("^v");
            }
            break;
          }
        default:
          break;
      }
    }

    private void listBoxFavorites_KeyPress(object sender, KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      if (c == 13)
      {
        PasteFavorites();
      }
    }

    private void listBoxFavorites_DoubleClick(object sender, EventArgs e)
    {
      PasteFavorites();
    }

    private void PasteClips()
    {
      int Index = listBoxClips.SelectedIndex;
      ClipItem Clip = fClips[Index];
      switch (Clip.Type)
      {
        case ClipItem.EType.eText:
          {
            fLocalCopy = true;
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, Clip.Content);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
            if (fCallFromHotkey)
            {
              fCallFromHotkey = false;
              Hide();
              SendKeys.SendWait("^v");
            }
            break;
          }
        case ClipItem.EType.eImage:
          {
            fLocalCopy = true;
            DataObject data = new DataObject();
            data.SetData(DataFormats.Bitmap, Clip.Image);
            Clipboard.Clear();
            Clipboard.SetDataObject(data);
            if (fCallFromHotkey)
            {
              fCallFromHotkey = false;
              Hide();
              SendKeys.SendWait("^v");
            }
            break;
          }
        default:
          break;
      }
    }

    private void listBoxClips_KeyPress(object sender, KeyPressEventArgs e)
    {
      char c = e.KeyChar;
      if (c == 13)
      {
        PasteClips();
      }
    }

    private void listBoxClips_DoubleClick(object sender, EventArgs e)
    {
      IntPtr Temp = (IntPtr)462908;
      int nProcessID = Process.GetCurrentProcess().Id;

      SetForegroundWindowInternal(Temp); //fExtApp);
      SendKeys.Send("^v");
      //PasteClips();
    }

    /*
    * http://www.codeguru.com/forum/showthread.php?t=474426
    * http://stackoverflow.com/questions/46030/c-sharp-force-form-focus
    * http://chabster.blogspot.com/2010/03/focus-and-window-activation-in-win32.html
    */
    private void SetForegroundWindowInternal(IntPtr hwnd)
    {
      if (APIFuncs.IsIconic(hwnd))
        APIFuncs.ShowWindowAsync(hwnd, APIFuncs.SW_RESTORE);

      APIFuncs.ShowWindowAsync(hwnd, APIFuncs.SW_SHOW);

      APIFuncs.SetForegroundWindow(hwnd);

      // Code from Karl E. Peterson, www.mvps.org/vb/sample.htm
      // Converted to Delphi by Ray Lischner
      // Published in The Delphi Magazine 55, page 16
      // Converted to C# by Kevin Gale
      IntPtr foregroundWindow = APIFuncs.GetForegroundWindow();
      IntPtr Dummy = IntPtr.Zero;

      uint foregroundThreadId = APIFuncs.GetWindowThreadProcessId(foregroundWindow, Dummy);
      uint thisThreadId = APIFuncs.GetWindowThreadProcessId(hwnd, Dummy);

      if (APIFuncs.AttachThreadInput(thisThreadId, foregroundThreadId, true))
      {
        APIFuncs.BringWindowToTop(hwnd); // IE 5.5 related hack
        APIFuncs.SetForegroundWindow(hwnd);
        APIFuncs.AttachThreadInput(thisThreadId, foregroundThreadId, false);
      }

      if (APIFuncs.GetForegroundWindow() != hwnd)
      {
        // Code by Daniel P. Stasinski
        // Converted to C# by Kevin Gale
        IntPtr Timeout = IntPtr.Zero;
        APIFuncs.SystemParametersInfo(APIFuncs.SPI_GETFOREGROUNDLOCKTIMEOUT, 0, Timeout, 0);
        APIFuncs.SystemParametersInfo(APIFuncs.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, Dummy, APIFuncs.SPIF_SENDCHANGE);
        APIFuncs.BringWindowToTop(hwnd); // IE 5.5 related hack
        APIFuncs.SetForegroundWindow(hwnd);
        APIFuncs.SystemParametersInfo(APIFuncs.SPI_SETFOREGROUNDLOCKTIMEOUT, 0, Timeout, APIFuncs.SPIF_SENDCHANGE);
      }
    }

    private void listBoxClips_MouseClick(object sender, MouseEventArgs e)
    {
      /*int Index = listBoxClips.SelectedIndex;
      if (Index != -1 && Index < listBoxClips.Items.Count)
      {
        ClipItem Clip = fClips[Index];
        switch (Clip.Type)
        {
          case ClipItem.EType.eImage:
            {
              splitContainerPreviewPan.Panel1Collapsed = false;
              splitContainerPreviewPan.Panel1.Show();
              splitContainerPreviewPan.Panel2Collapsed = true;
              splitContainerPreviewPan.Panel2.Hide();
              pictureBoxPreview.Image = Clip.Image;
              break;
            }
          case ClipItem.EType.eText:
            {
              richTextBoxPreview.Text = Clip.Content;
              splitContainerPreviewPan.Panel1Collapsed = true;
              splitContainerPreviewPan.Panel1.Hide();
              splitContainerPreviewPan.Panel2Collapsed = false;
              splitContainerPreviewPan.Panel2.Show();
              break;
            }
          default:
            break;
        }
      }*/
    }
    #endregion

    #region (Un)Register to clipboard viewers chain !
    private void RegisterClipboardViewer()
    {
      ClipboardViewerNext = APIFuncs.SetClipboardViewer(this.Handle);
    }

    private void UnregisterClipboardViewer()
    {
      APIFuncs.ChangeClipboardChain(this.Handle, ClipboardViewerNext);
    }
    #endregion

    #region Save / Restore Clips
    private void SaveClips()
    {
      try
      {
        //Normal
        using (Stream stream = File.Open(fSettingsFolder + "\\Clips.bin", FileMode.Create))
        {
          BinaryFormatter bin = new BinaryFormatter();
          bin.Serialize(stream, fClips);
        }
        //Favoris
        using (Stream stream = File.Open(fSettingsFolder + "\\Favorites.bin", FileMode.Create))
        {
          BinaryFormatter bin = new BinaryFormatter();
          bin.Serialize(stream, fFavorites);
        }
      }
      catch (IOException)
      {
      }
    }

    private void RestoreClips()
    {
      try
      {
        //Normal
        using (Stream stream = File.Open(fSettingsFolder + "\\Clips.bin", FileMode.Open))
        {
          BinaryFormatter bin = new BinaryFormatter();
          fClips = (List<ClipItem>)bin.Deserialize(stream);
          int Count = 0;
          listBoxClips.Items.Clear();
          foreach (ClipItem Item in fClips)
          {
            listBoxClips.Items.Add((Count++).ToString());
          }
        }
        //Favorites
        using (Stream stream = File.Open(fSettingsFolder + "\\Favorites.bin", FileMode.Open))
        {
          BinaryFormatter bin = new BinaryFormatter();
          fFavorites = (List<ClipItem>)bin.Deserialize(stream);
          int Count = 0;
          listBoxFavorites.Items.Clear();
          foreach (ClipItem Item in fFavorites)
          {
            listBoxFavorites.Items.Add((Count++).ToString());
          }
        }
      }
      catch (IOException e)
      {
        Console.WriteLine("!!! Exception caught here" + e.ToString());
      }
    }
    #endregion

    #region toolStrip callbacks
    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutBox aBox = new AboutBox();
      aBox.ShowDialog();
    }

    private void toolStripDelete_Click(object sender, EventArgs e)
    {
      if (listBoxClips.Focused)
      {
        int Index = listBoxClips.SelectedIndex;
        if (Index != -1)
        {
          fClips.RemoveAt(Index);
          listBoxClips.Items.RemoveAt(Index);
        }
      }
      if (listBoxFavorites.Focused)
      {
        int Index = listBoxFavorites.SelectedIndex;
        if (Index != -1)
        {
          fFavorites.RemoveAt(Index);
          listBoxFavorites.Items.RemoveAt(Index);
        }
      }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void toolStripUp_Click(object sender, EventArgs e)
    {
      int Index = listBoxClips.SelectedIndex;
      if (Index > 0)
      {
        ClipItem Clip = fClips[Index];
        fClips.RemoveAt(Index);
        fClips.Insert(Index - 1, Clip);
        listBoxClips.SelectedIndex = Index - 1;
      }
      listBoxClips.Refresh();
    }

    private void toolStripDown_Click(object sender, EventArgs e)
    {
      int Index = listBoxClips.SelectedIndex;
      if (Index != -1 && Index < (listBoxClips.Items.Count - 1))
      {
        ClipItem Clip = fClips[Index];
        fClips.RemoveAt(Index);
        fClips.Insert(Index + 1, Clip);
        listBoxClips.SelectedIndex = Index + 1;
      }
      listBoxClips.Refresh();
    }

    private void toolStripButtonExpandPreviewPan_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.DisplayPreview = !Properties.Settings.Default.DisplayPreview;
      Properties.Settings.Default.Save();
      applySettingsToUI();
    }

    private void toolStripButtonAddFav_Click(object sender, EventArgs e)
    {
      int Index = listBoxClips.SelectedIndex;
      if (Index != -1)
      {
        ClipItem Clip = fClips[Index];
        fFavorites.Add(Clip);
        listBoxFavorites.Items.Add(fFavorites.Count.ToString());
      }
      listBoxFavorites.Refresh();
    }

    private void toolStripFavorites_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.DisplayFavorites = !Properties.Settings.Default.DisplayFavorites;
      Properties.Settings.Default.Save();
      applySettingsToUI();
    }

    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
      SettingsBox sBox = new SettingsBox();
      sBox.ShowDialog();
      applySettingsToUI();
    }
    #endregion

    private void applySettingsToUI()
    {
      //If Preview & Favorites are closed, collapse pan.
      if (!Properties.Settings.Default.DisplayFavorites && !Properties.Settings.Default.DisplayPreview)
      {
        splitContainer1.Panel1Collapsed = true;
        splitContainer1.Panel1.Hide();
      }
      else
      {
        splitContainer1.Panel1Collapsed = false;
        splitContainer1.Panel1.Show();

        //Use Settings
        if (Properties.Settings.Default.DisplayPreview)
        {
          splitContainer2.Panel1Collapsed = false;
          splitContainer2.Panel1.Show();
        }
        else
        {
          splitContainer2.Panel1Collapsed = true;
          splitContainer2.Panel1.Hide();
        }

        if (Properties.Settings.Default.DisplayFavorites)
        {
          splitContainer2.Panel2Collapsed = false;
          splitContainer2.Panel2.Show();
        }
        else
        {
          splitContainer2.Panel2Collapsed = true;
          splitContainer2.Panel2.Hide();
        }
      }
    }

    private void OnResize(object sender, EventArgs e)
    {
      if (WindowState == FormWindowState.Minimized)
      {
        Hide();
      }
    }

    private void OnFormClosing(object sender, FormClosingEventArgs e)
    {
    }

    #region Tray icons callbacks
    private void trayIcon_DoubleClick(object sender, EventArgs e)
    {
      if (WindowState == FormWindowState.Minimized)
      {
        Show();
        WindowState = FormWindowState.Normal;
      }
      else
      {
        Hide();
        WindowState = FormWindowState.Minimized;
      }
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Show();
      WindowState = FormWindowState.Normal;
    }
    #endregion

    private void listBoxFavorites_MouseClick(object sender, MouseEventArgs e)
    {
      /*int Index = listBoxFavorites.SelectedIndex;
      if (Index != -1 && Index < listBoxFavorites.Items.Count)
      {
        ClipItem Clip = fFavorites[Index];
        switch (Clip.Type)
        {
          case ClipItem.EType.eImage:
            {
              splitContainerPreviewPan.Panel1Collapsed = false;
              splitContainerPreviewPan.Panel1.Show();
              splitContainerPreviewPan.Panel2Collapsed = true;
              splitContainerPreviewPan.Panel2.Hide();
              pictureBoxPreview.Image = Clip.Image;
              break;
            }
          case ClipItem.EType.eText:
            {
              richTextBoxPreview.Text = Clip.Content;
              splitContainerPreviewPan.Panel1Collapsed = true;
              splitContainerPreviewPan.Panel1.Hide();
              splitContainerPreviewPan.Panel2Collapsed = false;
              splitContainerPreviewPan.Panel2.Show();
              break;
            }
          default:
            break;
        }
      }*/
    }

    private void trayIcon_MouseClick(object sender, MouseEventArgs e)
    {
      /*const int nChars = 256;
      IntPtr handle = IntPtr.Zero;
      StringBuilder Buff = new StringBuilder(nChars);

      handle = APIFuncs.GetForegroundWindow();

      if (APIFuncs.GetWindowText(handle, Buff, nChars) > 0)
      {
        string temp = Buff.ToString();
        int a = 2;
      }*/
    }

    #region Drap & Drop stuff
    //http://www.dotnetcurry.com/ShowArticle.aspx?ID=179&AspxAutoDetectCookieSupport=1
    private void OnDragEnter(object sender, DragEventArgs e)
    {

    }

    private void OnDragLeave(object sender, EventArgs e)
    {

    }

    private void OnDragOver(object sender, DragEventArgs e)
    {

    }

    private void OnDragDrop(object sender, DragEventArgs e)
    {

    }

    private void listBoxFavorites_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {

        if (!fFromClips && !fFromFavorites)
        {
          ClipItem Clip = new ClipItem((string)(e.Data.GetData(DataFormats.Text)), true);
          // add the selected string to bottom of list
          fFavorites.Add(Clip);
          listBoxFavorites.Items.Add(e.Data.GetData(DataFormats.Text));
        }

        if (fFromClips)
        {
          if (fIndexOfItemUnderMouseToDrop >= 0 && fIndexOfItemUnderMouseToDrop < listBoxClips.Items.Count)
          {
            int Index = 0;
            try
            {
                Index = int.Parse((string)e.Data.GetData(DataFormats.Text));
            }
            catch(Exception)
            {
                  Console.WriteLine("Erreur de parsing");
            }

            ClipItem Clip = fClips[Index];
            fFavorites.Add(Clip);
            listBoxFavorites.Items.Add(fFavorites.Count.ToString());
          }
          fFromClips = false;
        }

        if (fFromFavorites)
        {
          if (fIndexOfItemUnderMouseToDrop >= 0 && fIndexOfItemUnderMouseToDrop < listBoxFavorites.Items.Count)
          {
          }
          fFromFavorites = false;
        }

        //Refresh
        listBoxFavorites.Refresh();
      }
      
    }

    private void listBoxFavorites_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void listBoxFavorites_DragLeave(object sender, EventArgs e)
    {

    }

    private void listBoxFavorites_DragOver(object sender, DragEventArgs e)
    {
      fIndexOfItemUnderMouseToDrop = listBoxFavorites.IndexFromPoint(listBoxFavorites.PointToClient(new Point(e.X, e.Y)));

      if (fIndexOfItemUnderMouseToDrop != ListBox.NoMatches)
      {
        listBoxFavorites.SelectedIndex = fIndexOfItemUnderMouseToDrop;
      }
      /*else
      {
        int Count = listBoxFavorites.Items.Count;
        listBoxFavorites.SelectedIndex = (Count == 0) ? 1 : Count;
      }*/

      /*if (e.Effect == DragDropEffects.Move)
        listBoxFavorites.Items.Remove((string)e.Data.GetData(DataFormats.Text));*/
    }

    private void listBoxFavorites_MouseDown(object sender, MouseEventArgs e)
    {
      int indexOfItem = listBoxFavorites.IndexFromPoint(e.X, e.Y);
      if (indexOfItem >= 0 && indexOfItem < listBoxFavorites.Items.Count)
      {
        fFromFavorites = true;
        listBoxFavorites.DoDragDrop(listBoxFavorites.Items[indexOfItem], DragDropEffects.Copy);
      }
    }

    private void listBoxFavorites_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      fScreenOffset = SystemInformation.WorkingArea.Location;

      ListBox lb = sender as ListBox;

      if (lb != null)
      {
        Form f = lb.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((Control.MousePosition.X - fScreenOffset.X) < f.DesktopBounds.Left) ||
          ((Control.MousePosition.X - fScreenOffset.X) > f.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - fScreenOffset.Y) < f.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - fScreenOffset.Y) > f.DesktopBounds.Bottom))
        {
          e.Action = DragAction.Cancel;
        }
      }
    }

    private void listBoxClips_DragDrop(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
      {
        if (fIndexOfItemUnderMouseToDrop >= 0 && fIndexOfItemUnderMouseToDrop < listBoxClips.Items.Count)
        {
          listBoxClips.Items.Insert(fIndexOfItemUnderMouseToDrop, e.Data.GetData(DataFormats.Text));
        }
        else
        {
          // add the selected string to bottom of list
          listBoxClips.Items.Add(e.Data.GetData(DataFormats.Text));
        }
      }
    }

    private void listBoxClips_DragEnter(object sender, DragEventArgs e)
    {
      if (e.Data.GetDataPresent(DataFormats.StringFormat))
        e.Effect = DragDropEffects.Copy;
      else
        e.Effect = DragDropEffects.None;
    }

    private void listBoxClips_DragLeave(object sender, EventArgs e)
    {

    }

    private void listBoxClips_DragOver(object sender, DragEventArgs e)
    {
      fIndexOfItemUnderMouseToDrop = listBoxClips.IndexFromPoint(listBoxClips.PointToClient(new Point(e.X, e.Y)));

      if (fIndexOfItemUnderMouseToDrop != ListBox.NoMatches)
      {
        listBoxClips.SelectedIndex = fIndexOfItemUnderMouseToDrop;
      }
      else
      {
        listBoxClips.SelectedIndex = 0;
      }

      /*if (e.Effect == DragDropEffects.Move)
        listBoxClips.Items.Remove((string)e.Data.GetData(DataFormats.Text));*/
    }

    private void listBoxClips_MouseDown(object sender, MouseEventArgs e)
    {
      int indexOfItem = listBoxClips.IndexFromPoint(e.X, e.Y);
      if (indexOfItem >= 0 && indexOfItem < listBoxClips.Items.Count)
      {
        fFromClips = true;
        listBoxClips.DoDragDrop(indexOfItem.ToString(), DragDropEffects.Copy);
      }
    }

    private void listBoxClips_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
    {
      fScreenOffset = SystemInformation.WorkingArea.Location;

      ListBox lb = sender as ListBox;

      if (lb != null)
      {
        Form f = lb.FindForm();
        // Cancel the drag if the mouse moves off the form. The screenOffset
        // takes into account any desktop bands that may be at the top or left
        // side of the screen.
        if (((Control.MousePosition.X - fScreenOffset.X) < f.DesktopBounds.Left) ||
          ((Control.MousePosition.X - fScreenOffset.X) > f.DesktopBounds.Right) ||
          ((Control.MousePosition.Y - fScreenOffset.Y) < f.DesktopBounds.Top) ||
          ((Control.MousePosition.Y - fScreenOffset.Y) > f.DesktopBounds.Bottom))
        {
          e.Action = DragAction.Cancel;
        }
      }
    }
    #endregion
  }
}
