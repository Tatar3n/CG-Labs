namespace Lab7_CG
{
    partial class Form1
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
            this.reflectionComboBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.numericUpDown4 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown5 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown6 = new System.Windows.Forms.NumericUpDown();
            this.projectionListBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.affineOpButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDown10 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown11 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown12 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.axisZNumeric = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.axisYNumeric = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.axisXNumeric = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxObj = new System.Windows.Forms.GroupBox();
            this.btnSaveObj = new System.Windows.Forms.Button();
            this.btnLoadObj = new System.Windows.Forms.Button();
            this.groupBoxRevolution = new System.Windows.Forms.GroupBox();
            this.btnBuildRevolution = new System.Windows.Forms.Button();
            this.btnClearProfile = new System.Windows.Forms.Button();
            this.chkProfileMode = new System.Windows.Forms.CheckBox();
            this.nudRevolveSegments = new System.Windows.Forms.NumericUpDown();
            this.lblRevolveSegments = new System.Windows.Forms.Label();
            this.comboRevolveAxis = new System.Windows.Forms.ComboBox();
            this.lblRevolveAxis = new System.Windows.Forms.Label();
            this.groupBoxFunction = new System.Windows.Forms.GroupBox();
            this.btnBuildFunctionSurface = new System.Windows.Forms.Button();
            this.nudYSteps = new System.Windows.Forms.NumericUpDown();
            this.nudXSteps = new System.Windows.Forms.NumericUpDown();
            this.lblStepsY = new System.Windows.Forms.Label();
            this.lblStepsX = new System.Windows.Forms.Label();
            this.nudYMax = new System.Windows.Forms.NumericUpDown();
            this.nudYMin = new System.Windows.Forms.NumericUpDown();
            this.nudXMax = new System.Windows.Forms.NumericUpDown();
            this.nudXMin = new System.Windows.Forms.NumericUpDown();
            this.lblYRange = new System.Windows.Forms.Label();
            this.lblXRange = new System.Windows.Forms.Label();
            this.comboFunction = new System.Windows.Forms.ComboBox();
            this.lblFunction = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.shadingComboBox = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkZBuffer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisZNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisYNumeric)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisXNumeric)).BeginInit();
            this.groupBoxObj.SuspendLayout();
            this.groupBoxRevolution.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRevolveSegments)).BeginInit();
            this.groupBoxFunction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Куб",
            "Тетраэдр",
            "Октаэдр",
            "Икосаэдр",
            "Додекаэдр"});
            this.comboBox1.Location = new System.Drawing.Point(995, 14);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(278, 24);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.Text = "Выберите фигуру";
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // reflectionComboBox
            // 
            this.reflectionComboBox.FormattingEnabled = true;
            this.reflectionComboBox.Items.AddRange(new object[] {
            "XY",
            "YZ",
            "XZ"});
            this.reflectionComboBox.Location = new System.Drawing.Point(996, 82);
            this.reflectionComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.reflectionComboBox.Name = "reflectionComboBox";
            this.reflectionComboBox.Size = new System.Drawing.Size(278, 24);
            this.reflectionComboBox.TabIndex = 1;
            this.reflectionComboBox.SelectedIndexChanged += new System.EventHandler(this.reflectionComboBox_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(8, 2);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(973, 940);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseClick);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.DecimalPlaces = 2;
            this.numericUpDown1.Location = new System.Drawing.Point(1004, 224);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(76, 22);
            this.numericUpDown1.TabIndex = 6;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Location = new System.Drawing.Point(1093, 224);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(78, 22);
            this.numericUpDown2.TabIndex = 7;
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.DecimalPlaces = 2;
            this.numericUpDown3.Location = new System.Drawing.Point(1180, 224);
            this.numericUpDown3.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown3.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown3.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(80, 22);
            this.numericUpDown3.TabIndex = 8;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "Смещение",
            "Масштабирование",
            "Поворот",
            "Поворот вокруг прямой",
            "Вращение параллельно OX",
            "Вращение параллельно OY",
            "Вращение параллельно OZ"});
            this.comboBox2.Location = new System.Drawing.Point(990, 147);
            this.comboBox2.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(283, 24);
            this.comboBox2.TabIndex = 5;
            this.comboBox2.Text = "Смещение";
            // 
            // numericUpDown4
            // 
            this.numericUpDown4.DecimalPlaces = 2;
            this.numericUpDown4.Location = new System.Drawing.Point(1005, 290);
            this.numericUpDown4.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown4.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown4.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new System.Drawing.Size(80, 22);
            this.numericUpDown4.TabIndex = 10;
            // 
            // numericUpDown5
            // 
            this.numericUpDown5.DecimalPlaces = 2;
            this.numericUpDown5.Location = new System.Drawing.Point(1094, 290);
            this.numericUpDown5.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown5.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown5.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new System.Drawing.Size(78, 22);
            this.numericUpDown5.TabIndex = 11;
            // 
            // numericUpDown6
            // 
            this.numericUpDown6.DecimalPlaces = 2;
            this.numericUpDown6.Location = new System.Drawing.Point(1180, 290);
            this.numericUpDown6.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown6.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown6.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new System.Drawing.Size(76, 22);
            this.numericUpDown6.TabIndex = 12;
            // 
            // projectionListBox
            // 
            this.projectionListBox.FormattingEnabled = true;
            this.projectionListBox.Items.AddRange(new object[] {
            "Перспективная",
            "Аксонометрическая"});
            this.projectionListBox.Location = new System.Drawing.Point(1007, 479);
            this.projectionListBox.Margin = new System.Windows.Forms.Padding(4);
            this.projectionListBox.Name = "projectionListBox";
            this.projectionListBox.Size = new System.Drawing.Size(249, 24);
            this.projectionListBox.TabIndex = 17;
            this.projectionListBox.Text = "Перспективная";
            this.projectionListBox.SelectedIndexChanged += new System.EventHandler(this.projectionListBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1093, 459);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 16);
            this.label5.TabIndex = 24;
            this.label5.Text = "Проекция";
            // 
            // affineOpButton
            // 
            this.affineOpButton.Location = new System.Drawing.Point(1064, 177);
            this.affineOpButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.affineOpButton.Name = "affineOpButton";
            this.affineOpButton.Size = new System.Drawing.Size(131, 27);
            this.affineOpButton.TabIndex = 9;
            this.affineOpButton.Text = "Применить";
            this.affineOpButton.UseVisualStyleBackColor = true;
            this.affineOpButton.Click += new System.EventHandler(this.affineOpButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(985, 292);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 16);
            this.label6.TabIndex = 25;
            this.label6.Text = "1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(985, 331);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "2";
            // 
            // numericUpDown10
            // 
            this.numericUpDown10.DecimalPlaces = 2;
            this.numericUpDown10.Location = new System.Drawing.Point(1180, 329);
            this.numericUpDown10.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown10.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown10.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new System.Drawing.Size(80, 22);
            this.numericUpDown10.TabIndex = 15;
            // 
            // numericUpDown11
            // 
            this.numericUpDown11.DecimalPlaces = 2;
            this.numericUpDown11.Location = new System.Drawing.Point(1094, 329);
            this.numericUpDown11.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown11.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown11.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown11.Name = "numericUpDown11";
            this.numericUpDown11.Size = new System.Drawing.Size(78, 22);
            this.numericUpDown11.TabIndex = 14;
            // 
            // numericUpDown12
            // 
            this.numericUpDown12.DecimalPlaces = 2;
            this.numericUpDown12.Location = new System.Drawing.Point(1005, 329);
            this.numericUpDown12.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown12.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown12.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.numericUpDown12.Name = "numericUpDown12";
            this.numericUpDown12.Size = new System.Drawing.Size(76, 22);
            this.numericUpDown12.TabIndex = 13;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1301, 14);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(265, 22);
            this.textBox1.TabIndex = 31;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1301, 53);
            this.textBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(265, 22);
            this.textBox2.TabIndex = 32;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1301, 94);
            this.textBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(77, 22);
            this.textBox3.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(996, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(126, 16);
            this.label9.TabIndex = 34;
            this.label9.Text = "Отражение по оси";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1189, 513);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 16);
            this.label4.TabIndex = 23;
            this.label4.Text = "Ось Z";
            // 
            // axisZNumeric
            // 
            this.axisZNumeric.DecimalPlaces = 2;
            this.axisZNumeric.Location = new System.Drawing.Point(1183, 533);
            this.axisZNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.axisZNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.axisZNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.axisZNumeric.Name = "axisZNumeric";
            this.axisZNumeric.ReadOnly = true;
            this.axisZNumeric.Size = new System.Drawing.Size(76, 22);
            this.axisZNumeric.TabIndex = 20;
            this.axisZNumeric.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1104, 513);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 16);
            this.label3.TabIndex = 22;
            this.label3.Text = "Ось Y";
            // 
            // axisYNumeric
            // 
            this.axisYNumeric.DecimalPlaces = 2;
            this.axisYNumeric.Location = new System.Drawing.Point(1096, 533);
            this.axisYNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.axisYNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.axisYNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.axisYNumeric.Name = "axisYNumeric";
            this.axisYNumeric.Size = new System.Drawing.Size(78, 22);
            this.axisYNumeric.TabIndex = 19;
            this.axisYNumeric.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.axisYNumeric.ValueChanged += new System.EventHandler(this.axisYNumeric_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1017, 513);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 16);
            this.label2.TabIndex = 21;
            this.label2.Text = "Ось X";
            // 
            // axisXNumeric
            // 
            this.axisXNumeric.DecimalPlaces = 2;
            this.axisXNumeric.Location = new System.Drawing.Point(1007, 533);
            this.axisXNumeric.Margin = new System.Windows.Forms.Padding(4);
            this.axisXNumeric.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.axisXNumeric.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.axisXNumeric.Name = "axisXNumeric";
            this.axisXNumeric.Size = new System.Drawing.Size(80, 22);
            this.axisXNumeric.TabIndex = 18;
            this.axisXNumeric.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.axisXNumeric.ValueChanged += new System.EventHandler(this.axisXNumeric_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1093, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "Прямая";
            // 
            // groupBoxObj
            // 
            this.groupBoxObj.Controls.Add(this.btnSaveObj);
            this.groupBoxObj.Controls.Add(this.btnLoadObj);
            this.groupBoxObj.Location = new System.Drawing.Point(988, 560);
            this.groupBoxObj.Name = "groupBoxObj";
            this.groupBoxObj.Size = new System.Drawing.Size(280, 70);
            this.groupBoxObj.TabIndex = 36;
            this.groupBoxObj.TabStop = false;
            this.groupBoxObj.Text = "OBJ-модель";
            // 
            // btnSaveObj
            // 
            this.btnSaveObj.Location = new System.Drawing.Point(146, 28);
            this.btnSaveObj.Name = "btnSaveObj";
            this.btnSaveObj.Size = new System.Drawing.Size(120, 27);
            this.btnSaveObj.TabIndex = 1;
            this.btnSaveObj.Text = "Сохранить OBJ";
            this.btnSaveObj.UseVisualStyleBackColor = true;
            this.btnSaveObj.Click += new System.EventHandler(this.btnSaveObj_Click);
            // 
            // btnLoadObj
            // 
            this.btnLoadObj.Location = new System.Drawing.Point(11, 28);
            this.btnLoadObj.Name = "btnLoadObj";
            this.btnLoadObj.Size = new System.Drawing.Size(120, 27);
            this.btnLoadObj.TabIndex = 0;
            this.btnLoadObj.Text = "Загрузить OBJ";
            this.btnLoadObj.UseVisualStyleBackColor = true;
            this.btnLoadObj.Click += new System.EventHandler(this.btnLoadObj_Click);
            // 
            // groupBoxRevolution
            // 
            this.groupBoxRevolution.Controls.Add(this.btnBuildRevolution);
            this.groupBoxRevolution.Controls.Add(this.btnClearProfile);
            this.groupBoxRevolution.Controls.Add(this.chkProfileMode);
            this.groupBoxRevolution.Controls.Add(this.nudRevolveSegments);
            this.groupBoxRevolution.Controls.Add(this.lblRevolveSegments);
            this.groupBoxRevolution.Controls.Add(this.comboRevolveAxis);
            this.groupBoxRevolution.Controls.Add(this.lblRevolveAxis);
            this.groupBoxRevolution.Location = new System.Drawing.Point(988, 636);
            this.groupBoxRevolution.Name = "groupBoxRevolution";
            this.groupBoxRevolution.Size = new System.Drawing.Size(280, 120);
            this.groupBoxRevolution.TabIndex = 37;
            this.groupBoxRevolution.TabStop = false;
            this.groupBoxRevolution.Text = "Фигура вращения";
            // 
            // btnBuildRevolution
            // 
            this.btnBuildRevolution.Location = new System.Drawing.Point(146, 82);
            this.btnBuildRevolution.Name = "btnBuildRevolution";
            this.btnBuildRevolution.Size = new System.Drawing.Size(120, 27);
            this.btnBuildRevolution.TabIndex = 6;
            this.btnBuildRevolution.Text = "Построить";
            this.btnBuildRevolution.UseVisualStyleBackColor = true;
            this.btnBuildRevolution.Click += new System.EventHandler(this.btnBuildRevolution_Click);
            // 
            // btnClearProfile
            // 
            this.btnClearProfile.Location = new System.Drawing.Point(11, 82);
            this.btnClearProfile.Name = "btnClearProfile";
            this.btnClearProfile.Size = new System.Drawing.Size(120, 27);
            this.btnClearProfile.TabIndex = 5;
            this.btnClearProfile.Text = "Очистить";
            this.btnClearProfile.UseVisualStyleBackColor = true;
            this.btnClearProfile.Click += new System.EventHandler(this.btnClearProfile_Click);
            // 
            // chkProfileMode
            // 
            this.chkProfileMode.AutoSize = true;
            this.chkProfileMode.Location = new System.Drawing.Point(11, 59);
            this.chkProfileMode.Name = "chkProfileMode";
            this.chkProfileMode.Size = new System.Drawing.Size(217, 20);
            this.chkProfileMode.TabIndex = 4;
            this.chkProfileMode.Text = "Режим образующей (щелчки)";
            this.chkProfileMode.UseVisualStyleBackColor = true;
            // 
            // nudRevolveSegments
            // 
            this.nudRevolveSegments.Location = new System.Drawing.Point(211, 33);
            this.nudRevolveSegments.Maximum = new decimal(new int[] {
            64,
            0,
            0,
            0});
            this.nudRevolveSegments.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudRevolveSegments.Name = "nudRevolveSegments";
            this.nudRevolveSegments.Size = new System.Drawing.Size(55, 22);
            this.nudRevolveSegments.TabIndex = 3;
            this.nudRevolveSegments.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // lblRevolveSegments
            // 
            this.lblRevolveSegments.AutoSize = true;
            this.lblRevolveSegments.Location = new System.Drawing.Point(153, 35);
            this.lblRevolveSegments.Name = "lblRevolveSegments";
            this.lblRevolveSegments.Size = new System.Drawing.Size(63, 16);
            this.lblRevolveSegments.TabIndex = 2;
            this.lblRevolveSegments.Text = "Секторы";
            // 
            // comboRevolveAxis
            // 
            this.comboRevolveAxis.FormattingEnabled = true;
            this.comboRevolveAxis.Items.AddRange(new object[] {
            "OX",
            "OY",
            "OZ"});
            this.comboRevolveAxis.Location = new System.Drawing.Point(53, 31);
            this.comboRevolveAxis.Name = "comboRevolveAxis";
            this.comboRevolveAxis.Size = new System.Drawing.Size(90, 24);
            this.comboRevolveAxis.TabIndex = 1;
            // 
            // lblRevolveAxis
            // 
            this.lblRevolveAxis.AutoSize = true;
            this.lblRevolveAxis.Location = new System.Drawing.Point(8, 35);
            this.lblRevolveAxis.Name = "lblRevolveAxis";
            this.lblRevolveAxis.Size = new System.Drawing.Size(31, 16);
            this.lblRevolveAxis.TabIndex = 0;
            this.lblRevolveAxis.Text = "Ось";
            // 
            // groupBoxFunction
            // 
            this.groupBoxFunction.Controls.Add(this.btnBuildFunctionSurface);
            this.groupBoxFunction.Controls.Add(this.nudYSteps);
            this.groupBoxFunction.Controls.Add(this.nudXSteps);
            this.groupBoxFunction.Controls.Add(this.lblStepsY);
            this.groupBoxFunction.Controls.Add(this.lblStepsX);
            this.groupBoxFunction.Controls.Add(this.nudYMax);
            this.groupBoxFunction.Controls.Add(this.nudYMin);
            this.groupBoxFunction.Controls.Add(this.nudXMax);
            this.groupBoxFunction.Controls.Add(this.nudXMin);
            this.groupBoxFunction.Controls.Add(this.lblYRange);
            this.groupBoxFunction.Controls.Add(this.lblXRange);
            this.groupBoxFunction.Controls.Add(this.comboFunction);
            this.groupBoxFunction.Controls.Add(this.lblFunction);
            this.groupBoxFunction.Location = new System.Drawing.Point(993, 772);
            this.groupBoxFunction.Name = "groupBoxFunction";
            this.groupBoxFunction.Size = new System.Drawing.Size(280, 170);
            this.groupBoxFunction.TabIndex = 38;
            this.groupBoxFunction.TabStop = false;
            this.groupBoxFunction.Text = "График f(x, y)";
            // 
            // btnBuildFunctionSurface
            // 
            this.btnBuildFunctionSurface.Location = new System.Drawing.Point(71, 137);
            this.btnBuildFunctionSurface.Name = "btnBuildFunctionSurface";
            this.btnBuildFunctionSurface.Size = new System.Drawing.Size(143, 27);
            this.btnBuildFunctionSurface.TabIndex = 12;
            this.btnBuildFunctionSurface.Text = "Построить график";
            this.btnBuildFunctionSurface.UseVisualStyleBackColor = true;
            this.btnBuildFunctionSurface.Click += new System.EventHandler(this.btnBuildFunctionSurface_Click);
            // 
            // nudYSteps
            // 
            this.nudYSteps.Location = new System.Drawing.Point(211, 109);
            this.nudYSteps.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudYSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudYSteps.Name = "nudYSteps";
            this.nudYSteps.Size = new System.Drawing.Size(55, 22);
            this.nudYSteps.TabIndex = 11;
            this.nudYSteps.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nudXSteps
            // 
            this.nudXSteps.Location = new System.Drawing.Point(88, 109);
            this.nudXSteps.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.nudXSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudXSteps.Name = "nudXSteps";
            this.nudXSteps.Size = new System.Drawing.Size(55, 22);
            this.nudXSteps.TabIndex = 10;
            this.nudXSteps.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblStepsY
            // 
            this.lblStepsY.AutoSize = true;
            this.lblStepsY.Location = new System.Drawing.Point(151, 111);
            this.lblStepsY.Name = "lblStepsY";
            this.lblStepsY.Size = new System.Drawing.Size(63, 16);
            this.lblStepsY.TabIndex = 9;
            this.lblStepsY.Text = "Шаг по Y";
            // 
            // lblStepsX
            // 
            this.lblStepsX.AutoSize = true;
            this.lblStepsX.Location = new System.Drawing.Point(8, 111);
            this.lblStepsX.Name = "lblStepsX";
            this.lblStepsX.Size = new System.Drawing.Size(62, 16);
            this.lblStepsX.TabIndex = 8;
            this.lblStepsX.Text = "Шаг по X";
            // 
            // nudYMax
            // 
            this.nudYMax.DecimalPlaces = 1;
            this.nudYMax.Location = new System.Drawing.Point(211, 83);
            this.nudYMax.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudYMax.Name = "nudYMax";
            this.nudYMax.Size = new System.Drawing.Size(55, 22);
            this.nudYMax.TabIndex = 7;
            this.nudYMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudYMin
            // 
            this.nudYMin.DecimalPlaces = 1;
            this.nudYMin.Location = new System.Drawing.Point(88, 85);
            this.nudYMin.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudYMin.Name = "nudYMin";
            this.nudYMin.Size = new System.Drawing.Size(55, 22);
            this.nudYMin.TabIndex = 6;
            // 
            // nudXMax
            // 
            this.nudXMax.DecimalPlaces = 1;
            this.nudXMax.Location = new System.Drawing.Point(211, 57);
            this.nudXMax.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudXMax.Name = "nudXMax";
            this.nudXMax.Size = new System.Drawing.Size(55, 22);
            this.nudXMax.TabIndex = 5;
            this.nudXMax.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // nudXMin
            // 
            this.nudXMin.DecimalPlaces = 1;
            this.nudXMin.Location = new System.Drawing.Point(88, 57);
            this.nudXMin.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            -2147483648});
            this.nudXMin.Name = "nudXMin";
            this.nudXMin.Size = new System.Drawing.Size(55, 22);
            this.nudXMin.TabIndex = 4;
            // 
            // lblYRange
            // 
            this.lblYRange.AutoSize = true;
            this.lblYRange.Location = new System.Drawing.Point(8, 85);
            this.lblYRange.Name = "lblYRange";
            this.lblYRange.Size = new System.Drawing.Size(69, 16);
            this.lblYRange.TabIndex = 3;
            this.lblYRange.Text = "Y min/max";
            // 
            // lblXRange
            // 
            this.lblXRange.AutoSize = true;
            this.lblXRange.Location = new System.Drawing.Point(8, 59);
            this.lblXRange.Name = "lblXRange";
            this.lblXRange.Size = new System.Drawing.Size(68, 16);
            this.lblXRange.TabIndex = 2;
            this.lblXRange.Text = "X min/max";
            // 
            // comboFunction
            // 
            this.comboFunction.FormattingEnabled = true;
            this.comboFunction.Items.AddRange(new object[] {
            "sin(sqrt(x^2 + y^2))",
            "Sin(x + y) + 1;",
            "sin(x) * cos(y)"});
            this.comboFunction.Location = new System.Drawing.Point(71, 24);
            this.comboFunction.Name = "comboFunction";
            this.comboFunction.Size = new System.Drawing.Size(195, 24);
            this.comboFunction.TabIndex = 1;
            // 
            // lblFunction
            // 
            this.lblFunction.AutoSize = true;
            this.lblFunction.Location = new System.Drawing.Point(8, 27);
            this.lblFunction.Name = "lblFunction";
            this.lblFunction.Size = new System.Drawing.Size(64, 16);
            this.lblFunction.TabIndex = 0;
            this.lblFunction.Text = "Функция";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "model.obj";
            // 
            // shadingComboBox
            // 
            this.shadingComboBox.FormattingEnabled = true;
            this.shadingComboBox.Items.AddRange(new object[] {
            "Flat",
            "Gouraud-Lambert",
            "Phong-Toon",
            "Texture"});
            this.shadingComboBox.Location = new System.Drawing.Point(999, 432);
            this.shadingComboBox.Name = "shadingComboBox";
            this.shadingComboBox.Size = new System.Drawing.Size(249, 24);
            this.shadingComboBox.TabIndex = 39;
            this.shadingComboBox.SelectedIndexChanged += new System.EventHandler(this.shadingComboBox_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1001, 403);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 16);
            this.label8.TabIndex = 40;
            this.label8.Text = "Шейдинг";
            // 
            // chkZBuffer
            // 
            this.chkZBuffer.AutoSize = true;
            this.chkZBuffer.Checked = true;
            this.chkZBuffer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkZBuffer.Location = new System.Drawing.Point(1173, 403);
            this.chkZBuffer.Name = "chkZBuffer";
            this.chkZBuffer.Size = new System.Drawing.Size(75, 20);
            this.chkZBuffer.TabIndex = 41;
            this.chkZBuffer.Text = "Z-Buffer";
            this.chkZBuffer.UseVisualStyleBackColor = true;
            this.chkZBuffer.CheckedChanged += new System.EventHandler(this.chkZBuffer_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1637, 950);
            this.Controls.Add(this.chkZBuffer);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.shadingComboBox);
            this.Controls.Add(this.groupBoxFunction);
            this.Controls.Add(this.groupBoxRevolution);
            this.Controls.Add(this.groupBoxObj);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.numericUpDown10);
            this.Controls.Add(this.numericUpDown11);
            this.Controls.Add(this.numericUpDown12);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.affineOpButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.axisXNumeric);
            this.Controls.Add(this.axisYNumeric);
            this.Controls.Add(this.axisZNumeric);
            this.Controls.Add(this.projectionListBox);
            this.Controls.Add(this.numericUpDown4);
            this.Controls.Add(this.numericUpDown5);
            this.Controls.Add(this.numericUpDown6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.numericUpDown3);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.reflectionComboBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Аффинные преобразования. OBJ. Фигура вращения. График f(x,y)";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisZNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisYNumeric)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axisXNumeric)).EndInit();
            this.groupBoxObj.ResumeLayout(false);
            this.groupBoxRevolution.ResumeLayout(false);
            this.groupBoxRevolution.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRevolveSegments)).EndInit();
            this.groupBoxFunction.ResumeLayout(false);
            this.groupBoxFunction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudYMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXMin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox reflectionComboBox;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.NumericUpDown numericUpDown4;
        private System.Windows.Forms.NumericUpDown numericUpDown5;
        private System.Windows.Forms.NumericUpDown numericUpDown6;
        private System.Windows.Forms.ComboBox projectionListBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button affineOpButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDown10;
        private System.Windows.Forms.NumericUpDown numericUpDown11;
        private System.Windows.Forms.NumericUpDown numericUpDown12;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown axisZNumeric;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown axisYNumeric;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown axisXNumeric;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxObj;
        private System.Windows.Forms.Button btnSaveObj;
        private System.Windows.Forms.Button btnLoadObj;
        private System.Windows.Forms.GroupBox groupBoxRevolution;
        private System.Windows.Forms.Button btnBuildRevolution;
        private System.Windows.Forms.Button btnClearProfile;
        private System.Windows.Forms.CheckBox chkProfileMode;
        private System.Windows.Forms.NumericUpDown nudRevolveSegments;
        private System.Windows.Forms.Label lblRevolveSegments;
        private System.Windows.Forms.ComboBox comboRevolveAxis;
        private System.Windows.Forms.Label lblRevolveAxis;
        private System.Windows.Forms.GroupBox groupBoxFunction;
        private System.Windows.Forms.Button btnBuildFunctionSurface;
        private System.Windows.Forms.NumericUpDown nudYSteps;
        private System.Windows.Forms.NumericUpDown nudXSteps;
        private System.Windows.Forms.Label lblStepsY;
        private System.Windows.Forms.Label lblStepsX;
        private System.Windows.Forms.NumericUpDown nudYMax;
        private System.Windows.Forms.NumericUpDown nudYMin;
        private System.Windows.Forms.NumericUpDown nudXMax;
        private System.Windows.Forms.NumericUpDown nudXMin;
        private System.Windows.Forms.Label lblYRange;
        private System.Windows.Forms.Label lblXRange;
        private System.Windows.Forms.ComboBox comboFunction;
        private System.Windows.Forms.Label lblFunction;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ComboBox shadingComboBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkZBuffer;
    }
}
