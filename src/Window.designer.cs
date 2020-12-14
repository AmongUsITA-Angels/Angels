namespace Angels
{
    partial class Window
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
            this.info = new System.Windows.Forms.Label();
            this.check = new System.Windows.Forms.Button();
            this.icon = new System.Windows.Forms.PictureBox();
            this.label = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.unzipThread = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.icon)).BeginInit();
            this.SuspendLayout();
            // 
            // info
            // 
            this.info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.info.BackColor = System.Drawing.Color.Transparent;
            this.info.ForeColor = System.Drawing.Color.White;
            this.info.Location = new System.Drawing.Point(0, 294);
            this.info.Name = "info";
            this.info.Size = new System.Drawing.Size(384, 13);
            this.info.TabIndex = 0;
            this.info.Text = "info";
            this.info.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // check
            // 
            this.check.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.check.BackColor = System.Drawing.SystemColors.Control;
            this.check.FlatAppearance.BorderSize = 0;
            this.check.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.check.Location = new System.Drawing.Point(128, 230);
            this.check.Name = "check";
            this.check.Size = new System.Drawing.Size(128, 32);
            this.check.TabIndex = 1;
            this.check.Text = "Check for updates";
            this.check.UseVisualStyleBackColor = false;
            // 
            // icon
            // 
            this.icon.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.icon.BackColor = System.Drawing.Color.Transparent;
            this.icon.Image = global::Angels.Properties.Resources.IconImage;
            this.icon.Location = new System.Drawing.Point(142, 32);
            this.icon.Name = "icon";
            this.icon.Size = new System.Drawing.Size(100, 100);
            this.icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.icon.TabIndex = 2;
            this.icon.TabStop = false;
            // 
            // label
            // 
            this.label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label.BackColor = System.Drawing.Color.Transparent;
            this.label.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.Color.White;
            this.label.Location = new System.Drawing.Point(0, 160);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(384, 39);
            this.label.TabIndex = 3;
            this.label.Text = "label";
            this.label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.BackColor = System.Drawing.SystemColors.Control;
            this.progress.ForeColor = System.Drawing.Color.Transparent;
            this.progress.Location = new System.Drawing.Point(42, 234);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(300, 23);
            this.progress.Step = 1;
            this.progress.TabIndex = 4;
            this.progress.Visible = false;
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.label);
            this.Controls.Add(this.icon);
            this.Controls.Add(this.check);
            this.Controls.Add(this.info);
            this.Controls.Add(this.progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PluginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.icon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label info;
        private System.Windows.Forms.Button check;
        private System.Windows.Forms.PictureBox icon;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.ProgressBar progress;
        private System.ComponentModel.BackgroundWorker unzipThread;
    }
}