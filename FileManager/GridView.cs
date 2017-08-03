using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    class GridView
    {
        protected DataGridView dataGridView = null;
        protected const string datebaseName = ".\\db.db3";
        protected const string tableName = "file_property";
        public const string dbPath = ".\\db.db3";
        public const string existFilessName = "exist_files";

        public virtual void Init()
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

        protected virtual bool CreateTable()
        {
            return false;
        }

        protected virtual void SetCol()
        {
           
        }

        protected int GetColIndex(string name)
        {
            return FileUtilty.GetDataGridViewIndex(dataGridView, name);
        }

        protected virtual void UpdateUI()
        {

        }

    }
}
