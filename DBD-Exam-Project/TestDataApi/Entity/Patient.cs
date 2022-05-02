using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Patient
    {
        public Patient()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public string Cpr { get; set; } = null!;
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalData { get; set; } = null!;
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
