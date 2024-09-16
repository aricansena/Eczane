using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.io;
using System.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;

namespace Proje
{
    public partial class Form12 : Form
    {

        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string kimlik_no;

        public Form12()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                string kayit = "insert into Eczaci(ad,soyad,kimlik_no,telefon_no,parola,yetki_turu) values (@ad,@soyad,@kimlik_no,@telefon_no,@parola,@yetki_turu)"; 
                SqlCommand komut = new SqlCommand(kayit, con);
                komut.Parameters.AddWithValue("@ad", textBox1.Text);
                komut.Parameters.AddWithValue("@soyad", textBox2.Text);
                komut.Parameters.AddWithValue("@kimlik_no", textBox3.Text);
                komut.Parameters.AddWithValue("@parola", textBox5.Text);
                komut.Parameters.AddWithValue("@telefon_no", textBox4.Text);
                komut.Parameters.AddWithValue("@yetki_turu", comboBox1.Text);


                SqlCommand ac = new SqlCommand("select * from Eczaci where kimlik_no='" + textBox3.Text + "'", con);
                SqlDataReader ab = ac.ExecuteReader();
                if (ab.Read())
                {
                    MessageBox.Show("Bu eczacı kayıtlı");
                }
                else
                {
                    if (textBox1.Text == string.Empty || textBox2.Text == string.Empty || textBox3.Text == string.Empty || textBox4.Text == string.Empty || textBox5.Text == string.Empty || comboBox1.Text == string.Empty)
                    {

                        MessageBox.Show("Eksik Bilgi");

                    }
                    else
                    {
                        ab.Close();
                        DialogResult sonuc = MessageBox.Show("Bu eczacıyı kaydetmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (sonuc == DialogResult.Yes)
                        {
                            komut.ExecuteNonQuery();
                            MessageBox.Show("Eczacı başarıyla kaydedildi");
                            textBox1.Clear();
                            textBox2.Clear();
                            textBox3.Clear();
                            textBox4.Clear();
                            textBox5.Clear();
                        }
                    }
                }
                con.Close();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //silmek için
            if (con.State == ConnectionState.Closed)
                con.Open();
            string silmeSorgusu = "DELETE from Eczaci where kimlik_no=@kimlik_no"; 
            SqlCommand silKomutu = new SqlCommand(silmeSorgusu, con);
            silKomutu.Parameters.AddWithValue("@kimlik_no", textBox10.Text);

            string kayit = "insert into silinmis_eczaci_kayitlar(eczaci_id,ad,soyad,kimlik_no,parola,telefon_no,yetki_turu) values (@eczaci_id,@ad,@soyad,@kimlik_no,@parola,@telefon_no,@yetki_turu)"; 
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@eczaci_id", label15.Text);
            komut.Parameters.AddWithValue("@ad", textBox12.Text);
            komut.Parameters.AddWithValue("@soyad", textBox11.Text);
            komut.Parameters.AddWithValue("@kimlik_no", textBox10.Text);
            komut.Parameters.AddWithValue("@parola", textBox8.Text);
            komut.Parameters.AddWithValue("@telefon_no", textBox9.Text);
            komut.Parameters.AddWithValue("@yetki_turu", textBox7.Text);

            SqlCommand ac = new SqlCommand("select * from Eczaci where kimlik_no='" + textBox10.Text + "'", con);
            SqlDataReader ab = ac.ExecuteReader();
            if (ab.Read())
            {
                if (textBox10.Text == label19.Text)
                {
                    MessageBox.Show("Kullanıcı kendi kaydını silemez");
                }
                else
                {
                    DialogResult sonuc = MessageBox.Show("Bu eczacıyı silmek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (sonuc == DialogResult.Yes)
                    {
                        ab.Close();
                        komut.ExecuteNonQuery();
                        silKomutu.ExecuteNonQuery();
                        MessageBox.Show("Eczacı kaydı silindi");
                        textBox12.Clear();
                        textBox11.Clear();
                        textBox10.Clear();
                        textBox9.Clear();
                        textBox8.Clear();
                        textBox7.Clear();
                    }
                }
                con.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //güncelleme
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand guncellemeeczaci = new SqlCommand("update Eczaci set ad='" + textBox12.Text + "' ,soyad= '" + textBox11.Text + "', kimlik_no= '" + textBox10.Text + "', telefon_no= '" + textBox9.Text + "', parola= '" + textBox8.Text + "', yetki_turu= '" + textBox7.Text + "'  where eczaci_id= '" + label15.Text + "'", con); //

            DialogResult sonuc = MessageBox.Show("Bilgileri güncellemek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                guncellemeeczaci.ExecuteNonQuery();
                MessageBox.Show("Eczacı bilgileri güncellendi");
            }
            con.Close();

        }
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            label15.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox12.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox11.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox10.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox7.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlDataAdapter veri;
            veri = new SqlDataAdapter("select * from Eczaci", con);
            DataTable tablo = new DataTable();
            veri.Fill(tablo);
            dataGridView1.DataSource = tablo;

            SqlDataAdapter veri2;
            veri2 = new SqlDataAdapter("select * from silinmis_eczaci_kayitlar", con);
            DataTable tablo2 = new DataTable();
            veri2.Fill(tablo2);
            dataGridView2.DataSource = tablo2;

            SqlDataAdapter veri3;
            veri3 = new SqlDataAdapter("Select ad,miligram,adet,kapsul_tablet_adedi,ucret,eczaci_id from satilanilac", con);
            DataTable tablo3 = new DataTable(); 
            veri3.Fill(tablo3);
            dataGridView12.DataSource= tablo3;

            SqlDataAdapter veri4;
            veri4 = new SqlDataAdapter("Select ad,miligram,stok_adedi,barkod,recete_turu,kapsul_tablet_adedi,ucret,ureten_firma,etkinMadde from Ilac", con);
            DataTable tablo4 = new DataTable();
            veri4.Fill(tablo4);
            dataGridView3.DataSource = tablo4;


            label19.Text = Form4.kimlik_no.ToString();
            label19.Hide();
            comboBox1.Items.Add("Okuma");
            comboBox1.Items.Add("Yazma");
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Form4 adminarayüz = new Form4();
            {
                adminarayüz.StartPosition = FormStartPosition.CenterScreen;
                adminarayüz.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            this.Hide();
            Form12 eczacisil = new Form12();
            {
                eczacisil.StartPosition = FormStartPosition.CenterScreen;
                eczacisil.Show();
            }
        }

        private void button5_Click(object sender, EventArgs e)//pdf butonu
        {
            if (dataGridView2.Rows.Count > 0)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "PDF (*.pdf)|*.pdf";
                save.FileName = "Result.pdf";
                bool ErrorMessage = false;
                if (save.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(save.FileName))
                    {
                        try
                        {
                            File.Delete(save.FileName);
                        }
                        catch (Exception ex)
                        {
                            ErrorMessage = true;
                            MessageBox.Show("Diskte Yeterli Alan Bulunamadı." + ex.Message);
                        }
                    }
                    if (!ErrorMessage)
                    {
                        try
                        {
                            PdfPTable pTable = new PdfPTable(dataGridView2.Columns.Count);
                            pTable.DefaultCell.Padding = 2;
                            pTable.WidthPercentage = 100;
                            pTable.HorizontalAlignment = Element.ALIGN_LEFT;
                            foreach (DataGridViewColumn col in dataGridView2.Columns)
                            {
                                PdfPCell pCell = new PdfPCell(new Phrase(col.HeaderText));
                                pTable.AddCell(pCell);
                            }
                            foreach (DataGridViewRow viewRow in dataGridView2.Rows)
                            {
                                foreach (DataGridViewCell dcell in viewRow.Cells)
                                {
                                    pTable.AddCell(dcell.Value.ToString());
                                }
                            }
                            using (FileStream fileStream = new FileStream(save.FileName, FileMode.Create))
                            {
                                Document document = new Document(PageSize.A4, 8f, 16f, 16f, 8f);
                                PdfWriter.GetInstance(document, fileStream);
                                document.Open();
                                document.Add(pTable);
                                document.Close();
                                fileStream.Close();
                            }
                            MessageBox.Show("PDF Başarıyla Kaydedildi.");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata Oluştu." + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Veri Bulunamadı.");
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {
            //şifre değiştirme kısmı
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand sifre = new SqlCommand("select * from Admin where parola='" + textBox14.Text + "'", con);
            string guncellemesorgusu = "Update Admin set parola=@parola where kimlik_no=98765432110";
            SqlCommand guncellemekomutu = new SqlCommand(guncellemesorgusu, con);
            guncellemekomutu.Parameters.AddWithValue("@parola", textBox13.Text);

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
                    textBox13.Clear();
                    textBox6.Clear();
                }
            }
            else
            {
                MessageBox.Show("mevcut şifre yanlış");
                textBox14.Clear();
            }
            con.Close();
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            con.Open();
            DataTable tbl = new DataTable();
            string vara, cumle;
            vara = textBox15.Text;
            cumle = "Select ad,miligram,stok_adedi,barkod,recete_turu,kapsul_tablet_adedi,ucret,ureten_firma,etkinMadde from Ilac where ad like '" + textBox15.Text + "%'";
            SqlDataAdapter adptr = new SqlDataAdapter(cumle, baglanti);
            adptr.Fill(tbl);
            con.Close();
            dataGridView3.DataSource = tbl;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
            con.Open();
            DataTable tbl = new DataTable();
            string vara, cumle;
            vara = textBox16.Text;
            cumle = "Select ad,miligram,adet,kapsul_tablet_adedi,ucret,eczaci_id from satilanilac where ad like '" + textBox16.Text + "%'";
            SqlDataAdapter adptr = new SqlDataAdapter(cumle, baglanti);
            adptr.Fill(tbl);
            con.Close();
            dataGridView12.DataSource = tbl;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //SqlConnection con = new SqlConnection();
            //con.ConnectionString = "DATA SOURCE = BEYZAKESKIN\\SQLEXPRESS;Initial Catalog=eczane;Integrated Security=True";
            //string komutcumle = "backup database Eczane to disk ='yedekEczane'";
            //SqlCommand komut = new SqlCommand(komutcumle, con);
            //con.Open();
            //komut.ExecuteNonQuery();
            //con.Close();

           
            Server dbserver = new Server(new ServerConnection(textBox17.Text));
            Backup dbbackup = new Backup();
            dbbackup.Action = BackupActionType.Database;
            dbbackup.Database=txtLog.Text;
            dbbackup.Devices.AddDevice(txtPath.Text, DeviceType.File);
            dbbackup.SqlBackup(dbserver);
            //dbbackup.Complete += Dbbackup_Complete;
            
            try
            {
                MessageBox.Show("Yedekleme işlemi başarılı bir şekilde gerçekleşti","Bilgi",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message,"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
           
        }

        //private void Dbbackup_Complete(object sender, ServerMessageEventArgs e)
        //{
            
        //}

        private void button9_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Yedeklenecek yolu belirtiniz.";
            saveFileDialog1.Filter = "Yedekleme dosyaları(*.bak) | *.bak | Tüm dosyalar(*.*) | *.*";
            if (saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                txtPath.Text = saveFileDialog1.FileName;

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}
