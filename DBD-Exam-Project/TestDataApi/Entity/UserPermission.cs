using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class UserPermission
    {
        public UserPermission()
        {
            Roles = new HashSet<UserRole>();
        }

        public int Id { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }
    }
}
