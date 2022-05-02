using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Medicine
    {
        public Medicine()
        {
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
