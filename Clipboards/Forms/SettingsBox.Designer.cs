﻿namespace Clipboards
{
    partial class SettingsBox
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
          this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
          this.checkBoxAutoRun = new System.Windows.Forms.CheckBox();
          this.checkBoxMode = new System.Windows.Forms.CheckBox();
          this.checkBoxDisplayPreview = new System.Windows.Forms.CheckBox();
          this.checkBoxDisplayFavorites = new System.Windows.Forms.CheckBox();
          this.checkBoxAvoidDuplicate = new System.Windows.Forms.CheckBox();
          this.tableLayoutPanel1.SuspendLayout();
          this.SuspendLayout();
          // 
          // tableLayoutPanel1
          // 
          this.tableLayoutPanel1.ColumnCount = 2;
          this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.Controls.Add(this.checkBoxAutoRun, 0, 0);
          this.tableLayoutPanel1.Controls.Add(this.checkBoxMode, 1, 0);
          this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplayPreview, 0, 1);
          this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplayFavorites, 1, 1);
          this.tableLayoutPanel1.Controls.Add(this.checkBoxAvoidDuplicate, 0, 2);
          this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
          this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
          this.tableLayoutPanel1.Name = "tableLayoutPanel1";
          this.tableLayoutPanel1.RowCount = 3;
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
          this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
          this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 144);
          this.tableLayoutPanel1.TabIndex = 0;
          // 
          // checkBoxAutoRun
          // 
          this.checkBoxAutoRun.AutoSize = true;
          this.checkBoxAutoRun.Checked = global::Clipboards.Properties.Settings.Default.AutoRun;
          this.checkBoxAutoRun.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxAutoRun.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "AutoRun", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.checkBoxAutoRun.Dock = System.Windows.Forms.DockStyle.Fill;
          this.checkBoxAutoRun.Location = new System.Drawing.Point(3, 3);
          this.checkBoxAutoRun.Name = "checkBoxAutoRun";
          this.checkBoxAutoRun.Size = new System.Drawing.Size(149, 42);
          this.checkBoxAutoRun.TabIndex = 3;
          this.checkBoxAutoRun.Text = "AutoRun";
          this.checkBoxAutoRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          this.checkBoxAutoRun.UseVisualStyleBackColor = true;
          // 
          // checkBoxMode
          // 
          this.checkBoxMode.AutoSize = true;
          this.checkBoxMode.Checked = global::Clipboards.Properties.Settings.Default.CompactMode;
          this.checkBoxMode.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxMode.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "CompactMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.checkBoxMode.Dock = System.Windows.Forms.DockStyle.Fill;
          this.checkBoxMode.Location = new System.Drawing.Point(158, 3);
          this.checkBoxMode.Name = "checkBoxMode";
          this.checkBoxMode.Size = new System.Drawing.Size(149, 42);
          this.checkBoxMode.TabIndex = 0;
          this.checkBoxMode.Text = "Compact mode";
          this.checkBoxMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          this.checkBoxMode.UseVisualStyleBackColor = true;
          // 
          // checkBoxDisplayPreview
          // 
          this.checkBoxDisplayPreview.AutoSize = true;
          this.checkBoxDisplayPreview.Checked = global::Clipboards.Properties.Settings.Default.DisplayPreview;
          this.checkBoxDisplayPreview.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxDisplayPreview.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "DisplayPreview", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.checkBoxDisplayPreview.Dock = System.Windows.Forms.DockStyle.Fill;
          this.checkBoxDisplayPreview.Location = new System.Drawing.Point(3, 51);
          this.checkBoxDisplayPreview.Name = "checkBoxDisplayPreview";
          this.checkBoxDisplayPreview.Size = new System.Drawing.Size(149, 42);
          this.checkBoxDisplayPreview.TabIndex = 2;
          this.checkBoxDisplayPreview.Text = "Display Preview\'s Pan";
          this.checkBoxDisplayPreview.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          this.checkBoxDisplayPreview.UseVisualStyleBackColor = true;
          // 
          // checkBoxDisplayFavorites
          // 
          this.checkBoxDisplayFavorites.AutoSize = true;
          this.checkBoxDisplayFavorites.Checked = global::Clipboards.Properties.Settings.Default.DisplayFavorites;
          this.checkBoxDisplayFavorites.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxDisplayFavorites.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "DisplayFavorites", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.checkBoxDisplayFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
          this.checkBoxDisplayFavorites.Location = new System.Drawing.Point(158, 51);
          this.checkBoxDisplayFavorites.Name = "checkBoxDisplayFavorites";
          this.checkBoxDisplayFavorites.Size = new System.Drawing.Size(149, 42);
          this.checkBoxDisplayFavorites.TabIndex = 1;
          this.checkBoxDisplayFavorites.Text = "Display Favorites\' Pan";
          this.checkBoxDisplayFavorites.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          this.checkBoxDisplayFavorites.UseVisualStyleBackColor = true;
          // 
          // checkBoxAvoidDuplicate
          // 
          this.checkBoxAvoidDuplicate.AutoSize = true;
          this.checkBoxAvoidDuplicate.Checked = global::Clipboards.Properties.Settings.Default.AvoidDuplicate;
          this.checkBoxAvoidDuplicate.CheckState = System.Windows.Forms.CheckState.Checked;
          this.checkBoxAvoidDuplicate.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "AvoidDuplicate", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.checkBoxAvoidDuplicate.Dock = System.Windows.Forms.DockStyle.Fill;
          this.checkBoxAvoidDuplicate.Location = new System.Drawing.Point(3, 99);
          this.checkBoxAvoidDuplicate.Name = "checkBoxAvoidDuplicate";
          this.checkBoxAvoidDuplicate.Size = new System.Drawing.Size(149, 42);
          this.checkBoxAvoidDuplicate.TabIndex = 4;
          this.checkBoxAvoidDuplicate.Text = "Avoid duplicate";
          this.checkBoxAvoidDuplicate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
          this.checkBoxAvoidDuplicate.UseVisualStyleBackColor = true;
          // 
          // SettingsBox
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(310, 144);
          this.Controls.Add(this.tableLayoutPanel1);
          this.Name = "SettingsBox";
          this.Text = "SettingsBox";
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
          this.tableLayoutPanel1.ResumeLayout(false);
          this.tableLayoutPanel1.PerformLayout();
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBoxMode;
        private System.Windows.Forms.CheckBox checkBoxDisplayFavorites;
        private System.Windows.Forms.CheckBox checkBoxDisplayPreview;
        private System.Windows.Forms.CheckBox checkBoxAutoRun;
        private System.Windows.Forms.CheckBox checkBoxAvoidDuplicate;
    }
}