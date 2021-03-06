using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class Patient
    {
        public int PatientID { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }
        public string Gender { get; set; }
      
        public string Mobile { get; set; }
        public string EmergencyContact { get; set; }
        public string Email { get; set; }
        public int PatientCode { get; set; }

       
    }
}
