using GESTOR_GASTOS.Entities;
using Microsoft.AspNetCore.Identity;

namespace GESTOR_GASTOS.Data
{
	public static class SeedData
	{
		public static async Task InitializeAsync(
			IServiceProvider serviceProvider,
			IConfiguration configuration)
		{
			var roleManager =
				serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			var userManager =
				serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

			// Crear roles
			var roles = new[] { "Admin", "User" };

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					var result = await roleManager.CreateAsync(
						new IdentityRole(role)
					);

					if (!result.Succeeded)
					{
						throw new Exception(
							$"No fue posible crear el rol {role}: " +
							string.Join(", ", result.Errors.Select(e => e.Description))
						);
					}
				}
			}

			// Configuración del administrador
			var adminConfig =
				configuration.GetSection("AdminUser");

			var adminUserName =
				adminConfig.GetValue<string>("UserName")
				?? "admin";

			var adminEmail =
				adminConfig.GetValue<string>("Email")
				?? "admin@localhost";

			var adminPassword =
				adminConfig.GetValue<string>("Password")
				?? "Admin123!";

			// Buscar administrador
			var admin =
				await userManager.FindByNameAsync(adminUserName);

			if (admin == null)
			{
				admin = new ApplicationUser
				{
					UserName = adminUserName,
					Email = adminEmail,
					EmailConfirmed = true
				};

				var result =
					await userManager.CreateAsync(
						admin,
						adminPassword
					);

				if (!result.Succeeded)
				{
					throw new Exception(
						"No fue posible crear el usuario administrador: " +
						string.Join(
							", ",
							result.Errors.Select(e => e.Description)
						)
					);
				}
			}

			// Garantizar que el administrador tenga el rol Admin
			if (!await userManager.IsInRoleAsync(admin, "Admin"))
			{
				var result =
					await userManager.AddToRoleAsync(
						admin,
						"Admin"
					);

				if (!result.Succeeded)
				{
					throw new Exception(
						"No fue posible asignar el rol Admin: " +
						string.Join(
							", ",
							result.Errors.Select(e => e.Description)
						)
					);
				}
			}
		}
	}
}