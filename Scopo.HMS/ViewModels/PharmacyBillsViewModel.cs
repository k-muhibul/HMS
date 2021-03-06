using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.ViewModels
{
    public class PharmacyBillsViewModel
    {
        public int PharmacyBillID { get; set; }
        public int AdmissionID { get; set; }
        public int PatientCode { get; set; }
        public string Room { get; set; }
        public string PatientName { get; set; }
        public int[] MedicineID { get; set; }
        public string MedicineName { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
