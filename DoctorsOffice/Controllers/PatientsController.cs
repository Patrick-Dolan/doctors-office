using DoctorsOffice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DoctorsOffice.Controllers
{
  public class PatientsController : Controller
  {
    private readonly DoctorsOfficeContext _db;

    public PatientsController(DoctorsOfficeContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      return View(_db.Patients.ToList());
    }

    public ActionResult Create()
    {
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Patient patient, int DoctorId)
    {
      _db.Patients.Add(patient);
      _db.SaveChanges();
      if (DoctorId != 0)
      {
        _db.DoctorPatients.Add(new DoctorPatient() {DoctorId = DoctorId, PatientId = patient.PatientId});
        _db.SaveChanges();
      }
      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      Patient foundPatient = _db.Patients
        .Include(patient => patient.JoinDoctorPatient)
        .ThenInclude(join => join.Doctor)
        .FirstOrDefault(model => model.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(foundPatient);
    }

    public ActionResult Edit(int id)
    {
      var foundPatient = _db.Patients.FirstOrDefault(patient => patient.PatientId == id);
      ViewBag.DoctorId = new SelectList(_db.Doctors, "DoctorId", "Name");
      return View(foundPatient);
    }

    [HttpPost]
    public ActionResult Edit(Patient patient, int DoctorId)
    {
      if (DoctorId !=0)
      {
        _db.DoctorPatients.Add(new DoctorPatient() {DoctorId = DoctorId, PatientId = patient.PatientId});        
      }
      _db.Entry(patient).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      Patient foundPatient = _db.Patients.FirstOrDefault(patient => patient.PatientId == id);
      return View(foundPatient);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      Patient foundPatient = _db.Patients.FirstOrDefault(patient => patient.PatientId == id);
      _db.Patients.Remove(foundPatient);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult AddDoctor(Patient patient, int DoctorId)
    {
      if (DoctorId !=0)
      {
        _db.DoctorPatients.Add(new DoctorPatient() {DoctorId = DoctorId, PatientId = patient.PatientId});
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = patient.PatientId});
    }

    [HttpPost]
    public ActionResult DeleteDoctor(int joinId)
    {
      var joinEntry = _db.DoctorPatients.FirstOrDefault(entry => entry.DoctorPatientId == joinId);
      int savedPatient = joinEntry.PatientId;
      _db.DoctorPatients.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Details", new {id = savedPatient});
    }
  }
}