using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class PersonalDatum
    {
        public PersonalDatum()
        {
            Doctors = new HashSet<Doctor>();
            Patients = new HashSet<Patient>();
            Pharmaceuts = new HashSet<Pharmaceut>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public int? LoginId { get; set; }
        public int? RoleId { get; set; }
        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual LoginInfo? Login { get; set; }
        public virtual UserRole? Role { get; set; }
        public virtual ICollection<Doctor> Doctors { get; set; }
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Pharmaceut> Pharmaceuts { get; set; }
    }
}
