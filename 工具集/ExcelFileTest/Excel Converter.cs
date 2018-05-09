using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Xml;
using System.Configuration;
namespace test
{
    public partial class Form1 : Form                 
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = GetAppConfig("source_dir");
            textBox3.Text = GetAppConfig("target_dir");
        
            this.Location = new Point(0, 0);
            if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
            {
                if (File.Exists(textBox1.Text))
                {
                    

                }
            }
            else
            {
                comboBox1.Items.Add("Sheet1");
            }
            if(comboBox1.Items.Count > 0)
            comboBox1.SelectedIndex = 0;
        }

        DataSet result = new DataSet();
        public void ExportToExcel(DataTable dt, string path)
        {
            string result = string.Empty;
            try
            {
                // 实例化流对象，以特定的编码向流中写入字符。  
                StreamWriter sw = new StreamWriter(path, false, Encoding.GetEncoding("gb2312"));

                StringBuilder sb = new StringBuilder();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    // 添加列名称  
                    sb.Append(dt.Columns[k].ColumnName.ToString() + "\t");
                }
                sb.Append(Environment.NewLine);
                // 添加行数据  
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow row = dt.Rows[i];
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        // 根据列数追加行数据  
                        sb.Append(row[j].ToString() + "\t");
                    }
                    sb.Append(Environment.NewLine);
                }
                sw.Write(sb.ToString());
                sw.Flush();
                sw.Close();
                sw.Dispose();

                // 导出成功后打开  
                //System.Diagnostics.Process.Start(path);  
            }
            catch (Exception)
            {
                result = "请保存或关闭可能已打开的Excel文件";
            }
            finally
            {
                dt.Dispose();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("请选择配置表目录");
                return;
            }
            try
            {
                if (Directory.Exists(textBox1.Text))
                {
                    
                }
                else if (File.Exists(textBox1.Text))
                {
                    if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
                    {
                        var xldata = this.xsldata(textBox1.Text, "Sheet1");
                        MessageBox.Show("转换完成11");
                    }
                    else
                    {
                        MessageBox.Show("不支持文件格式!");
                    }
                }
                else
                {
                    MessageBox.Show("文件路径或文件不存在!");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = this.folderBrowserDialog2.SelectedPath;
                SetAppConfig("source_dir", textBox1.Text);
            }
            else
            {
                return;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox3.Text = this.folderBrowserDialog2.SelectedPath;
                SetAppConfig("target_dir", textBox3.Text);
            }
            else
            {
                return;
            }
        }
     

 
        private DataSet xsldata(string filepath, string fileName)
        {
            string strCon = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 8.0; HDR=No; IMEX=1'";
            OleDbConnection Conn = new OleDbConnection(strCon);
            Conn.Open();
            string strCom = "select * from [" + fileName + "$]";
            strCom = strCom.ToLower();
            OleDbCommand cmd = new OleDbCommand(strCom, Conn);
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = cmd;
            result = new DataSet();
            adapter.Fill(result);
           
            Conn.Close();
            return result;
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
            // MessageBox.Show(path);
            textBox1.Text = path;
            SetAppConfig("source_dir", textBox1.Text);
            comboBox1.Items.Clear();
            if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
            {
               
            }
            else
            {
                comboBox1.Items.Add("Sheet1");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //排序下拉列表
        int Sort(string a,string b)
        {
            bool b1 = a.StartsWith("Sheet");
            bool b2 = b.StartsWith("Sheet");
            if(b1 == b2)
            {
                return a.CompareTo(b);
            }
            else
            {
                if (a.StartsWith("Sheet"))
                    return -1;
                if (b.StartsWith("Sheet"))
                    return 1;
            }
            return 0;
        }
        //读写配置
        public static void SetAppConfig(string appKey, string appValue)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");

            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", appValue);
            else
            {
                var xNewElem = xDoc.CreateElement("add");
                xNewElem.SetAttribute("key", appKey);
                xNewElem.SetAttribute("value", appValue);
                xNode.AppendChild(xNewElem);
            }
            xDoc.Save(Application.ExecutablePath + ".config");
        }
        public static void RemoveConfig(string key)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Application.ExecutablePath + ".config");

            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem != null)
            {
                xNode.RemoveChild(xElem);
            }
            xDoc.Save(Application.ExecutablePath + ".config");
        }
        public static string GetAppConfig(string appKey)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            var xNode = xDoc.SelectSingleNode("//appSettings");

            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");

            if (xElem != null)
            {
                return xElem.Attributes["value"].Value;
            }
            return string.Empty;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        void CSV2EXCEL()
        {

        }
    }
}