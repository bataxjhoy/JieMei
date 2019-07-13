namespace JieMei
{
    partial class ProductSetForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProductSetForm));
            this.dgvProductSetting = new System.Windows.Forms.DataGridView();
            this.gpbProductSet = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtComepleteNum = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtMissionID = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtMaterielCode = new System.Windows.Forms.TextBox();
            this.txtRobotID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtCurrentCounts = new System.Windows.Forms.TextBox();
            this.txtProNum = new System.Windows.Forms.TextBox();
            this.txtProDirection = new System.Windows.Forms.TextBox();
            this.txtProZone = new System.Windows.Forms.TextBox();
            this.txtProTotal = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnChangeProduct = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.completeStowCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Zone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComepleteNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Numbers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label6 = new System.Windows.Forms.Label();
            this.txtcompleteStowCode = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSetting)).BeginInit();
            this.gpbProductSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvProductSetting
            // 
            this.dgvProductSetting.AllowUserToAddRows = false;
            this.dgvProductSetting.AllowUserToDeleteRows = false;
            this.dgvProductSetting.AllowUserToResizeColumns = false;
            this.dgvProductSetting.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvProductSetting.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProductSetting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProductSetting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.completeStowCode,
            this.Column3,
            this.Column5,
            this.Column4,
            this.Zone,
            this.Total,
            this.ComepleteNum,
            this.Column1,
            this.Numbers,
            this.Direction,
            this.Column2});
            this.dgvProductSetting.Location = new System.Drawing.Point(20, 12);
            this.dgvProductSetting.Name = "dgvProductSetting";
            this.dgvProductSetting.ReadOnly = true;
            this.dgvProductSetting.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Blue;
            this.dgvProductSetting.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProductSetting.RowTemplate.Height = 23;
            this.dgvProductSetting.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProductSetting.Size = new System.Drawing.Size(1246, 426);
            this.dgvProductSetting.TabIndex = 1;
            this.dgvProductSetting.SelectionChanged += new System.EventHandler(this.dgvProductSetting_SelectionChanged);
            // 
            // gpbProductSet
            // 
            this.gpbProductSet.Controls.Add(this.txtcompleteStowCode);
            this.gpbProductSet.Controls.Add(this.label6);
            this.gpbProductSet.Controls.Add(this.label12);
            this.gpbProductSet.Controls.Add(this.txtComepleteNum);
            this.gpbProductSet.Controls.Add(this.label1);
            this.gpbProductSet.Controls.Add(this.txtSpec);
            this.gpbProductSet.Controls.Add(this.label21);
            this.gpbProductSet.Controls.Add(this.txtCustomer);
            this.gpbProductSet.Controls.Add(this.txtMissionID);
            this.gpbProductSet.Controls.Add(this.label20);
            this.gpbProductSet.Controls.Add(this.txtMaterielCode);
            this.gpbProductSet.Controls.Add(this.txtRobotID);
            this.gpbProductSet.Controls.Add(this.label2);
            this.gpbProductSet.Controls.Add(this.label3);
            this.gpbProductSet.Controls.Add(this.label4);
            this.gpbProductSet.Controls.Add(this.txtCurrentCounts);
            this.gpbProductSet.Controls.Add(this.txtProNum);
            this.gpbProductSet.Controls.Add(this.txtProDirection);
            this.gpbProductSet.Controls.Add(this.txtProZone);
            this.gpbProductSet.Controls.Add(this.txtProTotal);
            this.gpbProductSet.Controls.Add(this.label11);
            this.gpbProductSet.Controls.Add(this.label13);
            this.gpbProductSet.Controls.Add(this.label10);
            this.gpbProductSet.Controls.Add(this.label9);
            this.gpbProductSet.Controls.Add(this.label8);
            this.gpbProductSet.Location = new System.Drawing.Point(20, 443);
            this.gpbProductSet.Margin = new System.Windows.Forms.Padding(2);
            this.gpbProductSet.Name = "gpbProductSet";
            this.gpbProductSet.Padding = new System.Windows.Forms.Padding(2);
            this.gpbProductSet.Size = new System.Drawing.Size(1279, 138);
            this.gpbProductSet.TabIndex = 6;
            this.gpbProductSet.TabStop = false;
            this.gpbProductSet.Text = "产品设置详情";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label12.Location = new System.Drawing.Point(885, 116);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(390, 20);
            this.label12.TabIndex = 14;
            this.label12.Text = "注：码垛方向：1->一个方向，2->2个方向，4->4个方向";
            // 
            // txtComepleteNum
            // 
            this.txtComepleteNum.Location = new System.Drawing.Point(767, 25);
            this.txtComepleteNum.Margin = new System.Windows.Forms.Padding(2);
            this.txtComepleteNum.Name = "txtComepleteNum";
            this.txtComepleteNum.Size = new System.Drawing.Size(102, 21);
            this.txtComepleteNum.TabIndex = 20;
            this.txtComepleteNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(679, 28);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "完成数量：";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(1003, 68);
            this.txtSpec.Margin = new System.Windows.Forms.Padding(2);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(102, 21);
            this.txtSpec.TabIndex = 18;
            this.txtSpec.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(934, 71);
            this.label21.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 12);
            this.label21.TabIndex = 17;
            this.label21.Text = "规格：";
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(767, 68);
            this.txtCustomer.Margin = new System.Windows.Forms.Padding(2);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.ReadOnly = true;
            this.txtCustomer.Size = new System.Drawing.Size(102, 21);
            this.txtCustomer.TabIndex = 16;
            this.txtCustomer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMissionID
            // 
            this.txtMissionID.Location = new System.Drawing.Point(80, 68);
            this.txtMissionID.Margin = new System.Windows.Forms.Padding(2);
            this.txtMissionID.Name = "txtMissionID";
            this.txtMissionID.Size = new System.Drawing.Size(102, 21);
            this.txtMissionID.TabIndex = 14;
            this.txtMissionID.TextChanged += new System.EventHandler(this.txtMissionID_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(679, 77);
            this.label20.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(65, 12);
            this.label20.TabIndex = 15;
            this.label20.Text = "客户名称：";
            // 
            // txtMaterielCode
            // 
            this.txtMaterielCode.Location = new System.Drawing.Point(552, 68);
            this.txtMaterielCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaterielCode.Name = "txtMaterielCode";
            this.txtMaterielCode.Size = new System.Drawing.Size(102, 21);
            this.txtMaterielCode.TabIndex = 11;
            this.txtMaterielCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMaterielCode.TextChanged += new System.EventHandler(this.txtMaterielCode_TextChanged);
            // 
            // txtRobotID
            // 
            this.txtRobotID.Location = new System.Drawing.Point(80, 22);
            this.txtRobotID.Margin = new System.Windows.Forms.Padding(2);
            this.txtRobotID.Name = "txtRobotID";
            this.txtRobotID.Size = new System.Drawing.Size(102, 21);
            this.txtRobotID.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 77);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "任务单号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(469, 77);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "物料代码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "机器人ID：";
            // 
            // txtCurrentCounts
            // 
            this.txtCurrentCounts.Location = new System.Drawing.Point(1003, 25);
            this.txtCurrentCounts.Margin = new System.Windows.Forms.Padding(2);
            this.txtCurrentCounts.Name = "txtCurrentCounts";
            this.txtCurrentCounts.Size = new System.Drawing.Size(102, 21);
            this.txtCurrentCounts.TabIndex = 2;
            // 
            // txtProNum
            // 
            this.txtProNum.Location = new System.Drawing.Point(1195, 68);
            this.txtProNum.Margin = new System.Windows.Forms.Padding(2);
            this.txtProNum.Name = "txtProNum";
            this.txtProNum.ReadOnly = true;
            this.txtProNum.Size = new System.Drawing.Size(80, 21);
            this.txtProNum.TabIndex = 1;
            this.txtProNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtProDirection
            // 
            this.txtProDirection.Location = new System.Drawing.Point(1196, 25);
            this.txtProDirection.Margin = new System.Windows.Forms.Padding(2);
            this.txtProDirection.Name = "txtProDirection";
            this.txtProDirection.ReadOnly = true;
            this.txtProDirection.Size = new System.Drawing.Size(79, 21);
            this.txtProDirection.TabIndex = 1;
            this.txtProDirection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtProZone
            // 
            this.txtProZone.Location = new System.Drawing.Point(309, 22);
            this.txtProZone.Margin = new System.Windows.Forms.Padding(2);
            this.txtProZone.Name = "txtProZone";
            this.txtProZone.Size = new System.Drawing.Size(102, 21);
            this.txtProZone.TabIndex = 1;
            this.txtProZone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtProZone.TextChanged += new System.EventHandler(this.txtProZone_TextChanged);
            // 
            // txtProTotal
            // 
            this.txtProTotal.Location = new System.Drawing.Point(552, 22);
            this.txtProTotal.Margin = new System.Windows.Forms.Padding(2);
            this.txtProTotal.Name = "txtProTotal";
            this.txtProTotal.Size = new System.Drawing.Size(102, 21);
            this.txtProTotal.TabIndex = 1;
            this.txtProTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(240, 25);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "所属垛区：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(934, 28);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "已堆数量：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1131, 28);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "码垛方向：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1131, 77);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "每垛数量：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(469, 25);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "生产总数：";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.LightSalmon;
            this.btnCancel.Location = new System.Drawing.Point(1011, 631);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 41);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnSave.Location = new System.Drawing.Point(869, 631);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 41);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnChangeProduct
            // 
            this.btnChangeProduct.Location = new System.Drawing.Point(443, 631);
            this.btnChangeProduct.Name = "btnChangeProduct";
            this.btnChangeProduct.Size = new System.Drawing.Size(91, 41);
            this.btnChangeProduct.TabIndex = 11;
            this.btnChangeProduct.Text = "修改垛区数量";
            this.btnChangeProduct.UseVisualStyleBackColor = true;
            this.btnChangeProduct.Click += new System.EventHandler(this.btnChangeProduct_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(585, 631);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(91, 41);
            this.btnUpdate.TabIndex = 9;
            this.btnUpdate.Text = "刷新";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.Location = new System.Drawing.Point(727, 631);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(91, 41);
            this.btnDeleteProduct.TabIndex = 10;
            this.btnDeleteProduct.Text = "删除";
            this.btnDeleteProduct.UseVisualStyleBackColor = true;
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.Location = new System.Drawing.Point(301, 631);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(91, 41);
            this.btnAddProduct.TabIndex = 8;
            this.btnAddProduct.Text = "添加新订单";
            this.btnAddProduct.UseVisualStyleBackColor = true;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.label5.Location = new System.Drawing.Point(833, 694);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(433, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "注：添加到同一垛会覆盖之前的订单,修改只是改数量，不改订单";
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ID.DataPropertyName = "RobotID";
            this.ID.HeaderText = "机器人编号";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // completeStowCode
            // 
            this.completeStowCode.DataPropertyName = "completeStowCode";
            this.completeStowCode.HeaderText = "完成拖号";
            this.completeStowCode.Name = "completeStowCode";
            this.completeStowCode.ReadOnly = true;
            this.completeStowCode.Width = 120;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "MissionID";
            this.Column3.HeaderText = "任务单号";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 160;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "MaterielCode";
            this.Column5.HeaderText = "物料代码";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 160;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Customer";
            this.Column4.HeaderText = "客户名称";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 160;
            // 
            // Zone
            // 
            this.Zone.DataPropertyName = "Zone";
            this.Zone.HeaderText = "所属垛区";
            this.Zone.Name = "Zone";
            this.Zone.ReadOnly = true;
            this.Zone.Width = 80;
            // 
            // Total
            // 
            this.Total.DataPropertyName = "Total";
            this.Total.HeaderText = "生产总数";
            this.Total.Name = "Total";
            this.Total.ReadOnly = true;
            this.Total.Width = 80;
            // 
            // ComepleteNum
            // 
            this.ComepleteNum.DataPropertyName = "ComepleteNum";
            this.ComepleteNum.HeaderText = "完成数量";
            this.ComepleteNum.Name = "ComepleteNum";
            this.ComepleteNum.ReadOnly = true;
            this.ComepleteNum.Width = 80;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "CurrentCounts";
            this.Column1.HeaderText = "已堆数量";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 80;
            // 
            // Numbers
            // 
            this.Numbers.DataPropertyName = "PerZoneNumbers";
            this.Numbers.HeaderText = "每垛数量";
            this.Numbers.Name = "Numbers";
            this.Numbers.ReadOnly = true;
            this.Numbers.Visible = false;
            this.Numbers.Width = 80;
            // 
            // Direction
            // 
            this.Direction.DataPropertyName = "Direction";
            this.Direction.HeaderText = "码垛方向";
            this.Direction.Name = "Direction";
            this.Direction.ReadOnly = true;
            this.Direction.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Direction.Visible = false;
            this.Direction.Width = 80;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "Spec";
            this.Column2.HeaderText = "产品规格";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 120;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(240, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "完成托号：";
            // 
            // txtcompleteStowCode
            // 
            this.txtcompleteStowCode.Location = new System.Drawing.Point(309, 68);
            this.txtcompleteStowCode.Margin = new System.Windows.Forms.Padding(2);
            this.txtcompleteStowCode.Name = "txtcompleteStowCode";
            this.txtcompleteStowCode.Size = new System.Drawing.Size(102, 21);
            this.txtcompleteStowCode.TabIndex = 22;
            this.txtcompleteStowCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ProductSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 723);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnChangeProduct);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnAddProduct);
            this.Controls.Add(this.gpbProductSet);
            this.Controls.Add(this.dgvProductSetting);
            this.Controls.Add(this.btnDeleteProduct);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProductSetForm";
            this.Text = "产品设置";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProductSetForm_FormClosed);
            this.Load += new System.EventHandler(this.ProductSetForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProductSetting)).EndInit();
            this.gpbProductSet.ResumeLayout(false);
            this.gpbProductSet.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvProductSetting;
        private System.Windows.Forms.GroupBox gpbProductSet;
        private System.Windows.Forms.TextBox txtCurrentCounts;
        private System.Windows.Forms.TextBox txtProNum;
        private System.Windows.Forms.TextBox txtProDirection;
        private System.Windows.Forms.TextBox txtProZone;
        private System.Windows.Forms.TextBox txtProTotal;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnChangeProduct;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Button btnAddProduct;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtMissionID;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtMaterielCode;
        private System.Windows.Forms.TextBox txtRobotID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtComepleteNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn completeStowCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Zone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Total;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComepleteNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Numbers;
        private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.TextBox txtcompleteStowCode;
        private System.Windows.Forms.Label label6;
    }
}