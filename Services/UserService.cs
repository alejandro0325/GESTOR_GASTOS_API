using Microsoft.AspNetCore.Identity;
using GESTOR_GASTOS.Entities;

namespace GESTOR_GASTOS.Services;

public class UserService : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserService(
		UserManager<ApplicationUser> userManager,
		RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<IEnumerable<ApplicationUser>> GetUsersAsync()
	{
		return _userManager.Users.ToList();
	}

	public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
	{
		return await _userManager.FindByIdAsync(userId);
	}

	public async Task<bool> AssignRoleAsync(
		string userId,
		string role)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
			return false;

		if (!await _roleManager.RoleExistsAsync(role))
			return false;

		var result = await _userManager.AddToRoleAsync(user, role);

		return result.Succeeded;
	}

	public async Task<bool> RemoveRoleAsync(
		string userId,
		string role)
	{
		var user = await _userManager.FindByIdAsync(userId);

		if (user == null)
			return false;

		var result = await _userManager.RemoveFromRoleAsync(user, role);

		return result.Succeeded;
	}
}