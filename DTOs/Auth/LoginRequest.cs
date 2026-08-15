namespace GESTOR_GASTOS.DTOs.Auth
{
	public record LoginRequest(
		string UserName,
		string Password
	);
}