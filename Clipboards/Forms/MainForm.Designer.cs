﻿using Clipboards.Components;
namespace Clipboards
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      this.statusBar = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.mainMenu = new System.Windows.Forms.ToolStrip();
      this.toolStripButtonExpandPreviewPan = new System.Windows.Forms.ToolStripButton();
      this.toolStripFavorites = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripPaste = new System.Windows.Forms.ToolStripButton();
      this.toolStripDelete = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripButtonAddFav = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripUp = new System.Windows.Forms.ToolStripButton();
      this.toolStripDown = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      this.MainPanel = new System.Windows.Forms.Panel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.splitContainerPreviewPan = new System.Windows.Forms.SplitContainer();
      this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
      this.richTextBoxPreview = new System.Windows.Forms.RichTextBox();
      this.splitContainer3 = new System.Windows.Forms.SplitContainer();
      this.listBoxMFU = new Clipboards.Components.MFUListBox();
      this.listBoxFavorites = new Clipboards.Components.FavoritesListBox();
      this.listBoxClips = new Clipboards.Components.ClipsListBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
      this.contextMenuTray = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
      this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.statusBar.SuspendLayout();
      this.mainMenu.SuspendLayout();
      this.MainPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
      this.splitContainer2.Panel1.SuspendLayout();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerPreviewPan)).BeginInit();
      this.splitContainerPreviewPan.Panel1.SuspendLayout();
      this.splitContainerPreviewPan.Panel2.SuspendLayout();
      this.splitContainerPreviewPan.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
      this.splitContainer3.Panel1.SuspendLayout();
      this.splitContainer3.Panel2.SuspendLayout();
      this.splitContainer3.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.contextMenuTray.SuspendLayout();
      this.SuspendLayout();
      // 
      // statusBar
      // 
      this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusBar.Location = new System.Drawing.Point(0, 313);
      this.statusBar.Name = "statusBar";
      this.statusBar.Size = new System.Drawing.Size(918, 22);
      this.statusBar.SizingGrip = false;
      this.statusBar.TabIndex = 0;
      this.statusBar.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
      this.toolStripStatusLabel1.Text = "Status";
      // 
      // mainMenu
      // 
      this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonExpandPreviewPan,
            this.toolStripFavorites,
            this.toolStripSeparator4,
            this.toolStripPaste,
            this.toolStripDelete,
            this.toolStripSeparator2,
            this.toolStripButtonAddFav,
            this.toolStripSeparator1,
            this.toolStripUp,
            this.toolStripDown,
            this.toolStripSeparator5});
      this.mainMenu.Location = new System.Drawing.Point(0, 24);
      this.mainMenu.Name = "mainMenu";
      this.mainMenu.Size = new System.Drawing.Size(918, 25);
      this.mainMenu.TabIndex = 1;
      this.mainMenu.Text = "toolStrip1";
      // 
      // toolStripButtonExpandPreviewPan
      // 
      this.toolStripButtonExpandPreviewPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonExpandPreviewPan.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExpandPreviewPan.Image")));
      this.toolStripButtonExpandPreviewPan.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonExpandPreviewPan.Name = "toolStripButtonExpandPreviewPan";
      this.toolStripButtonExpandPreviewPan.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonExpandPreviewPan.Click += new System.EventHandler(this.toolStripButtonExpandPreviewPan_Click);
      // 
      // toolStripFavorites
      // 
      this.toolStripFavorites.Image = ((System.Drawing.Image)(resources.GetObject("toolStripFavorites.Image")));
      this.toolStripFavorites.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripFavorites.Name = "toolStripFavorites";
      this.toolStripFavorites.Size = new System.Drawing.Size(23, 22);
      this.toolStripFavorites.Click += new System.EventHandler(this.toolStripFavorites_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripPaste
      // 
      this.toolStripPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolStripPaste.Image")));
      this.toolStripPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripPaste.Name = "toolStripPaste";
      this.toolStripPaste.Size = new System.Drawing.Size(55, 22);
      this.toolStripPaste.Text = "Paste";
      // 
      // toolStripDelete
      // 
      this.toolStripDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDelete.Image")));
      this.toolStripDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripDelete.Name = "toolStripDelete";
      this.toolStripDelete.Size = new System.Drawing.Size(60, 22);
      this.toolStripDelete.Text = "Delete";
      this.toolStripDelete.Click += new System.EventHandler(this.toolStripDelete_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripButtonAddFav
      // 
      this.toolStripButtonAddFav.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripButtonAddFav.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAddFav.Image")));
      this.toolStripButtonAddFav.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripButtonAddFav.Name = "toolStripButtonAddFav";
      this.toolStripButtonAddFav.Size = new System.Drawing.Size(23, 22);
      this.toolStripButtonAddFav.Click += new System.EventHandler(this.toolStripButtonAddFav_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripUp
      // 
      this.toolStripUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripUp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripUp.Image")));
      this.toolStripUp.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripUp.Name = "toolStripUp";
      this.toolStripUp.Size = new System.Drawing.Size(23, 22);
      this.toolStripUp.Click += new System.EventHandler(this.toolStripUp_Click);
      // 
      // toolStripDown
      // 
      this.toolStripDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripDown.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDown.Image")));
      this.toolStripDown.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripDown.Name = "toolStripDown";
      this.toolStripDown.Size = new System.Drawing.Size(23, 22);
      this.toolStripDown.Text = "toolStripButton2";
      this.toolStripDown.Click += new System.EventHandler(this.toolStripDown_Click);
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
      // 
      // MainPanel
      // 
      this.MainPanel.BackColor = System.Drawing.Color.Transparent;
      this.MainPanel.Controls.Add(this.splitContainer1);
      this.MainPanel.Controls.Add(this.mainMenu);
      this.MainPanel.Controls.Add(this.menuStrip1);
      this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.MainPanel.Location = new System.Drawing.Point(0, 0);
      this.MainPanel.Name = "MainPanel";
      this.MainPanel.Size = new System.Drawing.Size(918, 313);
      this.MainPanel.TabIndex = 0;
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new System.Drawing.Point(0, 49);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.listBoxClips);
      this.splitContainer1.Size = new System.Drawing.Size(918, 264);
      this.splitContainer1.SplitterDistance = 675;
      this.splitContainer1.TabIndex = 3;
      this.splitContainer1.TabStop = false;
      // 
      // splitContainer2
      // 
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.IsSplitterFixed = true;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Name = "splitContainer2";
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.Controls.Add(this.splitContainerPreviewPan);
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
      this.splitContainer2.Size = new System.Drawing.Size(675, 264);
      this.splitContainer2.SplitterDistance = 218;
      this.splitContainer2.TabIndex = 0;
      this.splitContainer2.TabStop = false;
      // 
      // splitContainerPreviewPan
      // 
      this.splitContainerPreviewPan.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainerPreviewPan.Location = new System.Drawing.Point(0, 0);
      this.splitContainerPreviewPan.Name = "splitContainerPreviewPan";
      // 
      // splitContainerPreviewPan.Panel1
      // 
      this.splitContainerPreviewPan.Panel1.Controls.Add(this.pictureBoxPreview);
      // 
      // splitContainerPreviewPan.Panel2
      // 
      this.splitContainerPreviewPan.Panel2.Controls.Add(this.richTextBoxPreview);
      this.splitContainerPreviewPan.Size = new System.Drawing.Size(218, 264);
      this.splitContainerPreviewPan.SplitterDistance = 126;
      this.splitContainerPreviewPan.TabIndex = 0;
      this.splitContainerPreviewPan.TabStop = false;
      // 
      // pictureBoxPreview
      // 
      this.pictureBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBoxPreview.Location = new System.Drawing.Point(0, 0);
      this.pictureBoxPreview.Name = "pictureBoxPreview";
      this.pictureBoxPreview.Size = new System.Drawing.Size(126, 264);
      this.pictureBoxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.pictureBoxPreview.TabIndex = 0;
      this.pictureBoxPreview.TabStop = false;
      // 
      // richTextBoxPreview
      // 
      this.richTextBoxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
      this.richTextBoxPreview.Location = new System.Drawing.Point(0, 0);
      this.richTextBoxPreview.Name = "richTextBoxPreview";
      this.richTextBoxPreview.Size = new System.Drawing.Size(88, 264);
      this.richTextBoxPreview.TabIndex = 0;
      this.richTextBoxPreview.Text = "";
      // 
      // splitContainer3
      // 
      this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer3.Location = new System.Drawing.Point(0, 0);
      this.splitContainer3.Name = "splitContainer3";
      // 
      // splitContainer3.Panel1
      // 
      this.splitContainer3.Panel1.Controls.Add(this.listBoxMFU);
      // 
      // splitContainer3.Panel2
      // 
      this.splitContainer3.Panel2.Controls.Add(this.listBoxFavorites);
      this.splitContainer3.Size = new System.Drawing.Size(453, 264);
      this.splitContainer3.SplitterDistance = 207;
      this.splitContainer3.TabIndex = 1;
      // 
      // listBoxMFU
      // 
      this.listBoxMFU.AllowDrop = true;
      this.listBoxMFU.BackColor = System.Drawing.Color.White;
      this.listBoxMFU.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBoxMFU.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.listBoxMFU.FormattingEnabled = true;
      this.listBoxMFU.Location = new System.Drawing.Point(0, 0);
      this.listBoxMFU.Name = "listBoxMFU";
      this.listBoxMFU.Size = new System.Drawing.Size(207, 264);
      this.listBoxMFU.TabIndex = 0;
      // 
      // listBoxFavorites
      // 
      this.listBoxFavorites.AllowDrop = true;
      this.listBoxFavorites.BackColor = System.Drawing.Color.White;
      this.listBoxFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBoxFavorites.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.listBoxFavorites.FormattingEnabled = true;
      this.listBoxFavorites.Location = new System.Drawing.Point(0, 0);
      this.listBoxFavorites.Name = "listBoxFavorites";
      this.listBoxFavorites.Size = new System.Drawing.Size(242, 264);
      this.listBoxFavorites.TabIndex = 0;
      // 
      // listBoxClips
      // 
      this.listBoxClips.AllowDrop = true;
      this.listBoxClips.BackColor = System.Drawing.Color.White;
      this.listBoxClips.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listBoxClips.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
      this.listBoxClips.FormattingEnabled = true;
      this.listBoxClips.Location = new System.Drawing.Point(0, 0);
      this.listBoxClips.Name = "listBoxClips";
      this.listBoxClips.Size = new System.Drawing.Size(239, 264);
      this.listBoxClips.TabIndex = 0;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(918, 24);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripSeparator6,
            this.aboutToolStripMenuItem,
            this.toolStripSeparator3,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
      this.fileToolStripMenuItem.Text = "File";
      // 
      // settingsToolStripMenuItem
      // 
      this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
      this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.settingsToolStripMenuItem.Text = "Settings";
      this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
      // 
      // toolStripSeparator6
      // 
      this.toolStripSeparator6.Name = "toolStripSeparator6";
      this.toolStripSeparator6.Size = new System.Drawing.Size(113, 6);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.aboutToolStripMenuItem.Text = "About";
      this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(113, 6);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // trayIcon
      // 
      this.trayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
      this.trayIcon.BalloonTipText = "Enhanced Clipboard";
      this.trayIcon.BalloonTipTitle = "Clipboards";
      this.trayIcon.ContextMenuStrip = this.contextMenuTray;
      this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
      this.trayIcon.Text = "Clipboards";
      this.trayIcon.Visible = true;
      this.trayIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_MouseClick);
      this.trayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trayIcon_DoubleClick);
      // 
      // contextMenuTray
      // 
      this.contextMenuTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restoreToolStripMenuItem,
            this.aboutToolStripMenuItem1,
            this.toolStripSeparator7,
            this.quitToolStripMenuItem});
      this.contextMenuTray.Name = "contextMenuTray";
      this.contextMenuTray.Size = new System.Drawing.Size(114, 76);
      // 
      // restoreToolStripMenuItem
      // 
      this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
      this.restoreToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
      this.restoreToolStripMenuItem.Text = "Restore";
      this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
      // 
      // aboutToolStripMenuItem1
      // 
      this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
      this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
      this.aboutToolStripMenuItem1.Text = "About";
      this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
      // 
      // toolStripSeparator7
      // 
      this.toolStripSeparator7.Name = "toolStripSeparator7";
      this.toolStripSeparator7.Size = new System.Drawing.Size(110, 6);
      // 
      // quitToolStripMenuItem
      // 
      this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
      this.quitToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
      this.quitToolStripMenuItem.Text = "Quit";
      this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(918, 335);
      this.Controls.Add(this.MainPanel);
      this.Controls.Add(this.statusBar);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "MainForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Clipboards";
      this.TopMost = true;
      this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.OnDragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.OnDragEnter);
      this.DragOver += new System.Windows.Forms.DragEventHandler(this.OnDragOver);
      this.DragLeave += new System.EventHandler(this.OnDragLeave);
      this.Resize += new System.EventHandler(this.OnResize);
      this.statusBar.ResumeLayout(false);
      this.statusBar.PerformLayout();
      this.mainMenu.ResumeLayout(false);
      this.mainMenu.PerformLayout();
      this.MainPanel.ResumeLayout(false);
      this.MainPanel.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
      this.splitContainer1.ResumeLayout(false);
      this.splitContainer2.Panel1.ResumeLayout(false);
      this.splitContainer2.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
      this.splitContainer2.ResumeLayout(false);
      this.splitContainerPreviewPan.Panel1.ResumeLayout(false);
      this.splitContainerPreviewPan.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainerPreviewPan)).EndInit();
      this.splitContainerPreviewPan.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
      this.splitContainer3.Panel1.ResumeLayout(false);
      this.splitContainer3.Panel2.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
      this.splitContainer3.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.contextMenuTray.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusBar;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStrip mainMenu;
    private System.Windows.Forms.ToolStripButton toolStripPaste;
    private System.Windows.Forms.ToolStripButton toolStripFavorites;
    private System.Windows.Forms.ToolStripButton toolStripDelete;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripUp;
    private System.Windows.Forms.ToolStripButton toolStripDown;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.Panel MainPanel;
    private System.Windows.Forms.NotifyIcon trayIcon;
    public ClipsListBox listBoxClips;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    public FavoritesListBox listBoxFavorites;
    private System.Windows.Forms.ToolStripButton toolStripButtonExpandPreviewPan;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.SplitContainer splitContainerPreviewPan;
    private System.Windows.Forms.PictureBox pictureBoxPreview;
    private System.Windows.Forms.ToolStripButton toolStripButtonAddFav;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.RichTextBox richTextBoxPreview;
    private System.Windows.Forms.ContextMenuStrip contextMenuTray;
    private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer3;
    public MFUListBox listBoxMFU;
  }
}

