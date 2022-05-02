using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Prescription
    {
        public long Id { get; set; }
        public DateOnly? Expiration { get; set; }
        public bool? ExpirationWarningSent { get; set; }
        public DateTime Creation { get; set; }
        public int MedicineId { get; set; }
        public int PrescribedBy { get; set; }
        public int PrescribedTo { get; set; }
        public string PrescribedToCpr { get; set; } = null!;
        public int? LastAdministeredBy { get; set; }

        public virtual Pharmaceut? LastAdministeredByNavigation { get; set; }
        public virtual Medicine Medicine { get; set; } = null!;
        public virtual Doctor PrescribedByNavigation { get; set; } = null!;
        public virtual Patient PrescribedToNavigation { get; set; } = null!;
    }
}
