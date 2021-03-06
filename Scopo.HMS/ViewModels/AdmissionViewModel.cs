using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.ViewModels
{
    public class AdmissionViewModel
    {
        public int AdmissionID { get; set; }
        public string Room { get; set; }
        public string PatientName { get; set; }
        public int PatientCode { get; set; }
        public string ConsultantName { get; set; }
        public string PatientStatus { get; set;}
        public DateTime AdmissionDate { get; set; }
        public DateTime? ReleaseDate { get; set; }

    }
}
