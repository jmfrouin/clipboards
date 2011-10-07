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
            this.checkBoxMode = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayFavorites = new System.Windows.Forms.CheckBox();
            this.checkBoxDisplayPreview = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplayFavorites, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplayPreview, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 53.16456F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.83544F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 106);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // checkBoxMode
            // 
            this.checkBoxMode.AutoSize = true;
            this.checkBoxMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxMode.Location = new System.Drawing.Point(3, 3);
            this.checkBoxMode.Name = "checkBoxMode";
            this.checkBoxMode.Size = new System.Drawing.Size(192, 31);
            this.checkBoxMode.TabIndex = 0;
            this.checkBoxMode.Text = "Compact mode";
            this.checkBoxMode.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayFavorites
            // 
            this.checkBoxDisplayFavorites.AutoSize = true;
            this.checkBoxDisplayFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDisplayFavorites.Location = new System.Drawing.Point(3, 40);
            this.checkBoxDisplayFavorites.Name = "checkBoxDisplayFavorites";
            this.checkBoxDisplayFavorites.Size = new System.Drawing.Size(192, 27);
            this.checkBoxDisplayFavorites.TabIndex = 1;
            this.checkBoxDisplayFavorites.Text = "Display Favorites\' Pan";
            this.checkBoxDisplayFavorites.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayPreview
            // 
            this.checkBoxDisplayPreview.AutoSize = true;
            this.checkBoxDisplayPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDisplayPreview.Location = new System.Drawing.Point(3, 73);
            this.checkBoxDisplayPreview.Name = "checkBoxDisplayPreview";
            this.checkBoxDisplayPreview.Size = new System.Drawing.Size(192, 30);
            this.checkBoxDisplayPreview.TabIndex = 2;
            this.checkBoxDisplayPreview.Text = "Display Preview\'s Pan";
            this.checkBoxDisplayPreview.UseVisualStyleBackColor = true;
            // 
            // SettingsBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 106);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SettingsBox";
            this.Text = "SettingsBox";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox checkBoxMode;
        private System.Windows.Forms.CheckBox checkBoxDisplayFavorites;
        private System.Windows.Forms.CheckBox checkBoxDisplayPreview;
    }
}