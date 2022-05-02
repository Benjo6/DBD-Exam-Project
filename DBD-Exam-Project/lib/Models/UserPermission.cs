using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib.Models
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
