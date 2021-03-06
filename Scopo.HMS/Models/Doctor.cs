using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }

        public string Name { get; set; }
        public string Qualification { get; set; }
        public int MinVisitFee { get; set; }
        public int OutdoorFee { get; set; }
        public string Specialization { get; set; }
    }
}
