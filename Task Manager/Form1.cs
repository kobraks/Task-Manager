using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Task_Manager
{
    public partial class Form1 : Form
    {
        Table table = new Table();

        public Form1()
        {
            InitializeComponent();

            DataGV.DataSource = table.DTable;
            DataGV.Columns[0].Visible = false;

            var processs = Process.GetProcesses();
            
            foreach(var process in processs)
            {
                table.Add(process);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            table.Destroy();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Update();
        }

        Process[] processes = null;
        int index = 0;

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            table.Update();

            if (processes == null)
            {
                processes = Process.GetProcesses();
            }

            try
            {
                if (index >= 0 && index < processes.Length)
                {
                    table.Add(processes[index]);
                    index++;
                }
                else
                {
                    index = 0;
                    processes = null;
                }
            }
            catch (Exception)
            { }
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = DataGV.SelectedRows;
            
            for(int i = 0; i < selected.Count; i++)
            {
                table.Kill((string)selected[i].Cells[0].Value);
            }
        }
    }
}
