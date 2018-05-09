using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Threading;
using System.Timers;
namespace SensitiveNameCheck
{
    public partial class NameCheckForm : Form
    {
        List<string> resultInfos = new List<string>();
        static public int runTimes = 0, sleepT = 5, waitCount = 100;

        private delegate void SetInfo(string vinfo,string state);//代理
        private delegate void SetTimer(object source, ElapsedEventArgs e);
        System.Timers.Timer timer;
        private DateTime useTime;
        List<ResultInfo> resultInfo = new List<ResultInfo>();
        WorkThreadBase thread1 = null, thread2 = null, thread3 = null;
        public static NameCheckForm Instance = null;
        int finishedThreadCount = 0;
        public NameCheckForm()
        {
            Instance = this;
            InitializeComponent();
            this.Location = new Point(0, 0);
            this.listView1.Columns.Add("", 0);
            this.listView1.Columns.Add("冲突名字", 100);
            this.listView1.Columns.Add("名字库ID", 100);
            this.listView1.Columns.Add("敏感字库ID", 100);
            this.listView1.Columns.Add("敏感字", 100);
            listView1.Columns[0].Width = 100;
            listView1.Columns[0].Text = "组合方式";
            listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            listView1.LabelEdit = false;
            this.listView1.View = View.Details;
            this.listView1.Sorting = SortOrder.None;
            int rs = 0;
            if (Int32.TryParse(textBox5.Text, out rs))
            {
                waitCount = rs;
            }
            if (Int32.TryParse(textBox4.Text, out rs))
            {
                sleepT = rs;
            }
        }
        //修改循环执行多少次后休眠
        void OnWaitCountChange(object o, System.EventArgs e)
        {
            int rs = 0;
            if (Int32.TryParse(textBox5.Text, out rs))
            {
                waitCount = rs;
            }
        }
        //修改休眠时长,毫秒
        void OnSleepTimeChange(object o, System.EventArgs e)
        {
            int rs = 0;
            if (Int32.TryParse(textBox4.Text, out rs))
            {
                sleepT = rs;
            }
        }
        //事件内报错不会抛异常，会导致窗口无法正常关闭
        void OnClosed(object o, FormClosingEventArgs e)
        {
            if (null != timer)
            {
                timer.Enabled = false;
            }

            if (null != thread1)
            {
                thread1.Stop();
            }
            if (null != thread2)
            {
                thread2.Stop();
            }
            if (null != thread3)
            {
                thread3.Stop();
            }
            e.Cancel = false;
        }
        //选择名字库文件
        private void SelectNameFile(object sender, EventArgs e)
        {

            DialogResult result = this.folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = this.folderBrowserDialog.FileName;
            }
            else
            {
                return;
            }
        }
        //选择敏感字库文件
        private void SelectFilterNameFile(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox3.Text = this.folderBrowserDialog.FileName;
            }
            else
            {
                return;
            }
        }
        //导出文件
        private void WriteToFile(object sender, EventArgs e)
        {
            if (resultInfo.Count == 0)
            {
                MessageBox.Show(this, "列表为空!", "信息");
                return;
            }
            resultInfo.Sort();
            FileStream fs = new FileStream("result.txt", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs, Encoding.Unicode);
            for (int i = 0; i < resultInfo.Count; i++)
            {
                var ri = resultInfo[i];
                string rs = "";
                ri.style = Fill(ri.style, ' ', 24);
                rs += ri.style;
                ri.resultName = Fill(ri.resultName, ' ', 24);
                rs += ri.resultName;
                ri.ids = Fill(ri.ids, ' ', 24);
                rs += ri.ids;
                ri.sensitiveId = Fill(ri.sensitiveId, ' ', 24);
                rs += ri.sensitiveId;
                ri.sensitiveName = Fill(ri.sensitiveName, ' ', 24);
                rs += ri.sensitiveName;
                sw.WriteLine(rs);
            }
            sw.Close();
            fs.Close();
            MessageBox.Show(this, "文件导出完毕!", "信息");
        }
        string Fill(string target, char ch, int totalLen)
        {
            string ret = target;
            if (target.Length >= totalLen)
                return target;
            int fn = totalLen - target.Length;
            if (target.Length % 2 != 0)
                fn -= 2;
            for (int i = 0; i < fn; i++)
            {
                ret += ch;
            }
            return ret;
        }
        //开始检测
        private void BeginCheck(object sender, EventArgs e)
        {
            if (!textBox1.Text.EndsWith(".xlsx") || textBox1.Text.EndsWith(".xls"))
            {
                MessageBox.Show("请选择名字库文件,必须是excel文件!");
                return;
            }
            if (!textBox3.Text.EndsWith(".xlsx") || textBox3.Text.EndsWith(".xls"))
            {
                MessageBox.Show("敏感字库文件,必须是excel文件!");
                return;
            }
            if(this.label5.Text != ""&&this.label5.Text != "检测完成!")
            {
                MessageBox.Show("正在检测中，请稍候!");
                return;
            }
            useTime = DateTime.Now;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(this.Tick);//到达时间的时候执行事件；
            timer.AutoReset = true;
            timer.Enabled = true;

            thread1 = new CheckMaleWorkT(textBox1.Text, textBox3.Text);
            thread1.Start();
            thread2 = new CheckFemaleWorkT1(textBox1.Text, textBox3.Text);
            thread2.Start();
            thread3 = new CheckFemaleWorkT2(textBox1.Text, textBox3.Text);
            thread3.Start();
        }
        void Tick(object source, ElapsedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                SetTimer si = new SetTimer(Tick);
                this.Invoke(si, new object[] { null, null });
            }
            else
            {
                var ts = DateTime.Now - useTime;
                double n = ts.TotalSeconds;
                if (n == 0)
                    n = 1;
                int sp = (int)(runTimes / n);
                this.label6.Text = "耗时:" + ts.ToString(@"hh\:mm\:ss") + " 速度:" + sp + "次/秒";
            }
        }

        //设置显示信息
        public void ShowInfo(string _info,string _state)
        {
            if (this.InvokeRequired)
            {
                SetInfo si = new SetInfo(ShowInfo);
                this.Invoke(si, new object[] { _info,_state });
            }
            else
            {
                this.textBox2.Text = runTimes + "";
                
                switch(_state)
                {
                    case "running":
                        this.label5.Text = "正在检测......";
                        break;
                    case "finished":
                        finishedThreadCount++;
                        break;
                    default:
                        break;
                }
                if(finishedThreadCount == 3)
                {
                    this.label5.Text = "检测完成!";
                    timer.Enabled = false;
                }
                if (_info != "")
                {
                    string[] arr = _info.Split('*');
                    var arr1 = arr[3].Split(',');
                    ResultInfo si = new ResultInfo();
                    si.style = arr[0];
                    si.resultName = arr[1];
                    si.ids = arr[2];
                    si.sensitiveId = arr1[0];
                    si.sensitiveName = arr1[1];
                    resultInfo.Add(si);
                    this.AddItem(si);
                }

            }
        }
        void AddItem(ResultInfo si)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = si.style;
            lvi.SubItems.Add("");
            lvi.SubItems.Add(si.resultName);
            lvi.SubItems.Add(si.ids);
            lvi.SubItems.Add(si.sensitiveId);
            lvi.SubItems.Add(si.sensitiveName);
            listView1.Items.Add(lvi);
        }

        private List<string> getExcelData(string file)
        {
            return GetSheetsName(file);
        }

        public List<string> GetSheetsName(string pExcelAddress)
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
            var result = new DataSet();
            adapter.Fill(result);

            Conn.Close();
            return result;
        }
    }
    public class ResultInfo : IComparable<ResultInfo>
    {
        public string style;
        public string resultName;
        public string ids;
        public string sensitiveId;
        public string sensitiveName;

        public int CompareTo(ResultInfo other)
        {
            return sensitiveName.CompareTo(other.sensitiveName);
        }
    }
    //工作线程,抽象基类
    public abstract class WorkThreadBase
    {
        protected Thread thread;
        protected object locker = new object();
        protected List<string> FList = new List<string>();//姓
        protected List<string> MN1 = new List<string>();//男单
        protected List<string> MN2 = new List<string>();//男双
        protected List<string> FN1 = new List<string>();//女单
        protected List<string> FN2 = new List<string>();//女双
        protected List<string> filters = new List<string>();//敏感字库
        string nameFilePath = "", sensitveFilePath;
        protected int run = 0;
        protected string state;
        public WorkThreadBase(string _p1, string _p2)
        {
            nameFilePath = _p1;
            sensitveFilePath = _p2;
            thread = new Thread(new ThreadStart(this.Check));
            thread.Name = "check1";
            thread.Priority = ThreadPriority.AboveNormal;
            this.Load();
        }
        public void Stop()
        {
            if (null != thread)
            {
                thread.Abort();
            }
        }
        public void Start()
        {
            thread.Start();
        }
        protected abstract void Check();
        void Load()
        {
            FList.Clear();
            MN1.Clear();
            MN2.Clear();
            FN1.Clear();
            FN2.Clear();
            filters.Clear();

                                                            //读取名字库
        DataSet result = xsldata(nameFilePath, "Sheet1");
            int maxColumCount = 0;
            int row_no = 3, ind = 0;
            List<string> columnNames = new List<string>();
            for (int i = 0; i < result.Tables[0].Columns.Count; i++)
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

                List<string> feilds = new List<string>();
                for (int i = 0; i < maxColumCount; i++)
                {
                    var r = result.Tables[ind].Rows[row_no][i];
                    feilds.Add(Convert.ToString(r));
                }
                if (!string.IsNullOrEmpty(feilds[1]))
                    FList.Add(feilds[1]);
                if (!string.IsNullOrEmpty(feilds[2]))
                    MN1.Add(feilds[2]);
                if (!string.IsNullOrEmpty(feilds[3]))
                    MN2.Add(feilds[3]);
                if (!string.IsNullOrEmpty(feilds[4]))
                    FN1.Add(feilds[4]);
                if (!string.IsNullOrEmpty(feilds[5]))
                    FN2.Add(feilds[5]);
                row_no++;
            }
            //读取敏感字库
            result = xsldata(sensitveFilePath, "Sheet1");
            maxColumCount = 0;
            row_no = 3;
            columnNames.Clear();
            for (int i = 0; i < result.Tables[0].Columns.Count; i++)
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
                List<string> feilds = new List<string>();
                for (int i = 0; i < maxColumCount; i++)
                {

                    var r = result.Tables[ind].Rows[row_no][i];
                    feilds.Add(Convert.ToString(r));
                }
                if (!string.IsNullOrEmpty(feilds[1]))
                    filters.Add(feilds[1]);
                row_no++;
            }
        }
        private List<string> getExcelData(string file)
        {
            return GetSheetsName(file);
        }

        public List<string> GetSheetsName(string pExcelAddress)
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
            var result = new DataSet();
            adapter.Fill(result);

            Conn.Close();
            return result;
        }
        
        protected string IsValid(string targetName)
        {
            for (int i = 0; i < filters.Count; i++)
            {
                if (filters[i] == targetName)
                {
                    return (i + 1) + "," + filters[i];
                }
                if (targetName.Contains(filters[i]))
                {
                    return (i + 1) + "," + filters[i];
                }
            }
            return "";
        }
    }

    //检测男名工作线程
    public class CheckMaleWorkT : WorkThreadBase
    {
        public CheckMaleWorkT(string _p1, string _p2) : base(_p1, _p2)
        {

        }
        protected override void Check()
        {
            state = "running";
            //随机男名
            //姓+男单
            for (int i = 0; i < FList.Count; i++)
            {
                for (int j = 0; j < MN1.Count; j++)
                {
                    var v = IsValid(FList[i] + MN1[j]);
                    string info = "";
                    if (v != "")
                    {
                        //检测不通过
                        info += "姓+男单" + "*";
                        info += FList[i] + MN1[j] + "*";
                        info += (i + 1) + "," + (j + 1) + "*";
                        info += v;
                    }
                    NameCheckForm.Instance.ShowInfo(info,state);
                    NameCheckForm.runTimes++;
                    run++;
                    if (run % NameCheckForm.waitCount == 0)
                        Thread.Sleep(NameCheckForm.sleepT);
                }
            }
            // currentStyle = "男双+姓+男单";
            //男双+姓+男单
            for (int i = 0; i < MN2.Count; i++)
            {
                for (int j = 0; j < FList.Count; j++)
                {
                    for (int m = 0; m < MN1.Count; m++)
                    {
                        var v = IsValid(MN2[i] + FList[j] + MN1[m]);
                        string info = "";
                        if (v != "")
                        {
                            //检测不通过
                            info += "男双+姓+男单" + "*";
                            info += MN2[i] + FList[j] + MN1[m] + "*";
                            info += (i + 1) + "," + (j + 1) + "," + (m + 1) + "*";
                            info += v;
                        }
                        NameCheckForm.runTimes++;
                        run++;
                        NameCheckForm.Instance.ShowInfo(info,state);
                        if (run % NameCheckForm.waitCount == 0)
                            Thread.Sleep(NameCheckForm.sleepT);
                    }
                }
            }
            state = "finished";
            NameCheckForm.Instance.ShowInfo("", state);
        }
    }
    //检测女名工作线程1
    public class CheckFemaleWorkT1 : WorkThreadBase
    {
        public CheckFemaleWorkT1(string _p1, string _p2) : base(_p1, _p2)
        {

        }

        protected override void Check()
        {
            state = "running";
            //随机女名
            //姓+女单
            for (int i = 0; i < FList.Count; i++)
            {
                for (int j = 0; j < FN1.Count; j++)
                {
                    var v = IsValid(FList[i] + FN1[j]);
                    string info = "";
                    if (v != "")
                    {
                        //检测不通过
                        info += "姓+女单" + "*";
                        info += FList[i] + FN1[j] + "*";
                        info += (i + 1) + "," + (j + 1) + "*";
                        info += v;
                    }
                    NameCheckForm.runTimes++;
                    NameCheckForm.Instance.ShowInfo(info,state);
                    if (run % NameCheckForm.waitCount == 0)
                        Thread.Sleep(NameCheckForm.sleepT);
                }
            }

            //男双+姓+女单
            for (int i = 0; i < MN2.Count; i++)
            {
                for (int j = 0; j < FList.Count; j++)
                {
                    for (int m = 0; m < FN1.Count; m++)
                    {
                        var v = IsValid(MN2[i] + FList[j] + FN1[m]);
                        string info = "";
                        if (v != "")
                        {
                            //检测不通过
                            info += "男双+姓+女单" + "*";
                            info += MN2[i] + FList[j] + FN1[m] + "*";
                            info += (i + 1) + "," + (j + 1) + "," + (m + 1) + "*";
                            info += v;
                        }
                        NameCheckForm.runTimes++;
                        NameCheckForm.Instance.ShowInfo(info,state);
                        if (run % NameCheckForm.waitCount == 0)
                            Thread.Sleep(NameCheckForm.sleepT);
                    }
                }
            }
            state = "finished";
            NameCheckForm.Instance.ShowInfo("", state);
        }
    }
    //检测女名工作线程2
    public class CheckFemaleWorkT2 : WorkThreadBase
    {
        public CheckFemaleWorkT2(string _p1, string _p2) : base(_p1, _p2)
        {

        }

        protected override void Check()
        {
            state = "running";
            //女双+姓+女单
            for (int i = 0; i < FN2.Count; i++)
            {
                for (int j = 0; j < FList.Count; j++)
                {
                    for (int m = 0; m < FN1.Count; m++)
                    {
                        var v = IsValid(FN2[i] + FList[j] + FN1[m]);
                        string info = "";
                        if (v != "")
                        {
                            //检测不通过
                            info += "女双+姓+女单" + "*";
                            info += FN2[i] + FList[j] + FN1[m] + "*";
                            info += (i + 1) + "," + (j + 1) + "," + (m + 1) + "*";
                            info += v;
                        }
                        if (run % NameCheckForm.waitCount == 0)
                            Thread.Sleep(NameCheckForm.sleepT);
                        NameCheckForm.runTimes++;
                        NameCheckForm.Instance.ShowInfo(info,state);
                    }
                }
            }
            state = "finished";
            NameCheckForm.Instance.ShowInfo("", state);
        }
    }
}
