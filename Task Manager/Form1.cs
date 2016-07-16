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

            dataGridView1.DataSource = table.DTable;
            dataGridView1.Columns[0].Visible = false;

            var processs = Process.GetProcesses();
            
            foreach(var process in processs)
            {
                table.Add(process);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            table.Update();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            table.Destroy();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            table.Update();
        }
    }
}
