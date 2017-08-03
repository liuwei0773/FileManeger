using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.Data.SQLite;
using System.Windows.Forms;

namespace FileManager
{
    class ExistFiles : GridView
    {   
        public class FileInformation
        {
            public string FileName = "";
            public string Lable = "";
            public string FullName = "";
        }

        public ExistFiles(DataGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        public static ArrayList g_FileInfoCollector = new ArrayList();
     
        static public bool SentToDB()
        {
           
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
                    command.CommandText = "select count(*) from sqlite_master where type = 'table' and name = '" + existFilessName + "'";
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
                sql = "DROP TABLE " + existFilessName;
                db.ExecuteNonQuery(sql, null);
            }

            db = new SQLiteDBHelper(dbPath);
            
            sql = "CREATE TABLE "  +  existFilessName  +  @"(
                    name  TEXT,         
                    filePath   TEXT )";
            db.ExecuteNonQuery(sql, null);

            sql = "INSERT INTO " + existFilessName + "(name,filePath)values(@name, @filePath)";
            db = new SQLiteDBHelper(dbPath);
            for (int i = 0; i < g_FileInfoCollector.Count; i++)
            {
                SQLiteParameter[] parameters = new SQLiteParameter[]{
                    new SQLiteParameter("@name",((FileInformation)g_FileInfoCollector[i]).FileName),
                    new SQLiteParameter("@filePath",((FileInformation)g_FileInfoCollector[i]).FullName)};
                db.ExecuteNonQuery(sql, parameters);
            }

            return true;
        }

       
    }
}
