namespace 技能编辑器
{
    partial class Frame
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Frame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "Frame";
            this.Size = new System.Drawing.Size(1057, 150);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Frame_Paint);
            this.Enter += new System.EventHandler(this.Frame_Enter);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Frame_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Frame_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Frame_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Frame_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    }
}
