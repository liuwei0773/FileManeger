﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.IO;

namespace FileManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Opacity = 0.95;
            this.Width = UI.g_windowWidth;
            this.Height = UI.g_windowHeight;
            Init();
            //SQLiteDBHelper.CreateTable();
           // SQLiteDBHelper.InsertData();
           // SQLiteDBHelper.ShowData();
        }

        public void Init()
        {
            
            for (int i = 0; i < 4; i++)
            {
                DataGridViewColumn col = new DataGridViewTextBoxColumn();
                dataGridView1.Columns.Add(col);
            }
            dataGridView1.Columns[0].HeaderText = "-";
            dataGridView1.Columns[1].HeaderText = "-";
            dataGridView1.Columns[2].HeaderText = "-";
            dataGridView1.Columns[3].HeaderText = "-";          
            dataGridView1.Rows.Insert(0, 5);
            SetDataGridView1Width();
            Load_Click(null, null);
        }

        /// <summary>
        /// 设置数据报表长宽
        /// </summary>
        public void SetDataGridView1Width()
        {
            dataGridView1.Columns[0].Width = 80;
            dataGridView1.Columns[1].Width = 180;
            dataGridView1.Columns[2].Width = 180;
            dataGridView1.Columns[3].Width = 680;
        }

        /// <summary>
        /// 打开目录响应函数
        /// </summary>
        public void OpenDirectionary(string Path)
        {
            ArrayList FileList = File.GetAllFilesFullName(Path);
            for (int i = 0; i < FileList.Count; i++) 
            {
                File.FileInformation fi = new File.FileInformation();
                fi.FileName = File.FullNameToName(FileList[i].ToString());
                fi.FullName = FileList[i].ToString();
                fi.Lable = "Lable:";
                File.g_FileInfoCollector.Add(fi);
            }

            File.SentToDB();

            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "文件名";
            dataGridView1.Columns[2].HeaderText = "标签";
            dataGridView1.Columns[3].HeaderText = "路径";
            SetDataGridView1Width();
        }

  

        /// <summary>
        /// 数据报表鼠标点击事件
        /// </summary>
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

        private void btnOpenDirectionary_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {              
                File.g_FileInfoCollector.Clear();
                string fileName = openFileDialog.SelectedPath;
                OpenDirectionary(fileName);
                UpdataGridView(File.g_FileInfoCollector);
            }
        }

        private void button1_Click_0(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "文本文件|*.*|C#文件|*.cs|所有文件|*.*";
            openFileDialog.RestoreDirectory = false;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;
                OpenDirectionary(fileName);
            }
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            int index = File.GetDataGridViewIndex(dataGridView1, "路径");
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
                int index = File.GetDataGridViewIndex(dataGridView1, "路径");
                int count = dataGridView1.SelectedCells.Count;
                if (count < 1)
                    return;
                int selectedRowIndex = dataGridView1.SelectedCells[0].RowIndex;
                string FileName = dataGridView1.Rows[selectedRowIndex].Cells[index].Value.ToString();
                string direct = File.FullNameToDirectionary(FileName);
                direct = direct.Trim();
                System.Diagnostics.Process.Start(direct);
            }
            catch(Exception err)
            {
                MessageBox.Show(err.ToString());
            }
            
        }

        private void Save_Click(object sender, EventArgs e)
        {
            File.SaveFileInfomation(File.g_FileInfoCollector);
        }

        private void Load_Click(object sender, EventArgs e)
        {
            File.g_FileInfoCollector.Clear();
            File.ReadFileInfomation(File.g_FileInfoCollector);         
            UpdataGridView(File.g_FileInfoCollector);
        }

     

         public void UpdataGridView(ArrayList FileInfoCollector)
        {
            dataGridView1.Columns[0].HeaderText = "序号";
            dataGridView1.Columns[1].HeaderText = "文件名";
            dataGridView1.Columns[2].HeaderText = "标签";
            dataGridView1.Columns[3].HeaderText = "路径";
            dataGridView1.Rows.Clear();
              for (int i = 0; i < FileInfoCollector.Count; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = i.ToString();
                dataGridView1.Rows[i].Cells[1].Value = ((File.FileInformation)FileInfoCollector[i]).FileName;
                dataGridView1.Rows[i].Cells[2].Value = ((File.FileInformation)FileInfoCollector[i]).Lable.Replace("Lable:", "");
                dataGridView1.Rows[i].Cells[3].Value = ((File.FileInformation)FileInfoCollector[i]).FullName;
            }
             
        }   
        
    }

 
}