using TestDataApi;

namespace PrescriptionService.DAL
{
    public interface IPrescriptionRepo
    {
        public LoginInfo AddUser(string username, string password, string salt, string passwordRaw, string role);
    }
}
