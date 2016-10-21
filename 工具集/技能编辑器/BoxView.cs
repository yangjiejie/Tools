using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using UnityEngine;

namespace 技能编辑器
{
    public interface IBoxViewEvent
    {
        void onAddBox(BoxView view);
        void onRemoveBox(BoxView view);
        void onBoxChanged(BoxView view,Quad box);
        Quad[] GetBoxList();
    }

    public partial class BoxView : UserControl
    {
        //config_action_attack _atk;
        List<Quad> _boxList = new List<Quad>();
        int unit_gird_size = 50;
        public float scale = 1.0f;
        bool _snap_gird = true;
        float _snap_size=1.0f;


        //public delegate void Event_AddBox(BoxView view);
        //public delegate void Event_RemoveBox(BoxView view);
        //public delegate void Event_BoxChanged(BoxView view, Quad box);

        //public Event_AddBox onAddBox;
        //public Event_RemoveBox onRemoveBox;
        //public Event_BoxChanged onBoxChanged;

        public IBoxViewEvent _listener;

        public IBoxViewEvent listener
        {
            get
            {
                return _listener;
            }
            set
            {
                _listener = value;
                if(_listener == null)
                {
                    _boxList.Clear();
                    
                }
                else
                {
                    _boxList.Clear();
                    if(_listener.GetBoxList()!=null)
                        _boxList.AddRange(_listener.GetBoxList());
                }
                Refresh();
            }
        }

        Matrix _viewMat = new Matrix();

        Point _center_offset;

        public Quad[] box
        {
            get
            {
                return _boxList.ToArray();
            }
            //set 
            //{
            //    _boxList.Clear();
            //    if (value != null)
            //    {
            //        _boxList.AddRange(value);
            //    }
            //    Refresh();
            //}
        }

        public bool snap_gird
        {
            get { return _snap_gird; }
            set { _snap_gird = value; Refresh(); }
        }
        public float snap_size
        {
            get { return _snap_size; }
            set { 

                _snap_size = value;
                if (_snap_size <= 0.01f)
                    _snap_size = 0.01f;
                Refresh(); 
            }
        }

        Quad _selected_box;
        Point _lastMosePoint;
        enum EidtMode
        {
            None,
            MoveBox,
            CreateBox,
            EditBox,
            MoveView
        }
        EidtMode mode;



        public BoxView()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            
        }

        void DrawBox(Graphics g,Pen pen,Quad box,int unit_size)
        {
            Point[] pts = new Point[5];
            pts[0] = UnityPointToViewPoint(box.a);
            pts[1] = UnityPointToViewPoint(box.b);
            pts[2] = UnityPointToViewPoint(box.c);
            pts[3] = UnityPointToViewPoint(box.d);
            pts[4] = UnityPointToViewPoint(box.a);

            g.DrawPolygon(pen, pts);
        }

        //每个单位的像素大小
        int unit_size
        {
            get
            {
                return (int)(unit_gird_size * scale);
            }
        }

        //相对控件左上角的中心坐标
        Point center
        {
            get
            {
                return new Point(Size.Width / 2 + _center_offset.X, Size.Height / 2 + _center_offset.Y);
            }
        }

        //相对控件中心偏移坐标
        public Point center_offset
        {
            get
            {
                return _center_offset;
            }
            set
            {
                _center_offset = value;
                Refresh();
            }
        }

