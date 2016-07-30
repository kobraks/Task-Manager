using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Task_Manager
{
    public partial class Form2 : Form
    {
        public Config Config { get; set; }
        Form parent;
        public Form2(Config config, Form parent)
        {
            InitializeComponent();

            this.Config = config;
            this.parent = parent;

            password.Text = config.Password;
            port.Text = config.Port.ToString();
        }

        private void accept_Click(object sender, EventArgs e)
        {
            try
            {
                Config.Password = password.Text;
                Config.Port = ushort.Parse(port.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, "Conversion Error", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            close();
        }

        void close()
        {
            parent.Enabled = true;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
