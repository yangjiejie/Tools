using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace CSV2CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbPath.Text = Application.StartupPath;
        }

        //选择目录
        private void btnScan_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            fb.SelectedPath = Application.StartupPath;
            DialogResult dr = fb.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                tbPath.Text = fb.SelectedPath;
            }
        }
        //测试
        private void btTest_Click(object sender, EventArgs e)
        {
            string pattern = "^.[0-9]+$";
            Regex r = new Regex(pattern);
            string str = "23";
            Match m = r.Match(str);
            
            AddItem("是否匹配:" + r.IsMatch(str)+"   位置:"+m.Index);
            AddItem("匹配值:"+m.Value);
            //AddItem("选项:" + r.Options.ToString());
        }
        //生成代码
        int igonor = 0;
        private void btnConverter_Click(object sender, EventArgs e)
        {
            //获取状态
            string[] fileNames = new string[1];
            if (tbPath.Text.Contains(".csv"))
            {
                fileNames[0] =tbPath.Text;
            }
            else
            {
                igonor = (Directory.GetFiles(tbPath.Text, "*.*")).Length;
                fileNames = Directory.GetFiles(tbPath.Text, "*.csv");
            }
            //重命名源csv文件
            if (checkBox2.CheckState == CheckState.Checked)
            {
                string fileName = "";
                for (int i = 0; i < fileNames.Length; i++)
                {
                    fileName = Path.GetFileNameWithoutExtension(fileNames[i]);
                    if (fileName.StartsWith("table_"))
                        continue;
                    byte[] content = null;
                    using (FileStream FOpen = new FileStream(fileNames[i], FileMode.Open))
                    {
                        int fsLen = (int)FOpen.Length;
                        content = new byte[fsLen];
                        int r = FOpen.Read(content, 0, content.Length);
                        FOpen.Close();
                    }
                    File.Delete(fileNames[i]);
                    using (FileStream fsWrite = new FileStream(fileNames[i].Replace(fileName + ".csv", "table_" + fileName + ".csv"), FileMode.OpenOrCreate))
                    {
                        fsWrite.Write(content, 0, content.Length);
                        fsWrite.Close();
                    }
                }
                if (tbPath.Text.Contains(".csv"))
                {
                    if(!tbPath.Text.Contains("table_"))
                    {
                        fileNames[0] = fileNames[0].Replace(fileName + ".csv", "table_" + fileName + ".csv");
                    }
                }
                else
                {
                    fileNames = Directory.GetFiles(tbPath.Text, "*.csv");
                }
                    
            }
            igonor -= fileNames.Length;
            if (igonor < 0)
                igonor = 0;
            GenerateCode(fileNames);
        }
        void GenerateCode(string[]files)
        {
            int pos = tbPath.Text.LastIndexOf("\\");
            string path = "";
            int successCount = 0, failCount = 0;
            for(int i = 0;i < files.Length;i ++)
            {
                path = Path.GetFullPath(files[i]);
                //读取csv文件信息
                string myStr = "";
                using (FileStream FOpen = new FileStream(files[i], FileMode.Open))
                {
                    int fsLen = (int)FOpen.Length;
                    byte[] content = new byte[fsLen];
                    int r = FOpen.Read(content, 0, content.Length);
                    myStr = System.Text.Encoding.UTF8.GetString(content);
                }
                string info = "//该代码由工具生成。\n" +"//"+ string.Format("{0:f}", DateTime.Now)+ "\n[Serializable]\n";
                if(!string.IsNullOrEmpty(myStr))
                {
                    char[] chSeperator = {'\r','\n'};
                    string[] lines = myStr.Split(chSeperator);
                    string[] fieldNames = ParseCSVLines(lines[0]);//字段名
                    string[] Comments = ParseCSVLines(lines[2]);//注释
                    string[] fieldType = ParseCSVLines(lines[4]);//字段类型
                    if(fieldNames.Length == fieldType.Length)
                    {
                        successCount++;
                        string fileName = Path.GetFileNameWithoutExtension(files[i]);
                        string nameSp = "using System;\nusing System.Collections.Generic;\n";
                        string classBegin = "public class " + fileName + "\n{";
                        string instanceFun = "\n    public static " + fileName + " GetInstance(int id)\n    {\n        var ret = null;\n        Data.all" +
                            fileName + ".TryGetValue(id, out ret);\n        return ret;\n    }";
        

                        string classEnd = "\n}";
                        string body = "\n";
                        for(int m = 0;m < fieldNames.Length;m ++)
                        {
                            string comm =m < Comments.Length ?"//"+ Comments[m]+"\n" : "\n";
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
                                        int type = CheckType(lines[6], m);
                                        if(type == 0)
                                        {
                                            body += "    public string " + fieldNames[m] + ";" + comm;
                                        }
                                       else if(type == 1)
                                        {
                                           
                                            body += "    public string []"  + fieldNames[m] + ";" + comm;
                                        }
                                        else if(type == 2)
                                        {
                                            
                                            body += "    public string [][]" + fieldNames[m] + ";" + comm;
                                        }
                                        else if(type == 3)
                                        {
                                            body += "    public Dictionary<string,object>" + fieldNames[m] + ";" + comm;
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
                                        body += "    public string " + fieldNames[m] + ";" + comm;
                                    }
                                    break;
                                default:
                                    {
                                        if(!string.IsNullOrEmpty(fieldNames[m]))
                                        {
                                            body += "    public int " + fieldNames[m] + ";" + comm;
                                        }
                                    }
                                    break;
                            }
                        }
                        string str = "";
                        if(CheckBox1.CheckState == CheckState.Checked)
                        {
                            str = nameSp + info + classBegin + body +instanceFun+classEnd;
                        }
                        else
                        {
                            str = nameSp + info + classBegin + body + classEnd;
                        }
                         
                        byte[] classContent = System.Text.Encoding.UTF8.GetBytes(str);
                        using (FileStream fsWrite = new FileStream(path.Replace(fileName+".csv",fileName + ".cs"),FileMode.OpenOrCreate))
                        {
                            fsWrite.Write(classContent, 0, classContent.Length);
                        }
                    }
                    else
                    {
                        failCount++;
                    }
                   
                }
                else
                {
                    failCount++;
                }
                AddItem(files[i]);
                if(files.Length-1 == i)
                {
                    string log = string.Format("生成完毕，总共:{0:D1}个文件   成功:{1:D1}个   失败:{2:D1}个  忽略:{3:D1}个", files.Length, successCount, failCount,igonor);
                    AddItem(log);
                }
            }
        }
        //解析csv行，每列作为数组一个元素返回
        string[]ParseCSVLines(string str)
        {
            List<string> ret = new List<string>();
            bool IsInDoubleQuotes = false;
            string v = "";
            for (int i = 0;i < str.Length;i ++)
            {
                char c = str[i];
                if (c == ',')
                {
                    if(IsInDoubleQuotes)
                    {
                        v += str[i];
                    }
                    else
                    {
                        ret.Add(v);
                        v = "";
                    }
                   
                }
                else if(c == '"')
                {
                    IsInDoubleQuotes = !IsInDoubleQuotes;
                }
                else
                {
                    v += str[i];
                }
            }
            ret.Add(v);
            return ret.ToArray();
        }
        int CheckType(string v,int ind)//0,普通字符串，1，一维数组，2二维数组,3Json
        {
            int ret = 0;
            string[] fields = ParseCSVLines(v);
            if(ind < fields.Length)
            {
                if(fields[ind].StartsWith("[["))
                {
                    ret = 2;
                }
                else if(fields[ind].StartsWith("["))
                {
                    ret = 1;
                }
                else if(fields[ind].StartsWith("{"))
                {
                    ret = 3;
                }
            }
            return ret;
        }
        private void AddItem(string text)
        {

            if (text == null)
                return;
            listView1.Items.Add(text);
        }

        delegate void SetTextCallback(string text);

        private void cb_detectMissing_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tbPath_TextChanged(object sender, EventArgs e)
        {

        }
        private void tbPath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Link; //重要代码：表明是链接类型的数据，比如文件路径
            else e.Effect = DragDropEffects.None;
        }


        private void tbPath_DragDrop(object sender, DragEventArgs e)
        {
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            // MessageBox.Show(path);
            tbPath.Text = path;
        }
    }
}
