using System.Threading.Tasks;
using WebSample_API.Models;

namespace WebSample_API.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string passWord);
        Task<User> Login(string userName, string passWord);
        Task<bool> UserExists(string userName);
    } 
}