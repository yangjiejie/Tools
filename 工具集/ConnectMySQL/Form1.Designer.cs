namespace ConnectMySQL
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.server = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userid = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.connectBtn = new System.Windows.Forms.Button();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.databaseList = new System.Windows.Forms.ComboBox();
            this.tables = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // server
            // 
            this.server.Location = new System.Drawing.Point(104, 40);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(100, 21);
            this.server.TabIndex = 0;
            this.server.Text = "localhost";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ServerId";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "UserId";
            // 
            // userid
            // 
            this.userid.Location = new System.Drawing.Point(304, 40);
            this.userid.Name = "userid";
            this.userid.Size = new System.Drawing.Size(100, 21);
            this.userid.TabIndex = 3;
            this.userid.Text = "root";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(464, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(536, 40);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 21);
            this.password.TabIndex = 5;
            this.password.Text = "123456";
            // 
            // connectBtn
            // 
            this.connectBtn.Location = new System.Drawing.Point(696, 40);
            this.connectBtn.Name = "connectBtn";
            this.connectBtn.Size = new System.Drawing.Size(88, 23);
            this.connectBtn.TabIndex = 6;
            this.connectBtn.Text = "连接数据库";
            this.connectBtn.UseVisualStyleBackColor = true;
            this.connectBtn.Click += new System.EventHandler(this.connectBtn_Click);
            // 
            // dataGrid
            // 
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Location = new System.Drawing.Point(208, 96);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RowTemplate.Height = 23;
            this.dataGrid.Size = new System.Drawing.Size(784, 424);
            this.dataGrid.TabIndex = 7;
            // 
            // databaseList
            // 
            this.databaseList.FormattingEnabled = true;
            this.databaseList.Location = new System.Drawing.Point(32, 96);
            this.databaseList.Name = "databaseList";
            this.databaseList.Size = new System.Drawing.Size(121, 20);
            this.databaseList.TabIndex = 9;
            this.databaseList.SelectedIndexChanged += new System.EventHandler(this.databaseList_SelectedIndexChanged);
            // 
            // tables
            // 
            this.tables.FormattingEnabled = true;
            this.tables.ItemHeight = 12;
            this.tables.Location = new System.Drawing.Point(32, 144);
            this.tables.Name = "tables";
            this.tables.Size = new System.Drawing.Size(136, 376);
            this.tables.TabIndex = 11;
            this.tables.SelectedIndexChanged += new System.EventHandler(this.tables_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 556);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tables);
            this.Controls.Add(this.databaseList);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.connectBtn);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.userid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.server);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConnectMySQL";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userid;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button connectBtn;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.ComboBox databaseList;
        private System.Windows.Forms.ListBox tables;
    }
}

