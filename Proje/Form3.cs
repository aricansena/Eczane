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
    public partial class Form3 : Form
    {

        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog = Eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad,yetki_turu,kimlik_no,eczaci_id;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            
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
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand komut = new SqlCommand("select * from Eczaci where kimlik_no='" + textBox1.Text + "'and parola= '" + textBox2.Text + "'", con);
            SqlDataReader ab = komut.ExecuteReader();
            if (ab.Read())
            {
                eczaci_id=ab.GetValue(0).ToString();
                yetki_turu=ab.GetValue(6).ToString();
                ad = ab.GetValue(1).ToString();
                soyad = ab.GetValue(2).ToString();
                this.Hide();
                Form5 eczaciarayuz = new Form5();
                eczaciarayuz.Show(this);
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
