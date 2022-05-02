using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class UserRole
    {
        public UserRole()
        {
            PersonalData = new HashSet<PersonalDatum>();
            Permissions = new HashSet<UserPermission>();
        }

        public string Name { get; set; } = null!;
        public int Id { get; set; }

        public virtual ICollection<PersonalDatum> PersonalData { get; set; }

        public virtual ICollection<UserPermission> Permissions { get; set; }
    }
}
