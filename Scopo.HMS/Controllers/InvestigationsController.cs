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
    public class InvestigationsController : Controller
    {
        private readonly HMSDbContext _context;

        public InvestigationsController(HMSDbContext context)
        {
            _context = context;
        }

        // GET: Investigations
        public async Task<IActionResult> Index()
        {
            var res = await (from inv in _context.Investigations
                      join a in _context.Admissions on inv.AdmissionID equals a.AdmissionID
                      join p in _context.Patients on a.PatientCode equals p.PatientCode
                      join r in _context.Rooms on a.RoomID equals r.RoomID
                      join lab in _context.LabTests on inv.LabTestID equals lab.LabTestID
                      select new InvestigationBillViewModel
                      {
                          InvestigationID = inv.InvestigationID,
                          AdmissionID = inv.AdmissionID,
                          InvestigationDate = inv.InvestigationDate,
                          LabTestID = inv.LabTestID,
                          LabTestName = lab.Name,
                          PatientCode = p.PatientCode,
                          PtientName = p.Name,
                          Amount = inv.Amount,
                          Room = r.Name
                      }).ToListAsync();




            return View(res);
        }

        // GET: Investigations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations
                .FirstOrDefaultAsync(m => m.InvestigationID == id);
            if (investigation == null)
            {
                return NotFound();
            }

            return View(investigation);
        }

        // GET: Investigations/Create
        public IActionResult Create()
        {
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");
            var lo = from l in _context.LabTests select new { Value = l.LabTestID, Text = l.Name };
            ViewBag.LabTestList = new SelectList(lo, "Value", "Text");
            return View();
        }

        // POST: Investigations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvestigationBillViewModel investigationVM)
        {
           
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");
            if (ModelState.IsValid)
            {
                Investigation inv = new Investigation();
                inv.AdmissionID = investigationVM.AdmissionID;
                inv.InvestigationID = investigationVM.InvestigationID;
                inv.LabTestID = investigationVM.LabTestID;
                inv.InvestigationDate = investigationVM.InvestigationDate;
                inv.Amount = _context.LabTests.Find(investigationVM.LabTestID).Amount;

                _context.Add(inv);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(investigationVM);
        }

        // GET: Investigations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations.FindAsync(id);
            if (investigation == null)
            {
                return NotFound();
            }
            return View(investigation);
        }

        // POST: Investigations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InvestigationID,AdmissionID,LabTestID,InvestigationDate,Amount")] Investigation investigation)
        {
            if (id != investigation.InvestigationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(investigation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvestigationExists(investigation.InvestigationID))
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
            return View(investigation);
        }

        // GET: Investigations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var investigation = await _context.Investigations
                .FirstOrDefaultAsync(m => m.InvestigationID == id);
            if (investigation == null)
            {
                return NotFound();
            }

            return View(investigation);
        }

        // POST: Investigations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var investigation = await _context.Investigations.FindAsync(id);
            _context.Investigations.Remove(investigation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvestigationExists(int id)
        {
            return _context.Investigations.Any(e => e.InvestigationID == id);
        }
    }
}
