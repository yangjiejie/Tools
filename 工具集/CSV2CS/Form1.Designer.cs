namespace CSV2CS
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnConverter = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.TestButton = new System.Windows.Forms.Button();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "路径";
            // 
            // tbPath
            // 
            this.tbPath.AllowDrop = true;
            this.tbPath.Location = new System.Drawing.Point(153, 78);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(235, 21);
            this.tbPath.TabIndex = 1;
            this.tbPath.TextChanged += new System.EventHandler(this.tbPath_TextChanged);
            this.tbPath.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbPath_DragDrop);
            this.tbPath.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbPath_DragEnter);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(415, 76);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 23);
            this.btnScan.TabIndex = 2;
            this.btnScan.Text = "浏览";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnConverter
            // 
            this.btnConverter.Location = new System.Drawing.Point(239, 345);
            this.btnConverter.Name = "btnConverter";
            this.btnConverter.Size = new System.Drawing.Size(75, 23);
            this.btnConverter.TabIndex = 4;
            this.btnConverter.Text = "生成代码";
            this.btnConverter.UseVisualStyleBackColor = true;
            this.btnConverter.Click += new System.EventHandler(this.btnConverter_Click);
            // 
            // listView1
            // 
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.LabelWrap = false;
            this.listView1.Location = new System.Drawing.Point(100, 125);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(370, 190);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(353, 345);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(75, 23);
            this.TestButton.TabIndex = 5;
            this.TestButton.Text = "测试";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.btTest_Click);
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Location = new System.Drawing.Point(78, 348);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(138, 16);
            this.CheckBox1.TabIndex = 6;
            this.CheckBox1.Text = "生成GetInstance函数";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "字典",
            "数组"});
            this.comboBox1.Location = new System.Drawing.Point(34, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 7;
            this.comboBox1.Text = this.comboBox1.Items[0].ToString();
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(210, 14);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(102, 16);
            this.checkBox2.TabIndex = 8;
            this.checkBox2.Text = "规范命名源CSV";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 399);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.CheckBox1);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnConverter);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.tbPath_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.tbPath_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnConverter;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.CheckBox CheckBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.CheckBox checkBox2;
    }
}

