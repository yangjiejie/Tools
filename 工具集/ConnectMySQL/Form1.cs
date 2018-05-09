using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace ConnectMySQL
{
    public partial class Form1 : Form
    {
        private MySqlConnection conn;

        private DataTable data;

        private MySqlDataAdapter da;

        private MySqlCommandBuilder cb;
        public Form1()
        {
            InitializeComponent();
            
        }
        //事件内报错不会抛异常，会导致窗口无法正常关闭
        void OnClosed(object o, FormClosingEventArgs e)
        {
            if (conn != null)
                conn.Close();
            e.Cancel = false;
        }
        #region  建立MySql数据库连接
        /// <summary>
        /// 建立数据库连接.
        /// </summary>
        /// <returns>返回MySqlConnection对象</returns>
        public MySqlConnection getmysqlcon()
        {
            //http://sosoft.cnblogs.com/
            string M_str_sqlcon = "server=localhost;user id=root;password=123456;database=mydatabase"; //根据自己的设置
            MySqlConnection myCon = new MySqlConnection(M_str_sqlcon);
            return myCon;
        }
        #endregion

        #region  执行MySqlCommand命令
        /// <summary>
        /// 执行MySqlCommand
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        public void getmysqlcom(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = this.getmysqlcon();
            mysqlcon.Open();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcom.ExecuteNonQuery();
            mysqlcom.Dispose();
            mysqlcon.Close();
            mysqlcon.Dispose();
        }
        #endregion

        #region  创建MySqlDataReader对象
        /// <summary>
        /// 创建一个MySqlDataReader对象
        /// </summary>
        /// <param name="M_str_sqlstr">SQL语句</param>
        /// <returns>返回MySqlDataReader对象</returns>
        public MySqlDataReader getmysqlread(string M_str_sqlstr)
        {
            MySqlConnection mysqlcon = this.getmysqlcon();
            MySqlCommand mysqlcom = new MySqlCommand(M_str_sqlstr, mysqlcon);
            mysqlcon.Open();
            MySqlDataReader mysqlread = mysqlcom.ExecuteReader(CommandBehavior.CloseConnection);
            return mysqlread;
        }
        #endregion

        private void connectBtn_Click(object sender, System.EventArgs e)
        {
            if (conn != null)
                conn.Close();
            string connStr = String.Format("server={0};user id={1}; password={2}; port={3}; database=mysql; pooling=false; charset=utf8",
    server.Text, userid.Text, password.Text, 3306);

            try

            {

                conn = new MySqlConnection(connStr);

                conn.Open();



                GetDatabases();

               // MessageBox.Show("连接数据库成功!");

            }

            catch (MySqlException ex)

            {

                MessageBox.Show("Error connecting to the server: " + ex.Message);

            }

        }



        private void GetDatabases()
        {
            MySqlDataReader reader = null;
            MySqlCommand cmd = new MySqlCommand("SHOW DATABASES", conn);
            try

            {
                reader = cmd.ExecuteReader();

                databaseList.Items.Clear();
                while (reader.Read())
                {
                    databaseList.Items.Add(reader.GetString(0));
                    
                }
               

            }

            catch (MySqlException ex)

            {

                MessageBox.Show("Failed to populate database list: " + ex.Message);

            }

            finally

            {

                if (reader != null) reader.Close();

            }
            if (databaseList.Items.Count > 0)
                databaseList.SelectedIndex = 0;
        }



        private void databaseList_SelectedIndexChanged(object sender, System.EventArgs e)

        {
            MySqlDataReader reader = null;
            conn.ChangeDatabase(databaseList.SelectedItem.ToString());
            //http://sosoft.cnblogs.com/
            MySqlCommand cmd = new MySqlCommand("SHOW TABLES", conn);
            try
            {
                reader = cmd.ExecuteReader();
                tables.Items.Clear();
                while (reader.Read())
                {
                    tables.Items.Add(reader.GetString(0));
                }
               
            }

            catch (MySqlException ex)

            {

                MessageBox.Show("Failed to populate table list: " + ex.Message);

            }

            finally

            {

                if (reader != null) reader.Close();

            }
            if (tables.Items.Count > 0)
                tables.SelectedIndex = 0;
        }



        private void tables_SelectedIndexChanged(object sender, System.EventArgs e)

        {

            data = new DataTable();



            da = new MySqlDataAdapter("SELECT * FROM " + tables.SelectedItem.ToString(), conn);

            cb = new MySqlCommandBuilder(da); // 此处必须有，否则无法更新



            da.Fill(data);
            dataGrid.DataSource = data;

        }

        private void updateBtn_Click(object sender, System.EventArgs e)

        {

            DataTable changes = data.GetChanges();

            da.Update(changes);

            data.AcceptChanges();

        }
    }

}
