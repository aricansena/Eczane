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
    public partial class Form5 : Form
    {
  
        public Form5()
        {
            InitializeComponent();
        }
        public static string yetki_turu;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3();
            {
                form3.StartPosition = FormStartPosition.CenterScreen;
                form3.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "Okuma")
            {
                MessageBox.Show("Eczacı sadece okuma yetkisine sahip olduğu için ekleme yapamaz");
            }
            else
            {
            this.Hide();
            Form6 form6 = new Form6();
            {
                form6.StartPosition = FormStartPosition.CenterScreen;
                form6.Show();
            }
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form7 form7 = new Form7();
            {
                form7.StartPosition = FormStartPosition.CenterScreen;
                form7.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form10 recetegoruntuleme = new Form10();
            {
                recetegoruntuleme.StartPosition = FormStartPosition.CenterScreen;
                recetegoruntuleme.Show();
            }
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            label1.Text=Form3.yetki_turu.ToString();
            label1.Hide();
        }
    }
}
