using System;
using System.Windows.Forms;

namespace 技能编辑器
{
    public partial class Form_NewSkill : Form
    {
        public string name;
        public int id;

        public Form_NewSkill()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                id = Int32.Parse(tb_ID.Text);
                name = tb_Name.Text;
                Close();
            }
            catch(Exception exc)
            {

            }
        }
    }
}
