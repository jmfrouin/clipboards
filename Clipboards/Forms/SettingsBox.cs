using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Clipboards
{
    public partial class SettingsBox : Form
    {
        public SettingsBox()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();

            //AutoRun
            // The path to the key where Windows looks for startup applications
            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (Properties.Settings.Default.AutoRun)
            {
                // Add the value in the registry so that the application runs at startup
                rkApp.SetValue("Clipboards", Application.ExecutablePath.ToString());
            }
            else 
            {
                // Remove the value from the registry so that the application doesn't start
                rkApp.DeleteValue("Clipboards", false);
            }
        }
    }
}
