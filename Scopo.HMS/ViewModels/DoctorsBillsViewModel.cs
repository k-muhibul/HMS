using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.ViewModels
{
    public class DoctorsBillsViewModel
    {
        public int DoctorBillsID { get; set; }
        public int PatientCode { get; set; }
        public string PatientName { get; set; }
        public int DoctorsID { get; set; }
        public string ConsultantName { get; set; }
        public int AdmissionID { get; set; }
        public DateTime VisitDate { get; set; }
        public decimal Amount { get; set; }
    }
}
