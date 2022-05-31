using Dapper;
using lib.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TestDataAPI.DAP
{
    public class DapperPrescriptionRepo : IPrescriptionRepo
    {
        private string _connectionsString;

        public DapperPrescriptionRepo(string cs)
        {
            _connectionsString = cs;
        }

        public LoginInfo AddUser(string username, string password, string salt, string passwordRaw, string role)
        {
            try
            {
                var func = $"select prescriptions.create_{role}(@username::varchar,@password_hashed::varchar,@password_salt::varchar,@password_raw::varchar)";

                var param = new DynamicParameters();
                param.Add("@username", username);
                param.Add("@password_hashed", password);
                param.Add("@password_salt", salt);
                param.Add("@password_raw", passwordRaw);

                using (var con = new NpgsqlConnection(_connectionsString))
                {
                    DefaultTypeMap.MatchNamesWithUnderscores = true;

                    var result = con.Query(sql: func, param: param, commandType: CommandType.Text);

                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return GetUserByUsername(username);
        }

        private LoginInfo GetUserByUsername(string username)
        {
            using (var con = new NpgsqlConnection(_connectionsString))
            {
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

                var query = @$" 
                SELECT * FROM prescriptions.login_info login 
                WHERE login.username like @name::varchar";

                var param = new DynamicParameters();
                param.Add("@name", username);

                return con.QuerySingle<LoginInfo>(query, param: param);
            }
        }
    }
}
