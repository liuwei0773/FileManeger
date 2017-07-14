using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Data.SQLite;

namespace FileManager
{
    class File
    {
        public static ArrayList g_FileInfoCollector = new ArrayList();

        public class FileInformation
        {
            public string FileName = "";
            public string Lable = "";
            public string FullName = "";
        }

        static public bool SentToDB()
        {
            const string dbPath = ".\\db.db3";          
            if (!System.IO.File.Exists(dbPath))
            {
                SQLiteDBHelper.CreateDB(dbPath);
            }

            bool tableExist = false;
            using (SQLiteConnection connection = new SQLiteConnection("Data Source=" + dbPath))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "select count(*) from sqlite_master where type = 'table' and name = 'movie_name'";
                    int num = Convert.ToInt32(command.ExecuteScalar());
                    if (num > 0)
                        tableExist = true;
                }
            }

            SQLiteDBHelper db = null;
            string sql = null;
            if (tableExist)
            {
                db = new SQLiteDBHelper(dbPath);
                sql = "DROP TABLE movie_name";
                db.ExecuteNonQuery(sql, null);
            }

            db = new SQLiteDBHelper(dbPath);
            sql = "CREATE TABLE movie_name(Name TEXT NOT NULL , Actor1  REFERENCES actor_name(Name), Actor2 REFERENCES actor_name(Name),Actor3 REFERENCES actor_name(Name), Actor4  REFERENCES actor_name(Name), Actor5  REFERENCES actor_name(Name), FilePath TEXT PRIMARY KEY)";
            db.ExecuteNonQuery(sql, null);

            sql = "INSERT INTO movie_name(Name,FilePath)values(@Name,@FilePath)";
            db = new SQLiteDBHelper(dbPath);
            for (int i = 0; i < g_FileInfoCollector.Count; i++)
            {
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@Name",((FileInformation)g_FileInfoCollector[i]).FileName),
                    new SQLiteParameter("@FilePath",((FileInformation)g_FileInfoCollector[i]).FullName)};
                db.ExecuteNonQuery(sql, parameters);
            }

            return true;
        }

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

        static public ArrayList SearchLable(ArrayList fileInfomationArr, string lable)
        {
            ArrayList foundIndex = new ArrayList();
            for (int i = 0; i < fileInfomationArr.Count; i++)
            {
                FileInformation fi = fileInfomationArr[i] as FileInformation;
                if (fi.Lable.IndexOf(lable) != -1)
                {
                    foundIndex.Add(i);
                }
            }
            return foundIndex;
        }

        static public bool ModifyFileInfomation(ArrayList fileInfomationArr, int index, FileInformation fi)
        {
            if (index < 0 || index > fileInfomationArr.Count - 1)
                return false;
            fileInfomationArr[index] = fi;
            return true;
        }

        static public bool SaveFileInfomation(ArrayList fileInfomationArr, string path = "")
        {
            FileStream fs = new FileStream(".\\FileManagerData.txt", FileMode.Create);

            for (int i = 0; i < fileInfomationArr.Count; i++)
            {
                byte[] data = System.Text.Encoding.Default.GetBytes(((FileInformation)fileInfomationArr[i]).FileName + "[/] ");
                fs.Write(data, 0, data.Length);

                data = System.Text.Encoding.Default.GetBytes(((FileInformation)fileInfomationArr[i]).Lable + "[/] ");
                fs.Write(data, 0, data.Length);

                data = System.Text.Encoding.Default.GetBytes(((FileInformation)fileInfomationArr[i]).FullName + "[/] ");
                fs.Write(data, 0, data.Length);

                data = System.Text.Encoding.Default.GetBytes("\r\n");
                fs.Write(data, 0, data.Length);
            }
            
            
            fs.Flush();
            fs.Close();
            return true;

        }

        static public bool ReadFileInfomation(ArrayList fileInfomationArr, string fileName = "")
        {
            try
            {
                FileStream file = new FileStream(".\\FileManagerData.txt", FileMode.Open);
                byte[] byData = new byte[file.Length];
                char[] charData = new char[file.Length];

                file.Seek(0, SeekOrigin.Begin);
                file.Read(byData, 0, (int)file.Length);
                Decoder d = Encoding.Default.GetDecoder();
                d.GetChars(byData, 0, byData.Length, charData, 0);
                string strData = new string(charData);
                strData = strData.Replace("\r\n", "");
                string[] strSingles = strData.Split(new string[]{"[/]"}, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < (strSingles.Length-1)/3; i++)
                {
                    FileInformation inf = new FileInformation();
                    inf.FileName = strSingles[i * 3 + 0].Trim();
                    inf.Lable = strSingles[i * 3 + 1].Trim();
                    inf.FullName = strSingles[i * 3 + 2].Trim();
                    fileInfomationArr.Add(inf);
                }
                    file.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        static public bool CreateNewFileInformation(string path, ArrayList FileInfoCollector)
        {
            ArrayList fileFullNames = GetAllFilesFullName(path);
            
            for (int i = 0; i < fileFullNames.Count; i++)
            {
                string fileName = FullNameToName(fileFullNames[i].ToString());
                FileInformation fi = new FileInformation();
                fi.FileName = fileName;
                fi.FullName = fileFullNames[i].ToString();
                fi.Lable = "Lable:";
                FileInfoCollector.Add(fi);
            }            
            return true;
        }


        static public int GetListViewColumnIndex(System.Windows.Forms.ListView listView, string columnText)
        {
            int index = 0;
            while (index < listView.Columns.Count)
            {
                if (columnText == listView.Columns[index].Text)
                    break;
                index++;
            }
            return index;
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
