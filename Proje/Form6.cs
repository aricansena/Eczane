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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.OleDb;
using excel = Microsoft.Office.Interop.Excel;



namespace Proje
{
    public partial class Form6 : Form
    {
        //BAĞLANTI İÇİN
        static string baglanti = "DATA SOURCE = SENA\\SQLEXPRESS;Initial Catalog = Eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        public static string ad, soyad;
        public Form6()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

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
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //VERİLERİN GELMESİ İÇİN
            label12.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            textBox6.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            //textBox7.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            comboBox1.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
            textBox8.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            textBox9.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();

        }

        private void Form6_Load_1(object sender, EventArgs e)
        {
            //DATAGRID İÇİN BİLGİLERİ ÇEKMEK
            con.Open();
            SqlDataAdapter veri;
            veri = new SqlDataAdapter("select * from Ilac", con);
            DataTable tablo = new DataTable();
            veri.Fill(tablo);
            dataGridView1.DataSource = tablo;
            con.Close();
            //reçete türü için combobox
            comboBox1.Items.Add("Beyaz");
            comboBox1.Items.Add("Yeşil");
            comboBox1.Items.Add("Kırmızı");
            label14.Text = Form3.ad + " " + Form3.soyad;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //GÜNCELLEMEK İÇİN
            if (con.State == ConnectionState.Closed)
                con.Open();

            SqlCommand guncellemeilac = new SqlCommand("update Ilac set ad='" + textBox1.Text + "' ,stok_adedi= '" + textBox2.Text + "', miligram= '" + textBox3.Text + "', ucret= '" + textBox4.Text + "', kapsul_tablet_adedi= '" + textBox5.Text + "' ,barkod= '" + textBox6.Text + "' ,recete_turu= '" + comboBox1.Text + "' ,ureten_firma= '" + textBox8.Text + "' ,etkinMadde= '" + textBox9.Text + "'  where ilac_id= '" + label12.Text + "'", con);

            DialogResult sonuc = MessageBox.Show("Bilgileri güncellemek istediğinize emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                guncellemeilac.ExecuteNonQuery();
                MessageBox.Show("İlaç bilgileri güncellendi");
            }
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //TEMİZLEMEK İÇİN
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            //textBox7.Clear();
            textBox8.Clear();
            textBox9.Clear();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //İLAÇ EKLEMEK İÇİN
            if (con.State == ConnectionState.Closed)
                con.Open();
            string kayit = "insert into Ilac(ad,stok_adedi,miligram,ucret,kapsul_tablet_adedi,barkod,recete_turu,ureten_firma,etkinMadde) values (@ad,@stok_adedi,@miligram,@ucret,@kapsul_tablet_adedi,@barkod,@recete_turu,@ureten_firma,@etkinMadde)";
            SqlCommand komut = new SqlCommand(kayit, con);
            komut.Parameters.AddWithValue("@ad", textBox1.Text);
            komut.Parameters.AddWithValue("@stok_adedi", textBox2.Text);
            komut.Parameters.AddWithValue("@miligram", textBox3.Text);
            komut.Parameters.AddWithValue("@ucret", textBox4.Text);
            komut.Parameters.AddWithValue("@kapsul_tablet_adedi", textBox5.Text);
            komut.Parameters.AddWithValue("@barkod", textBox6.Text);
            komut.Parameters.AddWithValue("@recete_turu", comboBox1.Text);
            komut.Parameters.AddWithValue("@ureten_firma", textBox8.Text);
            komut.Parameters.AddWithValue("@etkinMadde", textBox9.Text);

            SqlCommand ac = new SqlCommand("select * from Ilac where barkod='" + textBox6.Text + "'", con);
            SqlDataReader ab = ac.ExecuteReader();
            if (ab.Read())
            {
                MessageBox.Show("Bu ilaç kayıtlı");
            }
            else
            {
                ab.Close();

                komut.ExecuteNonQuery();
                MessageBox.Show("İlaç kaydedildi");
            }
            con.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            exportDG();

        }

        private void exportDG()
        {
            excel.Application app = new excel.Application();
            excel.Workbook workbook = app.Workbooks.Add();
            excel.Worksheet worksheet = null;
            app.Visible = true;
            worksheet = workbook.Sheets["Sayfa1"];
            worksheet = workbook.ActiveSheet;
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dataGridView1.Columns[i].HeaderText;
            }
            for (int j = 0; j < dataGridView1.Rows.Count - 1; j++)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    worksheet.Cells[j + 2, i + 1] = dataGridView1.Rows[j].Cells[i].Value.ToString();
                }
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            //EXCEL BUTONU (İÇE)

            ExcelImport ice = new ExcelImport();
            {
                ice.StartPosition = FormStartPosition.CenterScreen;
                ice.Show();
            }






        }
        public static void Excel_Disa_Aktar(DataGridView dataGridView1)
        {

            //SaveFileDialog save = new SaveFileDialog();
            //save.OverwritePrompt = false;
            //save.Title = "Excel Dosyaları";
            //save.DefaultExt = "xlsx";
            //save.Filter = "xlsx Dosyaları (*.xlsx)|*.xlsx|Tüm Dosyalar(*.*)|*.*";

            //if (save.ShowDialog() == DialogResult.OK)
            //{
            //    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
            //    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
            //    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            //    app.Visible = true;
            //    worksheet = workbook.Sheets["Sayfa1"];
            //    worksheet = workbook.ActiveSheet;
            //    worksheet.Name = "Excel Dışa Aktarım";
            //    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
            //    {
            //        worksheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
            //    }
            //    for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            //    {
            //        for (int j = 0; j < dataGridView1.Columns.Count; j++)
            //        {
            //            worksheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
            //        }
            //    }
            //    workbook.SaveAs(save.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            //    app.Quit();
        }
    }
}

