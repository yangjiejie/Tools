using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Xml;
using Microsoft.Win32;
using System.Threading;
//using System.Timers;
namespace test
{
    public partial class Form1 : Form
    {
        int maxColumCount = 0;
        //System.Timers.Timer timer;
        private delegate void SetInfo();
        public Form1()
        {
            InitializeComponent();
            label2.Text = "";
            
            textBox1.Text = GetAppConfig("source_dir");
            textBox3.Text = GetAppConfig("target_dir");
            var isIgnoreRegister = GetAppConfig("IgnoreRegister");
            if (isIgnoreRegister == string.Empty)
            {
                SetAppConfig("IgnoreRegister","0");
            }
            var openFolder = GetAppConfig("isopen_folder");
            int isopen = 0;
            Int32.TryParse(openFolder, out isopen);
            if(isopen != 0)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }
            if (isIgnoreRegister == "0")
            {
                var IsIni = this.GetIniState();
                if (IsIni == 0)
                {
                    Thread t = new Thread(new ThreadStart(IniSetting));
                    t.Start();
                    label5.Text = "正在为首次运行程序初始化设置,请勿关闭程序或进行其他操作...";
                }
                else
                {
                    label5.Text = "";
                }
            }
            else
            {
                label5.Text = "";
            }
           
            this.Location = new Point(0, 0);
            if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
            {
                if (File.Exists(textBox1.Text))
                {
                    var list = GetSheetsName(textBox1.Text);
                    list.Sort(this.Sort);
                    for (int i = 0; i < list.Count; i++)
                    {
                        comboBox1.Items.Add(list[i]);
                    }

                }
            }
            else
            {
                comboBox1.Items.Add("Sheet1");
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            List<string> L = new List<string>() {"float","int","string","double" };
            for (int i = 0; i < L.Count; i++)
                comboBox2.Items.Add(L[i]);
            comboBox2.SelectedIndex = 0;
        }
        void UpdateInfo()
        {
            this.label5.Text = "初始化完毕!";
            SetIniState(1);
        }

        void IniSetting()
        {
            var node = Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
            SetRegistry(node);
            node.Close();
            if (this.InvokeRequired)
            {
                SetInfo si = new SetInfo(UpdateInfo);
                this.Invoke(si);
            }
            else
            {
                this.label5.Text = "";
            }
        }
        DataSet result = new DataSet();
        //转换成C#脚本文件
        private void SaveToCS(object sender, EventArgs e)
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
                            targetDir = targetDir.Replace(textBox1.Text, textBox1.Text + "\\CS");
                            if (!Directory.Exists(targetDir))
                            {
                                Directory.CreateDirectory(targetDir);
                            }
                            var sheetNames = getExcelData(file);
                            string fileName = Path.GetFileNameWithoutExtension(file);
                            //for (int i = 0; i < sheetNames.Count; i++)
                            //{
                            listView1.Items.Add(file);
                            listView1.Refresh();
                            label2.Text = "正在转换:" + file;
                            label2.Refresh();

