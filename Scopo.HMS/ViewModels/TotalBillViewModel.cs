using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.ViewModels
{
    public class TotalBillViewModel
    {
      
        public int PatientCode { get; set; }
        public string PatientName { get; set; }
        public string ConsultantName { get; set; }
        public int AdmissionID { get; set; }
       
        public decimal DoctorBillAmount { get; set; }

        public decimal InvestigationBillAmount { get; set; }
        public string PatientEmail { get; set; }
        public string PatientPhone { get; set; }

        public decimal PharmacyBillAmount { get; set; }

        public decimal TotalBill { get; set; }

        public List<DoctorsBillsViewModel> doctorsBillsViewModels { get; set; }
        public List<PharmacyBillsViewModel> pharmacyBillsViewModels { get; set; }
        public List<InvestigationBillViewModel> investigationBillViewModels { get; set; }





    }
}
