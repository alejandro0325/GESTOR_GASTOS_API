using GESTOR_GASTOS.Entities;

namespace GESTOR_GASTOS.Services
{
	

	public interface IUserService
	{
		Task<IEnumerable<ApplicationUser>> GetUsersAsync();

		Task<ApplicationUser?> GetUserByIdAsync(string userId);

		Task<bool> AssignRoleAsync(string userId, string role);

		Task<bool> RemoveRoleAsync(string userId, string role);
	}
}
