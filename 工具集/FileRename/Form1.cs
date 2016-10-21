using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace FileRename
{
    public partial class Form1 : Form
    {
        string targetPath = "";
        List<string> AllFilePaths = new List<string>();
        public Form1()
        {
            InitializeComponent();
            this.Location = new Point(0, 0);
        }
        //预览
        void Preview()
        {
            listBox2.Items.Clear();
            if (string.IsNullOrEmpty(textBox1.Text))
            {

            }
            else
            {
                for (int i = 0; i < AllFilePaths.Count; i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(AllFilePaths[i]);
                    string modieName = textBox1.Text.Replace("*", i.ToString());
                    string str = AllFilePaths[i].Replace(fileName, modieName);
                    listBox2.Items.Add(str);
                }
                listBox2.Update();
            }
        }
        //浏览
        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = fbd.SelectedPath;
                this.IniSelectedPath(fbd.SelectedPath);
            }
            else
            {
                return;
            }

        }
        //执行
        private void button2_Click(object sender, EventArgs e)
        {
            if(AllFilePaths.Count == 0)
            {
                MessageBox.Show("未选择文件!");
                return;
            }
            for (int i = 0;i < AllFilePaths.Count;i ++)
            {
                string fileName = Path.GetFileName(AllFilePaths[i]);
                string dir = AllFilePaths[i].Replace(fileName, "");
                string str = textBox1.Text.Replace("*", i.ToString());
                string _fileName = Path.GetFileNameWithoutExtension(AllFilePaths[i]);
                string modifiedName = fileName.Replace(_fileName, str);
                if (!File.Exists(dir + modifiedName))
                {
                    File.Move(AllFilePaths[i], dir + modifiedName);
                }
                else
                {
                    MessageBox.Show("文件已存在,忽略!");
                }
            }
            MessageBox.Show("操作成功!");
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
           
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            this.IniSelectedPath(path);
        }
        //通配符发生变化
        void OnTextBox1Change(object sender, EventArgs e)
        {
            //更新预览
            this.Preview();
        }
        //初始化选择的目录
        void IniSelectedPath(string path)
        {
            AllFilePaths.Clear();
            listBox1.Items.Clear();
            if(File.Exists(path))
            {
                string fileName = Path.GetFileName(path);
                path = path.Replace(fileName, "");
            }
            if (Directory.Exists(path))
            {
                targetPath = path;
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    AllFilePaths.Add(file);
                }
                AllFilePaths.Sort(comp);
                for (int i = 0; i < AllFilePaths.Count; i++)
                {
                    listBox1.Items.Add(AllFilePaths[i]);
                }
                listBox1.Update();
                //更新预览
                this.Preview();
            }
            else
            {
                MessageBox.Show("路径非法或不存在!");
            }
        }
        int comp(string a, string b)
        {
            string fileName1 = Path.GetFileNameWithoutExtension(a);
            string fileName2 = Path.GetFileNameWithoutExtension(b);
            if (fileName1.Length != fileName2.Length)
            {
                return fileName1.Length.CompareTo(fileName2.Length);
            }
            return fileName1.CompareTo(fileName2);
        }

    }
}
