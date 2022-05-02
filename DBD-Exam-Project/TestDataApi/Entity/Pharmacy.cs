using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class Pharmacy
    {
        public Pharmacy()
        {
            Pharmaceuts = new HashSet<Pharmaceut>();
        }

        public int Id { get; set; }
        public string PharmacyName { get; set; } = null!;
        public int? AddressId { get; set; }

        public virtual Address? Address { get; set; }
        public virtual ICollection<Pharmaceut> Pharmaceuts { get; set; }
    }
}
