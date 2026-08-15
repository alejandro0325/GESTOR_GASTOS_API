namespace GESTOR_GASTOS.DTOs.Auth
{
	public record RegisterRequest(
		string UserName,
		string Email,
		string Password
	);
}