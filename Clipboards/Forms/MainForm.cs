using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

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
        #endregion

        public MainForm()
        {
            InitializeComponent();
            fLocalCopy = false;
            fCallFromHotkey = false;

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
        }

        #region Callbacks !
        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            //this.Opacity = 1;
            this.Show();
        }

        private void MainForm_Deactivate(object sender, System.EventArgs e)
        {
            //this.Opacity = 0.3;
            this.Hide();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            RestoreClips();
            //Register CTRL+SHIFT+D
            //http://msdn.microsoft.com/en-us/library/windows/desktop/ms646279(v=vs.85).aspx
            APIFuncs.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 0x2, (int)'E');
        }

        private void MainForm_Closed(object sender, System.EventArgs e)
        {
            APIFuncs.UnregisterHotKey(this.Handle, this.GetType().GetHashCode());
            UnregisterClipboardViewer();
            SaveClips();
        }
        #endregion

        private void InsertClip()
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData != null)
            {
                //Handle TXT clipboard
                if (iData.GetDataPresent(DataFormats.Text))
                {
                    IntPtr hwnd = APIFuncs.getforegroundWindow();
                    Int32 pid = APIFuncs.GetWindowProcessID(hwnd);
                    Process p = Process.GetProcessById(pid);

                    ClipItem Item = new ClipItem(p.MainModule.FileName);
                    Item.Content = iData.GetData(DataFormats.StringFormat).ToString();
                    Item.Type = ClipItem.EType.eText;
                    if (fLocalCopy == false)
                    {
                        fClips.Add(Item);
                        listBoxClips.Items.Add(fClips.Count.ToString());
                    }
                    else 
                    {
                        fLocalCopy = false;
                    }
                }

                //Handle Bitmap element
                if (iData.GetDataPresent(DataFormats.Bitmap))
                {
                    IntPtr hwnd = APIFuncs.getforegroundWindow();
                    Int32 pid = APIFuncs.GetWindowProcessID(hwnd);
                    Process p = Process.GetProcessById(pid);

                    ClipItem Item = new ClipItem(p.MainModule.FileName);
                    Item.Image = (Bitmap)iData.GetData(DataFormats.Bitmap);
                    Item.Type = ClipItem.EType.eImage;
                    if (fLocalCopy == false)
                    {
                        fClips.Add(Item);
                        listBoxClips.Items.Add(fClips.Count.ToString());
                    }
                    else
                    {
                        fLocalCopy = false;
                    }
                }
            }
        }

        #region Customized listboxes ! 
        private void MeasureItem(object sender, MeasureItemEventArgs e)
        {
            ClipItem Clip = fClips[e.Index];
            int W = e.ItemWidth, H = e.ItemHeight;
            Clip.Measure(e, listBoxClips.Font, ref W, ref H);
            e.ItemWidth = W;
            e.ItemHeight = H;
        }

        private void DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1 && e.Index < fClips.Count)
            {
                Graphics g = e.Graphics;
                ClipItem Clip = fClips[e.Index];
                Clip.Draw(g, e.Bounds, e.State, e.Font);
                g.Dispose();
            }
        }

        private void FavMeasureItem(object sender, MeasureItemEventArgs e)
        {
            ClipItem Clip = fFavorites[e.Index];
            int W = e.ItemWidth, H = e.ItemHeight;
            Clip.Measure(e, listBoxFavorites.Font, ref W, ref H);
            e.ItemWidth = W;
            e.ItemHeight = H;
        }

        private void FavDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1 && e.Index < fClips.Count)
            {
                Graphics g = e.Graphics;
                ClipItem Clip = fFavorites[e.Index];
                Clip.Draw(g, e.Bounds, e.State, e.Font);
                g.Dispose();
            }
        }

        private void listBoxFavorites_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c == 13)
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
                                this.Hide();
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
                                this.Hide();
                                SendKeys.SendWait("^v");
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void listBoxFavorites_DoubleClick(object sender, EventArgs e)
        {

        }

        private void listBoxClips_KeyPress(object sender, KeyPressEventArgs e)
        {
            char c = e.KeyChar;
            if (c == 13)
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
                                this.Hide();
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
                                this.Hide();
                                SendKeys.SendWait("^v");
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void listBoxClips_DoubleClick(object sender, EventArgs e)
        {

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

        protected override void WndProc(ref Message m)
        {
            switch ((Clipboards.APIFuncs.Msgs)m.Msg)
            {
                case Clipboards.APIFuncs.Msgs.WM_HOTKEY:
                {
                    this.Show();
                    this.Activate();
                    fCallFromHotkey = true;
                    break;
                }
                case Clipboards.APIFuncs.Msgs.WM_DRAWCLIPBOARD:
                {
                    InsertClip();
                    if (ClipboardViewerNext != System.IntPtr.Zero)
                        APIFuncs.SendMessage(ClipboardViewerNext, m.Msg, m.WParam, m.LParam);
                    break;
                }
                case Clipboards.APIFuncs.Msgs.WM_CHANGECBCHAIN:
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
            int Index = listBoxClips.SelectedIndex;
            if (Index != -1)
            {
                fClips.RemoveAt(Index);
                listBoxClips.Items.RemoveAt(Index);
            }
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripUp_Click(object sender, EventArgs e)
        {
            int Index = listBoxClips.SelectedIndex;
            if (Index > 0)
            {
                ClipItem Clip = fClips[Index];
                fClips.RemoveAt(Index);
                fClips.Insert(Index - 1, Clip);
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
            }
            listBoxClips.Refresh();
        }

        private void toolStripButtonExpandPreviewPan_Click(object sender, EventArgs e)
        {
            if (splitContainer2.Panel1Collapsed)
            {
                splitContainer2.Panel1Collapsed = false;
                splitContainer2.Panel1.Show(); 
            }
            else 
            {
                splitContainer2.Panel1Collapsed = true;
                splitContainer2.Panel1.Hide(); 
            }
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
            if (splitContainer2.Panel2Collapsed)
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
        #endregion

        private void trayIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (Visible) this.Hide();
            else this.Show();
        }
    }
}
