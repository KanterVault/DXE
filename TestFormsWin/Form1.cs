using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestFormsWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Controls.Add(label1);
            Label label = new Label();
            label.Text = "dsgfsdgdsfgsd";
            label.Visible = true;
            label.Refresh();
            label.Dock = DockStyle.Fill;
            this.Controls.Add(label);
        }
    }
}
