using System.Windows.Forms;

namespace Lab2_CG
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Элементы управления для изображений
        private PictureBox pictureBoxOriginal;
        private PictureBox pictureBoxGray1;
        private PictureBox pictureBoxGray2;
        private PictureBox pictureBoxDiff;

        // Заголовки
        private Label lblTitleOriginal;
        private Label lblTitleGray1;
        private Label lblTitleGray2;
        private Label lblTitleDiff;

        // Информационные метки
        private Label lblOriginalInfo;
        private Label lblGray1Info;
        private Label lblGray2Info;
        private Label lblDiffInfo;

        // Гистограммы
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMethod1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMethod2;

        // Кнопки
        private Button btnLoadImage;

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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.pictureBoxGray1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxGray2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxDiff = new System.Windows.Forms.PictureBox();
            this.lblTitleOriginal = new System.Windows.Forms.Label();
            this.lblTitleGray1 = new System.Windows.Forms.Label();
            this.lblTitleGray2 = new System.Windows.Forms.Label();
            this.lblTitleDiff = new System.Windows.Forms.Label();
            this.lblOriginalInfo = new System.Windows.Forms.Label();
            this.lblGray1Info = new System.Windows.Forms.Label();
            this.lblGray2Info = new System.Windows.Forms.Label();
            this.lblDiffInfo = new System.Windows.Forms.Label();
            this.chartMethod1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMethod2 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnLoadImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGray1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGray2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMethod1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMethod2)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(12, 80);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(250, 200);
            this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginal.TabIndex = 0;
            this.pictureBoxOriginal.TabStop = false;

            // 
            // pictureBoxGray1
            // 
            this.pictureBoxGray1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxGray1.Location = new System.Drawing.Point(268, 80);
            this.pictureBoxGray1.Name = "pictureBoxGray1";
            this.pictureBoxGray1.Size = new System.Drawing.Size(250, 200);
            this.pictureBoxGray1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGray1.TabIndex = 1;
            this.pictureBoxGray1.TabStop = false;

            // 
            // pictureBoxGray2
            // 
            this.pictureBoxGray2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxGray2.Location = new System.Drawing.Point(12, 320);
            this.pictureBoxGray2.Name = "pictureBoxGray2";
            this.pictureBoxGray2.Size = new System.Drawing.Size(250, 200);
            this.pictureBoxGray2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGray2.TabIndex = 2;
            this.pictureBoxGray2.TabStop = false;

            // 
            // pictureBoxDiff
            // 
            this.pictureBoxDiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxDiff.Location = new System.Drawing.Point(268, 320);
            this.pictureBoxDiff.Name = "pictureBoxDiff";
            this.pictureBoxDiff.Size = new System.Drawing.Size(250, 200);
            this.pictureBoxDiff.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxDiff.TabIndex = 3;
            this.pictureBoxDiff.TabStop = false;

            // 
            // lblTitleOriginal
            // 
            this.lblTitleOriginal.Location = new System.Drawing.Point(12, 60);
            this.lblTitleOriginal.Name = "lblTitleOriginal";
            this.lblTitleOriginal.Size = new System.Drawing.Size(250, 20);
            this.lblTitleOriginal.TabIndex = 4;
            this.lblTitleOriginal.Text = "Оригинальное изображение";
            this.lblTitleOriginal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblTitleGray1
            // 
            this.lblTitleGray1.Location = new System.Drawing.Point(268, 60);
            this.lblTitleGray1.Name = "lblTitleGray1";
            this.lblTitleGray1.Size = new System.Drawing.Size(250, 20);
            this.lblTitleGray1.TabIndex = 5;
            this.lblTitleGray1.Text = "Метод 1: 0.299R + 0.587G + 0.114B";
            this.lblTitleGray1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblTitleGray2
            // 
            this.lblTitleGray2.Location = new System.Drawing.Point(12, 300);
            this.lblTitleGray2.Name = "lblTitleGray2";
            this.lblTitleGray2.Size = new System.Drawing.Size(250, 20);
            this.lblTitleGray2.TabIndex = 6;
            this.lblTitleGray2.Text = "Метод 2: 0.2126R + 0.7152G + 0.0722B";
            this.lblTitleGray2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblTitleDiff
            // 
            this.lblTitleDiff.Location = new System.Drawing.Point(268, 300);
            this.lblTitleDiff.Name = "lblTitleDiff";
            this.lblTitleDiff.Size = new System.Drawing.Size(250, 20);
            this.lblTitleDiff.TabIndex = 7;
            this.lblTitleDiff.Text = "Разность изображений";
            this.lblTitleDiff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblOriginalInfo
            // 
            this.lblOriginalInfo.Location = new System.Drawing.Point(12, 283);
            this.lblOriginalInfo.Name = "lblOriginalInfo";
            this.lblOriginalInfo.Size = new System.Drawing.Size(250, 15);
            this.lblOriginalInfo.TabIndex = 8;
            this.lblOriginalInfo.Text = "Размер: -";
            this.lblOriginalInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblGray1Info
            // 
            this.lblGray1Info.Location = new System.Drawing.Point(268, 283);
            this.lblGray1Info.Name = "lblGray1Info";
            this.lblGray1Info.Size = new System.Drawing.Size(250, 15);
            this.lblGray1Info.TabIndex = 9;
            this.lblGray1Info.Text = "Размер: -";
            this.lblGray1Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblGray2Info
            // 
            this.lblGray2Info.Location = new System.Drawing.Point(12, 523);
            this.lblGray2Info.Name = "lblGray2Info";
            this.lblGray2Info.Size = new System.Drawing.Size(250, 15);
            this.lblGray2Info.TabIndex = 10;
            this.lblGray2Info.Text = "Размер: -";
            this.lblGray2Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // lblDiffInfo
            // 
            this.lblDiffInfo.Location = new System.Drawing.Point(268, 523);
            this.lblDiffInfo.Name = "lblDiffInfo";
            this.lblDiffInfo.Size = new System.Drawing.Size(250, 15);
            this.lblDiffInfo.TabIndex = 11;
            this.lblDiffInfo.Text = "Размер: -";
            this.lblDiffInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // 
            // chartMethod1
            // 
            chartArea1.Name = "ChartArea1";
            this.chartMethod1.ChartAreas.Add(chartArea1);
            this.chartMethod1.Location = new System.Drawing.Point(524, 80);
            this.chartMethod1.Name = "chartMethod1";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chartMethod1.Series.Add(series1);
            this.chartMethod1.Size = new System.Drawing.Size(400, 200);
            this.chartMethod1.TabIndex = 12;
            this.chartMethod1.Text = "chart1";

            // 
            // chartMethod2
            // 
            chartArea2.Name = "ChartArea1";
            this.chartMethod2.ChartAreas.Add(chartArea2);
            this.chartMethod2.Location = new System.Drawing.Point(524, 320);
            this.chartMethod2.Name = "chartMethod2";
            series2.ChartArea = "ChartArea1";
            series2.Name = "Series1";
            this.chartMethod2.Series.Add(series2);
            this.chartMethod2.Size = new System.Drawing.Size(400, 200);
            this.chartMethod2.TabIndex = 13;
            this.chartMethod2.Text = "chart2";

            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(12, 12);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(150, 35);
            this.btnLoadImage.TabIndex = 14;
            this.btnLoadImage.Text = "Загрузить изображение";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(940, 550);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.chartMethod2);
            this.Controls.Add(this.chartMethod1);
            this.Controls.Add(this.lblDiffInfo);
            this.Controls.Add(this.lblGray2Info);
            this.Controls.Add(this.lblGray1Info);
            this.Controls.Add(this.lblOriginalInfo);
            this.Controls.Add(this.lblTitleDiff);
            this.Controls.Add(this.lblTitleGray2);
            this.Controls.Add(this.lblTitleGray1);
            this.Controls.Add(this.lblTitleOriginal);
            this.Controls.Add(this.pictureBoxDiff);
            this.Controls.Add(this.pictureBoxGray2);
            this.Controls.Add(this.pictureBoxGray1);
            this.Controls.Add(this.pictureBoxOriginal);
            this.Name = "MainForm";
            this.Text = "Преобразование RGB в оттенки серого";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGray1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGray2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMethod1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMethod2)).EndInit();
            this.ResumeLayout(false);
        }
    }
}