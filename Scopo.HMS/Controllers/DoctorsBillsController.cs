using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scopo.HMS.Models;
using Scopo.HMS.ViewModels;

namespace Scopo.HMS.Controllers
{
    public class DoctorsBillsController : Controller
    {
        private readonly HMSDbContext _context;

        public DoctorsBillsController(HMSDbContext context)
        {
            _context = context;
        }

        // GET: DoctorsBills
        public async Task<IActionResult> Index()
        {

            var res = await (from db in _context.DoctorsBills
                       join a in _context.Admissions on db.AdmissionID equals a.AdmissionID
                       join p in _context.Patients on a.PatientCode equals p.PatientCode
                       join d in _context.Doctors on a.DoctorID equals d.DoctorID
                       select new DoctorsBillsViewModel
                       {
                           AdmissionID = db.AdmissionID,
                           PatientCode = p.PatientCode,
                           PatientName = p.Name,
                           DoctorsID = d.DoctorID,
                           ConsultantName = d.Name,
                           DoctorBillsID = db.DoctorsBillID,
                           VisitDate = db.VisitDate,
                           Amount = db.Amount
                       }).ToListAsync();
            return View(res);
        }

        // GET: DoctorsBills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsBill = await _context.DoctorsBills
                .FirstOrDefaultAsync(m => m.DoctorsBillID == id);
            if (doctorsBill == null)
            {
                return NotFound();
            }

            return View(doctorsBill);
        }

        // GET: DoctorsBills/Create
        public IActionResult Create()
        {
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");

           





            return View();
        }

        // POST: DoctorsBills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorsBillsViewModel doctorsBillVM)
        {
            var res = (from a in _context.Admissions
                      join d in _context.Doctors on a.DoctorID equals d.DoctorID
                      where a.AdmissionID == doctorsBillVM.AdmissionID
                      select new
                      {
                          Amount = d.MinVisitFee
                      }).FirstOrDefault();

            if (ModelState.IsValid)
            {
                DoctorsBill doctorsBill = new DoctorsBill();
                doctorsBill.AdmissionID = doctorsBillVM.AdmissionID;
                doctorsBill.DoctorsBillID = doctorsBillVM.DoctorBillsID;
                doctorsBill.Amount = res.Amount;
                doctorsBill.VisitDate = doctorsBillVM.VisitDate;
                _context.Add(doctorsBill);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doctorsBillVM);
        }

        // GET: DoctorsBills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsBill = await _context.DoctorsBills.FindAsync(id);
            if (doctorsBill == null)
            {
                return NotFound();
            }
            return View(doctorsBill);
        }

        // POST: DoctorsBills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,DoctorsBill doctorsBill)
        {
            if (id != doctorsBill.DoctorsBillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctorsBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorsBillExists(doctorsBill.DoctorsBillID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(doctorsBill);
        }

        // GET: DoctorsBills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctorsBill = await _context.DoctorsBills
                .FirstOrDefaultAsync(m => m.DoctorsBillID == id);
            if (doctorsBill == null)
            {
                return NotFound();
            }

            return View(doctorsBill);
        }

        // POST: DoctorsBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctorsBill = await _context.DoctorsBills.FindAsync(id);
            _context.DoctorsBills.Remove(doctorsBill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DoctorsBillExists(int id)
        {
            return _context.DoctorsBills.Any(e => e.DoctorsBillID == id);
        }
    }
}
