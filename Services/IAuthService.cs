using GESTOR_GASTOS.DTOs.Auth;
using GESTOR_GASTOS.Entities;

namespace GESTOR_GASTOS.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest model);

        Task<string> LoginAsync(LoginRequest model);

        Task<string> GenerateJwtTokenAsync(ApplicationUser user);
    }
}
