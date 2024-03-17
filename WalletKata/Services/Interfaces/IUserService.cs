using System.Threading.Tasks;
using WalletKata.Models;

namespace WalletKata.Services.Interfaces
{
    public interface IUserService
    {
        Task<long> CreateUser(string username);
    }
}
