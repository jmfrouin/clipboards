namespace Clipboards
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
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(310, 172);
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
            this.checkBoxAutoRun.Size = new System.Drawing.Size(149, 80);
            this.checkBoxAutoRun.TabIndex = 3;
            this.checkBoxAutoRun.Text = "AutoRun";
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
            this.checkBoxMode.Size = new System.Drawing.Size(149, 80);
            this.checkBoxMode.TabIndex = 0;
            this.checkBoxMode.Text = "Compact mode";
            this.checkBoxMode.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayPreview
            // 
            this.checkBoxDisplayPreview.AutoSize = true;
            this.checkBoxDisplayPreview.Checked = global::Clipboards.Properties.Settings.Default.DisplayPreview;
            this.checkBoxDisplayPreview.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "DisplayPreview", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxDisplayPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDisplayPreview.Location = new System.Drawing.Point(3, 89);
            this.checkBoxDisplayPreview.Name = "checkBoxDisplayPreview";
            this.checkBoxDisplayPreview.Size = new System.Drawing.Size(149, 80);
            this.checkBoxDisplayPreview.TabIndex = 2;
            this.checkBoxDisplayPreview.Text = "Display Preview\'s Pan";
            this.checkBoxDisplayPreview.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayFavorites
            // 
            this.checkBoxDisplayFavorites.AutoSize = true;
            this.checkBoxDisplayFavorites.Checked = global::Clipboards.Properties.Settings.Default.DisplayFavorites;
            this.checkBoxDisplayFavorites.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Clipboards.Properties.Settings.Default, "DisplayFavorites", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxDisplayFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDisplayFavorites.Location = new System.Drawing.Point(158, 89);
            this.checkBoxDisplayFavorites.Name = "checkBoxDisplayFavorites";
            this.checkBoxDisplayFavorites.Size = new System.Drawing.Size(149, 80);
            this.checkBoxDisplayFavorites.TabIndex = 1;
            this.checkBoxDisplayFavorites.Text = "Display Favorites\' Pan";
            this.checkBoxDisplayFavorites.UseVisualStyleBackColor = true;
            // 
            // SettingsBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 172);
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
    }
}