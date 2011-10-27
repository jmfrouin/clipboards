using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using DragDropLib;
using Microsoft.Win32;
using ComIDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;
using DataObject = System.Windows.Forms.DataObject;
using System.Drawing.Imaging;

namespace Clipboards
{
  public partial class MainForm : Form
  {
    #region Members data
    //Used when registering/unregistering the clipboard viewer!
    private IntPtr ClipboardViewerNext;

    //Pour gérer le cas où Clipboards, lui même, push quelque chose dans le ClipBoard system.
    private bool fLocalCopy;
    private bool fCallFromHotkey;

    private string fSettingsFolder;

    //To avoid catching system content @ launch
    private bool fInitialRun;

    IntPtr fExtApp;

    //Drag & Drop stuff
    public int fIndexToDragFromClips;
    public int fIndexToDragFromFavorites;
    public int fIndexToDragFromMFU;

    public bool fFromClips;
    public bool fFromFavorites;
    public bool fFromMFU;

    //Allow shutdown
    Boolean fShutdownApp = false;

    //Hide @startup
    //private bool fHideStartup = true;
    #endregion

    #region Constructor
    public MainForm()
    {
      InitializeComponent();

      listBoxClips.SetMainForm(this);
      listBoxFavorites.SetMainForm(this);
      listBoxMFU.SetMainForm(this);

      fLocalCopy = false;
      fCallFromHotkey = false;
      fInitialRun = true;
      fExtApp = IntPtr.Zero;

      fFromClips = false;
      fFromFavorites = false;
      fFromMFU = false;
      
      //Drag & Drop
      fIndexToDragFromClips = -1;
      fIndexToDragFromFavorites = -1;
      fIndexToDragFromMFU = -1;

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

    #region Methods
    public void DisplayClip(ClipItem clip)
    {
      switch (clip.Type)
      {
        case ClipItem.EType.eImage:
          {
            splitContainerPreviewPan.Panel1Collapsed = false;
            splitContainerPreviewPan.Panel1.Show();
            splitContainerPreviewPan.Panel2Collapsed = true;
            splitContainerPreviewPan.Panel2.Hide();
            pictureBoxPreview.Image = clip.Image;
            break;
          }
        case ClipItem.EType.eText:
          {
            richTextBoxPreview.Text = clip.Content;
            splitContainerPreviewPan.Panel1Collapsed = true;
            splitContainerPreviewPan.Panel1.Hide();
            splitContainerPreviewPan.Panel2Collapsed = false;
            splitContainerPreviewPan.Panel2.Show();
            break;
          }
        default:
          break;
      }
    }

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
            bool foundedDuplicate = false;
            foreach (ClipItem clip in listBoxClips.fClips)
            {
              if (clip.Content == Item.Content)
              {
                //Add stats
                Item.Count = ++clip.Count;
                bool foundedMFU = false;
                foreach (ClipItem mfclip in listBoxMFU.fMFU)
                {
                  if (mfclip.Content == Item.Content)
                  {
                    foundedMFU = true;
                    mfclip.Count = Item.Count;
                    listBoxMFU.fMFU.Remove(mfclip);
                    listBoxMFU.fMFU.Add(mfclip);
                    break;
                  }
                }
                if (!foundedMFU)
                {
                  listBoxMFU.fMFU.Add(Item);
                  listBoxMFU.Items.Add(listBoxMFU.fMFU.Count.ToString());
                }

                //Duplicate
                if (Properties.Settings.Default.AvoidDuplicate)
                {
                  foundedDuplicate = true;
                  listBoxClips.fClips.Remove(clip);
                  listBoxClips.fClips.Add(clip);
                  break;
                }
              }
            }
            listBoxMFU.Refresh();
            //EO Duplicate
            if (!foundedDuplicate)
            {
              listBoxClips.fClips.Add(Item);
              listBoxClips.Items.Add(listBoxClips.fClips.Count.ToString());
            }
            trayIcon.ShowBalloonTip(500, Properties.Resources.ClipOfTypeText, Item.Content, ToolTipIcon.Info);
          }
          else
          {
            fLocalCopy = false;
          }
          listBoxClips.Refresh();
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
            listBoxClips.fClips.Add(Item);
            listBoxClips.Items.Add(listBoxClips.fClips.Count.ToString());
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

