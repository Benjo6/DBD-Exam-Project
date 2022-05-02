using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Doctor
    {
        public Doctor()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalData { get; set; } = null!;
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