        private void BoxView_Paint(object sender, PaintEventArgs e)
        {

            //绘制标尺网格
            {
                Graphics g = e.Graphics;
                Rectangle rc = new Rectangle(0, 0,Size.Width , Size.Height);
                Rectangle rcdest = new Rectangle(center.X, center.Y, Size.Width, Size.Height);
                GraphicsContainer gc = g.BeginContainer(rcdest, rc, GraphicsUnit.Pixel);
                

                //对齐网格
                if (snap_gird)
                {
                    Pen pen2 = new Pen(System.Drawing.Color.FromArgb(50, System.Drawing.Color.Black));

                    pen2.DashStyle = DashStyle.Custom;

                    pen2.DashPattern = new float[] { 3f, 3f };

                    float snap_unit_size = snap_size * unit_size;

                    int snap_x_lines = (int)(Size.Height / snap_unit_size);
                    for (int i = -snap_x_lines ; i <= snap_x_lines ; i++)
                    {
                        g.DrawLine(pen2, new Point(-Size.Width , (int)(i * snap_unit_size)), new Point(Size.Width , (int)(i * snap_unit_size)));
                    }

                    int snap_y_lines = (int)(Size.Width / snap_unit_size);
                    for (int i = -snap_y_lines ; i <= snap_y_lines ; i++)
                    {
                        g.DrawLine(pen2, new Point((int)(i * snap_unit_size), -Size.Height ), new Point((int)(i * snap_unit_size), Size.Height ));
                    }
                }

                Pen pen = new Pen(Brushes.Black);
                //横线
                g.DrawLine(pen, new Point(-Size.Width, 0), new Point(Size.Width, 0));
                //竖线
                g.DrawLine(pen, new Point(0, -Size.Height), new Point(0, Size.Height));

                Pen penUnreal = new Pen(System.Drawing.Color.FromArgb(100, System.Drawing.Color.Black));
                //竖线
                int y_lines = Size.Height/unit_size;
                for (int i = -y_lines ; i <= y_lines ; i++)
                {
                    g.DrawLine(penUnreal, new Point(-Size.Width , i * unit_size), new Point(Size.Width , i * unit_size));
                }

                //横线
                int x_lines = Size.Width / unit_size;
                for (int i = -x_lines; i <= x_lines ; i++)
                {
                    g.DrawLine(penUnreal, new Point(i * unit_size, -Size.Height ), new Point(i * unit_size, Size.Height ));
                }

                

                g.EndContainer(gc);

            }


            {
                Graphics g = e.Graphics;
                Rectangle rc = new Rectangle(0, 0, 100, 100);
                Rectangle rcdest = new Rectangle(10, Size.Height-110, 100, 100);
                GraphicsContainer gc = g.BeginContainer(rcdest, rc, GraphicsUnit.Pixel);

                //右下角坐标轴
                g.DrawLine(new Pen(System.Drawing.Color.Red), new Point(0, 100), new Point(100, 100));
                g.DrawString("x(左右)", Font, Brushes.Black, new Point(70,80));
                g.DrawLine(new Pen(System.Drawing.Color.Blue), new Point(0, 100), new Point(0, 0));
                g.DrawString("y(前后)", Font, Brushes.Black, new Point(0, 0));

                g.EndContainer(gc);
            }


            //绘制攻击框
            {
                Graphics g = e.Graphics;
                Point center = new Point(Size.Width / 2 + _center_offset.X, Size.Height / 2 + _center_offset.Y);
                Rectangle rc = new Rectangle(0, 0, Size.Width, Size.Height);
                Rectangle rcdest = new Rectangle(center.X, center.Y, Size.Width, Size.Height);
                GraphicsContainer gc = g.BeginContainer(rcdest, rc, GraphicsUnit.Pixel);

                Pen pen = new Pen(Brushes.Red,2);
                //if (_atk != null && _atk.box != null)
                {
                    for (int i = 0; i < _boxList.Count; i++)
                    {
                        DrawBox(g, pen, _boxList[i], unit_size);
                    }
                }

                pen = new Pen(Brushes.Green, 2);
                if (_selected_box != null)
                {
                    DrawBox(g, pen, _selected_box, unit_size);
                }

                g.EndContainer(gc);
            }

        }

        //视图上的像素坐标转换为u3d坐标
        Vector2 ViewPointToUnityPoint(Point viewPt)
        {
            Vector2 pt = new Vector2();
            pt.x = ((float)(viewPt.X - center.X)) / unit_size;
            pt.y = (-(float)(viewPt.Y - center.Y)) / unit_size;
            return pt;
        }

        //视图的向量转换为u3d向量
        Vector2 ViewVectorToUnityVector(Point viewVec)
        {
            Vector2 pt = new Vector2();
            pt.x = ((float)(viewVec.X)) / unit_size;
            pt.y = (-(float)(viewVec.Y)) / unit_size;
            return pt;
        }

        Point UnityPointToViewPoint(Vector2 up)
        {
            return new Point((int)(up.x * unit_size), -(int)(up.y * unit_size));
        }


        //对齐一个像素坐标到网格
        float Snap(float x)
        {
            if(x>0)
            {
                float left_x = ((int)(x / snap_size)) * snap_size;
                float right_x = ((int)(x / snap_size) + 1) * snap_size;
                return Math.Abs(x - left_x) > Math.Abs(x - right_x) ? right_x : left_x;
            }
            else
            {
                float right_x = ((int)(x / snap_size)) * snap_size;
                float left_x = ((int)(x / snap_size) - 1) * snap_size;
                return Math.Abs(x - left_x) > Math.Abs(x - right_x) ? right_x : left_x;
            }

        }
        Vector2 Snap(Vector2 pt)
        {
            Vector2 newPos = new Vector2();
            newPos.x = Snap(pt.x);
            newPos.y = Snap(pt.y);
            return newPos;
        }


