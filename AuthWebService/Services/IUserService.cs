using AuthWebService.Models;
using System.Threading.Tasks;

namespace AuthWebService.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string UserId);
        Task<bool> Authenticate(string username, string password);
        Task<string> GetToken(string username);
    }
}
