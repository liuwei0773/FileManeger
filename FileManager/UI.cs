using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    class UI
    {
        static public int g_windowWidth = 1200;
        static public int g_windowHeight = 800;
        static public int g_listViewColumnNum = 0;
        static public int[] g_dataGridView1Width = new int[128];

        static public bool SaveUI(Form1 form)
        {
            RWIni rwIni = new RWIni(".\\Config.ini");

            rwIni.WriteInteger("UI", "g_windowWidth", g_windowWidth);
            rwIni.WriteInteger("UI", "g_windowHeight", g_windowHeight);

            g_listViewColumnNum = form.dataGridView1.Columns.Count;
            rwIni.WriteInteger("UI", "g_listViewColumnNum", g_listViewColumnNum);
            for (int i = 0; i < g_listViewColumnNum; i++)
            {
                g_dataGridView1Width[i] = form.dataGridView1.Columns[i].Width;
                rwIni.WriteInteger("UI", "g_listViewColumnWidth" + i.ToString(), g_dataGridView1Width[i]);
            }
            return true;
        }

        static public void LoadUI()
        {
            RWIni rwIni = new RWIni(".\\Config.ini");

            g_windowWidth = rwIni.ReadInteger("UI", "g_windowWidth", 1200);
            g_windowHeight = rwIni.ReadInteger("UI", "g_windowHeight", 800);

            g_listViewColumnNum = rwIni.ReadInteger("UI", "g_listViewColumnNum", 5);
            for (int i = 0; i < g_listViewColumnNum; i++)
            {
                g_dataGridView1Width[i] = rwIni.ReadInteger("UI", "g_listViewColumnWidth" + i.ToString(), 80);
                if (g_dataGridView1Width[i] > 10000 || g_dataGridView1Width[i] < 20)
                    g_dataGridView1Width[i] = 80;

            }
            g_dataGridView1Width[0] = 80;
            g_dataGridView1Width[1] = 120;
            g_dataGridView1Width[2] = 380;
            g_dataGridView1Width[3] = 380;
            g_windowWidth = 1200;
            g_windowHeight = 800;
        }

    }
}
