using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTextSharp.awt.geom.Point2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace Proje
{
    public partial class Form9 : Form
    {
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad, kimlik_no, hasta_id, dogum_tarihi, cinsiyet, doktor_id, kod,sigorta_bilgisi,telefon_no;

        DataTable tablo = new DataTable();
        DataSet daset = new DataSet();



        private void button2_Click(object sender, EventArgs e)
        {


            DataGridViewRow selectedRow = dataGridView1.CurrentRow;

            ilac ilac1 = new ilac();

            if (textBox7.Text == string.Empty)
            {
                MessageBox.Show("Adet giriniz");
            }
            else
            {

                ilac1.ilac_id = (int)dataGridView1.CurrentRow.Cells[0].Value;
                ilac1.ad = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                ilac1.miligram = (int)dataGridView1.CurrentRow.Cells[2].Value;
                ilac1.adet = Convert.ToInt32(textBox7.Text);
                ilac1.doz = comboBox1.Text;
                ilac1.kapsul_tablet_adedi= (int)dataGridView1.CurrentRow.Cells[3].Value;
                //ilac1.ucret = (decimal)dataGridView1.CurrentRow.Cells[4].Value;//hata veriyor !!


                dataGridView2.Rows.Add(ilac1.ilac_id, ilac1.ad, ilac1.miligram, ilac1.doz, ilac1.adet,ilac1.kapsul_tablet_adedi,ilac1.ucret);
                textBox7.Clear();
            }

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView2.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView2.Rows.RemoveAt(selectedIndex);
                dataGridView2.Refresh();
            }
            else
            {
                MessageBox.Show("Seçim Yapınız");
            }

            ////ilaç sil butonu


        }

       

        private void button9_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand sifre = new SqlCommand("select * from Doktor where parola='" + textBox3.Text + "'", con);
            string guncellemesorgusu = "Update Doktor set parola=@parola where kimlik_no=12345678990";
            SqlCommand guncellemekomutu = new SqlCommand(guncellemesorgusu, con);
            guncellemekomutu.Parameters.AddWithValue("@parola", textBox4.Text);

            SqlDataReader ab = sifre.ExecuteReader();
            if (ab.Read())
            {
                if (textBox4.Text == textBox5.Text)
                {
                    DialogResult sonuc = MessageBox.Show("Şifreyi değiştirmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sonuc == DialogResult.Yes)
                    {
                        ab.Close();
                        guncellemekomutu.ExecuteNonQuery();
                        MessageBox.Show("Şifre başarıyla değiştirildi");
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("yeni şifreler uyuşmuyor");
                    textBox4.Clear();
                    textBox5.Clear();
                }
            }
            else
            {
                MessageBox.Show("mevcut şifre yanlış");
                textBox3.Clear();
            }
            con.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)//arama butonu
        {
            con.Open();
            DataTable tbl = new DataTable();
            string vara, cumle;
            vara = textBox2.Text;
            cumle = "Select ilac_id,ad,miligram,kapsul_tablet_adedi from Ilac where ad like '%" + textBox2.Text + "%'";
            SqlDataAdapter adptr = new SqlDataAdapter(cumle, baglanti);
            adptr.Fill(tbl);
            con.Close();
            dataGridView1.DataSource = tbl;

        }



        private void button6_Click(object sender, EventArgs e)
        {
            //reçeteyi kaydet butonu

            if (con.State == ConnectionState.Closed)
                con.Open();


            //reçete tablosundaki sütunlara textbox ve label içindekileri yazdırma fonksiyonu

            string recete = "insert into Recete(hasta_kimlik_no,doktor_kimlik_no,kod,tarih) values (@hasta_kimlik_no,@doktor_kimlik_no,@kod,@tarih)"; //
            SqlCommand recetekaydet = new SqlCommand(recete, con);
            recetekaydet.Parameters.AddWithValue("@hasta_kimlik_no", textBox1.Text);
            recetekaydet.Parameters.AddWithValue("@doktor_kimlik_no", label9.Text);
            recetekaydet.Parameters.AddWithValue("@kod", label7.Text);
            DateTime tarih = DateTime.Parse(label10.Text);
            recetekaydet.Parameters.AddWithValue("@tarih", tarih);


            //alınan kodu hasta tablosundaki kod sütununa yazdırma fonksiyonu

            SqlCommand hastakodekle = new SqlCommand("update Hasta set kod='" + label7.Text + "' where kimlik_no= '" + textBox1.Text + "'", con);
            if (label7.Text == "")
            {
                //kodu görüntüle butonuna basılmamışsa çıkan hata
                MessageBox.Show("Önce kodu görüntüleyin");
            }
            else if (textBox1.Text == string.Empty)
            {
                //kimlik numarası girilmezse çıkan hata
                MessageBox.Show("Hasta kimlik numarası giriniz");
            }
            else if (label5.Text == "label5")
            {   //hastayı görüntüle butonuna basılmamışsa
                MessageBox.Show("önce hastayı görüntüleyiniz");
            }
            else if (dataGridView2.RowCount == 0)
            {   //hiç ilaç kaydedilmemişse
                MessageBox.Show("ilaç ekleyin");
            }

            else
            {
                DialogResult sonuc = MessageBox.Show("Reçeteyi kaydetmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (sonuc == DialogResult.Yes)
                {
                    //kimlik numarası girildiyse ve kod alındıysa reçetekaydet ve hastakodekle fonksiyonlarını çağırarak sütun içine verileri ekler
                    recetekaydet.ExecuteNonQuery();
                    hastakodekle.ExecuteNonQuery();

                    //string[] sayi = new string[dataGridView2.RowCount];
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        //bir hastaya birden fazla ilaç yazıldığında ilaçların aynı koda ve hasta idsine sahip olması için listboxın içindeki ilaçlara kurduğum döngü
                        string ilac = "insert into Verilenilac(hasta_kimlik_no,ilac_id,kod,ad,miligram,doz,adet,kapsul_tablet_adedi,ucret) values (@hasta_kimlik_no,@ilac_id,@kod,@ad,@miligram,@doz,@adet,@kapsul_tablet_adedi,@ucret)";
                        SqlCommand ilacekle = new SqlCommand(ilac, con);
                        ilacekle.Parameters.AddWithValue("@hasta_kimlik_no", textBox1.Text);
                        ilacekle.Parameters.AddWithValue("@ilac_id", (int)dataGridView2.Rows[i].Cells[0].Value);
                        ilacekle.Parameters.AddWithValue("@kod", label7.Text);
                        ilacekle.Parameters.AddWithValue("@ad", dataGridView2.Rows[i].Cells[1].Value.ToString());
                        ilacekle.Parameters.AddWithValue("@miligram", (int)dataGridView2.Rows[i].Cells[2].Value);
                        ilacekle.Parameters.AddWithValue("@doz", dataGridView2.Rows[i].Cells[3].Value.ToString());
                        ilacekle.Parameters.AddWithValue("@adet",(int)dataGridView2.Rows[i].Cells[4].Value);
                        ilacekle.Parameters.AddWithValue("@kapsul_tablet_adedi", (int)dataGridView2.Rows[i].Cells[5].Value);
                        ilacekle.Parameters.AddWithValue("@ucret", (decimal)dataGridView2.Rows[i].Cells[6].Value);

                        kod = label7.Text;
                        ilacekle.ExecuteNonQuery();

                    }
                    MessageBox.Show("Reçete kaydedildi");
                    label7.Text = "";

                    Form13 receteolustur = new Form13();
                    {
                        receteolustur.StartPosition = FormStartPosition.CenterScreen;
                        receteolustur.Show();
                    }


                }
            }
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();

            //hastayı görüntüle butonu
            //textboxın içine girilen kimlik numarası eğer hasta tablosunu içerisinde yer alıyorsa hastanın adını soyadını ve idsinin görüntülenmesini sağlar

            SqlCommand ac = new SqlCommand("select * from Hasta where kimlik_no='" + textBox1.Text + "'", con);
            SqlDataReader ab = ac.ExecuteReader();
            if (ab.Read())
            {
                //listBox1.Items.Clear();              veritabanına bağlı olduğu için çalışmıyor               //listboxı temizler              
                label7.Text = "";                                   //label2deki yazıyı temizler
                //hasta_id=ab.GetValue(0).ToString();
                ad = ab.GetValue(3).ToString();
                soyad = ab.GetValue(4).ToString();
                dogum_tarihi = ab.GetValue(6).ToString();               //hastanın bilgilerini veritabanından çekmek için kullanılır
                cinsiyet = ab.GetValue(10).ToString();
                kimlik_no = ab.GetValue(8).ToString();
                telefon_no = ab.GetValue(5).ToString();
                sigorta_bilgisi = ab.GetValue(1).ToString();
                label5.Text = Form9.dogum_tarihi; 
                label12.Text = Form9.cinsiyet;
                label4.Text = Form9.ad + " " + Form9.soyad;
                // label13.Text = Form9.hasta_id;
                label5.Show();
                label4.Show();                                      //en alttaki kodda gizlenen labelları görünür kılar
                label12.Show();
                ab.Close();
            }
            else
            {
                //eğer kimlik numarası bulunamamışsa 

                MessageBox.Show("Hasta bulunamadı");
            }
            con.Close();
        }

        private void Form9_Load(object sender, EventArgs e)
        {

            // TODO: Bu kod satırı 'eczaneDataSet.Ilac' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            label9.Text = Form2.kimlik_no;             //ekranda gözükmez doktorun kimlik numarasını çekerek label3te ad soyad yazdırmayı sağlar
            label8.Text = Form2.ad + " " + Form2.soyad; //giriş yapan doktoru kimlik numarasından tanıyarak ad soyad olarak ekran başına yazdırır
            label5.Text = Form9.ad + " " + Form9.soyad; //giriş yapan doktoru kimlik numarasından tanıyarak ad soyad olarak ekran başına yazdırır
            label10.Text = DateTime.Now.ToShortDateString();      //zamanı gösterir reçete yazılma zamanını kaydetmek için kullanılır
            label7.Text = Form9.kod;
            label5.Hide();
            label4.Hide();                              //butona basılıp herhangi bir işlem yaptırana kadar görünmez olarak dururlar, butona basıldığında otomatik görünür hale gelirler
            label12.Hide();
            label7.Hide();
            label9.Hide();

            con.Open();
            SqlDataAdapter veri;           
            veri = new SqlDataAdapter("select ilac_id,ad,miligram,kapsul_tablet_adedi,ucret from Ilac", con);
            DataTable tablo = new DataTable();
            veri.Fill(tablo);
            dataGridView1.DataSource = tablo;

            
            con.Close();
            comboBox1.Items.Add("Günde 1");
            comboBox1.Items.Add("Günde 2");
            comboBox1.Items.Add("Günde 3");

            this.dataGridView1.Columns["ucret"].Visible = false;

            //ilaclistele();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //kodu görüntüle butonu
            //reçete kodunu belirlediğim karakterlerle random olarak atar

            String karakterler = "0123456789ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
            Random rnd = new Random();
            String pano = "";
            for (int i = 0; i < 7; i++)
            {
                pano += karakterler[rnd.Next(karakterler.Length)];
            }
            label7.Text = pano;
            label7.Show(); //en altta gizlediğim label2'yi butona basıldığı
        }

        public Form9()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Form2 doktorgiris = new Form2();
            {
                doktorgiris.StartPosition = FormStartPosition.CenterScreen;
                doktorgiris.Show();
            }

        }
    }
}
