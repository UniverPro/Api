using System.Threading.Tasks;
using Uni.DataAccess.Models;

namespace Uni.Identity.Web.Services
{
    public interface IUserService
    {
        Task<Person> FindUserByIdAsync(int userId);
        Task<Person> FindAsync(string login, string password);
    }
}