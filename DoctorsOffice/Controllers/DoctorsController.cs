using DoctorsOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class DoctorsController: Controller
  {
    private readonly DoctorsOfficeContext _db;

    public DoctorsController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Doctors.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.PatientId = new SelectList(_db.Patients, "PatientId", "Name");
      ViewBag.SpecialtyId = new SelectList(_db.Specialties, "SpecialtyId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Doctor doctor, int PatientId, int SpecialtyId)
    {
      _db.Doctors.Add(doctor);
      _db.SaveChanges();
      if (PatientId != 0)
      {
        _db.DoctorPatients.Add(new DoctorPatient() {PatientId = PatientId, DoctorId = doctor.DoctorId});
        _db.SaveChanges();
      }
      if (SpecialtyId != 0)
      {
        _db.DoctorSpecialty.Add(new DoctorSpecialty() {SpecialtyId = SpecialtyId, DoctorId = doctor.DoctorId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Doctor foundDoctor = _db.Doctors
        .Include(doctor => doctor.JoinDoctorPatient)
        .ThenInclude(joinPatient => joinPatient.Patient)
        .Include(doctor => doctor.JoinDoctorSpecialty)
        .ThenInclude(joinSpecialty => joinSpecialty.Specialty)
        .FirstOrDefault(model => model.DoctorId == id);
      return View(foundDoctor);
    }

    public ActionResult Edit(int id)
    {
      Doctor foundDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(foundDoctor);
    }

    [HttpPost]
    public ActionResult Edit(Doctor doctor)
    {
      _db.Entry(doctor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var foundDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      return View(foundDoctor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var foundDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      _db.Doctors.Remove(foundDoctor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeletePatient(int joinId)
    {
      var joinEntry = _db.DoctorPatients.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
      int savedDoctor = joinEntry.DoctorId;
      _db.DoctorPatients.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = savedDoctor});
    }

    [HttpPost]
    public ActionResult DeleteSpecialty(int joinId)
    {
      var joinEntry = _db.DoctorSpecialty.FirstOrDefault(entry => entry.DoctorSpecialtyId == joinId);
      int savedDoctor = joinEntry.DoctorId;
      _db.DoctorSpecialty.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new { id = savedDoctor});
    }

    public ActionResult AddPatient(int id)
    {
      Doctor foundDoctor = _db.Doctors.FirstOrDefault(doctor => doctor.DoctorId == id);
      ViewBag.PatientId = new SelectList(_db.Patients, "PatientId", "Name");
      return View(foundDoctor);
    }

    [HttpPost]
    public ActionResult AddPatient(Doctor doctor, int PatientId)
    {
      if (PatientId !=0)
      {
        _db.DoctorPatients.Add(new DoctorPatient() {PatientId = PatientId, DoctorId = doctor.DoctorId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }
  }
}