                            var sheetName = comboBox1.SelectedItem.ToString();
                            xsldata(file, sheetName);
                            converToCS(0, targetDir, fileName);
                        }
                    }
                  
                    MessageBox.Show("生成完毕!");
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
                        //for (int i = 0; i < sheetNames.Count; i++)
                        //{
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = files;
                        lvi.UseItemStyleForSubItems = false;
                        listView1.Items.Add(lvi);
                        label2.Text = "正在转换:" + files;
                        //textBox2.Text = sheetNames[0];
                        var sheetName = comboBox1.SelectedItem.ToString();
                        xsldata(files, sheetName);
                        converToCS(0, targetDir, fileName);

                        //    break;
                        //}
                        //listView1.EndUpdate();
                        MessageBox.Show("生成完毕");
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
            var IsIni = GetIniState();
            if (IsIni == 0)
            {
                MessageBox.Show(this, "尚未初始化程序设置!");
                return;
            }
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
                            //for (int i = 0; i < sheetNames.Count; i++)
                            //{
                            listView1.Items.Add(file);
                            listView1.Refresh();
                            label2.Text = "正在转换:" + file;
                            label2.Refresh();

                            var sheetName = comboBox1.SelectedItem.ToString();
                            xsldata(file, sheetName);
                            converToCSV(0, targetDir, fileName);
                            if (!string.IsNullOrEmpty(textBox3.Text))
                            {
                                if (Directory.Exists(textBox3.Text))
                                {
                                    string sp = targetDir + "\\" + fileName + ".csv"; ;
                                    File.Copy(sp, textBox3.Text + "\\" + fileName + ".csv", true);
                                }
                            }
                            //    break;
                            //}

                        }
                    }
                    if(checkBox1.Checked)
                    {
                        if (Directory.Exists(textBox1.Text + "\\CSV"))
                        {
                            System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
                            processStartInfo.FileName = "explorer.exe";  //资源管理器
                            processStartInfo.Arguments = textBox1.Text + "\\CSV";
                            System.Diagnostics.Process.Start(processStartInfo);

                        }
                    }
                    else
                    {
                        MessageBox.Show("转换完成");
                    }
                    //
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
                        //for (int i = 0; i < sheetNames.Count; i++)
                        //{
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = files;
                        lvi.UseItemStyleForSubItems = false;
                        listView1.Items.Add(lvi);
                        label2.Text = "正在转换:" + files;
                        //textBox2.Text = sheetNames[0];
                        var sheetName = comboBox1.SelectedItem.ToString();
                        xsldata(files, sheetName);
                        converToCSV(0, targetDir, fileName);
                        if (!string.IsNullOrEmpty(textBox3.Text))
                        {
                            if (Directory.Exists(textBox3.Text))
                            {
                                string sp = targetDir + "\\" + fileName + ".csv"; ;
                                File.Copy(sp, textBox3.Text + "\\" + fileName + ".csv", true);
                            }
                        }
                        //    break;
                        //}
                        //listView1.EndUpdate();
               
                       if(checkBox1.Checked)
                        {
                            MessageBox.Show("转换完成");
                        }
                       else
                        {
                            string folder = Path.GetDirectoryName(textBox1.Text);
                            if (Directory.Exists(folder))
                            {
                                System.Diagnostics.ProcessStartInfo processStartInfo = new System.Diagnostics.ProcessStartInfo();
                                processStartInfo.FileName = "explorer.exe";  //资源管理器
                                processStartInfo.Arguments = folder;
                                System.Diagnostics.Process.Start(processStartInfo);

                            }
                        }
                       
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
                        string sn = dt.Rows[i][2].ToString().Replace("$", "");
                        vSheetsName.Add(sn);
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
        //数组的维数
        private int GetArrayDemen(string s)
        {
            if (!s.Contains("["))
                return 0;
            List<object> dic=fastJSON.JSON.Parse(s) as List<object>;
            if (dic == null || dic.Count == 0)
                return 1;
            var rs = dic[0].ToString();
            if (rs.Contains("["))
                return 2;
            else
                return 1;
        }
        private void converToCS(int ind, string dir, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            int row_no = 0;
            string output = dir + "\\config_" + fileName + ".cs";
            StreamWriter cs = new StreamWriter(@output, false, Encoding.UTF8);
            maxColumCount = 0;
            List<string> columnNames = new List<string>();
            for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
            {
                var r = result.Tables[ind].Rows[row_no][i].ToString();
                columnNames.Add(r);
            }
            columnNames.Reverse();
            for (int i = 0; i < columnNames.Count; i++)
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
            Dictionary<int, List<string>> data = new Dictionary<int, List<string>>();
            while (row_no < result.Tables[ind].Rows.Count)
            {
                List<string> feilds = new List<string>();
                for (int i = 0; i < maxColumCount; i++)
                {
                    var r = result.Tables[ind].Rows[row_no][i];
                    feilds.Add(Convert.ToString(r));
                }
                data.Add(row_no, feilds);
                row_no++;
                if (row_no >= 4)
                    break;
            }
            cs.Close();

            string info = "//该代码由工具生成。\n" + "//" + string.Format("{0:f}", DateTime.Now) + "\n[Serializable]\n";
            var fieldNames = data[0];//字段名
            var Comments = data[1];//注释
            var fieldType = data[2];//字段类型
            var fieldValue = data[3];//字段值
            if (fieldNames.Count == fieldType.Count)
            {
                string nameSp = "using System;\nusing System.Collections.Generic;\n";
                string classBegin ="public class config_" + fileName + "\n{";

                //string ESOFun = "\n    public void OnBeforeSerialize()\n    {\n\n    }";
                //string DSOFun = "\n    public void OnAfterDeserialize()\n    {\n\n    }";
                string classEnd = "\n}";
                string body = "\n";
                for (int m = 0; m < fieldNames.Count; m++)
                {
                    string comm = m < Comments.Count ? "//" + Comments[m] + "\n" : "\n";
                    if (comm == "//\n")
                        comm = "\n";
                    switch (fieldType[m])
                    {
                        case "int":
                            {

                                body += "    public int " + fieldNames[m] + ";" + comm;

                            }
                            break;
                        case "string":
                            {
                                int type = 0;
                                if (type == 0)
                                {
                                    body += "    public string " + fieldNames[m] + ";" + comm;
                                }
                            }
                            break;
                        case "float":
                            {
                                body += "    public float " + fieldNames[m] + ";" + comm;
                            }
                            break;
                        case "json":
                            {
                                if(checkBox2.Checked)
                                {
                                    if(GetArrayDemen(fieldValue[m]) == 1)
                                    {
                                        var T = comboBox2.SelectedItem.ToString();
                                        body += string.Format("    public {0}[] {1};{2}", T, fieldNames[m], comm);
                                    }
                                    else if (GetArrayDemen(fieldValue[m]) == 2)
                                    {
                                        var T = comboBox2.SelectedItem.ToString();
                                        body += string.Format("    public {0}[][] {1};{2}", T, fieldNames[m], comm);
                                    }
                                    else
                                    {
                                        //不是二维数组的json串
                                        body += "    public string " + fieldNames[m] + ";" + comm;
                                    }
                                }
                                else
                                {
                                    body += "    public string " + fieldNames[m] + ";" + comm;
                                }
                                
                            }
                            break;
                        default:
                            {
                                if (!string.IsNullOrEmpty(fieldNames[m]))
                                {
                                    body += "    public int " + fieldNames[m] + ";" + comm;
                                }
                            }
                            break;
                    }
                }
                string str = "";
                if (false)
                {
                    str = nameSp + info + classBegin + body + classEnd;
                }
                else
                {
                    str = nameSp + info + classBegin + body + classEnd;
                }

                byte[] classContent = Encoding.UTF8.GetBytes(str);
                using (FileStream fsWrite = new FileStream(output, FileMode.OpenOrCreate))
                {
                    fsWrite.Write(classContent, 0, classContent.Length);
                }
            }

        }
        private void converToCSV(int ind, string dir, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            int row_no = 0;
            string output = dir + "\\" + fileName + ".csv";
            StreamWriter csv = new StreamWriter(@output, false, Encoding.UTF8);
            maxColumCount = 0;
            List<string> columnNames = new List<string>();
            for (int i = 0; i < result.Tables[ind].Columns.Count; i++)
            {
                var r = result.Tables[ind].Rows[row_no][i].ToString();
                columnNames.Add(r);
            }
            columnNames.Reverse();
            for (int i = 0; i < columnNames.Count; i++)
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
                    var r = result.Tables[ind].Rows[row_no][i];
                    str += Convert.ToString(r);
                    feilds.Add(Convert.ToString(r));
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
            string path = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            // MessageBox.Show(path);
            textBox1.Text = path;
            SetAppConfig("source_dir", textBox1.Text);
            comboBox1.Items.Clear();
            if (textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
            {
                var list = GetSheetsName(path);
                list.Sort(this.Sort);
                for (int i = 0; i < list.Count; i++)
                    comboBox1.Items.Add(list[i]);
            }
            else
            {
                comboBox1.Items.Add("Sheet1");
            }
            comboBox1.SelectedIndex = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //排序下拉列表
        int Sort(string a, string b)
        {
            bool b1 = a.StartsWith("Sheet");
            bool b2 = b.StartsWith("Sheet");
            if (b1 == b2)
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
        //设置注册表，防止单元格超255字符时产生截断的问题
        void SetRegistry(RegistryKey node)
        {
            var subValues = node.GetValueNames();
            List<string> tmp = new List<string>(subValues);
            if (tmp.Contains("TypeGuessRows"))
            {
                node.SetValue("TypeGuessRows", 0, RegistryValueKind.DWord);
                node.Flush();
            }

            var subKeys = node.GetSubKeyNames();
            for (int i = 0; i < subKeys.Length; i++)
            {
                try
                {
                    var next = node.OpenSubKey(subKeys[i], false);
                    subValues = next.GetValueNames();
                    tmp = new List<string>(subValues);
                    if (tmp.Contains("TypeGuessRows"))
                    {
                        next = node.OpenSubKey(subKeys[i], true);
                    }
                    SetRegistry(next);

                }
                catch (Exception e)
                {
                    continue;
                }

            }
            node.Close();
        }
        //获取初始状态
        public int GetIniState()
        {
            var isIgnoreRegister = GetAppConfig("IgnoreRegister");
            if (isIgnoreRegister != "0")
                return 1;
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey conv = rk.CreateSubKey(@"SOFTWARE\ExcelConvert");
            rk.Close();
            var ret = conv.GetValue("IniState", 0);
            conv.Close();
            return Convert.ToInt32(ret);
        }
        //设置初始化状态
        public void SetIniState(int state)
        {
            RegistryKey rk = Registry.LocalMachine;
            RegistryKey conv = rk.CreateSubKey(@"SOFTWARE\ExcelConvert");
            rk.Close();
            conv.SetValue("IniState", state, RegistryValueKind.DWord);
            conv.Flush();
            conv.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
          if(checkBox1.Checked)
            {
                SetAppConfig("isopen_folder", "1");
            }
            else
            {
                SetAppConfig("isopen_folder", "0");
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                label5.Text = "";
            }
            else
            {
                label5.Text = "";
            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(IniSetting));
            t.Start();
            label5.Text = "正在为首次运行程序初始化设置,请勿关闭程序或进行其他操作...";
        }
    }
}
