namespace ArrowPointCANBusTool.Forms {
    partial class MainFormPi {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainFormPi));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.BMUdataGridView = new System.Windows.Forms.DataGridView();
            this.TestBTN = new System.Windows.Forms.Button();
            this.TXTCellVDifference = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TXTMaxCellVTemp = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TXTMaxCellV = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.TXTMinCellVTemp = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.TXTMinCellV = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.TXTBatTemp = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.BTNLowBatteryWarn = new System.Windows.Forms.Button();
            this.BTNChargeWarn = new System.Windows.Forms.Button();
            this.BTNVoltWarn = new System.Windows.Forms.Button();
            this.BTNTempWarn = new System.Windows.Forms.Button();
            this.TXTBatStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TXTBatPercentage = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.batteryPRO = new System.Windows.Forms.ProgressBar();
            this.tabHome = new System.Windows.Forms.TabControl();
            this.HomeTab = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmuTelemetry = new System.Windows.Forms.GroupBox();
            this.CMUdataGridView = new System.Windows.Forms.DataGridView();
            this.CellNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Serial = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PCBTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellTemperature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellVoltage0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell1Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell2Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell3Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell4Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell5Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell6Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell7Voltage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TwelveVoltSystem = new System.Windows.Forms.GroupBox();
            this.TwelveVoltDataGridView = new System.Windows.Forms.DataGridView();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CellTemp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell0mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell1mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell2mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cell3mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Net12vCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HVDc2DcCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusFlags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusEvents = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.header = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MinmV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Max_mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Min_C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Max_C = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pack_mV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pack_mA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalancePositive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BalanceNegative = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CMU_Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BMUdataGridView)).BeginInit();
            this.tabHome.SuspendLayout();
            this.HomeTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.cmuTelemetry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CMUdataGridView)).BeginInit();
            this.TwelveVoltSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TwelveVoltDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.TwelveVoltSystem);
            this.tabPage2.Controls.Add(this.cmuTelemetry);
            this.tabPage2.Controls.Add(this.BMUdataGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 54);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(663, 313);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // BMUdataGridView
            // 
            this.BMUdataGridView.AllowUserToAddRows = false;
            this.BMUdataGridView.AllowUserToDeleteRows = false;
            this.BMUdataGridView.AllowUserToResizeColumns = false;
            this.BMUdataGridView.AllowUserToResizeRows = false;
            this.BMUdataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BMUdataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.BMUdataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.BMUdataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BMUdataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.BMUdataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BMUdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.header,
            this.MinmV,
            this.Max_mV,
            this.Min_C,
            this.Max_C,
            this.Pack_mV,
            this.Pack_mA,
            this.BalancePositive,
            this.BalanceNegative,
            this.CMU_Count});
            this.BMUdataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.BMUdataGridView.EnableHeadersVisualStyles = false;
            this.BMUdataGridView.Location = new System.Drawing.Point(3, 6);
            this.BMUdataGridView.MultiSelect = false;
            this.BMUdataGridView.Name = "BMUdataGridView";
            this.BMUdataGridView.ReadOnly = true;
            this.BMUdataGridView.RowHeadersVisible = false;
            this.BMUdataGridView.RowHeadersWidth = 100;
            this.BMUdataGridView.RowTemplate.Height = 10;
            this.BMUdataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.BMUdataGridView.ShowEditingIcon = false;
            this.BMUdataGridView.Size = new System.Drawing.Size(657, 125);
            this.BMUdataGridView.TabIndex = 3;
            // 
            // TestBTN
            // 
            this.TestBTN.Location = new System.Drawing.Point(379, 215);
            this.TestBTN.Name = "TestBTN";
            this.TestBTN.Size = new System.Drawing.Size(195, 34);
            this.TestBTN.TabIndex = 101;
            this.TestBTN.Text = "SIMULATE";
            this.TestBTN.UseVisualStyleBackColor = true;
            this.TestBTN.Click += new System.EventHandler(this.TestBTN_Click);
            // 
            // TXTCellVDifference
            // 
            this.TXTCellVDifference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTCellVDifference.AutoSize = true;
            this.TXTCellVDifference.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTCellVDifference.ForeColor = System.Drawing.Color.Blue;
            this.TXTCellVDifference.Location = new System.Drawing.Point(535, 180);
            this.TXTCellVDifference.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTCellVDifference.Name = "TXTCellVDifference";
            this.TXTCellVDifference.Size = new System.Drawing.Size(26, 25);
            this.TXTCellVDifference.TabIndex = 99;
            this.TXTCellVDifference.Text = "X";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(343, 180);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(178, 25);
            this.label4.TabIndex = 98;
            this.label4.Text = "Voltage Difference:";
            // 
            // TXTMaxCellVTemp
            // 
            this.TXTMaxCellVTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTMaxCellVTemp.AutoSize = true;
            this.TXTMaxCellVTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaxCellVTemp.ForeColor = System.Drawing.Color.Blue;
            this.TXTMaxCellVTemp.Location = new System.Drawing.Point(535, 146);
            this.TXTMaxCellVTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTMaxCellVTemp.Name = "TXTMaxCellVTemp";
            this.TXTMaxCellVTemp.Size = new System.Drawing.Size(26, 25);
            this.TXTMaxCellVTemp.TabIndex = 97;
            this.TXTMaxCellVTemp.Text = "X";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(450, 144);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 25);
            this.label8.TabIndex = 96;
            this.label8.Text = "Temp:";
            // 
            // TXTMaxCellV
            // 
            this.TXTMaxCellV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTMaxCellV.AutoSize = true;
            this.TXTMaxCellV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMaxCellV.ForeColor = System.Drawing.Color.Blue;
            this.TXTMaxCellV.Location = new System.Drawing.Point(535, 119);
            this.TXTMaxCellV.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTMaxCellV.Name = "TXTMaxCellV";
            this.TXTMaxCellV.Size = new System.Drawing.Size(40, 25);
            this.TXTMaxCellV.TabIndex = 95;
            this.TXTMaxCellV.Text = "XV";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(352, 119);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(167, 25);
            this.label10.TabIndex = 94;
            this.label10.Text = "Max Cell Voltage:";
            // 
            // TXTMinCellVTemp
            // 
            this.TXTMinCellVTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTMinCellVTemp.AutoSize = true;
            this.TXTMinCellVTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMinCellVTemp.ForeColor = System.Drawing.Color.Blue;
            this.TXTMinCellVTemp.Location = new System.Drawing.Point(535, 85);
            this.TXTMinCellVTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTMinCellVTemp.Name = "TXTMinCellVTemp";
            this.TXTMinCellVTemp.Size = new System.Drawing.Size(26, 25);
            this.TXTMinCellVTemp.TabIndex = 93;
            this.TXTMinCellVTemp.Text = "X";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(450, 85);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 25);
            this.label6.TabIndex = 92;
            this.label6.Text = "Temp:";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(132, 249);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(118, 47);
            this.button7.TabIndex = 91;
            this.button7.Text = "XWarning";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(8, 249);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(118, 47);
            this.button6.TabIndex = 90;
            this.button6.Text = "XWarning";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // TXTMinCellV
            // 
            this.TXTMinCellV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTMinCellV.AutoSize = true;
            this.TXTMinCellV.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTMinCellV.ForeColor = System.Drawing.Color.Blue;
            this.TXTMinCellV.Location = new System.Drawing.Point(535, 58);
            this.TXTMinCellV.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTMinCellV.Name = "TXTMinCellV";
            this.TXTMinCellV.Size = new System.Drawing.Size(40, 25);
            this.TXTMinCellV.TabIndex = 89;
            this.TXTMinCellV.Text = "XV";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(358, 58);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 25);
            this.label2.TabIndex = 88;
            this.label2.Text = "Min Cell Voltage:";
            // 
            // TXTBatTemp
            // 
            this.TXTBatTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTBatTemp.AutoSize = true;
            this.TXTBatTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTBatTemp.ForeColor = System.Drawing.Color.Blue;
            this.TXTBatTemp.Location = new System.Drawing.Point(535, 16);
            this.TXTBatTemp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTBatTemp.Name = "TXTBatTemp";
            this.TXTBatTemp.Size = new System.Drawing.Size(26, 25);
            this.TXTBatTemp.TabIndex = 87;
            this.TXTBatTemp.Text = "X";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(325, 16);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(196, 25);
            this.label5.TabIndex = 86;
            this.label5.Text = "Battery Temperature:";
            // 
            // BTNLowBatteryWarn
            // 
            this.BTNLowBatteryWarn.Location = new System.Drawing.Point(132, 196);
            this.BTNLowBatteryWarn.Name = "BTNLowBatteryWarn";
            this.BTNLowBatteryWarn.Size = new System.Drawing.Size(118, 47);
            this.BTNLowBatteryWarn.TabIndex = 85;
            this.BTNLowBatteryWarn.Text = "Low Battery";
            this.BTNLowBatteryWarn.UseVisualStyleBackColor = true;
            // 
            // BTNChargeWarn
            // 
            this.BTNChargeWarn.Location = new System.Drawing.Point(8, 196);
            this.BTNChargeWarn.Name = "BTNChargeWarn";
            this.BTNChargeWarn.Size = new System.Drawing.Size(118, 47);
            this.BTNChargeWarn.TabIndex = 84;
            this.BTNChargeWarn.Text = "Charge Rate Warning";
            this.BTNChargeWarn.UseVisualStyleBackColor = true;
            // 
            // BTNVoltWarn
            // 
            this.BTNVoltWarn.Location = new System.Drawing.Point(132, 143);
            this.BTNVoltWarn.Name = "BTNVoltWarn";
            this.BTNVoltWarn.Size = new System.Drawing.Size(118, 47);
            this.BTNVoltWarn.TabIndex = 83;
            this.BTNVoltWarn.Text = "Voltage Warning";
            this.BTNVoltWarn.UseVisualStyleBackColor = true;
            // 
            // BTNTempWarn
            // 
            this.BTNTempWarn.Location = new System.Drawing.Point(8, 143);
            this.BTNTempWarn.Name = "BTNTempWarn";
            this.BTNTempWarn.Size = new System.Drawing.Size(118, 47);
            this.BTNTempWarn.TabIndex = 82;
            this.BTNTempWarn.Text = "Temperature Warning";
            this.BTNTempWarn.UseVisualStyleBackColor = true;
            // 
            // TXTBatStatus
            // 
            this.TXTBatStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTBatStatus.AutoSize = true;
            this.TXTBatStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTBatStatus.ForeColor = System.Drawing.Color.Blue;
            this.TXTBatStatus.Location = new System.Drawing.Point(83, 106);
            this.TXTBatStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTBatStatus.Name = "TXTBatStatus";
            this.TXTBatStatus.Size = new System.Drawing.Size(92, 25);
            this.TXTBatStatus.TabIndex = 81;
            this.TXTBatStatus.Text = "Charging";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 105);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 25);
            this.label3.TabIndex = 80;
            this.label3.Text = "Status:";
            // 
            // TXTBatPercentage
            // 
            this.TXTBatPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TXTBatPercentage.AutoSize = true;
            this.TXTBatPercentage.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TXTBatPercentage.ForeColor = System.Drawing.Color.Blue;
            this.TXTBatPercentage.Location = new System.Drawing.Point(206, 80);
            this.TXTBatPercentage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TXTBatPercentage.Name = "TXTBatPercentage";
            this.TXTBatPercentage.Size = new System.Drawing.Size(44, 25);
            this.TXTBatPercentage.TabIndex = 79;
            this.TXTBatPercentage.Text = "X%";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 80);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 25);
            this.label1.TabIndex = 78;
            this.label1.Text = "Battery Percentage:";
            // 
            // batteryPRO
            // 
            this.batteryPRO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.batteryPRO.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.batteryPRO.ForeColor = System.Drawing.Color.Green;
            this.batteryPRO.Location = new System.Drawing.Point(23, 16);
            this.batteryPRO.Margin = new System.Windows.Forms.Padding(2);
            this.batteryPRO.Name = "batteryPRO";
            this.batteryPRO.Size = new System.Drawing.Size(227, 53);
            this.batteryPRO.Step = 5;
            this.batteryPRO.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.batteryPRO.TabIndex = 77;
            this.batteryPRO.Value = 20;
            // 
            // tabHome
            // 
            this.tabHome.Controls.Add(this.HomeTab);
            this.tabHome.Controls.Add(this.tabPage2);
            this.tabHome.ItemSize = new System.Drawing.Size(96, 50);
            this.tabHome.Location = new System.Drawing.Point(-1, -2);
            this.tabHome.Name = "tabHome";
            this.tabHome.SelectedIndex = 0;
            this.tabHome.Size = new System.Drawing.Size(671, 371);
            this.tabHome.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabHome.TabIndex = 1;
            // 
            // HomeTab
            // 
            this.HomeTab.Controls.Add(this.TestBTN);
            this.HomeTab.Controls.Add(this.pictureBox1);
            this.HomeTab.Controls.Add(this.TXTCellVDifference);
            this.HomeTab.Controls.Add(this.label4);
            this.HomeTab.Controls.Add(this.TXTMaxCellVTemp);
            this.HomeTab.Controls.Add(this.label8);
            this.HomeTab.Controls.Add(this.TXTMaxCellV);
            this.HomeTab.Controls.Add(this.label10);
            this.HomeTab.Controls.Add(this.TXTMinCellVTemp);
            this.HomeTab.Controls.Add(this.label6);
            this.HomeTab.Controls.Add(this.button7);
            this.HomeTab.Controls.Add(this.button6);
            this.HomeTab.Controls.Add(this.TXTMinCellV);
            this.HomeTab.Controls.Add(this.label2);
            this.HomeTab.Controls.Add(this.TXTBatTemp);
            this.HomeTab.Controls.Add(this.label5);
            this.HomeTab.Controls.Add(this.BTNLowBatteryWarn);
            this.HomeTab.Controls.Add(this.BTNChargeWarn);
            this.HomeTab.Controls.Add(this.BTNVoltWarn);
            this.HomeTab.Controls.Add(this.BTNTempWarn);
            this.HomeTab.Controls.Add(this.TXTBatStatus);
            this.HomeTab.Controls.Add(this.label3);
            this.HomeTab.Controls.Add(this.TXTBatPercentage);
            this.HomeTab.Controls.Add(this.label1);
            this.HomeTab.Controls.Add(this.batteryPRO);
            this.HomeTab.Location = new System.Drawing.Point(4, 54);
            this.HomeTab.Name = "HomeTab";
            this.HomeTab.Padding = new System.Windows.Forms.Padding(3);
            this.HomeTab.Size = new System.Drawing.Size(663, 313);
            this.HomeTab.TabIndex = 0;
            this.HomeTab.Text = "Home";
            this.HomeTab.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(290, 268);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(368, 40);
            this.pictureBox1.TabIndex = 100;
            this.pictureBox1.TabStop = false;
            // 
            // cmuTelemetry
            // 
            this.cmuTelemetry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmuTelemetry.AutoSize = true;
            this.cmuTelemetry.Controls.Add(this.CMUdataGridView);
            this.cmuTelemetry.Location = new System.Drawing.Point(3, 137);
            this.cmuTelemetry.Name = "cmuTelemetry";
            this.cmuTelemetry.Size = new System.Drawing.Size(654, 96);
            this.cmuTelemetry.TabIndex = 4;
            this.cmuTelemetry.TabStop = false;
            this.cmuTelemetry.Text = "CMU Telemetry";
            // 
            // CMUdataGridView
            // 
            this.CMUdataGridView.AllowUserToAddRows = false;
            this.CMUdataGridView.AllowUserToDeleteRows = false;
            this.CMUdataGridView.AllowUserToResizeColumns = false;
            this.CMUdataGridView.AllowUserToResizeRows = false;
            this.CMUdataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.CMUdataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CMUdataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CMUdataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CMUdataGridView.ColumnHeadersHeight = 22;
            this.CMUdataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CellNumber,
            this.Serial,
            this.PCBTemperature,
            this.CellTemperature,
            this.CellVoltage0,
            this.Cell1Voltage,
            this.Cell2Voltage,
            this.Cell3Voltage,
            this.Cell4Voltage,
            this.Cell5Voltage,
            this.Cell6Voltage,
            this.Cell7Voltage});
            this.CMUdataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CMUdataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.CMUdataGridView.EnableHeadersVisualStyles = false;
            this.CMUdataGridView.Location = new System.Drawing.Point(3, 16);
            this.CMUdataGridView.MultiSelect = false;
            this.CMUdataGridView.Name = "CMUdataGridView";
            this.CMUdataGridView.ReadOnly = true;
            this.CMUdataGridView.RowHeadersVisible = false;
            this.CMUdataGridView.RowHeadersWidth = 100;
            this.CMUdataGridView.RowTemplate.ReadOnly = true;
            this.CMUdataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.CMUdataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.CMUdataGridView.Size = new System.Drawing.Size(648, 77);
            this.CMUdataGridView.TabIndex = 3;
            // 
            // CellNumber
            // 
            this.CellNumber.HeaderText = "";
            this.CellNumber.Name = "CellNumber";
            this.CellNumber.ReadOnly = true;
            // 
            // Serial
            // 
            this.Serial.DataPropertyName = "SerialNumber";
            this.Serial.HeaderText = "Serial";
            this.Serial.Name = "Serial";
            this.Serial.ReadOnly = true;
            this.Serial.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // PCBTemperature
            // 
            this.PCBTemperature.DataPropertyName = "PCBTemp";
            this.PCBTemperature.HeaderText = "PCB (C)";
            this.PCBTemperature.Name = "PCBTemperature";
            this.PCBTemperature.ReadOnly = true;
            this.PCBTemperature.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // CellTemperature
            // 
            this.CellTemperature.DataPropertyName = "CellTemp";
            this.CellTemperature.HeaderText = "Cell (C)";
            this.CellTemperature.Name = "CellTemperature";
            this.CellTemperature.ReadOnly = true;
            this.CellTemperature.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // CellVoltage0
            // 
            this.CellVoltage0.DataPropertyName = "Cell0mV";
            dataGridViewCellStyle2.NullValue = null;
            this.CellVoltage0.DefaultCellStyle = dataGridViewCellStyle2;
            this.CellVoltage0.HeaderText = "Cell 0 mV";
            this.CellVoltage0.Name = "CellVoltage0";
            this.CellVoltage0.ReadOnly = true;
            this.CellVoltage0.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell1Voltage
            // 
            this.Cell1Voltage.DataPropertyName = "Cell1mV";
            this.Cell1Voltage.HeaderText = "Cell 1 mV";
            this.Cell1Voltage.Name = "Cell1Voltage";
            this.Cell1Voltage.ReadOnly = true;
            this.Cell1Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell2Voltage
            // 
            this.Cell2Voltage.DataPropertyName = "Cell2mV";
            this.Cell2Voltage.HeaderText = "Cell 2 mV";
            this.Cell2Voltage.Name = "Cell2Voltage";
            this.Cell2Voltage.ReadOnly = true;
            this.Cell2Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell3Voltage
            // 
            this.Cell3Voltage.DataPropertyName = "Cell3mV";
            this.Cell3Voltage.HeaderText = "Cell 3 mV";
            this.Cell3Voltage.Name = "Cell3Voltage";
            this.Cell3Voltage.ReadOnly = true;
            this.Cell3Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell4Voltage
            // 
            this.Cell4Voltage.DataPropertyName = "Cell4mV";
            this.Cell4Voltage.HeaderText = "Cell 4 mV";
            this.Cell4Voltage.Name = "Cell4Voltage";
            this.Cell4Voltage.ReadOnly = true;
            this.Cell4Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell5Voltage
            // 
            this.Cell5Voltage.DataPropertyName = "Cell5mV";
            this.Cell5Voltage.HeaderText = "Cell 5 mV";
            this.Cell5Voltage.Name = "Cell5Voltage";
            this.Cell5Voltage.ReadOnly = true;
            this.Cell5Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell6Voltage
            // 
            this.Cell6Voltage.DataPropertyName = "Cell6mV";
            this.Cell6Voltage.HeaderText = "Cell 6 mV";
            this.Cell6Voltage.Name = "Cell6Voltage";
            this.Cell6Voltage.ReadOnly = true;
            this.Cell6Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Cell7Voltage
            // 
            this.Cell7Voltage.DataPropertyName = "Cell7mV";
            this.Cell7Voltage.HeaderText = "Cell 7 mV";
            this.Cell7Voltage.Name = "Cell7Voltage";
            this.Cell7Voltage.ReadOnly = true;
            this.Cell7Voltage.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // TwelveVoltSystem
            // 
            this.TwelveVoltSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TwelveVoltSystem.Controls.Add(this.TwelveVoltDataGridView);
            this.TwelveVoltSystem.Location = new System.Drawing.Point(6, 236);
            this.TwelveVoltSystem.Name = "TwelveVoltSystem";
            this.TwelveVoltSystem.Size = new System.Drawing.Size(654, 81);
            this.TwelveVoltSystem.TabIndex = 5;
            this.TwelveVoltSystem.TabStop = false;
            this.TwelveVoltSystem.Text = "12v System";
            // 
            // TwelveVoltDataGridView
            // 
            this.TwelveVoltDataGridView.AllowUserToAddRows = false;
            this.TwelveVoltDataGridView.AllowUserToDeleteRows = false;
            this.TwelveVoltDataGridView.AllowUserToResizeColumns = false;
            this.TwelveVoltDataGridView.AllowUserToResizeRows = false;
            this.TwelveVoltDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TwelveVoltDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TwelveVoltDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TwelveVoltDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.TwelveVoltDataGridView.ColumnHeadersHeight = 22;
            this.TwelveVoltDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNumber,
            this.CellTemp,
            this.Cell0mV,
            this.Cell1mV,
            this.Cell2mV,
            this.Cell3mV,
            this.Net12vCurrent,
            this.HVDc2DcCurrent,
            this.StatusFlags,
            this.StatusEvents});
            this.TwelveVoltDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.TwelveVoltDataGridView.EnableHeadersVisualStyles = false;
            this.TwelveVoltDataGridView.Location = new System.Drawing.Point(3, 16);
            this.TwelveVoltDataGridView.MultiSelect = false;
            this.TwelveVoltDataGridView.Name = "TwelveVoltDataGridView";
            this.TwelveVoltDataGridView.ReadOnly = true;
            this.TwelveVoltDataGridView.RowHeadersVisible = false;
            this.TwelveVoltDataGridView.RowHeadersWidth = 100;
            this.TwelveVoltDataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TwelveVoltDataGridView.ShowEditingIcon = false;
            this.TwelveVoltDataGridView.Size = new System.Drawing.Size(648, 57);
            this.TwelveVoltDataGridView.TabIndex = 0;
            // 
            // SerialNumber
            // 
            this.SerialNumber.HeaderText = "Serial Number";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            // 
            // CellTemp
            // 
            this.CellTemp.HeaderText = "Cell Temp C";
            this.CellTemp.Name = "CellTemp";
            this.CellTemp.ReadOnly = true;
            // 
            // Cell0mV
            // 
            this.Cell0mV.HeaderText = "Cell 0 mV";
            this.Cell0mV.Name = "Cell0mV";
            this.Cell0mV.ReadOnly = true;
            // 
            // Cell1mV
            // 
            this.Cell1mV.HeaderText = "Cell 1 mV";
            this.Cell1mV.Name = "Cell1mV";
            this.Cell1mV.ReadOnly = true;
            // 
            // Cell2mV
            // 
            this.Cell2mV.HeaderText = "Cell 2 mV";
            this.Cell2mV.Name = "Cell2mV";
            this.Cell2mV.ReadOnly = true;
            // 
            // Cell3mV
            // 
            this.Cell3mV.HeaderText = "Cell 3 mV";
            this.Cell3mV.Name = "Cell3mV";
            this.Cell3mV.ReadOnly = true;
            // 
            // Net12vCurrent
            // 
            this.Net12vCurrent.HeaderText = "Net 12V mA";
            this.Net12vCurrent.Name = "Net12vCurrent";
            this.Net12vCurrent.ReadOnly = true;
            // 
            // HVDc2DcCurrent
            // 
            this.HVDc2DcCurrent.HeaderText = "DC 2 DC mA";
            this.HVDc2DcCurrent.Name = "HVDc2DcCurrent";
            this.HVDc2DcCurrent.ReadOnly = true;
            // 
            // StatusFlags
            // 
            this.StatusFlags.HeaderText = "Status Flags";
            this.StatusFlags.Name = "StatusFlags";
            this.StatusFlags.ReadOnly = true;
            // 
            // StatusEvents
            // 
            this.StatusEvents.HeaderText = "Status Events";
            this.StatusEvents.Name = "StatusEvents";
            this.StatusEvents.ReadOnly = true;
            // 
            // header
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            this.header.DefaultCellStyle = dataGridViewCellStyle3;
            this.header.HeaderText = "";
            this.header.Name = "header";
            this.header.ReadOnly = true;
            // 
            // MinmV
            // 
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinmV.DefaultCellStyle = dataGridViewCellStyle4;
            this.MinmV.HeaderText = "Min mV";
            this.MinmV.Name = "MinmV";
            this.MinmV.ReadOnly = true;
            this.MinmV.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Max_mV
            // 
            this.Max_mV.HeaderText = "Max mV";
            this.Max_mV.Name = "Max_mV";
            this.Max_mV.ReadOnly = true;
            this.Max_mV.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Min_C
            // 
            this.Min_C.HeaderText = "Min C";
            this.Min_C.Name = "Min_C";
            this.Min_C.ReadOnly = true;
            this.Min_C.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Max_C
            // 
            this.Max_C.HeaderText = "Max C";
            this.Max_C.Name = "Max_C";
            this.Max_C.ReadOnly = true;
            this.Max_C.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Pack_mV
            // 
            this.Pack_mV.HeaderText = "Pack mV";
            this.Pack_mV.Name = "Pack_mV";
            this.Pack_mV.ReadOnly = true;
            this.Pack_mV.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Pack_mA
            // 
            this.Pack_mA.HeaderText = "Pack mA";
            this.Pack_mA.Name = "Pack_mA";
            this.Pack_mA.ReadOnly = true;
            this.Pack_mA.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // BalancePositive
            // 
            this.BalancePositive.HeaderText = "Balance +";
            this.BalancePositive.Name = "BalancePositive";
            this.BalancePositive.ReadOnly = true;
            this.BalancePositive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // BalanceNegative
            // 
            this.BalanceNegative.HeaderText = "Balance -";
            this.BalanceNegative.Name = "BalanceNegative";
            this.BalanceNegative.ReadOnly = true;
            this.BalanceNegative.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // CMU_Count
            // 
            this.CMU_Count.HeaderText = "CMU Count";
            this.CMU_Count.Name = "CMU_Count";
            this.CMU_Count.ReadOnly = true;
            this.CMU_Count.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // MainFormPi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 367);
            this.Controls.Add(this.tabHome);
            this.IsMdiContainer = true;
            this.Name = "MainFormPi";
            this.Text = "MainFormPi";
            this.Load += new System.EventHandler(this.MainFormPi_Load);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BMUdataGridView)).EndInit();
            this.tabHome.ResumeLayout(false);
            this.HomeTab.ResumeLayout(false);
            this.HomeTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.cmuTelemetry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CMUdataGridView)).EndInit();
            this.TwelveVoltSystem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TwelveVoltDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button TestBTN;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label TXTCellVDifference;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label TXTMaxCellVTemp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label TXTMaxCellV;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label TXTMinCellVTemp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label TXTMinCellV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label TXTBatTemp;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BTNLowBatteryWarn;
        private System.Windows.Forms.Button BTNChargeWarn;
        private System.Windows.Forms.Button BTNVoltWarn;
        private System.Windows.Forms.Button BTNTempWarn;
        private System.Windows.Forms.Label TXTBatStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label TXTBatPercentage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar batteryPRO;
        private System.Windows.Forms.TabControl tabHome;
        private System.Windows.Forms.TabPage HomeTab;
        private System.Windows.Forms.DataGridView BMUdataGridView;
        private System.Windows.Forms.GroupBox cmuTelemetry;
        private System.Windows.Forms.DataGridView CMUdataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Serial;
        private System.Windows.Forms.DataGridViewTextBoxColumn PCBTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellTemperature;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellVoltage0;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell1Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell2Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell3Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell4Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell5Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell6Voltage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell7Voltage;
        private System.Windows.Forms.GroupBox TwelveVoltSystem;
        private System.Windows.Forms.DataGridView TwelveVoltDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn CellTemp;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell0mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell1mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell2mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cell3mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Net12vCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn HVDc2DcCurrent;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusEvents;
        private System.Windows.Forms.DataGridViewTextBoxColumn header;
        private System.Windows.Forms.DataGridViewTextBoxColumn MinmV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Max_mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Min_C;
        private System.Windows.Forms.DataGridViewTextBoxColumn Max_C;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pack_mV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pack_mA;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalancePositive;
        private System.Windows.Forms.DataGridViewTextBoxColumn BalanceNegative;
        private System.Windows.Forms.DataGridViewTextBoxColumn CMU_Count;
    }
}