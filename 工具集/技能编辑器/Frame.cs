using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace 技能编辑器
{
    public partial class Frame : UserControl
    {
        public config_skill _skill;
        public int selected_frame_index;

        int unit_width = 10;
        int unit_height = 20;
        int title_width = 100; //左边标题宽度
        int time_scale_height = 20; //刻度文字高度

        public delegate void Event_OnSelectFrame(int frame);
        public delegate void Event_OnAddFrame();
        public delegate void Event_OnDelFrame();

        public Event_OnSelectFrame onSelectFrame;
        public Event_OnAddFrame onAddFrame;
        public Event_OnDelFrame onDelFrame;

        public config_skill_frame selected_frame
        {
            get
            {
                if(selected_frame_index!=-1 && _skill!=null)
                {
                    return _skill.frames[selected_frame_index];
                }
                return null;
            }
        }

        int selected_start_frame_index;
        int selected_end_frame_index;


        enum EditMode
        {
            None,
            Move,
            SelectFrame
        }
        EditMode _mode;
        Point _lastMouseLocation;

        public Frame()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            Type typeSkillFrameAction = typeof(SkillFrameAction);
            foreach (var s in typeSkillFrameAction.GetEnumNames())
            {
                System.Reflection.FieldInfo fi = typeSkillFrameAction.GetField(s);
                EditorNameAttribute menuNameAttribute = XPropDescriptor.GetCustomAttribute<EditorNameAttribute>(fi);
                List<string> menuItems = new List<string>();
                if (menuNameAttribute==null)
                {
                    menuItems.Add("通用");
                    menuItems.Add(s);
                }
                else
                {
                    menuItems.AddRange(menuNameAttribute.EditorName.Split('/'));
                    menuItems.Add(s);
                }
                AddMenu(menuItems);
            }
            contextMenuStrip1.ItemClicked += contextMenuStrip1_OnItemClicked;
        }

        void AddMenu(List<string> menuItems)
        {
            ToolStripItemCollection itemLast = contextMenuStrip1.Items;
            while(menuItems.Count>0)
            {
                string itemNow = menuItems[0];
                menuItems.RemoveAt(0);

                ToolStripMenuItem item = null;
                if (itemLast.ContainsKey(itemNow))
                {
                    item = itemLast[itemNow] as ToolStripMenuItem;
                }
                else
                {
                    item = new ToolStripMenuItem(itemNow);
                    item.Name = itemNow;
                    itemLast.Add(item);
                    item.DropDownItemClicked += contextMenuStrip1_OnItemClicked;
                }
                itemLast = item.DropDownItems;
            }
        }

        public void UpdateSkill(config_skill skill)
        {
            _skill = skill;
            selected_frame_index = -1;
            //if (onSelectFrame!=null)
            //    onSelectFrame(selected_frame_index);
            this.Refresh();
        }


        int timeToFrameIndex(float t)
        {
            return (int)(t * 30.0f);
        }
        int timeToFrameCount(float t_l)
        {
            return (int)(t_l * 30.0f)+1;
        }
        float frameIndexToTime(float frame_index)
        {
            return Math.Max(frame_index / 30.0f,0);
        }


        private void Frame_Paint(object sender, PaintEventArgs e)
        {
            int size_x = 100;
            int size_y = 100;
            if (_skill != null && _skill.frames != null)
            {

                //优化处理
                for(int i=0;i<_skill.frames.Length;i++)
                {
                    int min_frame_count = timeToFrameIndex(_skill.frames[i].time) + timeToFrameCount(_skill.frames[i].time_length);
                    if (_skill.frame_count < min_frame_count)
                    {
                        _skill.frame_count = min_frame_count;
                    }
                }
                
                int gird_width = _skill.frame_count * unit_width;
                int gird_height = _skill.frames.Length * unit_height;
                int width = title_width+gird_width;
                int height = gird_height + time_scale_height;

                size_x = width + 50;
                size_y = height + 50;

                if(size_x !=Size.Width || size_y!=Size.Height)
                    this.Size = new Size(size_x, size_y);

                //刻度
                {
                    Graphics g = e.Graphics;
                    Rectangle rc = new Rectangle(0, 0, gird_width, time_scale_height);
                    Rectangle rcdest = new Rectangle(title_width, 0, gird_width, time_scale_height);
                    GraphicsContainer gc = g.BeginContainer(rcdest, rc, GraphicsUnit.Pixel);
                    Font font = new System.Drawing.Font("宋体", 10);

                    for (int i = 0; i <= _skill.frame_count; i++)
                    {
                        int frame_count = i + 1;
                        if (frame_count != 1 && frame_count % 5 == 0 && i != _skill.frame_count)
                        {
                            g.DrawString((frame_count).ToString(), font, Brushes.Black, new PointF(i * unit_width, 0));
                        }
                    }


                    g.EndContainer(gc);
                }


                //表头
                {
                    Graphics g = e.Graphics;
                    Rectangle rc = new Rectangle(0, 0, title_width, gird_height);
                    Rectangle rcdest = new Rectangle(0, time_scale_height, title_width, gird_height);
                    GraphicsContainer gc = g.BeginContainer(rcdest, rc, GraphicsUnit.Pixel);
                    Font font = new System.Drawing.Font("宋体", 10);
                    Pen pen = new Pen(Brushes.Black);
                    for(int i=0;i<_skill.frames.Length;i++)
                    {
                        g.DrawString(_skill.frames[i].action.ToString(), font, Brushes.Black, new PointF(0, unit_height * i));
                    }
                    e.Graphics.EndContainer(gc);
                }

                //格子，关键帧
                {
                    Graphics g = e.Graphics;
                    Rectangle rc = new Rectangle(0, 0, gird_width, gird_height);
                    Rectangle rcdest = new Rectangle(title_width, time_scale_height, gird_width, gird_height);
                    GraphicsContainer gc = g.BeginContainer(rcdest, rc,GraphicsUnit.Pixel);
                    
                    Pen pen = new Pen(Brushes.Gray);
                    //Pen pen_key = new Pen(Brushes.Blue);
                    Brush brush_sp_key = new SolidBrush(Color.FromArgb(100, Color.Gray));

                    //竖线
                    for (int i = 0; i <= _skill.frame_count; i++)
                    {
                        g.DrawLine(pen, new Point(i * unit_width, 0), new Point(i * 10, gird_height));

                        //关键帧数
                        int frame_count = i + 1;
                        if (frame_count != 1 && frame_count % 5 == 0 && i != _skill.frame_count)
                        {
                            g.FillRectangle(brush_sp_key, i * unit_width, 0, unit_width, gird_height);
                        }
                    }
                    //横线
                    for (int i = 0; i <= _skill.frames.Length; i++)
                    {
                        g.DrawLine(pen, new Point(0, i * unit_height), new Point(gird_width, i * unit_height));
                    }

                    Brush brush_frame = new SolidBrush(Color.FromArgb(200, Color.Red));
                    //关键帧
                    for (int i = 0; i < _skill.frames.Length;i++ )
                    {
                        int frame_index = timeToFrameIndex(_skill.frames[i].time);
                        int frame_number = timeToFrameCount(_skill.frames[i].time_length);

                        Rectangle rect = new Rectangle(frame_index * unit_width + 1, i * unit_height + 1, frame_number*unit_width - 1, unit_height - 1);
                        g.FillRectangle(brush_frame, rect);
                    }

                    //选择帧
                    if (selected_end_frame_index>selected_start_frame_index && selected_frame_index!=-1)
                    {
                        Brush brush_select_frame = new SolidBrush(Color.FromArgb(200, Color.Blue));

                        int frame_index = selected_start_frame_index;
                        int frame_number = selected_end_frame_index - selected_start_frame_index;

                        Rectangle rect = new Rectangle(frame_index * unit_width + 1, selected_frame_index * unit_height + 1, frame_number * unit_width - 1, unit_height - 1);
                        g.FillRectangle(brush_select_frame, rect);
                    }

                    e.Graphics.EndContainer(gc);
                }

                //选择效果
                if (selected_frame_index != -1)
                {
                    Graphics g = e.Graphics;
                    Brush selected_brush_frame = new SolidBrush(Color.FromArgb(50, Color.Green));
                    g.FillRectangle(selected_brush_frame, 0, selected_frame_index * unit_height+time_scale_height, width, unit_height);


                }
            }

            
        }
        private void Frame_MouseDown(object sender, MouseEventArgs e)
        {
            //选择行
            if (_skill != null && _skill.frames != null)
            {
                int selected_line_index = (e.Location.Y - time_scale_height) / unit_height;
                if (selected_line_index >= 0 && selected_line_index < _skill.frames.Length)
                {
                    selected_frame_index = selected_line_index;

                    if (onSelectFrame != null)
                        onSelectFrame(selected_line_index);

                    Refresh();
                }

                

            }


           //编辑
            if (selected_frame != null && e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                int x_frame_index = (e.Location.X - title_width) / unit_width;


                if(x_frame_index == timeToFrameIndex(selected_frame.time))
                {
                    _mode = EditMode.Move;
                    _lastMouseLocation = e.Location;
                }
                else
                {
                    //记录起始帧
                    selected_start_frame_index = x_frame_index;
                    _mode = EditMode.SelectFrame;
                    _lastMouseLocation = e.Location;
                }


            }
        }

        private void Frame_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mode == EditMode.Move)
            {
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)  //判断Ctrl键
                {
                    float x_frame_pos = ((float)(e.Location.X - title_width)) / unit_width;
                    selected_frame.time_length = Math.Max(frameIndexToTime(x_frame_pos) - selected_frame.time,0);
                }
                else
                {
                    float x_frame_pos = ((float)(e.Location.X - title_width)) / unit_width;
                    selected_frame.time = Math.Max(frameIndexToTime(x_frame_pos), 0);
                    
                }
                Refresh();
            }
            else if(_mode == EditMode.SelectFrame)
            {
                if(e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    float x_frame_pos = ((float)(e.Location.X - title_width)) / unit_width;
                    selected_end_frame_index = (int)x_frame_pos;
                    Refresh();
                }

            }
        }

        private void Frame_MouseUp(object sender, MouseEventArgs e)
        {
            _mode = EditMode.None;
        }

        private void Frame_MouseClick(object sender, MouseEventArgs e)
        {
            if(selected_frame!=null && e.X<this.title_width)
            {
                contextMenuStrip1.Show(this, e.Location);
            }
        }

        private void contextMenuStrip1_OnItemClicked(object sender,ToolStripItemClickedEventArgs e)
        {
            try
            {
                SkillFrameAction type = (SkillFrameAction)Enum.Parse(typeof(SkillFrameAction), e.ClickedItem.Text);
                if (selected_frame != null)
                {
                    selected_frame.action = type;
                    if (onSelectFrame != null)
                    {
                        onSelectFrame(this.selected_frame_index);
                    }
                    Refresh();
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        public void InsertFrameCount()
        {
            if (selected_end_frame_index > selected_start_frame_index && selected_frame_index != -1)
            {
                int frameCount = Math.Abs(selected_end_frame_index - selected_start_frame_index);
                if(frameCount>0)
                {
                    float timeStart = frameIndexToTime(selected_start_frame_index);
                    float timeLength = frameIndexToTime(frameCount);
                    foreach(var f in this._skill.frames)
                    {
                        if (f.time >= timeStart)
                        {
                            f.time += timeLength;
                        }
                    }
                    selected_start_frame_index += frameCount;
                    selected_end_frame_index += frameCount;
                    Refresh();
                }
            }
        }
        public void AppendFrameCount()
        {
            if (selected_end_frame_index > selected_start_frame_index && selected_frame_index != -1)
            {
                int frameCount = Math.Abs(selected_end_frame_index - selected_start_frame_index);
                if (frameCount > 0)
                {
                    float timeStart = frameIndexToTime(selected_end_frame_index);
                    float timeLength = frameIndexToTime(frameCount);
                    foreach (var f in this._skill.frames)
                    {
                        if (f.time >= timeStart)
                        {
                            f.time += timeLength;
                        }
                    }
                    Refresh();
                }
            }
        }
        public void DeleteFrameCount()
        {
            if (selected_end_frame_index > selected_start_frame_index && selected_frame_index != -1)
            {
                int frameCount = Math.Abs(selected_end_frame_index - selected_start_frame_index);
                if (frameCount > 0)
                {
                    float timeStart = frameIndexToTime(selected_start_frame_index);
                    float timeLength = frameIndexToTime(frameCount);
                    foreach (var f in this._skill.frames)
                    {
                        if (f.time >= timeStart)
                        {
                            f.time = Math.Max(f.time - timeLength,0);
                        }
                    }
                    selected_start_frame_index = -1;
                    selected_end_frame_index = -1;
                    Refresh();
                }
            }
        }

        protected delegate void AutoScrollPositionDelegate(ScrollableControl ctrl, Point p);
        private void Frame_Enter(object sender, EventArgs e)
        {
            if (Parent is Panel)
            {
                Point p = (this.Parent as Panel).AutoScrollPosition;
                AutoScrollPositionDelegate del = new AutoScrollPositionDelegate(SetAutoScrollPosition);
                Object[] args = { this.Parent as Panel, p };
                BeginInvoke(del, args);
            }
        }
        private void SetAutoScrollPosition(ScrollableControl sender, Point p)
        {
            p.X = Math.Abs(p.X);
            p.Y = Math.Abs(p.Y);
            sender.AutoScrollPosition = p;
        }
    }
}
