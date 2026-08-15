using GESTOR_GASTOS.DTOs.Auth;
using GESTOR_GASTOS.Services;
using Microsoft.AspNetCore.Mvc;

namespace GESTOR_GASTOS.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
		{
			_authService = authService;
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register(
			RegisterRequest model)
		{
			await _authService.RegisterAsync(model);

			return Ok(new
			{
				message = "User created successfully"
			});
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login(
			LoginRequest model)
		{
			var token = await _authService.LoginAsync(model);

			return Ok(new
			{
				token
			});
		}
	}
}