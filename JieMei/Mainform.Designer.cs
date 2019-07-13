namespace JieMei
{
    partial class Mainform
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Mainform));
            this.label3 = new System.Windows.Forms.Label();
            this.Img_modeBox = new System.Windows.Forms.ComboBox();
            this.ptbDisplay = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtSampleResult = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSampling = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.zone5Btn = new System.Windows.Forms.Button();
            this.zone4Btn = new System.Windows.Forms.Button();
            this.zone3Btn = new System.Windows.Forms.Button();
            this.zone2Btn = new System.Windows.Forms.Button();
            this.zone1Btn = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtMaterielCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCurrentCounts = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtMissionID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProZone = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTCPInfo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.QrcCheckBtn = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnClearZone5 = new System.Windows.Forms.Button();
            this.btnClearZone4 = new System.Windows.Forms.Button();
            this.btnClearZone3 = new System.Windows.Forms.Button();
            this.btnClearZone2 = new System.Windows.Forms.Button();
            this.btnClearZone1 = new System.Windows.Forms.Button();
            this.btnClearZone0 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQRC = new System.Windows.Forms.TextBox();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Numbers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnProSet = new System.Windows.Forms.Button();
            this.btnInitPos = new System.Windows.Forms.Button();
            this.btnStartServers = new System.Windows.Forms.Button();
            this.btn3DForm = new System.Windows.Forms.Button();
            this.btnWorking = new System.Windows.Forms.Button();
            this.btnModeChange = new System.Windows.Forms.Button();
            this.carArriveStatusBtn = new System.Windows.Forms.Button();
            this.btnClearAlarm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(781, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "图像模式:";
            // 
            // Img_modeBox
            // 
            this.Img_modeBox.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Img_modeBox.FormattingEnabled = true;
            this.Img_modeBox.Items.AddRange(new object[] {
            "工件识别",
            "样品学习",
            "深度相机",
            "彩色相机"});
            this.Img_modeBox.Location = new System.Drawing.Point(875, 28);
            this.Img_modeBox.Name = "Img_modeBox";
            this.Img_modeBox.Size = new System.Drawing.Size(154, 32);
            this.Img_modeBox.TabIndex = 10;
            this.Img_modeBox.SelectedIndexChanged += new System.EventHandler(this.Img_modeBox_SelectedIndexChanged);
            // 
            // ptbDisplay
            // 
            this.ptbDisplay.BackColor = System.Drawing.Color.Black;
            this.ptbDisplay.Location = new System.Drawing.Point(12, 28);
            this.ptbDisplay.Name = "ptbDisplay";
            this.ptbDisplay.Size = new System.Drawing.Size(736, 773);
            this.ptbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbDisplay.TabIndex = 11;
            this.ptbDisplay.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 26);
            this.label1.TabIndex = 14;
            this.label1.Text = "采样距离:";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.Cornsilk;
            this.trackBar1.Location = new System.Drawing.Point(10, 48);
            this.trackBar1.Maximum = 2200;
            this.trackBar1.Minimum = 300;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(581, 42);
            this.trackBar1.TabIndex = 13;
            this.trackBar1.Value = 750;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(786, 239);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(603, 590);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtSampleResult);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.btnSampling);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtTypeName);
            this.tabPage1.Controls.Add(this.trackBar1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(595, 552);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "样品学习";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtSampleResult
            // 
            this.txtSampleResult.Location = new System.Drawing.Point(20, 312);
            this.txtSampleResult.Multiline = true;
            this.txtSampleResult.Name = "txtSampleResult";
            this.txtSampleResult.Size = new System.Drawing.Size(419, 228);
            this.txtSampleResult.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(16, 283);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 26);
            this.label4.TabIndex = 27;
            this.label4.Text = "采样结果：";
            // 
            // btnSampling
            // 
            this.btnSampling.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSampling.ForeColor = System.Drawing.Color.Blue;
            this.btnSampling.Location = new System.Drawing.Point(20, 223);
            this.btnSampling.Name = "btnSampling";
            this.btnSampling.Size = new System.Drawing.Size(218, 45);
            this.btnSampling.TabIndex = 26;
            this.btnSampling.Text = "采        样";
            this.btnSampling.UseVisualStyleBackColor = true;
            this.btnSampling.Click += new System.EventHandler(this.btnSampling_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(16, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 26);
            this.label2.TabIndex = 16;
            this.label2.Text = "输入工件名称:";
            // 
            // txtTypeName
            // 
            this.txtTypeName.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTypeName.Location = new System.Drawing.Point(20, 158);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.Size = new System.Drawing.Size(219, 32);
            this.txtTypeName.TabIndex = 15;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.zone5Btn);
            this.tabPage2.Controls.Add(this.zone4Btn);
            this.tabPage2.Controls.Add(this.zone3Btn);
            this.tabPage2.Controls.Add(this.zone2Btn);
            this.tabPage2.Controls.Add(this.zone1Btn);
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(595, 552);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "托盘位置识别";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // zone5Btn
            // 
            this.zone5Btn.Location = new System.Drawing.Point(216, 196);
            this.zone5Btn.Name = "zone5Btn";
            this.zone5Btn.Size = new System.Drawing.Size(139, 96);
            this.zone5Btn.TabIndex = 4;
            this.zone5Btn.Text = "托盘5位置";
            this.zone5Btn.UseVisualStyleBackColor = true;
            this.zone5Btn.Click += new System.EventHandler(this.zone5Btn_Click);
            // 
            // zone4Btn
            // 
            this.zone4Btn.Location = new System.Drawing.Point(59, 196);
            this.zone4Btn.Name = "zone4Btn";
            this.zone4Btn.Size = new System.Drawing.Size(139, 96);
            this.zone4Btn.TabIndex = 3;
            this.zone4Btn.Text = "托盘4位置";
            this.zone4Btn.UseVisualStyleBackColor = true;
            this.zone4Btn.Click += new System.EventHandler(this.zone4Btn_Click);
            // 
            // zone3Btn
            // 
            this.zone3Btn.Location = new System.Drawing.Point(377, 70);
            this.zone3Btn.Name = "zone3Btn";
            this.zone3Btn.Size = new System.Drawing.Size(139, 96);
            this.zone3Btn.TabIndex = 2;
            this.zone3Btn.Text = "托盘3位置";
            this.zone3Btn.UseVisualStyleBackColor = true;
            this.zone3Btn.Click += new System.EventHandler(this.zone3Btn_Click);
            // 
            // zone2Btn
            // 
            this.zone2Btn.Location = new System.Drawing.Point(216, 70);
            this.zone2Btn.Name = "zone2Btn";
            this.zone2Btn.Size = new System.Drawing.Size(139, 96);
            this.zone2Btn.TabIndex = 1;
            this.zone2Btn.Text = "托盘2位置";
            this.zone2Btn.UseVisualStyleBackColor = true;
            this.zone2Btn.Click += new System.EventHandler(this.zone2Btn_Click);
            // 
            // zone1Btn
            // 
            this.zone1Btn.Location = new System.Drawing.Point(59, 70);
            this.zone1Btn.Name = "zone1Btn";
            this.zone1Btn.Size = new System.Drawing.Size(139, 96);
            this.zone1Btn.TabIndex = 0;
            this.zone1Btn.Text = "托盘1位置";
            this.zone1Btn.UseVisualStyleBackColor = true;
            this.zone1Btn.Click += new System.EventHandler(this.zone1Btn_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtMaterielCode);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.txtCurrentCounts);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.txtCustomer);
            this.tabPage3.Controls.Add(this.label20);
            this.tabPage3.Controls.Add(this.txtMissionID);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.txtProZone);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txtTCPInfo);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.QrcCheckBtn);
            this.tabPage3.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage3.Location = new System.Drawing.Point(4, 34);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(595, 552);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "信息显示";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtMaterielCode
            // 
            this.txtMaterielCode.Location = new System.Drawing.Point(98, 77);
            this.txtMaterielCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaterielCode.Name = "txtMaterielCode";
            this.txtMaterielCode.ReadOnly = true;
            this.txtMaterielCode.Size = new System.Drawing.Size(154, 33);
            this.txtMaterielCode.TabIndex = 71;
            this.txtMaterielCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 85);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 25);
            this.label7.TabIndex = 70;
            this.label7.Text = "物料代码：";
            // 
            // txtCurrentCounts
            // 
            this.txtCurrentCounts.Location = new System.Drawing.Point(427, 76);
            this.txtCurrentCounts.Margin = new System.Windows.Forms.Padding(2);
            this.txtCurrentCounts.Name = "txtCurrentCounts";
            this.txtCurrentCounts.ReadOnly = true;
            this.txtCurrentCounts.Size = new System.Drawing.Size(102, 33);
            this.txtCurrentCounts.TabIndex = 69;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(294, 80);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 25);
            this.label13.TabIndex = 68;
            this.label13.Text = "已堆数量：";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(98, 121);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(154, 33);
            this.txtCustomer.TabIndex = 67;
            this.txtCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(5, 131);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(107, 25);
            this.label20.TabIndex = 66;
            this.label20.Text = "客户名称：";
            // 
            // txtMissionID
            // 
            this.txtMissionID.Location = new System.Drawing.Point(98, 26);
            this.txtMissionID.Margin = new System.Windows.Forms.Padding(2);
            this.txtMissionID.Name = "txtMissionID";
            this.txtMissionID.ReadOnly = true;
            this.txtMissionID.Size = new System.Drawing.Size(154, 33);
            this.txtMissionID.TabIndex = 61;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 30);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 25);
            this.label8.TabIndex = 60;
            this.label8.Text = "任务单号：";
            // 
            // txtProZone
            // 
            this.txtProZone.Location = new System.Drawing.Point(427, 26);
            this.txtProZone.Margin = new System.Windows.Forms.Padding(2);
            this.txtProZone.Name = "txtProZone";
            this.txtProZone.ReadOnly = true;
            this.txtProZone.Size = new System.Drawing.Size(102, 33);
            this.txtProZone.TabIndex = 59;
            this.txtProZone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(294, 30);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 25);
            this.label11.TabIndex = 58;
            this.label11.Text = "所属垛区：";
            // 
            // txtTCPInfo
            // 
            this.txtTCPInfo.Location = new System.Drawing.Point(22, 184);
            this.txtTCPInfo.Multiline = true;
            this.txtTCPInfo.Name = "txtTCPInfo";
            this.txtTCPInfo.Size = new System.Drawing.Size(554, 362);
            this.txtTCPInfo.TabIndex = 51;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(8, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 25);
            this.label5.TabIndex = 50;
            this.label5.Text = "信息显示：";
            // 
            // QrcCheckBtn
            // 
            this.QrcCheckBtn.BackColor = System.Drawing.Color.MistyRose;
            this.QrcCheckBtn.Location = new System.Drawing.Point(299, 131);
            this.QrcCheckBtn.Name = "QrcCheckBtn";
            this.QrcCheckBtn.Size = new System.Drawing.Size(183, 47);
            this.QrcCheckBtn.TabIndex = 58;
            this.QrcCheckBtn.Text = "测试条码";
            this.QrcCheckBtn.UseVisualStyleBackColor = false;
            this.QrcCheckBtn.Click += new System.EventHandler(this.QrcCheckBtn_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnClearZone5);
            this.tabPage4.Controls.Add(this.btnClearZone4);
            this.tabPage4.Controls.Add(this.btnClearZone3);
            this.tabPage4.Controls.Add(this.btnClearZone2);
            this.tabPage4.Controls.Add(this.btnClearZone1);
            this.tabPage4.Controls.Add(this.btnClearZone0);
            this.tabPage4.Location = new System.Drawing.Point(4, 34);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(595, 552);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "清空托盘";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnClearZone5
            // 
            this.btnClearZone5.Location = new System.Drawing.Point(398, 155);
            this.btnClearZone5.Name = "btnClearZone5";
            this.btnClearZone5.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone5.TabIndex = 63;
            this.btnClearZone5.Text = "托盘5清空";
            this.btnClearZone5.UseVisualStyleBackColor = true;
            this.btnClearZone5.Click += new System.EventHandler(this.btnClearZone5_Click);
            // 
            // btnClearZone4
            // 
            this.btnClearZone4.Location = new System.Drawing.Point(202, 155);
            this.btnClearZone4.Name = "btnClearZone4";
            this.btnClearZone4.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone4.TabIndex = 62;
            this.btnClearZone4.Text = "托盘4清空";
            this.btnClearZone4.UseVisualStyleBackColor = true;
            this.btnClearZone4.Click += new System.EventHandler(this.btnClearZone4_Click);
            // 
            // btnClearZone3
            // 
            this.btnClearZone3.Location = new System.Drawing.Point(9, 155);
            this.btnClearZone3.Name = "btnClearZone3";
            this.btnClearZone3.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone3.TabIndex = 61;
            this.btnClearZone3.Text = "托盘3清空";
            this.btnClearZone3.UseVisualStyleBackColor = true;
            this.btnClearZone3.Click += new System.EventHandler(this.btnClearZone3_Click);
            // 
            // btnClearZone2
            // 
            this.btnClearZone2.Location = new System.Drawing.Point(398, 77);
            this.btnClearZone2.Name = "btnClearZone2";
            this.btnClearZone2.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone2.TabIndex = 60;
            this.btnClearZone2.Text = "托盘2清空";
            this.btnClearZone2.UseVisualStyleBackColor = true;
            this.btnClearZone2.Click += new System.EventHandler(this.btnClearZone2_Click);
            // 
            // btnClearZone1
            // 
            this.btnClearZone1.Location = new System.Drawing.Point(202, 77);
            this.btnClearZone1.Name = "btnClearZone1";
            this.btnClearZone1.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone1.TabIndex = 59;
            this.btnClearZone1.Text = "托盘1清空";
            this.btnClearZone1.UseVisualStyleBackColor = true;
            this.btnClearZone1.Click += new System.EventHandler(this.btnClearZone1_Click);
            // 
            // btnClearZone0
            // 
            this.btnClearZone0.Location = new System.Drawing.Point(9, 77);
            this.btnClearZone0.Name = "btnClearZone0";
            this.btnClearZone0.Size = new System.Drawing.Size(167, 62);
            this.btnClearZone0.TabIndex = 58;
            this.btnClearZone0.Text = "不良区清空";
            this.btnClearZone0.UseVisualStyleBackColor = true;
            this.btnClearZone0.Click += new System.EventHandler(this.btnClearZone0_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(809, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 25);
            this.label6.TabIndex = 54;
            this.label6.Text = "条码显示：";
            // 
            // txtQRC
            // 
            this.txtQRC.Location = new System.Drawing.Point(922, 162);
            this.txtQRC.Multiline = true;
            this.txtQRC.Name = "txtQRC";
            this.txtQRC.Size = new System.Drawing.Size(384, 44);
            this.txtQRC.TabIndex = 53;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "编号";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "Total";
            this.Total.HeaderText = "总数";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 80;
            // 
            // Zone
            // 
            this.Zone.DataPropertyName = "Zone";
            this.Zone.HeaderText = "垛区";
            this.Zone.Name = "Zone";
            this.Zone.ReadOnly = true;
            this.Zone.Width = 80;
            // 
            // Numbers
            // 
            this.Numbers.DataPropertyName = "PerZoneNumbers";
            this.Numbers.HeaderText = "个数/垛";
            this.Numbers.Name = "Numbers";
            this.Numbers.ReadOnly = true;
            this.Numbers.Width = 110;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CurrentCounts";
            this.Column1.HeaderText = "已堆数量";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 115;
            // 
            // Direction
            // 
            this.Direction.DataPropertyName = "Direction";
            this.Direction.HeaderText = "方向";
            this.Direction.Name = "Direction";
            this.Direction.ReadOnly = true;
            this.Direction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Direction.Width = 80;
            // 
            // btnProSet
            // 
            this.btnProSet.BackColor = System.Drawing.Color.MistyRose;
            this.btnProSet.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnProSet.Location = new System.Drawing.Point(799, 85);
            this.btnProSet.Name = "btnProSet";
            this.btnProSet.Size = new System.Drawing.Size(114, 41);
            this.btnProSet.TabIndex = 55;
            this.btnProSet.Text = "产品设置";
            this.btnProSet.UseVisualStyleBackColor = false;
            this.btnProSet.Click += new System.EventHandler(this.btnProSet_Click);
            // 
            // btnInitPos
            // 
            this.btnInitPos.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInitPos.Location = new System.Drawing.Point(619, 662);
            this.btnInitPos.Name = "btnInitPos";
            this.btnInitPos.Size = new System.Drawing.Size(113, 41);
            this.btnInitPos.TabIndex = 0;
            this.btnInitPos.Text = "初始位置";
            this.btnInitPos.UseVisualStyleBackColor = true;
            this.btnInitPos.Click += new System.EventHandler(this.btnInitPos_Click);
            // 
            // btnStartServers
            // 
            this.btnStartServers.BackColor = System.Drawing.SystemColors.Desktop;
            this.btnStartServers.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStartServers.Location = new System.Drawing.Point(1046, 23);
            this.btnStartServers.Name = "btnStartServers";
            this.btnStartServers.Size = new System.Drawing.Size(113, 41);
            this.btnStartServers.TabIndex = 48;
            this.btnStartServers.Text = "启动服务器";
            this.btnStartServers.UseVisualStyleBackColor = false;
            this.btnStartServers.Click += new System.EventHandler(this.btnStartServers_Click);
            // 
            // btn3DForm
            // 
            this.btn3DForm.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn3DForm.Location = new System.Drawing.Point(619, 709);
            this.btn3DForm.Name = "btn3DForm";
            this.btn3DForm.Size = new System.Drawing.Size(113, 41);
            this.btn3DForm.TabIndex = 1;
            this.btn3DForm.Text = "启动3D窗体";
            this.btn3DForm.UseVisualStyleBackColor = true;
            this.btn3DForm.Click += new System.EventHandler(this.btn3DForm_Click);
            // 
            // btnWorking
            // 
            this.btnWorking.BackColor = System.Drawing.Color.Lime;
            this.btnWorking.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWorking.Location = new System.Drawing.Point(1209, 23);
            this.btnWorking.Name = "btnWorking";
            this.btnWorking.Size = new System.Drawing.Size(120, 55);
            this.btnWorking.TabIndex = 1;
            this.btnWorking.Text = "启动";
            this.btnWorking.UseVisualStyleBackColor = false;
            this.btnWorking.Click += new System.EventHandler(this.btnWorking_Click);
            // 
            // btnModeChange
            // 
            this.btnModeChange.BackColor = System.Drawing.Color.ForestGreen;
            this.btnModeChange.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnModeChange.Location = new System.Drawing.Point(931, 85);
            this.btnModeChange.Name = "btnModeChange";
            this.btnModeChange.Size = new System.Drawing.Size(107, 41);
            this.btnModeChange.TabIndex = 56;
            this.btnModeChange.Text = "小车运送";
            this.btnModeChange.UseVisualStyleBackColor = false;
            this.btnModeChange.Click += new System.EventHandler(this.btnModeChange_Click);
            // 
            // carArriveStatusBtn
            // 
            this.carArriveStatusBtn.BackColor = System.Drawing.Color.CornflowerBlue;
            this.carArriveStatusBtn.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.carArriveStatusBtn.Location = new System.Drawing.Point(1209, 85);
            this.carArriveStatusBtn.Name = "carArriveStatusBtn";
            this.carArriveStatusBtn.Size = new System.Drawing.Size(76, 49);
            this.carArriveStatusBtn.TabIndex = 57;
            this.carArriveStatusBtn.Text = "堆垛完成 小车离开";
            this.carArriveStatusBtn.UseVisualStyleBackColor = false;
            this.carArriveStatusBtn.Click += new System.EventHandler(this.carArriveStatusBtn_Click);
            // 
            // btnClearAlarm
            // 
            this.btnClearAlarm.BackColor = System.Drawing.Color.SandyBrown;
            this.btnClearAlarm.Font = new System.Drawing.Font("微软雅黑", 14.25F);
            this.btnClearAlarm.Location = new System.Drawing.Point(1046, 85);
            this.btnClearAlarm.Name = "btnClearAlarm";
            this.btnClearAlarm.Size = new System.Drawing.Size(113, 41);
            this.btnClearAlarm.TabIndex = 59;
            this.btnClearAlarm.Text = "清除报警";
            this.btnClearAlarm.UseVisualStyleBackColor = false;
            this.btnClearAlarm.Click += new System.EventHandler(this.btnClearAlarm_Click);
            // 
            // Mainform
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1284, 841);
            this.Controls.Add(this.btnClearAlarm);
            this.Controls.Add(this.carArriveStatusBtn);
            this.Controls.Add(this.btnModeChange);
            this.Controls.Add(this.txtQRC);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnProSet);
            this.Controls.Add(this.btnWorking);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ptbDisplay);
            this.Controls.Add(this.Img_modeBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnInitPos);
            this.Controls.Add(this.btnStartServers);
            this.Controls.Add(this.btn3DForm);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Mainform";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "1904089N86068";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Mainform_FormClosing);
            this.Load += new System.EventHandler(this.Mainform_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ptbDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Img_modeBox;
        private System.Windows.Forms.PictureBox ptbDisplay;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtSampleResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSampling;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Button btn3DForm;
        private System.Windows.Forms.Button btnStartServers;
        private System.Windows.Forms.Button btnInitPos;
        private System.Windows.Forms.TextBox txtTCPInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtQRC;
        private System.Windows.Forms.Button btnWorking;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Numbers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button zone5Btn;
        private System.Windows.Forms.Button zone4Btn;
        private System.Windows.Forms.Button zone3Btn;
        private System.Windows.Forms.Button zone2Btn;
        private System.Windows.Forms.Button zone1Btn;
        private System.Windows.Forms.Button btnProSet;
        private System.Windows.Forms.Button btnModeChange;
        private System.Windows.Forms.TextBox txtProZone;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtMissionID;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtCurrentCounts;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button btnClearZone5;
        private System.Windows.Forms.Button btnClearZone4;
        private System.Windows.Forms.Button btnClearZone3;
        private System.Windows.Forms.Button btnClearZone2;
        private System.Windows.Forms.Button btnClearZone1;
        private System.Windows.Forms.Button btnClearZone0;
        private System.Windows.Forms.Button carArriveStatusBtn;
        private System.Windows.Forms.Button QrcCheckBtn;
        private System.Windows.Forms.Button btnClearAlarm;
        private System.Windows.Forms.TextBox txtMaterielCode;
        private System.Windows.Forms.Label label7;
    }
}

