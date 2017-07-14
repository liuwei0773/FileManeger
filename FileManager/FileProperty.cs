using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class FileProperty
    {
        private const string datebaseName = ".\\db.db3";
        private const string tableName = "file_property";


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
    }
}
