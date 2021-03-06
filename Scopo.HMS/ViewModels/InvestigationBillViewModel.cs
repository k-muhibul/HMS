using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.ViewModels
{
    public class InvestigationBillViewModel
    {
        public int InvestigationID { get; set; }
        public int AdmissionID { get; set; }
        public int PatientCode { get; set; }
        public string PtientName { get; set; }
        public string Room { get; set; }
        public DateTime InvestigationDate { get; set; }
        public int LabTestID { get; set; }
        public string LabTestName { get; set; }
        public decimal Amount { get; set; }
    }
}
