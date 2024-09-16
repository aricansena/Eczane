using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proje
{
    class ilac
    {
        public int ilac_id { get; set; }
        public string doz { get; set; }
        public int adet { get; set; }
        public string ad { get; set; }
        public int miligram { get; set; }
        public int kapsul_tablet_adedi { get; set; }
        public decimal ucret { get; set; }

    }

}
