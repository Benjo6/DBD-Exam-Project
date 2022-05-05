using lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestDataAPI.DAP
{
    public interface IPrescriptionRepo
    {
        public LoginInfo AddUser(string username, string password, string salt, string passwordRaw, string role);
    }
}
