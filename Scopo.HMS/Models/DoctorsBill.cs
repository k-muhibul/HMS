using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class DoctorsBill
    {
        public int DoctorsBillID { get; set; }
        public int AdmissionID { get; set; }
        public DateTime VisitDate { get; set; }
        public decimal Amount { get; set; }
    }
}
