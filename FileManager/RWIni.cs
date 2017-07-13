using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace FileManager
{
    /// <summary> 
    /// IniFiles的类 
    /// </summary> 
    public class RWIni
    {
        public string FileName; //INI文件名 
        //声明读写INI文件的API函数 
        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);
        //类的构造函数，传递INI文件名 

        public RWIni(string AFileName)
        {
            // 判断文件是否存在 
            FileInfo fileInfo = new FileInfo(AFileName);
            if ((!fileInfo.Exists))
            { 
                System.IO.StreamWriter sw = new System.IO.StreamWriter(AFileName, false, System.Text.Encoding.Default);
                try
                {
                    sw.Write("#表格配置档案");
                    sw.Close();
                }
                catch
                {
                    throw (new ApplicationException("Ini文件不存在"));
                }
            }
            //必须是完全路径，不能是相对路径 
            FileName = fileInfo.FullName;
        }
        //写INI文件 
        public void WriteString(string section, string key, string value)
        {
            if (!WritePrivateProfileString(section, key, value, FileName))
            {
                throw (new ApplicationException("写Ini文件出错"));
            }
        }

        //读取INI文件指定 
        public string ReadString(string section, string key, string def)
        {
            Byte[] Buffer = new Byte[65535];
            int bufLen = GetPrivateProfileString(section, key, def, Buffer, Buffer.GetUpperBound(0), FileName);
            //必须设定0（系统默认的代码页）的编码方式，否则无法支持中文 
            string s = Encoding.GetEncoding(0).GetString(Buffer);
            s = s.Substring(0, bufLen);
            return s.Trim();
        }

        //读整数 
        public int ReadInteger(string section, string key, int def)
        {
            string intStr = ReadString(section, key, Convert.ToString(def));
            try
            {
                return Convert.ToInt32(intStr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return def;
            }
        }

        //写整数 
        public void WriteInteger(string section, string key, int value)
        {
            WriteString(section, key, value.ToString());
        }

        //读bool 
        public bool ReadBool(string section, string key, bool def)
        {
            try
            {
                return Convert.ToBoolean(ReadString(section, key, Convert.ToString(def)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return def;
            }
        }

        //写bool 
        public void WriteBool(string section, string key, bool value)
        {
            WriteString(section, key, Convert.ToString(value));
        }

        //写double
        public void WriteDouble(string section, string key, double value)
        {
            WriteString(section, key, Convert.ToString(value));
        }

        //读double
        public double ReadDouble(string section, string key, double def)
        {
            try
            {
                return Convert.ToDouble(ReadString(section, key, Convert.ToString(def)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return def;
            }
        }

        //从Ini文件中，将指定的section名称中的所有key添加到列表中 
        public void ReadSection(string section, StringCollection Keys)
        {
            Byte[] Buffer = new Byte[16384];

            int bufLen = GetPrivateProfileString(section, null, null, Buffer, Buffer.GetUpperBound(0),
              FileName);

            //对section进行解析 
            GetStringsFromBuffer(Buffer, bufLen, Keys);
        }

        private void GetStringsFromBuffer(Byte[] Buffer, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffer[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffer, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        //从Ini文件中，读取所有的Sections的名称 
        public void ReadSections(StringCollection SectionList)
        {
            //Note:必须得用Bytes来实现，StringBuilder只能取到第一个section 
            byte[] Buffer = new byte[65535];
            int bufLen = 0;
            bufLen = GetPrivateProfileString(null, null, null, Buffer,
              Buffer.GetUpperBound(0), FileName);
            GetStringsFromBuffer(Buffer, bufLen, SectionList);
        }

        //读取指定的section的所有value到列表中 
        public void ReadSectionValues(string section, NameValueCollection Values)
        {
            StringCollection KeyList = new StringCollection();
            ReadSection(section, KeyList);
            Values.Clear();
            foreach (string key in KeyList)
            {
                Values.Add(key, ReadString(section, key, ""));

            }
        }
   
        //清除某个section 
        public void EraseSection(string section)
        {
            if (!WritePrivateProfileString(section, null, null, FileName))
            {
                throw (new ApplicationException("无法清除Ini文件中的section"));
            }
        }

        //删除某个section下的键 
        public void DeleteKey(string section, string key)
        {
            WritePrivateProfileString(section, key, null, FileName);
        }

        //Note:对于Win9X，来说需要实现UpdateFile方法将缓冲中的数据写入文件 
        //在Win NT, 2000和XP上，都是直接写文件，没有缓冲，所以，无须实现UpdateFile 
        //执行完对Ini文件的修改之后，应该调用本方法更新缓冲区。 
        public void UpdateFile()
        {
            WritePrivateProfileString(null, null, null, FileName);
        }

        //检查某个section下的某个键值是否存在 
        public bool ValueExists(string section, string key)
        {
            StringCollection Idents = new StringCollection();
            ReadSection(section, Idents);
            return Idents.IndexOf(key) > -1;
        }

        //确保资源的释放 
        ~RWIni()
        {
            UpdateFile();
        }
    }
}

