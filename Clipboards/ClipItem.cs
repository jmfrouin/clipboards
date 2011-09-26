using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Clipboards
{
    [Serializable()]
    public class ClipItem
    {
        #region Enumeration, types, accessors & mutators.        
        public enum EType
        {
            eAudio,
            eCustom,
            eDrop,
            eHTML,
            eImage,
            eNone,
            eRaw,
            eRTF,
            eText
        }

        private EType fType = EType.eNone;
        public EType Type
        {
            get { return fType; }
            set { fType = value; }
        }

        private string fContent = string.Empty;
        public string Content
        {
            get { return fContent; }
            set { fContent = value; }
        }

        private Rectangle fImagePreview;
        private Image fImage = null;
        public Image Image
        {
            get { return fImage; }
            set 
            { 
                fImage = value;
                int H = fImage.Height;
                int W = fImage.Width;
                if( H > 100 || W > 100)
                {
                    W = H < W ? 100 : ((W * 100) / H);
                    H = H < W ? ((H * 100) / W) : 100;
                }
                fImagePreview = new Rectangle(0, 0, W, H);
            }
        }
        
        private Image fOrigProgSmallIcon = null;
        private Image fOrigProgLargeIcon = null;

        private string fExePath = null;
        public string ExePath
        {
            get { return fExePath; }
            set { fExePath = value; }
        }

        public DateTime fTime = DateTime.Now;
        #endregion
        
        #region Constructors
        public ClipItem(string ep)
        {
            fExePath = ep;
            fOrigProgSmallIcon = ShellIcon.GetSmallIcon(ep).ToBitmap();
            fOrigProgLargeIcon = ShellIcon.GetLargeIcon(ep).ToBitmap();
        }
        #endregion

        public void Measure(MeasureItemEventArgs e, Font font, ref int w, ref int h)
        {
            Rectangle imageRect;
            //Handle thumbnail
            if (fImage != null)
            {
                imageRect = fImagePreview;
                //w = imageRect.Width;
                h = imageRect.Height + 4;
            }
            
            if (fContent != string.Empty)
            {
                SizeF size = e.Graphics.MeasureString(fContent, font);
                h = (int)size.Height + 4;
            }
            
            //Minimal size for Icon
            if (h < 16 + 4)
            {
                h = 16 + 4;
            }
        }

        public void Draw(Graphics g, Rectangle bounds, DrawItemState state, Font font)
        {
            //Draw border if item is selected only! 
            Rectangle borderRect = bounds;
            borderRect.Width = bounds.Width - 1;
                
            System.Drawing.SolidBrush myBrush;
            if ((state & DrawItemState.Selected) == DrawItemState.Selected)
            {
                Color myColor = Color.FromArgb(0xFF, 0xCC, 0xCC, 0xFF);
                myBrush = new System.Drawing.SolidBrush(myColor);
            }
            else
            {
                myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            }
            g.FillRectangle(myBrush, borderRect);
            g.DrawRectangle(Pens.Black, borderRect);
            
            //Draw text I/A
            if (fContent != string.Empty)
            {
                Brush textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                Rectangle textRect = bounds;
                textRect.X = bounds.X + 4 + (bounds.Height < 36 ? 16 : 32);
                textRect.Y = bounds.Y + 2;
                g.DrawString(fContent, font, textBrush, textRect);
            }

            //Draw thumbnail I/A
            if (fImage != null)
            {
                Rectangle imageRect = fImagePreview;
                imageRect.X = bounds.Width - fImagePreview.Width - 3;
                imageRect.Y = bounds.Y + 2;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(fImage, imageRect);
                g.DrawRectangle(Pens.Black, imageRect);
            }

            //Draw the application icon  ! 
            if (fOrigProgLargeIcon != null && bounds.Height > 36)
            {
                int X = bounds.Width - 2 - fOrigProgLargeIcon.Width;
                g.DrawImage(fOrigProgLargeIcon, bounds.X + 2, bounds.Y + 2);
            }
            else 
            {
                if (fOrigProgSmallIcon != null)
                {
                    int X = bounds.Width - 2 - fOrigProgSmallIcon.Width;
                    g.DrawImage(fOrigProgSmallIcon, bounds.X + 2, bounds.Y + 2);
                }
            }
        }
    }
}
