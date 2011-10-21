using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace Clipboards
{
  [Serializable()]
  public class ClipItem : ISerializable
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

    private SizeF fContentSize;
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
        int S = 100;
        if (Properties.Settings.Default.CompactMode)
          S = 25;
        if (H > S || W > S)
        {
          int tW = H < W ? S : ((W * S) / H);
          int tH = H < W ? ((H * S) / W) : S;
          W = tW;
          H = tH;
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

    private SizeF fTimeStampSize1;
    private SizeF fTimeStampSize2;
    private DateTime fTimeStamp = DateTime.Now;
    #endregion

    #region Serialize
    public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
    {
      info.AddValue("Type", this.fType);
      info.AddValue("Content", this.fContent);
      info.AddValue("Image", this.fImage);
      info.AddValue("ImagePreview", this.fImagePreview);
      info.AddValue("OrigProgSmallIcon", this.fOrigProgSmallIcon);
      info.AddValue("OrigProgLargeIcon", this.fOrigProgLargeIcon);
      info.AddValue("ExePath", this.fExePath);
      info.AddValue("TimeStamp", this.fTimeStamp);
    }
    #endregion

    #region Constructors
    public ClipItem(string ep, bool fromClip = false)
    {
      fExePath = ep;
      if (!fromClip)
      {
        fOrigProgSmallIcon = ShellIcon.GetSmallIcon(ep).ToBitmap();
        fOrigProgLargeIcon = ShellIcon.GetLargeIcon(ep).ToBitmap();
      }
    }

    public ClipItem(SerializationInfo info, StreamingContext ctxt)
    {
      this.fType = (EType)info.GetValue("Type", typeof(EType));
      this.fContent = (string)info.GetValue("Content", typeof(string));
      this.fImage = (Image)info.GetValue("Image", typeof(Image));
      this.fImagePreview = (Rectangle)info.GetValue("ImagePreview", typeof(Rectangle));
      this.fOrigProgSmallIcon = (Image)info.GetValue("OrigProgSmallIcon", typeof(Image));
      this.fOrigProgLargeIcon = (Image)info.GetValue("OrigProgLargeIcon", typeof(Image));
      this.fExePath = (string)info.GetValue("ExePath", typeof(string));
      this.fTimeStamp = (DateTime)info.GetValue("TimeStamp", typeof(DateTime));
    }
    #endregion

    public void Measure(MeasureItemEventArgs e, Font font, ref int w, ref int h)
    {
      //Initial setup
      h = 0;
      fTimeStampSize1 = e.Graphics.MeasureString(fTimeStamp.ToLongDateString(), font);
      fTimeStampSize2 = e.Graphics.MeasureString(fTimeStamp.ToLongTimeString(), font);

      if (fContent != string.Empty)
        fContentSize = e.Graphics.MeasureString(fContent, font);

      if (Properties.Settings.Default.CompactMode)
      {
        h = ((int)fTimeStampSize1.Height + 4) << 1;
      }
      else
      {
        //Handle thumbnail
        if (fImage != null)
        {
          h += fImagePreview.Height + 4;
        }

        //Content
        if (fContent != string.Empty)
        {
          h += (int)fContentSize.Height + 2;
          h += (int)fContentSize.Height + 2; //For Misc infos :)
        }

        //Timestamp
        h += (int)fTimeStampSize1.Height + 2;
        h += 2;

        //Minimal size for Icon
        if (h < 16 + 4)
        {
          h = 16 + 4;
        }
      }
    }

    public void Draw(Graphics g, Rectangle bounds, DrawItemState state, Font font)
    {
      int origX = bounds.X + 2;
      int origY = bounds.Y + 2;

      //Before everything else, set Clipping region to avoid pb ! 
      Rectangle clippingRect = bounds;
      clippingRect.Inflate(1, 1);
      g.SetClip(clippingRect, CombineMode.Replace);

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

      if (Properties.Settings.Default.CompactMode)
      {
        //Draw the application icon  ! 
        if (fOrigProgSmallIcon != null)
        {
          g.DrawImage(fOrigProgSmallIcon, bounds.X + 2, bounds.Y + 2);
          origX += fOrigProgSmallIcon.Width + 2;
        }

        //Draw text I/A
        if (fContent != string.Empty)
        {
          Brush textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
          int tX = bounds.X + 20;
          Point textOrig = new Point(tX, origY);
          Rectangle textRect = new Rectangle(textOrig, Size.Round(fContentSize));
          textRect.Inflate(1, 1);
          g.DrawString(fContent, font, textBrush, textRect);
          origY += textRect.Height + 2;
        }

        //Draw thumbnail I/A
        if (fImage != null)
        {
          Rectangle imageRect = fImagePreview;
          imageRect.X = bounds.Width - fImagePreview.Width - 3;
          imageRect.Y = origY;
          g.InterpolationMode = InterpolationMode.HighQualityBicubic;
          g.DrawImage(fImage, imageRect);
          g.DrawRectangle(Pens.Black, imageRect);
        }

        //Draw the timestamp
        Brush tsBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int tsX = bounds.X + 20;
        Point tsOrig = new Point(tsX, origY);
        Rectangle tsRect = new Rectangle(tsOrig, Size.Truncate(fTimeStampSize1));
        tsRect.Width += 1;
        g.DrawString(fTimeStamp.ToLongDateString(), font, tsBrush, tsRect);
        tsX += (int)fTimeStampSize1.Width + 4;
        tsRect.X = tsX;
        g.DrawString(fTimeStamp.ToLongTimeString(), font, tsBrush, tsRect);

        //Draw size infos
        if (fImage != null)
        {
          tsRect.X = origX;
          tsRect.Y += tsRect.Height + 2;
          g.DrawString(fImage.Size.ToString(), font, tsBrush, tsRect);
        }
        if (fContent != string.Empty)
        {
          tsRect.X += tsRect.Width + 2;
          g.DrawString(fContent.Length.ToString() + "char(s).", font, tsBrush, tsRect);
        }
      }
      else
      {
        //Draw text I/A
        if (fContent != string.Empty)
        {
          Brush textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
          int tX = bounds.X + 4 + (bounds.Height < 36 ? 16 : 32);
          Point textOrig = new Point(tX, origY);
          Rectangle textRect = new Rectangle(textOrig, Size.Round(fContentSize));
          textRect.Inflate(1, 1);
          g.DrawString(fContent, font, textBrush, textRect);
          origY += textRect.Height + 2;
        }

        //Draw thumbnail I/A
        if (fImage != null)
        {
          Rectangle imageRect = fImagePreview;
          imageRect.X = bounds.Width - fImagePreview.Width - 3;
          imageRect.Y = origY;
          g.InterpolationMode = InterpolationMode.HighQualityBicubic;
          g.DrawImage(fImage, imageRect);
          g.DrawRectangle(Pens.Black, imageRect);
        }

        //Draw the timestamp
        Brush tsBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        int tsX = bounds.X + 4 + (bounds.Height < 36 ? 16 : 32);
        Point tsOrig = new Point(tsX, origY);
        Rectangle tsRect = new Rectangle(tsOrig, Size.Truncate(fTimeStampSize1));
        tsRect.Width += 1;
        g.DrawString(fTimeStamp.ToLongDateString(), font, tsBrush, tsRect);
        tsX += (int)fTimeStampSize1.Width + 4;
        if (fImage != null)
        {
          tsRect.Y += tsRect.Height;
        }
        else
        {
          tsRect.X = tsX;
        }
        g.DrawString(fTimeStamp.ToLongTimeString(), font, tsBrush, tsRect);

        //Draw size infos
        tsRect.Y += tsRect.Height + 2; ;
        if (fImage != null)
          g.DrawString(fImage.Width.ToString() + "x" + fImage.Height.ToString() + "px", font, tsBrush, tsRect);
        if (fContent != string.Empty)
          g.DrawString(fContent.Length.ToString() + "char(s).", font, tsBrush, tsRect);

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
}
