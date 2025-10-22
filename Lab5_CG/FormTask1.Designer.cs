namespace CG_Lab
{
    partial class FormTask1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnPrevIteration = new System.Windows.Forms.Button();
            this.btnNextIteration = new System.Windows.Forms.Button();
            this.lblIterationInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Кривая Коха",
            "Снежинка Коха",
            "Остров Коха",
            "Ковер Серпинского",
            "Наконечник Серпинского",
            "Кривая Гильберта",
            "Кривая дракона",
            "Кривая Госпера",
            "Простое дерево",
            "Дерево со случайностью"});
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(250, 28);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnPrevIteration
            // 
            this.btnPrevIteration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnPrevIteration.Location = new System.Drawing.Point(12, 50);
            this.btnPrevIteration.Name = "btnPrevIteration";
            this.btnPrevIteration.Size = new System.Drawing.Size(120, 35);
            this.btnPrevIteration.TabIndex = 1;
            this.btnPrevIteration.Text = "< Пред.";
            this.btnPrevIteration.UseVisualStyleBackColor = true;
            this.btnPrevIteration.Click += new System.EventHandler(this.btnPrevIteration_Click);
            // 
            // btnNextIteration
            // 
            this.btnNextIteration.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnNextIteration.Location = new System.Drawing.Point(142, 50);
            this.btnNextIteration.Name = "btnNextIteration";
            this.btnNextIteration.Size = new System.Drawing.Size(120, 35);
            this.btnNextIteration.TabIndex = 2;
            this.btnNextIteration.Text = "След. >";
            this.btnNextIteration.UseVisualStyleBackColor = true;
            this.btnNextIteration.Click += new System.EventHandler(this.btnNextIteration_Click);
            // 
            // lblIterationInfo
            // 
            this.lblIterationInfo.AutoSize = true;
            this.lblIterationInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblIterationInfo.Location = new System.Drawing.Point(12, 90);
            this.lblIterationInfo.Name = "lblIterationInfo";
            this.lblIterationInfo.Size = new System.Drawing.Size(98, 17);
            this.lblIterationInfo.TabIndex = 3;
            this.lblIterationInfo.Text = "Итерация: 0/5";
            // 
            // FormTask1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.lblIterationInfo);
            this.Controls.Add(this.btnNextIteration);
            this.Controls.Add(this.btnPrevIteration);
            this.Controls.Add(this.comboBox1);
            this.Name = "FormTask1";
            this.Text = "L-Системы - Фракталы";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button btnPrevIteration;
        private System.Windows.Forms.Button btnNextIteration;
        private System.Windows.Forms.Label lblIterationInfo;
    }
}