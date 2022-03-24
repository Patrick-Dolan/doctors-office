using System.Collections.Generic;
using System.Linq;

namespace DoctorsOffice.Models
{
  public class Doctor
  {
    public Doctor()
    {
      this.JoinDoctorPatient = new HashSet<DoctorPatient>();
      this.JoinDoctorSpecialty = new HashSet<DoctorSpecialty>();
    }

    public int DoctorId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DoctorPatient> JoinDoctorPatient { get; set; }
    public virtual ICollection<DoctorSpecialty> JoinDoctorSpecialty { get; set; }

    public bool isDuplicateSpecialty(DoctorsOfficeContext _db, int specialtyId)
    {
      var specialties =  _db.DoctorSpecialty.Where(specialty => specialty.DoctorId == this.DoctorId).ToList();
      bool isDuplicate = false;
      foreach (var specialty in specialties)
      {
        if (specialtyId == specialty.SpecialtyId)
        {
          isDuplicate = true;
        }
      }
      return isDuplicate;
    }
  }
}