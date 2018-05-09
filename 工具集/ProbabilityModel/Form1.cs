using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProbabilityModel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ScreenShot();
        }
        void ScreenShot()
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
                long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数
                string filenName = timeStamp + ".jpg";
                bitmap.Save(filenName, ImageFormat.Jpeg);
            }
        }
        void OnRecord(object sender, KeyEventArgs args)
        {
            if(args.KeyCode == Keys.Enter)
            {
                this.ScreenShot();
            }

        }
    }
}