        private void BoxView_MouseDown(object sender, MouseEventArgs e)
        {
            if (listener == null)
                return;

            //查找BOX
            Vector2 pt = ViewPointToUnityPoint(e.Location);

            foreach (var box in _boxList.ToArray())
            {
                if (box.PointIn(pt))
                {
                    //左键移动
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        _selected_box = box;
                        if ((Control.ModifierKeys & Keys.Control) == Keys.Control)  //判断Ctrl键
                            mode = EidtMode.EditBox;
                        else
                            mode = EidtMode.MoveBox;

                        _lastMosePoint = e.Location;
                        Refresh();
                        return;
                    }
                    //右键删除
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        _boxList.Remove(box);
                        if (listener != null)
                            listener.onRemoveBox(this);
                        Refresh();
                        return;
                    }
                }
            }
                
            

            //创建BOX
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                
                mode = EidtMode.CreateBox;
                _lastMosePoint = e.Location;
                _selected_box = new Quad();
                _selected_box.a = ViewPointToUnityPoint(e.Location);
                _selected_box.b = ViewPointToUnityPoint(e.Location);
                _selected_box.c = ViewPointToUnityPoint(e.Location);
                _selected_box.d = ViewPointToUnityPoint(e.Location);

            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                mode = EidtMode.MoveView;
                _lastMosePoint = e.Location;
            }


        }

        private void BoxView_MouseMove(object sender, MouseEventArgs e)
        {
            if(mode == EidtMode.MoveBox)
            {
                Point moved = new Point(e.Location.X - _lastMosePoint.X, e.Location.Y - _lastMosePoint.Y);
                Vector2 u3dMoved = ViewVectorToUnityVector(moved);
                _selected_box.a = _selected_box.a + u3dMoved;
                _selected_box.b = _selected_box.b + u3dMoved;
                _selected_box.c = _selected_box.c + u3dMoved;
                _selected_box.d = _selected_box.d + u3dMoved;
                Refresh();
                _lastMosePoint = e.Location;
            }
            else if(mode == EidtMode.CreateBox || mode == EidtMode.EditBox)
            {
                Point moved = new Point(e.Location.X - _lastMosePoint.X, e.Location.Y - _lastMosePoint.Y);
                Vector2 u3dMoved = ViewVectorToUnityVector(moved);

                _selected_box.b.x += u3dMoved.x;
                _selected_box.c = _selected_box.c+u3dMoved;
                _selected_box.d.y += u3dMoved.y;
                Refresh();
                _lastMosePoint = e.Location;

            }
            else if(mode == EidtMode.MoveView)
            {
                Point moved = new Point(e.Location.X - _lastMosePoint.X, e.Location.Y - _lastMosePoint.Y);
                _lastMosePoint = e.Location;
                _center_offset.X += moved.X;
                _center_offset.Y += moved.Y;
                Refresh();
            }
        }

        private void BoxView_MouseUp(object sender, MouseEventArgs e)
        {
            if(mode == EidtMode.MoveBox)
            {
                if (snap_gird)
                {
                    if (_selected_box != null)
                    {
                        Vector2 newPos = Snap(_selected_box.a);
                        Vector2 offset = newPos - _selected_box.a;
                        _selected_box.a = newPos;
                        _selected_box.b += offset;
                        _selected_box.c += offset;
                        _selected_box.d += offset;
                    }
                }
            }
            else if(mode == EidtMode.CreateBox)
            {
                

                if (snap_gird)
                {
                    if (_selected_box != null)
                    {
                        _selected_box.a = Snap(_selected_box.a);
                        _selected_box.b = Snap(_selected_box.b);
                        _selected_box.c = Snap(_selected_box.c);
                        _selected_box.d = Snap(_selected_box.d);
                    }
                }

                //忽略面积太小的BOX
                Vector2 range = _selected_box.c - _selected_box.a;
                if (Math.Abs(range.x * range.y) > 0.01f)
                {
                    _boxList.Add(_selected_box);
                    if(listener!=null)
                        listener.onAddBox(this);
                }

            }
            else if(mode == EidtMode.EditBox)
            {
                if (snap_gird)
                {
                    if (_selected_box != null)
                    {
                        _selected_box.a = Snap(_selected_box.a);
                        _selected_box.b = Snap(_selected_box.b);
                        _selected_box.c = Snap(_selected_box.c);
                        _selected_box.d = Snap(_selected_box.d);
                        
                    }
                }

                //删除面积太小的BOX
                Vector2 range = _selected_box.c - _selected_box.a;
                if (Math.Abs(range.x * range.y) <= 0.01f)
                {
                    _boxList.Remove(_selected_box);
                    if (listener != null)
                        listener.onRemoveBox(this);
                }
                else
                {
                    if (listener != null)
                        listener.onBoxChanged(this, _selected_box);
                }
            }


            mode = EidtMode.None;
            _selected_box = null;
            Refresh();
        }
    }
}
