using System;
using System.Collections.Generic;

namespace TestDataApi
{
    public partial class LoginInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;

        public virtual PersonalDatum? PersonalDatum { get; set; }
    }
}
