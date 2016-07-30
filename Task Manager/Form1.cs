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
        Config config = new Config();
        Form2 configForm = null;

        public Form1()
        {
            InitializeComponent();

            dataGV.DataSource = table.DTable;
            //dataGridView1.Columns[0].Visible = false;

            var processs = Process.GetProcesses();
            
            foreach(var process in processs)
            {
                table.Add(process);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            table.Update();
            //dataGV.ClearSelection();
            dataGV.DataSource = table.DTable;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            table.Destroy();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Update();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            configForm = new Form2(config, this);
            configForm.Visible = true;
        }

        private void Form1_EnabledChanged(object sender, EventArgs e)
        {
            if (configForm == null) return;
            config = configForm.Config;
            configForm.Visible = false;
            configForm = null;
            config.Save();
        }

        private void killToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selected = dataGV.SelectedRows;

            table.Kill((string)selected[0].Cells[0].Value);
        }

        Process[] processes = null;
        int index = 0;

        private void checkNew_Tick(object sender, EventArgs e)
        {
            if (processes == null)
            {
                processes = Process.GetProcesses();
            }

            if (index < processes.Length && index >= 0)
            {
                table.Add(processes[index]);
                index++;
            }
            else
            {
                processes = null;
                index = 0;
            }
        }
    }
}
