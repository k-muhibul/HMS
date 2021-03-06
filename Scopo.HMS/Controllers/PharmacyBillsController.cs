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
    public class PharmacyBillsController : Controller
    {
        private readonly HMSDbContext _context;

        public PharmacyBillsController(HMSDbContext context)
        {
            _context = context;
        }

        // GET: PharmacyBills
        public async Task<IActionResult> Index()
        {
            var res = await (from p in _context.PharmacyBills
                      join a in _context.Admissions on p.AdmissionID equals a.AdmissionID
                      join pt in _context.Patients on a.PatientCode equals pt.PatientCode
                      join m in _context.Medicines on p.MedicineID equals m.MedicineID
                      join r in _context.Rooms on a.RoomID equals r.RoomID
                      select new PharmacyBillsViewModel
                      {
                          AdmissionID = p.AdmissionID,
                          PatientCode = pt.PatientCode,
                          PatientName = pt.Name,
                          MedicineName = m.Name,
                          PharmacyBillID = p.PharmacyBillID,
                          Quantity = p.Quantity,
                          Amount = p.Amount,
                          Date = p.Date,
                          Room=r.Name
                      }).ToListAsync();

            return View(res);
        }

        // GET: PharmacyBills/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyBill = await _context.PharmacyBills
                .FirstOrDefaultAsync(m => m.PharmacyBillID == id);
            if (pharmacyBill == null)
            {
                return NotFound();
            }

            return View(pharmacyBill);
        }

        // GET: PharmacyBills/Create
        public IActionResult Create()
        {
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");
            var b = from m in _context.Medicines select new { Value = m.MedicineID, Text = m.Name };
            ViewBag.Medicines = new SelectList(b, "Value", "Text");
            return View();
        }

        // POST: PharmacyBills/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PharmacyBillsViewModel pharmacyBillVM)
        {
            var res = (from a in _context.Admissions select new { Value = a.AdmissionID, Text = a.PatientCode });
            ViewBag.AdmissionID = new SelectList(res, "Value", "Text");
            var b = from m in _context.Medicines select new { Value = m.MedicineID, Text = m.Name };
            ViewBag.Medicines = new SelectList(b, "Value", "Text");

            if (ModelState.IsValid)
            {
                for(int i=0;i<pharmacyBillVM.MedicineID.Length;i++)
                {
                    PharmacyBill pharmacyBill = new PharmacyBill();
                    pharmacyBill.AdmissionID = pharmacyBillVM.AdmissionID;
                    pharmacyBill.MedicineID = pharmacyBillVM.MedicineID[i];
                    pharmacyBill.Quantity = pharmacyBillVM.Quantity;
                    pharmacyBill.Date = pharmacyBillVM.Date;
                    var cost = (from m in _context.Medicines where m.MedicineID == pharmacyBill.MedicineID select m.Price).FirstOrDefault();
                    pharmacyBill.Amount = cost * pharmacyBill.Quantity;
                    _context.Add(pharmacyBill);
                }
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pharmacyBillVM);
        }

        // GET: PharmacyBills/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyBill = await _context.PharmacyBills.FindAsync(id);
            if (pharmacyBill == null)
            {
                return NotFound();
            }
            return View(pharmacyBill);
        }

        // POST: PharmacyBills/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  PharmacyBill pharmacyBill)
        {
            if (id != pharmacyBill.PharmacyBillID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pharmacyBill);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PharmacyBillExists(pharmacyBill.PharmacyBillID))
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
            return View(pharmacyBill);
        }

        // GET: PharmacyBills/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pharmacyBill = await _context.PharmacyBills
                .FirstOrDefaultAsync(m => m.PharmacyBillID == id);
            if (pharmacyBill == null)
            {
                return NotFound();
            }

            return View(pharmacyBill);
        }

        // POST: PharmacyBills/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pharmacyBill = await _context.PharmacyBills.FindAsync(id);
            _context.PharmacyBills.Remove(pharmacyBill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PharmacyBillExists(int id)
        {
            return _context.PharmacyBills.Any(e => e.PharmacyBillID == id);
        }
    }
}
