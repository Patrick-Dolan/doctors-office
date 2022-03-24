using DoctorsOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class SpecialtiesController : Controller
  {
    private readonly DoctorsOfficeContext _db;

    public SpecialtiesController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Specialties.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Specialty specialty, int DoctorId)
    {
      _db.Specialties.Add(specialty);
      _db.SaveChanges();
      if (DoctorId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Specialty foundSpecialty = _db.Specialties
        .Include(specialty => specialty.JoinDoctorSpecialty)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(model => model.SpecialtyId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(foundSpecialty);
    }

    public ActionResult Edit(int id)
    {
      var foundSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(foundSpecialty);
    }

    [HttpPost]
    public ActionResult Edit(Specialty specialty, int DoctorId)
    {
      if (DoctorId !=0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId});        
      }
      _db.Entry(specialty).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Specialty foundSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      return View(foundSpecialty);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Specialty foundSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      _db.Specialties.Remove(foundSpecialty);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddDoctor(int id)
    {
      Specialty foundSpecialty = _db.Specialties.FirstOrDefault(specialty => specialty.SpecialtyId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(foundSpecialty);
    }

    [HttpPost]
    public ActionResult AddDoctor(Specialty specialty, int DoctorId)
    {
      bool isDuplicate = specialty.isDuplicateDoctor(_db, DoctorId);
      if (DoctorId !=0 && isDuplicate == false)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {DoctorId = DoctorId, SpecialtyId = specialty.SpecialtyId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = specialty.SpecialtyId});
    }

    [HttpPost]
    public ActionResult DeleteDoctor(int joinId)
    {
      var joinEntry = _db.DoctorSpecialty.FirstOrDefault(entry => entry.DoctorSpecialtyId == joinId);
      int savedSpecialty = joinEntry.SpecialtyId;
      _db.DoctorSpecialty.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = savedSpecialty});
    }
  }
}