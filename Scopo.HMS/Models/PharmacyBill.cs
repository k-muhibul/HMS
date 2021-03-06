using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class PharmacyBill
    {
        public int PharmacyBillID { get; set; }
        public int AdmissionID { get; set; }
        public int MedicineID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
    }
}
