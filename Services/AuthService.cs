using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GESTOR_GASTOS.DTOs.Auth;
using GESTOR_GASTOS.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace GESTOR_GASTOS.Services
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;
		private readonly UserManager<ApplicationUser> _userManager;

		public AuthService(
			IConfiguration configuration,
			UserManager<ApplicationUser> userManager)
		{
			_configuration = configuration;
			_userManager = userManager;
		}

		public async Task RegisterAsync(RegisterRequest model)
		{
			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email
			};

			var result = await _userManager.CreateAsync(
				user,
				model.Password
			);

			if (!result.Succeeded)
			{
				throw new Exception(
					string.Join(
						", ",
						result.Errors.Select(e => e.Description)
					)
				);
			}

			var roleResult = await _userManager.AddToRoleAsync(
				user,
				"User"
			);

			if (!roleResult.Succeeded)
			{
				await _userManager.DeleteAsync(user);

				throw new Exception(
					string.Join(
						", ",
						roleResult.Errors.Select(e => e.Description)
					)
				);
			}
		}

		public async Task<string> LoginAsync(LoginRequest model)
		{
			var user = await _userManager.FindByNameAsync(
				model.UserName
			);

			if (user == null)
			{
				throw new UnauthorizedAccessException(
					"Usuario o contraseña incorrectos."
				);
			}

			var validPassword =
				await _userManager.CheckPasswordAsync(
					user,
					model.Password
				);

			if (!validPassword)
			{
				throw new UnauthorizedAccessException(
					"Usuario o contraseña incorrectos."
				);
			}

			return await GenerateJwtTokenAsync(user);
		}

		public async Task<string> GenerateJwtTokenAsync(
			ApplicationUser user)
		{
			var authClaims = new List<Claim>
			{
				new Claim(
					ClaimTypes.NameIdentifier,
					user.Id
				),

				new Claim(
					ClaimTypes.Name,
					user.UserName ?? string.Empty
				),

				new Claim(
					JwtRegisteredClaimNames.Jti,
					Guid.NewGuid().ToString()
				)
			};

			var roles = await _userManager.GetRolesAsync(user);

			foreach (var role in roles)
			{
				authClaims.Add(
					new Claim(
						ClaimTypes.Role,
						role
					)
				);
			}

			var jwtSettings =
				_configuration.GetSection("JwtSettings");

			var secret = jwtSettings["Secret"];

			if (string.IsNullOrWhiteSpace(secret))
			{
				throw new InvalidOperationException(
					"JwtSettings:Secret no está configurado."
				);
			}

			var authSigningKey =
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(secret)
				);

			var token = new JwtSecurityToken(
				issuer: jwtSettings["Issuer"],
				audience: jwtSettings["Audience"],
				expires: DateTime.UtcNow.AddMinutes(
					jwtSettings.GetValue<int>(
						"ExpiresInMinutes"
					)
				),
				claims: authClaims,
				signingCredentials:
					new SigningCredentials(
						authSigningKey,
						SecurityAlgorithms.HmacSha256
					)
			);

			return new JwtSecurityTokenHandler()
				.WriteToken(token);
		}
	}
}