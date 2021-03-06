using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class PatientLog
    {
        public int PatientLogID { get; set; }
        public int PatientCode { get; set; }
        public int HeartRate { get; set; }
        public string BloodPressure { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Height { get; set; }
        public decimal? Weight { get; set; }
        public decimal? BMI { get; set; }
        public Patient patient { get; set; }
     
    }
}
