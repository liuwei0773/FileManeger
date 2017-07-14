using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    class FileProperty
    {
        public DataGridView dataGridView = null;
        private const string datebaseName = ".\\db.db3";
        private const string tableName = "file_property";

        public FileProperty(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        public void Init()
        {
            dataGridView.Columns.Clear();
            for (int i = 0; i < 13; i++)
            {
                DataGridViewColumn col = new DataGridViewTextBoxColumn();
                dataGridView.Columns.Add(col);
                dataGridView.Columns[i].HeaderText = "-";
            }
            dataGridView.Rows.Insert(0, 100);
        }

        /// <summary>
        /// 创建文件属性表
        /// </summary>
        private bool CreateTable()
        {
            try
            {
                SQLiteDBHelper db = null;
                string sql = null;

                db = new SQLiteDBHelper(datebaseName);

                if(db.IsTableExist(tableName))
                {
                    db.DeleteTable(tableName);
                }

                sql = @"CREATE TABLE file_name(
                    name  TEXT,
                    property1  TEXT,
                    property2  TEXT,
                    property3  TEXT,
                    property4  TEXT,
                    property5  TEXT,
                    property6  TEXT,
                    property7  TEXT,
                    property8  TEXT,
                    property9  TEXT,
                    property10 TEXT)";
                db.ExecuteNonQuery(sql, null);
            }
            catch(Exception)
            {
                return false;
            }
           

            return true;
        }

        /// <summary>
        /// 设置列
        /// </summary>
        protected void SetCol()
        {
            dataGridView.Columns[0].HeaderText = "序号";
            dataGridView.Columns[1].HeaderText = "文件名";
            for (int i = 2; i < 12; i++)
            {
                dataGridView.Columns[i].HeaderText = "属性" + (i - 1).ToString();
            }         
        }

        protected int GetColIndex(string name)
        {
            return File.GetDataGridViewIndex(dataGridView, "name");
        }

        private void UpdateUI()
        {
            SetCol();

            dataGridView.Rows.Clear();
            SQLiteDBHelper db = new SQLiteDBHelper(datebaseName);
            string sql = "select * from file_name";
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                int index = 0;
                while (reader.Read())
                {
                    dataGridView.Rows.Add();
                    dataGridView.Rows[index].Cells[GetColIndex("序号")].Value = index.ToString();
                    dataGridView.Rows[index].Cells[GetColIndex("文件名")].Value = reader["name"].ToString();               
                    ++index;
                }
            }
        }




    }
}