    public void Paste(ClipItem clip)
    {
      switch (clip.Type)
      {
        case ClipItem.EType.eText:
          {
            fLocalCopy = true;
            DataObject data = new DataObject();
            data.SetData(DataFormats.Text, clip.Content);
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
            data.SetData(DataFormats.Bitmap, clip.Image);
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

    private void OnResize(object sender, EventArgs e)
    {
      if (WindowState == FormWindowState.Minimized)
      {
        Hide();
      }
    }

    private void OnFormClosing(object sender, FormClosingEventArgs e)
    {
      if (!fShutdownApp)
      {
        Hide();
        WindowState = FormWindowState.Minimized;
        e.Cancel = true;
      }
    }

    /*protected override void SetVisibleCore(bool value)
    {
      if (!fHideStartup)
      {
        base.SetVisibleCore(value);
      }
      else
      {
        fHideStartup = false;
      }
    }*/

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
        case APIFuncs.Msgs.WM_QUERYENDSESSION:
          {
            fShutdownApp = true;
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

    #region (De)Serialize clips.
    private void SaveClips()
    {
      try
      {
        //Normal
        using (Stream stream = File.Open(fSettingsFolder + "\\Clips.bin", FileMode.Create))
        {
          BinaryFormatter bin = new BinaryFormatter();
          bin.Serialize(stream, listBoxClips.fClips);
        }
        //Favorites
        using (Stream stream = File.Open(fSettingsFolder + "\\Favorites.bin", FileMode.Create))
        {
          BinaryFormatter bin = new BinaryFormatter();
          bin.Serialize(stream, listBoxFavorites.fFavorites);
        }
        //Most Frequently Used
        using (Stream stream = File.Open(fSettingsFolder + "\\MFU.bin", FileMode.Create))
        {
          BinaryFormatter bin = new BinaryFormatter();
          bin.Serialize(stream, listBoxMFU.fMFU);
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
          listBoxClips.fClips = (List<ClipItem>)bin.Deserialize(stream);
          int Count = 0;
          listBoxClips.Items.Clear();
          foreach (ClipItem Item in listBoxClips.fClips)
          {
            listBoxClips.Items.Add((Count++).ToString());
          }
        }
        //Favorites
        using (Stream stream = File.Open(fSettingsFolder + "\\Favorites.bin", FileMode.Open))
        {
          BinaryFormatter bin = new BinaryFormatter();
          listBoxFavorites.fFavorites = (List<ClipItem>)bin.Deserialize(stream);
          int Count = 0;
          listBoxFavorites.Items.Clear();
          foreach (ClipItem Item in listBoxFavorites.fFavorites)
          {
            listBoxFavorites.Items.Add((Count++).ToString());
          }
        }
        //Most Frequently Used
        using (Stream stream = File.Open(fSettingsFolder + "\\MFU.bin", FileMode.Open))
        {
          BinaryFormatter bin = new BinaryFormatter();
          listBoxMFU.fMFU = (List<ClipItem>)bin.Deserialize(stream);
          int Count = 0;
          listBoxMFU.Items.Clear();
          foreach (ClipItem Item in listBoxMFU.fMFU)
          {
            listBoxMFU.Items.Add((Count++).ToString());
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
    private void toolStripDelete_Click(object sender, EventArgs e)
    {
      if (listBoxClips.Focused)
      {
        int Index = listBoxClips.SelectedIndex;
        if (Index != -1)
        {
          listBoxClips.fClips.RemoveAt(Index);
          listBoxClips.Items.RemoveAt(Index);
        }
      }
      if (listBoxFavorites.Focused)
      {
        int Index = listBoxFavorites.SelectedIndex;
        if (Index != -1)
        {
          listBoxFavorites.fFavorites.RemoveAt(Index);
          listBoxFavorites.Items.RemoveAt(Index);
        }
      }
      if (listBoxMFU.Focused)
      {
        int Index = listBoxMFU.SelectedIndex;
        if (Index != -1)
        {
          listBoxMFU.fMFU.RemoveAt(Index);
          listBoxMFU.Items.RemoveAt(Index);
        }
      }
    }

    private void toolStripUp_Click(object sender, EventArgs e)
    {
      if (listBoxClips.Focused)
      {
        listBoxClips.MoveUp();
      }
      if (listBoxFavorites.Focused)
      {
        listBoxFavorites.MoveUp();
      }
      if (listBoxMFU.Focused)
      {
        listBoxMFU.MoveUp();
      }
    }

    private void toolStripDown_Click(object sender, EventArgs e)
    {
      if (listBoxClips.Focused)
      {
        listBoxClips.MoveDown();
      }
      if (listBoxFavorites.Focused)
      {
        listBoxFavorites.MoveDown();
      }
      if (listBoxMFU.Focused)
      {
        listBoxMFU.MoveDown();
      }
    }

    private void toolStripButtonExpandPreviewPan_Click(object sender, EventArgs e)
    {
      Properties.Settings.Default.DisplayPreview = !Properties.Settings.Default.DisplayPreview;
      Properties.Settings.Default.Save();
      applySettingsToUI();
    }

    private void toolStripButtonAddFav_Click(object sender, EventArgs e)
    {
      if (listBoxClips.Focused)
      {
        int Index = listBoxClips.SelectedIndex;
        if (Index != -1)
        {
          ClipItem Clip = listBoxClips.fClips[Index];
          listBoxFavorites.fFavorites.Add(Clip);
          listBoxFavorites.Items.Add(listBoxFavorites.fFavorites.Count.ToString());
        }
        listBoxFavorites.Refresh();
      }
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

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      fShutdownApp = true;
      Close();
    }

    private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Show();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      fShutdownApp = true;
      Close();
    }

    private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
    {
      AboutBox aBox = new AboutBox();
      aBox.ShowDialog();
    }
    #endregion
    
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
    #endregion

    #region Drap & Drop callbacks
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
    #endregion
  }
}
