namespace 技能编辑器
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
            this.components = new System.ComponentModel.Container();
            this.listBox_skills = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lbskill_tsmi_Refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.lbskill_tsmi_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.lbskill_tsmi_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_OpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel_Root = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.frame1 = new 技能编辑器.Frame();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmi_frame_Add = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_frame_copy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_frame_delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_frame_addframecount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_frame_appendframecount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_frame_deleteframecount = new System.Windows.Forms.ToolStripMenuItem();
            this.btnResetView = new System.Windows.Forms.Button();
            this.numericUpDown_align = new System.Windows.Forms.NumericUpDown();
            this.checkBox_Align = new System.Windows.Forms.CheckBox();
            this.boxView1 = new 技能编辑器.BoxView();
            this.label_BoxViewSize = new System.Windows.Forms.Label();
            this.trackBar_BoxViewSize = new System.Windows.Forms.TrackBar();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel_Root.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_align)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BoxViewSize)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox_skills
            // 
            this.listBox_skills.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox_skills.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox_skills.FormattingEnabled = true;
            this.listBox_skills.ItemHeight = 12;
            this.listBox_skills.Location = new System.Drawing.Point(3, 3);
            this.listBox_skills.Name = "listBox_skills";
            this.listBox_skills.Size = new System.Drawing.Size(156, 592);
            this.listBox_skills.TabIndex = 0;
            this.listBox_skills.SelectedIndexChanged += new System.EventHandler(this.listBox_skills_SelectedIndexChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbskill_tsmi_Refresh,
            this.lbskill_tsmi_Add,
            this.lbskill_tsmi_Delete});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 70);
            // 
            // lbskill_tsmi_Refresh
            // 
            this.lbskill_tsmi_Refresh.Name = "lbskill_tsmi_Refresh";
            this.lbskill_tsmi_Refresh.Size = new System.Drawing.Size(100, 22);
            this.lbskill_tsmi_Refresh.Text = "刷新";
            this.lbskill_tsmi_Refresh.Click += new System.EventHandler(this.lbskill_tsmi_Refresh_Click);
            // 
            // lbskill_tsmi_Add
            // 
            this.lbskill_tsmi_Add.Name = "lbskill_tsmi_Add";
            this.lbskill_tsmi_Add.Size = new System.Drawing.Size(100, 22);
            this.lbskill_tsmi_Add.Text = "添加";
            this.lbskill_tsmi_Add.Click += new System.EventHandler(this.lbskill_tsmi_Add_Click);
            // 
            // lbskill_tsmi_Delete
            // 
            this.lbskill_tsmi_Delete.Name = "lbskill_tsmi_Delete";
            this.lbskill_tsmi_Delete.Size = new System.Drawing.Size(100, 22);
            this.lbskill_tsmi_Delete.Text = "删除";
            this.lbskill_tsmi_Delete.Click += new System.EventHandler(this.lbskill_tsmi_Delete_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Location = new System.Drawing.Point(4, 5);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(293, 587);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            this.propertyGrid1.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid1_SelectedGridItemChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1229, 25);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_OpenFile,
            this.tsmi_Save,
            this.tsmi_Exit});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // tsmi_OpenFile
            // 
            this.tsmi_OpenFile.Name = "tsmi_OpenFile";
            this.tsmi_OpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmi_OpenFile.Size = new System.Drawing.Size(147, 22);
            this.tsmi_OpenFile.Text = "打开";
            this.tsmi_OpenFile.Click += new System.EventHandler(this.tsmi_OpenFile_Click);
            // 
            // tsmi_Save
            // 
            this.tsmi_Save.Name = "tsmi_Save";
            this.tsmi_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmi_Save.Size = new System.Drawing.Size(147, 22);
            this.tsmi_Save.Text = "保存";
            this.tsmi_Save.Click += new System.EventHandler(this.tsmi_Save_Click);
            // 
            // tsmi_Exit
            // 
            this.tsmi_Exit.Name = "tsmi_Exit";
            this.tsmi_Exit.Size = new System.Drawing.Size(147, 22);
            this.tsmi_Exit.Text = "退出";
            this.tsmi_Exit.Click += new System.EventHandler(this.tsmi_Exit_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // panel_Root
            // 
            this.panel_Root.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Root.Controls.Add(this.splitContainer2);
            this.panel_Root.Location = new System.Drawing.Point(13, 29);
            this.panel_Root.Name = "panel_Root";
            this.panel_Root.Size = new System.Drawing.Size(1216, 606);
            this.panel_Root.TabIndex = 5;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.Location = new System.Drawing.Point(3, 8);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listBox_skills);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(1201, 595);
            this.splitContainer2.SplitterDistance = 162;
            this.splitContainer2.TabIndex = 5;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer3.Size = new System.Drawing.Size(1035, 595);
            this.splitContainer3.SplitterDistance = 731;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.splitContainer1.Location = new System.Drawing.Point(3, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel1.Controls.Add(this.frame1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Panel2.Controls.Add(this.btnResetView);
            this.splitContainer1.Panel2.Controls.Add(this.numericUpDown_align);
            this.splitContainer1.Panel2.Controls.Add(this.checkBox_Align);
            this.splitContainer1.Panel2.Controls.Add(this.boxView1);
            this.splitContainer1.Panel2.Controls.Add(this.label_BoxViewSize);
            this.splitContainer1.Panel2.Controls.Add(this.trackBar_BoxViewSize);
            this.splitContainer1.Size = new System.Drawing.Size(725, 587);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.TabIndex = 4;
            // 
            // frame1
            // 
            this.frame1.ContextMenuStrip = this.contextMenuStrip2;
            this.frame1.Location = new System.Drawing.Point(3, 3);
            this.frame1.Name = "frame1";
            this.frame1.Size = new System.Drawing.Size(1057, 135);
            this.frame1.TabIndex = 0;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_frame_Add,
            this.tsmi_frame_copy,
            this.tsmi_frame_delete,
            this.tsmi_frame_addframecount,
            this.tsmi_frame_appendframecount,
            this.tsmi_frame_deleteframecount});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(113, 136);
            // 
            // tsmi_frame_Add
            // 
            this.tsmi_frame_Add.Name = "tsmi_frame_Add";
            this.tsmi_frame_Add.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_Add.Text = "添加";
            this.tsmi_frame_Add.Click += new System.EventHandler(this.tsmi_frame_Add_Click);
            // 
            // tsmi_frame_copy
            // 
            this.tsmi_frame_copy.Name = "tsmi_frame_copy";
            this.tsmi_frame_copy.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_copy.Text = "复制";
            this.tsmi_frame_copy.Click += new System.EventHandler(this.tsmi_frame_copy_Click);
            // 
            // tsmi_frame_delete
            // 
            this.tsmi_frame_delete.Name = "tsmi_frame_delete";
            this.tsmi_frame_delete.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_delete.Text = "删除";
            this.tsmi_frame_delete.Click += new System.EventHandler(this.tsmi_frame_delete_Click);
            // 
            // tsmi_frame_addframecount
            // 
            this.tsmi_frame_addframecount.Name = "tsmi_frame_addframecount";
            this.tsmi_frame_addframecount.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_addframecount.Text = "插入帧";
            this.tsmi_frame_addframecount.Click += new System.EventHandler(this.tsmi_frame_addframecount_Click);
            // 
            // tsmi_frame_appendframecount
            // 
            this.tsmi_frame_appendframecount.Name = "tsmi_frame_appendframecount";
            this.tsmi_frame_appendframecount.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_appendframecount.Text = "附加帧";
            this.tsmi_frame_appendframecount.Click += new System.EventHandler(this.tsmi_frame_appendframecount_Click);
            // 
            // tsmi_frame_deleteframecount
            // 
            this.tsmi_frame_deleteframecount.Name = "tsmi_frame_deleteframecount";
            this.tsmi_frame_deleteframecount.Size = new System.Drawing.Size(112, 22);
            this.tsmi_frame_deleteframecount.Text = "删除帧";
            this.tsmi_frame_deleteframecount.Click += new System.EventHandler(this.tsmi_frame_deleteframecount_Click);
            // 
            // btnResetView
            // 
            this.btnResetView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetView.Location = new System.Drawing.Point(637, 6);
            this.btnResetView.Name = "btnResetView";
            this.btnResetView.Size = new System.Drawing.Size(75, 23);
            this.btnResetView.TabIndex = 8;
            this.btnResetView.Text = "重置";
            this.btnResetView.UseVisualStyleBackColor = true;
            this.btnResetView.Click += new System.EventHandler(this.btnResetView_Click);
            // 
            // numericUpDown_align
            // 
            this.numericUpDown_align.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown_align.DecimalPlaces = 2;
            this.numericUpDown_align.Location = new System.Drawing.Point(476, 9);
            this.numericUpDown_align.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_align.Name = "numericUpDown_align";
            this.numericUpDown_align.Size = new System.Drawing.Size(120, 21);
            this.numericUpDown_align.TabIndex = 7;
            this.numericUpDown_align.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_align.ValueChanged += new System.EventHandler(this.numericUpDown_align_ValueChanged);
            // 
            // checkBox_Align
            // 
            this.checkBox_Align.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_Align.AutoSize = true;
            this.checkBox_Align.Checked = true;
            this.checkBox_Align.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Align.Location = new System.Drawing.Point(401, 12);
            this.checkBox_Align.Name = "checkBox_Align";
            this.checkBox_Align.Size = new System.Drawing.Size(72, 16);
            this.checkBox_Align.TabIndex = 5;
            this.checkBox_Align.Text = "对齐网格";
            this.checkBox_Align.UseVisualStyleBackColor = true;
            this.checkBox_Align.CheckedChanged += new System.EventHandler(this.checkBox_Align_CheckedChanged);
            // 
            // boxView1
            // 
            this.boxView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.boxView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.boxView1.center_offset = new System.Drawing.Point(0, 0);
            this.boxView1.listener = null;
            this.boxView1.Location = new System.Drawing.Point(3, 39);
            this.boxView1.Name = "boxView1";
            this.boxView1.Size = new System.Drawing.Size(719, 345);
            this.boxView1.snap_gird = true;
            this.boxView1.snap_size = 1F;
            this.boxView1.TabIndex = 0;
            // 
            // label_BoxViewSize
            // 
            this.label_BoxViewSize.AutoSize = true;
            this.label_BoxViewSize.Location = new System.Drawing.Point(160, 18);
            this.label_BoxViewSize.Name = "label_BoxViewSize";
            this.label_BoxViewSize.Size = new System.Drawing.Size(29, 12);
            this.label_BoxViewSize.TabIndex = 2;
            this.label_BoxViewSize.Text = "100%";
            // 
            // trackBar_BoxViewSize
            // 
            this.trackBar_BoxViewSize.Location = new System.Drawing.Point(1, 3);
            this.trackBar_BoxViewSize.Maximum = 25;
            this.trackBar_BoxViewSize.Name = "trackBar_BoxViewSize";
            this.trackBar_BoxViewSize.Size = new System.Drawing.Size(150, 45);
            this.trackBar_BoxViewSize.TabIndex = 1;
            this.trackBar_BoxViewSize.Value = 5;
            this.trackBar_BoxViewSize.Scroll += new System.EventHandler(this.trackBar_BoxViewSize_Scroll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1229, 636);
            this.Controls.Add(this.panel_Root);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "技能编辑器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel_Root.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_align)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_BoxViewSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_skills;
        private Frame frame1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmi_OpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Save;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Exit;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.Panel panel_Root;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem lbskill_tsmi_Refresh;
        private System.Windows.Forms.ToolStripMenuItem lbskill_tsmi_Add;
        private System.Windows.Forms.ToolStripMenuItem lbskill_tsmi_Delete;
        private System.Windows.Forms.TrackBar trackBar_BoxViewSize;
        private System.Windows.Forms.Label label_BoxViewSize;
        private BoxView boxView1;
        private System.Windows.Forms.CheckBox checkBox_Align;
        private System.Windows.Forms.NumericUpDown numericUpDown_align;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_Add;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_copy;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_delete;
        private System.Windows.Forms.Button btnResetView;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_addframecount;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_appendframecount;
        private System.Windows.Forms.ToolStripMenuItem tsmi_frame_deleteframecount;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
    }
}

