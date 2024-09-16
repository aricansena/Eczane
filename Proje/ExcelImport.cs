using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.ExceptionServices;
//using DocumentFormat.OpenXml.Vml.Spreadsheet;


namespace Proje
{
    public partial class ExcelImport : Form
    {
        static string baglanti = "DATA SOURCE = BEYZAKESKIN\\SQLEXPRESS;Initial Catalog = Eczane;Integrated Security=True";
        SqlConnection con = new SqlConnection(baglanti);
        SqlTransaction process = null;
        public ExcelImport()
        {
            InitializeComponent();
            InitializeDataGridView();
        }
        private void InitializeDataGridView()
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ReadOnly = true;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode =
            DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.RowHeadersWidthSizeMode =
            DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files |*.xlsx| Excel Files |*.xls";
            openFileDialog.Title = "Import excel to database";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + openFileDialog.FileName + ";Extended Properties='Excel 12.0 Xml;HDR=YES'");
                con.Open();
                OleDbDataAdapter oda = new OleDbDataAdapter("Select * from [Sayfa1$]", con);
                DataTable dt = new DataTable();
                oda.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                MessageBox.Show("Lütfen ilgili dosyayı seçiniz.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                con.Open();

                string ilac = "insert into Ilac(ad,stok_adedi,miligram,ucret,kapsul_tablet_adedi,barkod,recete_turu,ureten_firma,etkinMadde) values (@ad,@stok_adedi,@miligram,@ucret,@kapsul_tablet_adedi,@barkod,@recete_turu,@ureten_firma,@etkinMadde)";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {

                    SqlCommand sqlCommand = new SqlCommand(ilac, con);

                    //sqlCommand.Parameters.AddWithValue("@ilac_id", row.Cells["ilac_id"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@ad", row.Cells["ad"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@stok_adedi", row.Cells["stok_adedi"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@miligram", row.Cells["miligram"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@ucret", row.Cells["ucret"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@kapsul_tablet_adedi", row.Cells["kapsul_tablet_adedi"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@barkod", row.Cells["barkod"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@recete_turu", row.Cells["recete_turu"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@ureten_firma", row.Cells["ureten_firma"].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@etkinMadde", row.Cells["etkinMadde"].Value.ToString());

          
                    try
                    {
                        //con.Open();
                        sqlCommand.ExecuteNonQuery();
                        process = con.BeginTransaction();
                        process.Commit();
                        MessageBox.Show("Yükleme başarıyla yapıldı.", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //con.Close();

                    }
                    catch (Exception ex)
                    {

                        //process.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    //finally
                    //{
                    //    con.Close();
                    //}

                }
                con.Close();

            }
        }
        private void ExcelImport_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
      

    }
}
