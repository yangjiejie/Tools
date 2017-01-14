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
            int x = Screen.PrimaryScreen.WorkingArea.Width / 2;
            int y = Screen.PrimaryScreen.WorkingArea.Height / 2;
            this.Location = new Point(0, 0);
        }

        DataSet result = new DataSet();

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
                    var files = Directory.GetFiles(textBox1.Text, "*.*", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        if (file.EndsWith(".xlsx") || file.EndsWith(".xls"))
                        {
                            string targetDir = "";
                            targetDir = Path.GetFullPath(file);
                            targetDir = targetDir.Replace(Path.GetFileName(file), "");
                            targetDir = targetDir.Replace(textBox1.Text, textBox1.Text + "\\CSV");
                            if (!Directory.Exists(targetDir))
                            {
                                Directory.CreateDirectory(targetDir);
                            }
                            var sheetNames = getExcelData(file);
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            for (int i = 0; i < sheetNames.Count; i++)
                            {
                                listView1.Items.Add(file);
                                listView1.Refresh();
                                label2.Text = "正在转换:" + file;
                                label2.Refresh();
                                xsldata(file, textBox2.Text);
                                converToCSV(i, targetDir, fileName);
                                if (!string.IsNullOrEmpty(textBox3.Text))
                                {
                                    if (Directory.Exists(textBox3.Text))
                                    {
                                        string sp = targetDir + "\\" + fileName + ".csv"; ;
                                        File.Copy(sp, textBox3.Text + "\\" + fileName + ".csv", true);
                                    }
                                }
                                break;
                            }

                        }
                    }
                    MessageBox.Show("转换完成");
                }
                else if (File.Exists(textBox1.Text))
                {
                    if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
                    {
                        var files = Path.GetFullPath(textBox1.Text);
                        string targetDir = "";
                        targetDir = Path.GetFullPath(files);
                        targetDir = targetDir.Replace(Path.GetFileName(files), "");
                        targetDir = targetDir.Replace(textBox1.Text, textBox1.Text + "\\CSV");
                        if (!Directory.Exists(targetDir))
                        {
                            Directory.CreateDirectory(targetDir);
                        }

                        //listView1.BeginUpdate();
                        var sheetNames = getExcelData(files);
                        string fileName = Path.GetFileNameWithoutExtension(files);
                        for (int i = 0; i < sheetNames.Count; i++)
                        {
                            ListViewItem lvi = new ListViewItem();
                            lvi.Text = files;
                            lvi.UseItemStyleForSubItems = false;
                            listView1.Items.Add(lvi);
                            label2.Text = "正在转换:" + files;
                            xsldata(files, textBox2.Text);
                            converToCSV(i, targetDir, fileName);
                            if (!string.IsNullOrEmpty(textBox3.Text))
                            {
                                if (Directory.Exists(textBox3.Text))
                                {
                                    string sp = targetDir + "\\" + fileName + ".csv"; ;
                                    File.Copy(sp, textBox3.Text+"\\"+fileName+".csv", true);
                                }
                            }
                            break;
                        }
                        //listView1.EndUpdate();
                        MessageBox.Show("转换完成");
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
        private List<string> getExcelData(string file)
        {
            return GetSheetsName(file);
            // if (file.EndsWith(".xlsx"))
            // {
            //     // Reading from a binary Excel file (format; *.xlsx)
            //     //FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
            //     //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //     //result = excelReader.AsDataSet();
            //     //excelReader.Close();
            //   
            // }
            //
            // if (file.EndsWith(".xls"))
            // {
            //     // Reading from a binary Excel file ('97-2003 format; *.xls)
            //     FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
            //     IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //     result = excelReader.AsDataSet();
            //     excelReader.Close();
            // }
            //
            // List<string> items = new List<string>();
            // for (int i = 0; i < result.Tables.Count; i++)
            //     items.Add(result.Tables[i].TableName.ToString());         
            // return items;

        }
        public static List<string> GetSheetsName(string pExcelAddress)
        {
            try
            {
                string strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + pExcelAddress + ";Extended Properties='Excel 12.0; HDR=YES; IMEX=1'";
                List<string> vSheetsName = new List<string>();
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        vSheetsName.Add(dt.Rows[i][2].ToString().Replace("$", ""));
                        //只转Sheet1
                        break;
                    }
                    return vSheetsName;
                }
            }
            catch (Exception vErr)
            {
                return null;
            }
        }
        private DataSet xsldata(string filepath, string fileName)
        {
            string strCon = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filepath + ";Extended Properties='Excel 12.0; HDR=No; IMEX=1'";
            OleDbConnection Conn = new OleDbConnection(strCon);
            Conn.Open();
            string strCom = "select * from [" + fileName + "$]";
            using (OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, Conn))
            {
                result = new DataSet();
                myCommand.Fill(result);
            }
            Conn.Close();
            return result;
        }
        private void converToCSV(int ind, string dir, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            int row_no = 0;
            string output = dir + "\\" + fileName + ".csv";
            StreamWriter csv = new StreamWriter(@output, false, Encoding.UTF8);
            int maxColumCount = 0;
            List<string> columnNames = new List<string>();
            for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
            {
                var r = result.Tables[ind].Rows[row_no][i].ToString();
                columnNames.Add(r);
            }
            columnNames.Reverse();
            for(int i = 0;i < columnNames.Count;i ++)
            {
                if (string.IsNullOrEmpty(columnNames[i]))
                {
                    maxColumCount++;
                }
                else
                {
                    break;
                }
            }
            maxColumCount = columnNames.Count - maxColumCount;
            while (row_no < result.Tables[ind].Rows.Count)
            {
                string str = "";
                List<string> feilds = new List<string>();
                for (int i = 0; i < maxColumCount; i++)
                {
                    var colName = result.Tables[ind].Columns["TABLE_NAME"];
                    var r = result.Tables[ind].Rows[row_no][i].ToString();
                    str += r;
                    feilds.Add(r);
                }
                if (!string.IsNullOrEmpty(str))
                {
                    WriteRecord(feilds, csv);
                }
                row_no++;

            }
            csv.Close();

            return;
        }
        void WriteRecord(IList<string> fields, TextWriter writer)
        {
            for (int i = 0; i < fields.Count; i++)
            {
                bool quotesRequired = fields[i].Contains(",");
                bool escapeQuotes = fields[i].Contains("\"");
                string fieldValue = (escapeQuotes ? fields[i].Replace("\"", "\"\"") : fields[i]);

                if (fieldValue.Contains("\r") || fieldValue.Contains("\n"))
                {
                    quotesRequired = true;
                    fieldValue = fieldValue.Replace("\r\n", ",");
                    fieldValue = fieldValue.Replace("\r", ",");
                    fieldValue = fieldValue.Replace("\n", ",");
                }

                writer.Write(string.Format("{0}{1}{0}{2}",
                    (quotesRequired || escapeQuotes ? "\"" : string.Empty),
                    fieldValue,
                    (i < (fields.Count - 1) ? "," : string.Empty)));
            }

            writer.WriteLine();
        }
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else e.Effect = DragDropEffects.None;
        }
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            // MessageBox.Show(path);
            textBox1.Text = path;
            SetAppConfig("source_dir", textBox1.Text);
        }
        private void Form1_Load(object sender, EventArgs e)
        {

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