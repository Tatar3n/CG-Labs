namespace Lab2_CG
{
    partial class Form2
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel hsvPanel;
        private System.Windows.Forms.Button saveButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.loadButton = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.hsvPanel = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();

            this.loadButton.Location = new System.Drawing.Point(10, 10);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(100, 30);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Load Image";
            this.loadButton.UseVisualStyleBackColor = true;

            this.pictureBox.Location = new System.Drawing.Point(10, 50);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(200, 200);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 1;
            this.pictureBox.TabStop = false;

            this.hsvPanel.Location = new System.Drawing.Point(200, 50);
            this.hsvPanel.Name = "hsvPanel";
            this.hsvPanel.Size = new System.Drawing.Size(600, 400);
            this.hsvPanel.TabIndex = 2;

            this.saveButton.Location = new System.Drawing.Point(110, 10);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(100, 30);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Save Image";
            this.saveButton.UseVisualStyleBackColor = true;

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 800);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.hsvPanel);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.loadButton);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HSV Filter";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
        }
    }
}