using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace FileManager
{
    public partial class Form1 : Form
    {
        private FileProperty fileProperty = null;
        private ExistFiles existFiles = null;
        public Form1()
        {
            InitializeComponent();
            this.Opacity = 0.95;
            this.Width = UI.g_windowWidth;
            this.Height = UI.g_windowHeight;

            fileProperty = new FileProperty(dataGridView1);
            fileProperty.Init();
            //Init();         
        }

        private void LoadFileProperty()
        {
            fileProperty = new FileProperty(dataGridView1);
            fileProperty.Init();
        }

        private void LoadExistFiles()
        {
            existFiles = new ExistFiles(dataGridView1);
            existFiles.Init();
        }

        /// <summary>
        /// 设置数据报表长宽
        /// </summary>
        public void SetDataGridView1Width()
        {
            dataGridView1.Columns[0].Width = 60;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 120;
            dataGridView1.Columns[3].Width = 120;
        }  

        private void SetCol()
        {
            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "文件名";
            for (int i = 2; i < 12; i++)
            {
                dataGridView1.Columns[i].HeaderText = "属性" + (i - 1).ToString();
            }
            dataGridView1.Columns[12].HeaderText = "路径";
        }

        public void UpdataGridView(ArrayList FileInfoCollector)
        {
            SetCol();
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Insert(0, 100);

            int serialIndex = FileUtilty.GetDataGridViewIndex(dataGridView1, "序号");
            int fileNameIndex = FileUtilty.GetDataGridViewIndex(dataGridView1, "文件名");
            int pathIndex = FileUtilty.GetDataGridViewIndex(dataGridView1, "路径");

            const string dbPath = ExistFiles.dbPath;
            if (!System.IO.File.Exists(dbPath))
                return;


            SQLiteDBHelper db = new SQLiteDBHelper(dbPath);
            string sql = "select * from " + ExistFiles.existFilessName;
            using (SQLiteDataReader reader = db.ExecuteReader(sql, null))
            {
                int index = 0;
                while (reader.Read())
                {
                    if (index > dataGridView1.Rows.Count - 2)
                        dataGridView1.Rows.Add();

                    dataGridView1.Rows[index].Cells[serialIndex].Value = index.ToString();
                    dataGridView1.Rows[index].Cells[fileNameIndex].Value = reader["name"].ToString();
                    dataGridView1.Rows[index].Cells[pathIndex].Value = reader["filePath"].ToString();
                    ++index;
                }
            }

        }

        #region 菜单响应函数
        /// <summary>
        /// 打开目录响应函数
        /// </summary>
        public void OpenDirectionary(string Path)
        {
            ArrayList FileList = FileUtilty.GetAllFilesFullName(Path);
            for (int i = 0; i < FileList.Count; i++)
            {
                ExistFiles.FileInformation fi = new ExistFiles.FileInformation();
                fi.FileName = FileUtilty.FullNameToName(FileList[i].ToString());
                fi.FullName = FileList[i].ToString();
                fi.Lable = "Lable:";
                ExistFiles.g_FileInfoCollector.Add(fi);
            }

            ExistFiles.SentToDB();

            SetCol();
            SetDataGridView1Width();
        }

        private void btnOpenDirectionary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExistFiles.g_FileInfoCollector.Clear();
                string fileName = openFileDialog.SelectedPath;
                OpenDirectionary(fileName);
                UpdataGridView(ExistFiles.g_FileInfoCollector);
            }
        }



        private void tsmOpen_Click(object sender, EventArgs e)
        {
            int index = FileUtilty.GetDataGridViewIndex(dataGridView1, "路径");
            int count = dataGridView1.SelectedCells.Count;
            if (count < 1)
                return;
            int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
            string FileName = dataGridView1.Rows[selectedRowIndex].Cells[index].Value.ToString();
            FileName = FileName.Trim();
            System.Diagnostics.Process.Start(FileName);
        }

        private void tsmOpenDirect_Click(object sender, EventArgs e)
        {
            try
            {
                int index = FileUtilty.GetDataGridViewIndex(dataGridView1, "路径");
                int count = dataGridView1.SelectedCells.Count;
                if (count < 1)
                    return;
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                string FileName = dataGridView1.Rows[selectedRowIndex].Cells[index].Value.ToString();
                string direct = FileUtilty.FullNameToDirectionary(FileName);
                direct = direct.Trim();
                System.Diagnostics.Process.Start(direct);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.ToString());
            }

        }
        #endregion


        #region 表格UI响应函数
        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            int selectedItem = 0;
            if (dataGridView1.SelectedRows.Count > 0)
            {
                selectedItem = dataGridView1.SelectedRows[0].Index;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                Point PointMouse = dataGridView1.PointToScreen(new Point(e.X, e.Y));
                contextMenuStrip1.Show(PointMouse);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                int a = 0;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
        }
        #endregion


        #region 主界面按钮响应函数
        private void Save_Click(object sender, EventArgs e)
        {

        }

        private void Load_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }

 
}
