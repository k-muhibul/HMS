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
    public class AdmissionsController : Controller
    {
        private readonly HMSDbContext _context;

        public AdmissionsController(HMSDbContext context)
        {
            _context = context;
        }

        // GET: Admissions
        public async Task<IActionResult> Index()
        {
            var res =await ( from a in _context.Admissions
                      join r in _context.Rooms on a.RoomID equals r.RoomID
                      join p in _context.Patients on a.PatientCode equals p.PatientCode
                      join d in _context.Doctors on a.DoctorID equals d.DoctorID
                      select new AdmissionViewModel
                      {
                          AdmissionID=a.AdmissionID,
                          Room = r.Name,
                          PatientName = p.Name,
                          PatientCode = a.PatientCode,
                          ConsultantName = d.Name,
                          PatientStatus = a.PatientStatus,
                          AdmissionDate = a.AdmissionDate,
                          ReleaseDate = a.ReleaseDate
                      }).ToListAsync();




            return View(res);
        }

        // GET: Admissions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admission = await _context.Admissions
                .FirstOrDefaultAsync(m => m.AdmissionID == id);
            if (admission == null)
            {
                return NotFound();
            }

            return View(admission);
        }

        // GET: Admissions/Create
        public IActionResult Create()
        {
            var roomList = from r in _context.Rooms where r.IsOccupied==false select new { Value = r.RoomID, Text = r.Name };
            ViewBag.roomList = new SelectList(roomList, "Value", "Text");
            var doctorList = from d in _context.Doctors select new { Value = d.DoctorID, Text = d.Name };
            ViewBag.doctorList = new SelectList(doctorList, "Value", "Text");
            return View();
        }

        // POST: Admissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Admission admission)
        {
            var roomList = from r in _context.Rooms where r.IsOccupied == false select new { Value = r.RoomID, Text = r.Name };
            ViewBag.roomList = new SelectList(roomList, "Value", "Text");
            var doctorList = from d in _context.Doctors select new { Value = d.DoctorID, Text = d.Name };
            ViewBag.doctorList = new SelectList(doctorList, "Value", "Text");
            
            if (ModelState.IsValid)
            {
                _context.Add(admission);
                if (admission.AdmissionDate.ToShortDateString() != "" && admission.ReleaseDate == null)
                {
                    Room room = (from r in _context.Rooms
                               where r.RoomID == admission.RoomID
                               select new Room
                               {
                                   RoomID = admission.RoomID,
                                   Name = r.Name,
                                   Rent = r.Rent,
                                   IsOccupied = true

                               }).FirstOrDefault();
                    _context.Update(room);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admission);
        }

        // GET: Admissions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var roomList = from r in _context.Rooms where r.IsOccupied == false select new { Value = r.RoomID, Text = r.Name };
            ViewBag.roomList = new SelectList(roomList, "Value", "Text");
            var doctorList = from d in _context.Doctors select new { Value = d.DoctorID, Text = d.Name };
            ViewBag.doctorList = new SelectList(doctorList, "Value", "Text");
            if (id == null)
            {
                return NotFound();
            }

            var admission = await _context.Admissions.FindAsync(id);
            if (admission == null)
            {
                return NotFound();
            }
            return View(admission);
        }

        // POST: Admissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Admission admission)
        {
            var roomList = from r in _context.Rooms where r.IsOccupied == false select new { Value = r.RoomID, Text = r.Name };
            ViewBag.roomList = new SelectList(roomList, "Value", "Text");
            var doctorList = from d in _context.Doctors select new { Value = d.DoctorID, Text = d.Name };
            ViewBag.doctorList = new SelectList(doctorList, "Value", "Text");
            if (id != admission.AdmissionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (admission.AdmissionDate.ToShortDateString() != "" && admission.ReleaseDate != null)
                    {
                        Room room = (from r in _context.Rooms
                                     where r.RoomID == admission.RoomID
                                     select new Room
                                     {
                                         RoomID = admission.RoomID,
                                         Name = r.Name,
                                         Rent = r.Rent,
                                         IsOccupied = false

                                     }).FirstOrDefault();
                        _context.Update(room);
                    }




                    _context.Update(admission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdmissionExists(admission.AdmissionID))
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
            return View(admission);
        }

        // GET: Admissions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admission = await _context.Admissions
                .FirstOrDefaultAsync(m => m.AdmissionID == id);
            if (admission == null)
            {
                return NotFound();
            }

            return View(admission);
        }

        // POST: Admissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admission = await _context.Admissions.FindAsync(id);
            _context.Admissions.Remove(admission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdmissionExists(int id)
        {
            return _context.Admissions.Any(e => e.AdmissionID == id);
        }
    }
}
