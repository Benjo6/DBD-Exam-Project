using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Pharmaceut
    {
        public Pharmaceut()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public int? PharmacyId { get; set; }
        public int PersonalDataId { get; set; }

        public virtual PersonalDatum PersonalData { get; set; } = null!;
        public virtual Pharmacy? Pharmacy { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
