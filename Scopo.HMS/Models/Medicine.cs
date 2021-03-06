using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Medicine
    {
        public int MedicineID { get; set; }
        public string Name { get; set; }

        public decimal Price { get; set; }
        public string Unit { get; set; }

    }
}
