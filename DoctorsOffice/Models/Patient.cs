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
  }
}