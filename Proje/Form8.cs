using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proje
{
    public partial class Form8 : Form
    {
        //public static string ucret;

        public Form8()
        {
            InitializeComponent();
        }
        

        private void Form8_Load(object sender, EventArgs e)
        {
            /*Form10 frm = (Form10)Application.OpenForms["Form10"];
            frm.label11= new System.Windows.Forms.Label();*/
            //ucret = label11.Text;
            label7.Text = Form10.gonderilecekveri;
            label12.Text = DateTime.Now.ToShortDateString();
            
        }

    }
}
