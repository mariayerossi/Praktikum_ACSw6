using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace M5_220180519
{
    public partial class Form1 : Form
    {
        public String data;
        public String username;
        public String pass;
        public Form1()
        {
            InitializeComponent();
            this.Text = "MainWindow";
        }

        public static OracleConnection conn;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            
        }
    }
}
