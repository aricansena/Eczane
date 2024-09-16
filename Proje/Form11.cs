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
    public partial class Form11 : Form
    {
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=Eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public Form11()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Form10 adminarayuz = new Form10();
            {
                adminarayuz.StartPosition = FormStartPosition.CenterScreen;
                adminarayuz.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //şifre değiştirme kısmı
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand sifre = new SqlCommand("select * from Admin where parola='" + textBox1.Text + "'", con);
            string guncellemesorgusu = "Update Admin set parola=@parola where kimlik_no=98765432110";
            SqlCommand guncellemekomutu = new SqlCommand(guncellemesorgusu, con);
            guncellemekomutu.Parameters.AddWithValue("@parola", textBox2.Text);

            SqlDataReader ab = sifre.ExecuteReader();
            if (ab.Read())
            {
                if (textBox2.Text == textBox3.Text)
                {
                    DialogResult sonuc = MessageBox.Show("Şifreyi değiştirmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sonuc == DialogResult.Yes)
                    {
                        ab.Close();
                        guncellemekomutu.ExecuteNonQuery();
                        MessageBox.Show("Şifre başarıyla değiştirildi");
                        textBox1.Clear();
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("yeni şifreler uyuşmuyor");
                    textBox2.Clear();
                    textBox3.Clear();
                }
            }
            else
            {
                MessageBox.Show("mevcut şifre yanlış");
                textBox1.Clear();
            }
            con.Close();
        }

        private void Form11_Load(object sender, EventArgs e)
        {

        }
    }
}

