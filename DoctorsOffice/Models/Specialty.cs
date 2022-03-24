using System.Collections.Generic;
using System.Linq;
namespace DoctorsOffice.Models
{
  public class Specialty
  {
    public Specialty()
    {
      this.JoinDoctorSpecialty = new HashSet<DoctorSpecialty>();
    }

    public int SpecialtyId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<DoctorSpecialty> JoinDoctorSpecialty { get; set; }

    public bool isDuplicateDoctor(DoctorsOfficeContext _db, int doctorId)
    {
      var doctors =  _db.DoctorSpecialty.Where(doctor => doctor.SpecialtyId == this.SpecialtyId).ToList();
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