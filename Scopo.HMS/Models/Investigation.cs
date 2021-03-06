using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Investigation
    {
        public int InvestigationID { get; set; }
        public int AdmissionID { get; set; }
        public int LabTestID { get; set; }
        public DateTime InvestigationDate { get; set; }
        public decimal Amount { get; set; }
    }
}
