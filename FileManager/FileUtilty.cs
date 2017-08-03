using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class FileUtilty
    {
        static public ArrayList GetAllFiles(DirectoryInfo dir)
        {
            ArrayList FileList = new ArrayList();
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi.Name);
            }

            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                ArrayList FileListSub = GetAllFiles(d);
                FileList.AddRange(FileListSub);
            }
            return FileList;
        }

        static public ArrayList GetAllFiles(string Path)
        {
            ArrayList FileList = new ArrayList();
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi.Name);
            }

            DirectoryInfo[] allDir = dir.GetDirectories();

            foreach (DirectoryInfo d in allDir)
            {
                ArrayList FileListSub = GetAllFiles(d);
                FileList.AddRange(FileListSub);
            }
            return FileList;
        }

        static public ArrayList GetAllFilesFullName(DirectoryInfo dir)
        {
            ArrayList FileList = new ArrayList();
            FileInfo[] allFile = dir.GetFiles();

            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi.FullName);
            }
            DirectoryInfo[] allDir = dir.GetDirectories();

            foreach (DirectoryInfo d in allDir)
            {
                ArrayList FileListSub = GetAllFilesFullName(d);
                FileList.AddRange(FileListSub);
            }
            return FileList;
        }

        static public ArrayList GetAllFilesFullName(string Path)
        {
            ArrayList FileList = new ArrayList();
            DirectoryInfo dir = new DirectoryInfo(Path);
            FileInfo[] allFile = dir.GetFiles();

            foreach (FileInfo fi in allFile)
            {
                FileList.Add(fi.FullName);
            }

            DirectoryInfo[] allDir = dir.GetDirectories();

            foreach (DirectoryInfo d in allDir)
            {
                ArrayList FileListSub = GetAllFilesFullName(d);
                FileList.AddRange(FileListSub);
            }
            return FileList;
        }

        static public string FullNameToName(string FullName)
        {
            string[] str = FullName.Split('\\');
            return str[str.Length - 1];
        }

        static public string FullNameToDirectionary(string FullName)
        {
            return FullName.Remove(FullName.LastIndexOf('\\'));
        }


        static public int GetDataGridViewIndex(System.Windows.Forms.DataGridView dataGridView, string columnText)
        {
            int index = 0;
            while (index < dataGridView.Columns.Count)
            {
                string str = dataGridView.Columns[index].HeaderText;
                if (columnText == str)
                    break;
                index++;
            }
            return index;
        }
    }
}
