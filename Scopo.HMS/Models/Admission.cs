using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Admission
    {
        public int AdmissionID { get; set; }
        public int RoomID { get; set; }
        public int PatientCode { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PatientStatus { get; set; }
        public int DoctorID { get; set; }
    }
}
