using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form5 form5 = new Form5();
            {
                form5.StartPosition = FormStartPosition.CenterScreen;
                form5.Show();
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
