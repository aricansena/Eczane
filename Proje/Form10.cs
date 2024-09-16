using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje
{
    public partial class Form10 : Form
    {
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad, hasta_kimlik_no, sigorta_bilgisi, ucret, telefon_no, eczaci_id, recete_durumu;
        public static string gonderilecekveri;


        public Form10()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form11 sifredegistirme = new Form11();
            {
                sifredegistirme.StartPosition = FormStartPosition.CenterScreen;
                sifredegistirme.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form4 admingiris = new Form4();
            {
                admingiris.StartPosition = FormStartPosition.CenterScreen;
                admingiris.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form5 eczacisayfa = new Form5();
            {
                eczacisayfa.StartPosition = FormStartPosition.CenterScreen;
                eczacisayfa.Show();
            }
        }

        private void Form10_Load(object sender, EventArgs e)
        {
            label13.Text = Form3.eczaci_id.ToString();
            label14.Text = DateTime.Now.ToShortDateString();
            //label11.Text = gonderilecekveri.ToString();  
            label13.Hide();


        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            //reçeteyi kaydet butonu

            //if (con.State == ConnectionState.Closed)
            con.Open();


            //reçete tablosundaki sütunlara textbox ve label içindekileri yazdırma fonksiyonu

            string fatura = "insert into Fatura(ucret,hasta_kimlik_no,recete_kod,tarih,eczaci_id) values (@ucret,@hasta_kimlik_no,@recete_kod,@tarih,@eczaci_id)"; //
            SqlCommand faturakaydet = new SqlCommand(fatura, con);
            faturakaydet.Parameters.AddWithValue("@ucret", label11.Text);
            faturakaydet.Parameters.AddWithValue("@hasta_kimlik_no", textBox1.Text);
            faturakaydet.Parameters.AddWithValue("@recete_kod", textBox2.Text);
            DateTime tarih = DateTime.Parse(label14.Text);
            faturakaydet.Parameters.AddWithValue("@tarih", tarih);
            faturakaydet.Parameters.AddWithValue("@eczaci_id", label13.Text);


            DialogResult sonuc = MessageBox.Show("Faturayı kaydetmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                //kimlik numarası girildiyse ve kod alındıysa reçetekaydet ve hastakodekle fonksiyonlarını çağırarak sütun içine verileri ekler

                faturakaydet.ExecuteNonQuery();

                //string[] sayi = new string[dataGridView2.RowCount];
                for (int i = 0; i < dataGridView1.Rows.Count; i++)

                {

                    //bir hastaya birden fazla ilaç yazıldığında ilaçların aynı koda ve hasta idsine sahip olması için listboxın içindeki ilaçlara kurduğum döngü
                    string ilac = "insert into satilanilac(hasta_kimlik_no,kod,ilac_id,ad,miligram,doz,adet,kapsul_tablet_adedi,ucret,eczaci_id) values (@hasta_kimlik_no,@kod,@ilac_id,@ad,@miligram,@doz,@adet,@kapsul_tablet_adedi,@ucret,@eczaci_id)";
                    SqlCommand satilanilacekle = new SqlCommand(ilac, con);
                    satilanilacekle.Parameters.AddWithValue("@hasta_kimlik_no", textBox1.Text);
                    satilanilacekle.Parameters.AddWithValue("@kod", textBox2.Text);
                    satilanilacekle.Parameters.AddWithValue("@ilac_id", dataGridView1.Rows[i].Cells[0].Value.ToString());
                    satilanilacekle.Parameters.AddWithValue("@ad", dataGridView1.Rows[i].Cells[1].Value.ToString());
                    satilanilacekle.Parameters.AddWithValue("@miligram", (int)dataGridView1.Rows[i].Cells[2].Value);
                    satilanilacekle.Parameters.AddWithValue("@doz", dataGridView1.Rows[i].Cells[3].Value.ToString());
                    satilanilacekle.Parameters.AddWithValue("@adet", (int)dataGridView1.Rows[i].Cells[4].Value);
                    satilanilacekle.Parameters.AddWithValue("@kapsul_tablet_adedi", (int)dataGridView1.Rows[i].Cells[5].Value);
                    satilanilacekle.Parameters.AddWithValue("@ucret", (decimal)dataGridView1.Rows[i].Cells[6].Value);
                    satilanilacekle.Parameters.AddWithValue("@eczaci_id", label13.Text);
                    satilanilacekle.ExecuteNonQuery();



                }
                MessageBox.Show("Fatura kaydedildi");
                label7.Text = "";

                Form8 faturasayfasi = new Form8();
                {
                    faturasayfasi.StartPosition = FormStartPosition.CenterScreen;
                    faturasayfasi.Show();
                }
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    faturasayfasi.dataGridView3.Rows.Add(dataGridView1.Rows[i].Cells[1].Value, dataGridView1.Rows[i].Cells[2].Value, dataGridView1.Rows[i].Cells[3].Value, dataGridView1.Rows[i].Cells[4].Value, dataGridView1.Rows[i].Cells[5].Value, dataGridView1.Rows[i].Cells[6].Value);
                }



            }

            con.Close();

        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand ac = new SqlCommand("select * from Recete where kod='" + textBox2.Text + "' and recete_durumu= 'Aktif'", con);
            SqlDataReader ab = ac.ExecuteReader();
            if (ab.Read())
            {

                //SqlCommand anc = new SqlCommand("select * from Recete where recete_durumu= 'Aktif'", con);
                //SqlDataReader anb = anc.ExecuteReader();
                //if (anb.Read())
                //{
                hasta_kimlik_no = ab.GetValue(1).ToString();
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("Hastanın kimlik numarasını giriniz.");
                }
                else
                {
                    if (textBox1.Text != hasta_kimlik_no)
                    {
                        MessageBox.Show("Reçete bu hastaya ait değil");
                    }
                    else
                    {
                        ab.Close();
                        SqlDataAdapter veri;
                        veri = new SqlDataAdapter("select ilac_id, ad,miligram,doz,adet,kapsul_tablet_adedi,ucret from Verilenilac where kod='" + textBox2.Text + "'", con);
                        DataTable tablo = new DataTable();
                        veri.Fill(tablo);
                        dataGridView1.DataSource = tablo;

                    }

                }
                int toplam = 0;
                if (label7.Text == string.Empty)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                    }
                    label11.Text = toplam.ToString();
                }
                else
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                    }
                    toplam = toplam * 4 / 10;
                    label11.Text = toplam.ToString();
                }

                gonderilecekveri = label11.Text.ToString() + " TL";

            }
            else
            {
                MessageBox.Show("Verilen kod aktif değil");
            }
        
            
 
            con.Close();


        }


        private void button5_Click(object sender, EventArgs e)
        {
            int selectedIndex = dataGridView1.CurrentCell.RowIndex;
            if (selectedIndex > -1)
            {
                dataGridView1.Rows.RemoveAt(selectedIndex);
                dataGridView1.Refresh();
                int toplam = 0;
                if (label7.Text == string.Empty)
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                    }
                    label11.Text = toplam.ToString();
                }
                else
                {
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        toplam += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value) * Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);
                    }
                    toplam = toplam * 4 / 10;
                    label11.Text = toplam.ToString();
                }
            }
            else
            {
                MessageBox.Show("Seçim Yapınız");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            Form12 eczacilar = new Form12();
            {
                eczacilar.StartPosition = FormStartPosition.CenterScreen;
                eczacilar.Show();
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
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
                //label7.Text = "";                                   //label2deki yazıyı temizler
                //hasta_id=ab.GetValue(0).ToString();
                ad = ab.GetValue(3).ToString();
                soyad = ab.GetValue(4).ToString();
                telefon_no = ab.GetValue(5).ToString();               //hastanın bilgilerini veritabanından çekmek için kullanılır
                sigorta_bilgisi = ab.GetValue(1).ToString();
                label6.Text = Form10.ad + " " + Form10.soyad;
                label7.Text = Form10.sigorta_bilgisi;
                label8.Text = Form10.telefon_no;
                label9.Text = DateTime.Now.ToString();
                label6.Show();
                label7.Show();                                      //en alttaki kodda gizlenen labelları görünür kılar
                label8.Show();
                label9.Show();
                ab.Close();
            }
            else
            {
                //eğer kimlik numarası bulunamamışsa 

                MessageBox.Show("Hasta bulunamadı");
            }
            con.Close();
        }
    }
}
