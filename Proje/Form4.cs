using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Proje
{
    public partial class Form4 : Form
    {
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog = Eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string kimlik_no;
        public Form4()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form1 = new Form1();
            {
                form1.StartPosition = FormStartPosition.CenterScreen;
                form1.Show();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //bağlantı gelecek
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand komut = new SqlCommand("select * from Admin where kimlik_no='" + textBox1.Text + "'and parola= '" + textBox2.Text + "'", con);
            SqlDataReader ab = komut.ExecuteReader();
            if (ab.Read())
            {
                kimlik_no = ab.GetValue(3).ToString();
                this.Hide();
                Form12 adminarayuz = new Form12();
                {
                    adminarayuz.StartPosition = FormStartPosition.CenterScreen;
                    adminarayuz.Show();
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Parola Yanlış");
                textBox1.Clear();
                textBox2.Clear();
            }
            con.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}
