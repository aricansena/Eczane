using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Proje
{
    public partial class Form13 : Form
    {
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad, kimlik_no, dogum_tarihi, cinsiyet, sigorta_bilgisi, telefon_no, kod;

      

        public Form13()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //YAZDIR BUTONU
            PrintDocument doc = new PrintDocument();
            doc.PrintPage += this.Doc_PrintPage;
            PrintDialog dlgSettings = new PrintDialog();
            dlgSettings.Document = doc;
            if (dlgSettings.ShowDialog() == DialogResult.OK)
            {
                doc.Print();
            }
           

        } 
        private void Doc_PrintPage(object sender, PrintPageEventArgs e)
            {
                float x = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;
                //Bitmap bmp = new Bitmap(this.groupBox1.Width, this.groupBox1.Height);
                //this.groupBox1.DrawToBitmap(bmp, new Rectangle(0, 0, this.groupBox1.Width, this.groupBox1.Height));
                //e.Graphics.DrawImage((Image)bmp, x, y);
            }

        private void Form13_Load(object sender, EventArgs e)
        {
            label13.Text = Form9.ad + " " + Form9.soyad;
            label14.Text = Form9.kimlik_no; //giriş yapan doktoru kimlik numarasından tanıyarak ad soyad olarak ekran başına yazdırır
            label15.Text = Form9.sigorta_bilgisi; //giriş yapan doktoru kimlik numarasından tanıyarak ad soyad olarak ekran başına yazdırır
            label16.Text = Form9.telefon_no;
            label17.Text = Form9.kod;
            label12.Text = DateTime.Now.ToShortDateString();
            label17.Hide();

            con.Open();
            SqlDataAdapter veri;
            veri = new SqlDataAdapter("select ad,miligram,doz,adet,kapsul_tablet_adedi from Verilenilac where kod='" + label17.Text + "'", con);
            DataTable tablo = new DataTable();
            veri.Fill(tablo);
            dataGridView3.DataSource = tablo;
            con.Close();

        }
    }
}
