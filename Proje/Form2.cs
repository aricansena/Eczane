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
    public partial class Form2 : Form
    {
        //bağlantı kurmak için
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad, kimlik_no , doktor_id;

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        public Form2()
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
            //bağlantıyı açmak için 
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand komut = new SqlCommand("select * from Doktor where kimlik_no='" + textBox1.Text + "'and parola= '" + textBox2.Text + "'", con);
            SqlDataReader ab = komut.ExecuteReader();
            if (ab.Read())
            {
                //doktor_id = ab.GetValues(0).ToString();
                ad = ab.GetValue(1).ToString();
                soyad = ab.GetValue(2).ToString();
                doktor_id = ab.GetValue(0).ToString();
                kimlik_no= ab.GetValue(3).ToString();
                
                this.Hide();
                Form9 doktorarayuz = new Form9();
                {
                    doktorarayuz.StartPosition = FormStartPosition.CenterScreen;
                    doktorarayuz.Show();
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
    }
}
