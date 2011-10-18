using System.Windows.Forms;
using System.Drawing;

namespace Clipboards
{
  public partial class QuickPaste : Form
  {
    public QuickPaste()
    {
      InitializeComponent();

      //Position it ! 
      Screen screen = Screen.PrimaryScreen;
      int ScreenW = screen.Bounds.Width;
      int ScreenH = screen.Bounds.Height;
      int formW = Size.Width;
      int formH = Size.Height;
      int TaskbarH = Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom;
      Location = new Point(ScreenW - formW, ScreenH - formH - TaskbarH);
    }
  }
}
