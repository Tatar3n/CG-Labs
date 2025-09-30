namespace GradientTriangleApp
{
    partial class GradientTriangleForm
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
            this.x1Input = new System.Windows.Forms.NumericUpDown();
            this.y1Input = new System.Windows.Forms.NumericUpDown();
            this.x2Input = new System.Windows.Forms.NumericUpDown();
            this.y2Input = new System.Windows.Forms.NumericUpDown();
            this.x3Input = new System.Windows.Forms.NumericUpDown();
            this.y3Input = new System.Windows.Forms.NumericUpDown();
            this.drawButton = new System.Windows.Forms.Button();
            this.gradientButton = new System.Windows.Forms.Button();
            this.canvas = new System.Windows.Forms.PictureBox();
            this.colorInputs = new System.Windows.Forms.Panel();
            this.colorButton3 = new System.Windows.Forms.Button();
            this.colorButton2 = new System.Windows.Forms.Button();
            this.colorButton1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.x1Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y1Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x2Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y2Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.x3Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.y3Input)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.colorInputs.SuspendLayout();
            this.SuspendLayout();
            // 
            // x1Input
            // 
            this.x1Input.Location = new System.Drawing.Point(35, 12);
            this.x1Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.x1Input.Name = "x1Input";
            this.x1Input.Size = new System.Drawing.Size(50, 20);
            this.x1Input.TabIndex = 0;
            this.x1Input.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // y1Input
            // 
            this.y1Input.Location = new System.Drawing.Point(91, 12);
            this.y1Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.y1Input.Name = "y1Input";
            this.y1Input.Size = new System.Drawing.Size(50, 20);
            this.y1Input.TabIndex = 1;
            this.y1Input.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // x2Input
            // 
            this.x2Input.Location = new System.Drawing.Point(35, 38);
            this.x2Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.x2Input.Name = "x2Input";
            this.x2Input.Size = new System.Drawing.Size(50, 20);
            this.x2Input.TabIndex = 2;
            this.x2Input.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // y2Input
            // 
            this.y2Input.Location = new System.Drawing.Point(91, 38);
            this.y2Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.y2Input.Name = "y2Input";
            this.y2Input.Size = new System.Drawing.Size(50, 20);
            this.y2Input.TabIndex = 3;
            this.y2Input.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // x3Input
            // 
            this.x3Input.Location = new System.Drawing.Point(35, 64);
            this.x3Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.x3Input.Name = "x3Input";
            this.x3Input.Size = new System.Drawing.Size(50, 20);
            this.x3Input.TabIndex = 4;
            this.x3Input.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // y3Input
            // 
            this.y3Input.Location = new System.Drawing.Point(91, 64);
            this.y3Input.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.y3Input.Name = "y3Input";
            this.y3Input.Size = new System.Drawing.Size(50, 20);
            this.y3Input.TabIndex = 5;
            this.y3Input.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(12, 90);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(120, 30);
            this.drawButton.TabIndex = 6;
            this.drawButton.Text = "Draw Triangle";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.DrawTriangle);
            // 
            // gradientButton
            // 
            this.gradientButton.Location = new System.Drawing.Point(138, 90);
            this.gradientButton.Name = "gradientButton";
            this.gradientButton.Size = new System.Drawing.Size(120, 30);
            this.gradientButton.TabIndex = 7;
            this.gradientButton.Text = "Draw Gradient";
            this.gradientButton.UseVisualStyleBackColor = true;
            this.gradientButton.Click += new System.EventHandler(this.DrawGradientTriangle);
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.White;
            this.canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.canvas.Location = new System.Drawing.Point(264, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(480, 480);
            this.canvas.TabIndex = 8;
            this.canvas.TabStop = false;
            // 
            // colorInputs
            // 
            this.colorInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorInputs.Controls.Add(this.label9);
            this.colorInputs.Controls.Add(this.label8);
            this.colorInputs.Controls.Add(this.label7);
            this.colorInputs.Controls.Add(this.colorButton3);
            this.colorInputs.Controls.Add(this.colorButton2);
            this.colorInputs.Controls.Add(this.colorButton1);
            this.colorInputs.Location = new System.Drawing.Point(12, 126);
            this.colorInputs.Name = "colorInputs";
            this.colorInputs.Size = new System.Drawing.Size(246, 120);
            this.colorInputs.TabIndex = 9;
            this.colorInputs.Visible = false;
            // 
            // colorButton3
            // 
            this.colorButton3.BackColor = System.Drawing.Color.Blue;
            this.colorButton3.Location = new System.Drawing.Point(70, 80);
            this.colorButton3.Name = "colorButton3";
            this.colorButton3.Size = new System.Drawing.Size(80, 25);
            this.colorButton3.TabIndex = 2;
            this.colorButton3.UseVisualStyleBackColor = false;
            this.colorButton3.Click += new System.EventHandler(this.ColorButton3_Click);
            // 
            // colorButton2
            // 
            this.colorButton2.BackColor = System.Drawing.Color.Green;
            this.colorButton2.Location = new System.Drawing.Point(70, 45);
            this.colorButton2.Name = "colorButton2";
            this.colorButton2.Size = new System.Drawing.Size(80, 25);
            this.colorButton2.TabIndex = 1;
            this.colorButton2.UseVisualStyleBackColor = false;
            this.colorButton2.Click += new System.EventHandler(this.ColorButton2_Click);
            // 
            // colorButton1
            // 
            this.colorButton1.BackColor = System.Drawing.Color.Red;
            this.colorButton1.Location = new System.Drawing.Point(70, 10);
            this.colorButton1.Name = "colorButton1";
            this.colorButton1.Size = new System.Drawing.Size(80, 25);
            this.colorButton1.TabIndex = 0;
            this.colorButton1.UseVisualStyleBackColor = false;
            this.colorButton1.Click += new System.EventHandler(this.ColorButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "V1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "V2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "V3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(147, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "X";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(147, 40);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "X";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(147, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "X";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(156, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(156, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Y";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(156, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Y";
            // 
            // GradientTriangleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 504);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.colorInputs);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.gradientButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.y3Input);
            this.Controls.Add(this.x3Input);
            this.Controls.Add(this.y2Input);
            this.Controls.Add(this.x2Input);
            this.Controls.Add(this.y1Input);
            this.Controls.Add(this.x1Input);
            this.Name = "GradientTriangleForm";
            this.Text = "Gradient Triangle";
            ((System.ComponentModel.ISupportInitialize)(this.x1Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y1Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x2Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y2Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.x3Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.y3Input)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.colorInputs.ResumeLayout(false);
            this.colorInputs.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown x1Input;
        private System.Windows.Forms.NumericUpDown y1Input;
        private System.Windows.Forms.NumericUpDown x2Input;
        private System.Windows.Forms.NumericUpDown y2Input;
        private System.Windows.Forms.NumericUpDown x3Input;
        private System.Windows.Forms.NumericUpDown y3Input;
        private System.Windows.Forms.Button drawButton;
        private System.Windows.Forms.Button gradientButton;
        private System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Panel colorInputs;
        private System.Windows.Forms.Button colorButton1;
        private System.Windows.Forms.Button colorButton3;
        private System.Windows.Forms.Button colorButton2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.ColorDialog colorDialog3;
    }
}