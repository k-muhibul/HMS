using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scopo.HMS.Models
{
    public class HMSDbContext:DbContext
    {

        public HMSDbContext(DbContextOptions<HMSDbContext> options)
         : base(options)
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientLog> PatientLogs { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<DoctorsBill> DoctorsBills { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Investigation> Investigations { get; set; }
        public DbSet<PharmacyBill> PharmacyBills { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<LabTest> LabTests { get; set; }




    }
}

