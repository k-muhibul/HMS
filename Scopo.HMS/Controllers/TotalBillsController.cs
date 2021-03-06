using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Scopo.HMS.Models;
using Scopo.HMS.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Scopo.HMS.Controllers
{
    public class TotalBillsController : Controller
    {
        private readonly HMSDbContext _context;
        public TotalBillsController(HMSDbContext context)
        {
            _context = context;
        }

        public  IActionResult Index()
        {
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");

            return View();
        }
        [HttpPost]
        public IActionResult GetBill (TotalBillViewModel totalBillVM)
        {
            decimal docbill = 0;
            decimal pharmBill = 0;
            decimal InvBill = 0;
           var docc = from a in _context.Admissions join docb in _context.DoctorsBills on a.AdmissionID equals docb.AdmissionID 
                      where a.AdmissionID==totalBillVM.AdmissionID select docb.Amount;
            foreach(var i in docc)
            {
                docbill = docbill + i;
            } 
            var phab = from a in _context.Admissions join phb in _context.PharmacyBills on a.AdmissionID equals phb.AdmissionID 
                      where a.AdmissionID==totalBillVM.AdmissionID select phb.Amount;
            foreach(var i in phab)
            {
                pharmBill = pharmBill + i;
            }

            var invb = from a in _context.Admissions
                       join inv in _context.Investigations on a.AdmissionID equals inv.AdmissionID
                       where a.AdmissionID == totalBillVM.AdmissionID
                       select inv.Amount;
            foreach (var i in invb)
            {
                InvBill = InvBill + i;
            }

            totalBillVM.DoctorBillAmount = docbill;
            totalBillVM.PharmacyBillAmount = pharmBill;
            totalBillVM.InvestigationBillAmount = InvBill;
            totalBillVM.TotalBill = docbill + pharmBill + InvBill;

            var res = (from a in _context.Admissions
                      join p in _context.Patients on a.PatientCode equals p.PatientCode
                      where a.AdmissionID == totalBillVM.AdmissionID
                      select p).FirstOrDefault();
            totalBillVM.PatientCode = res.PatientCode;
            totalBillVM.PatientEmail = res.Email;
            totalBillVM.PatientName = res.Name;
            totalBillVM.PatientPhone = res.Mobile;
            var docdetails = (from a in _context.Admissions join d in _context.Doctors on a.DoctorID equals d.DoctorID  where a.AdmissionID == totalBillVM.AdmissionID select d).FirstOrDefault();
            totalBillVM.ConsultantName = docdetails.Name;

            var docbillList = (from docb in _context.DoctorsBills
                              join a in _context.Admissions on docb.AdmissionID equals a.AdmissionID
                              where a.AdmissionID == totalBillVM.AdmissionID
                              select new DoctorsBillsViewModel
                              {
                                  VisitDate = docb.VisitDate,
                                  Amount = docb.Amount
                              }).ToList();
            totalBillVM.doctorsBillsViewModels = docbillList;
            var pharmbillList = (from ph in _context.PharmacyBills
                               join a in _context.Admissions on ph.AdmissionID equals a.AdmissionID
                               join m in _context.Medicines on ph.MedicineID equals m.MedicineID
                               where a.AdmissionID == totalBillVM.AdmissionID
                               select new PharmacyBillsViewModel
                               {
                                   MedicineName=m.Name,
                                   Quantity=ph.Quantity,
                                   Amount=ph.Amount
                               }).ToList();
            totalBillVM.pharmacyBillsViewModels = pharmbillList;
            var invBillList = (from inbill in _context.Investigations
                               join a in _context.Admissions on inbill.AdmissionID equals a.AdmissionID
                               join lab in _context.LabTests on inbill.LabTestID equals lab.LabTestID
                               where a.AdmissionID == totalBillVM.AdmissionID
                               select new InvestigationBillViewModel
                               {
                                   LabTestName=lab.Name,
                                   InvestigationDate=inbill.InvestigationDate,
                                   Amount=inbill.Amount
                               }).ToList();
            totalBillVM.investigationBillViewModels = invBillList;



            return View(totalBillVM);
        }
    }
}
