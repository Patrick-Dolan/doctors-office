using System;
using System.Collections.Generic;
using System.Linq;

namespace DoctorsOffice.Models
{
  public class Patient
  {
    public Patient()
    {
      this.JoinDoctorPatient = new HashSet<DoctorPatient>();
    }

    public int PatientId { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; }
    public virtual ICollection<DoctorPatient> JoinDoctorPatient { get; set; }

    public bool isDuplicateDoctor(DoctorsOfficeContext _db, int doctorId)
    {
      var doctors =  _db.DoctorPatients.Where(doctor => doctor.PatientId == this.PatientId).ToList();
      bool isDuplicate = false;
      foreach (var doctor in doctors)
      {
        if (doctorId == doctor.DoctorId)
        {
          isDuplicate = true;
        }
      }
      return isDuplicate;
    }
  }
}