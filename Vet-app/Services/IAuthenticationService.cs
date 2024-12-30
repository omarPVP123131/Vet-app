using System.Threading.Tasks;
using VeterinaryManagementSystem.Models;

namespace VeterinaryManagementSystem.Services
{
    public interface IAuthenticationService
    {
        Task<(bool success, string message, User user)> LoginAsync(string username, string password);
        Task<bool> LogoutAsync();
        bool IsAuthenticated { get; }
        User CurrentUser { get; }
    }
}