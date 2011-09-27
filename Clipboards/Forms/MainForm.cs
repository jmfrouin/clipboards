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

        //List of Clips
        List<ClipItem> fClips = new List<ClipItem>();
        #endregion

        public MainForm()
        {
            InitializeComponent();

            //Callbacks
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Closed += new System.EventHandler(this.MainForm_Closed);
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Deactivate += new System.EventHandler(this.MainForm_Deactivate);

            //Settings
            Screen screen = Screen.PrimaryScreen;
            int ScreenW = screen.Bounds.Width;
            int ScreenH = screen.Bounds.Height;
            int AppW = this.Size.Width;
            int AppH = this.Size.Height;
            int TaskbarH = Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom;
            this.Location = new Point(ScreenW - AppW, ScreenH - AppH - TaskbarH);

            RegisterClipboardViewer();
        }

        #region Callbacks !
        private void MainForm_Activated(object sender, System.EventArgs e)
        {
            this.Opacity = 1;
        }

        private void MainForm_Deactivate(object sender, System.EventArgs e)
        {
            this.Opacity = 0.3;
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            RestoreClips();
            //Register CTRL+SHIFT+D
            APIFuncs.RegisterHotKey(this.Handle, this.GetType().GetHashCode(), 2, (int)'D');  
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
                    fClips.Add(Item);

                    listBoxClips.Items.Add(fClips.Count.ToString());
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
                    fClips.Add(Item);

                    listBoxClips.Items.Add(fClips.Count.ToString());
                }
            }
        }

        #region Customized listbox ! 
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
                    InsertClip();
                    this.Activate();
                    break;
                }
                case Clipboards.APIFuncs.Msgs.WM_DRAWCLIPBOARD:
                {
                    InsertClip();
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

        #region toolStrip callbacks
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aBox = new AboutBox();
            aBox.Show();
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
        #endregion

        #region Save / Restore Clips
        private void SaveClips()
        {
            try
            {
                using (Stream stream = File.Open("Clips.bin", FileMode.Create))
                {
                    BinaryFormatter bin = new BinaryFormatter();
                    bin.Serialize(stream, fClips);
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
                using (Stream stream = File.Open("Clips.bin", FileMode.Open))
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
            }
            catch (IOException e)
            {
                Console.WriteLine("!!! Exception caught here" + e.ToString());
            }
        }
        #endregion
        
    }
}
