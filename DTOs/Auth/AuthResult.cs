namespace GESTOR_GASTOS.DTOs.Auth
{
	public class AuthResult
	{
		public bool Succeeded { get; init; }

		public string? Token { get; init; }

		public IEnumerable<string> Errors { get; init; }
			= Enumerable.Empty<string>();

		public static AuthResult Success(string? token = null)
		{
			return new AuthResult
			{
				Succeeded = true,
				Token = token
			};
		}

		public static AuthResult Failure(
			IEnumerable<string> errors)
		{
			return new AuthResult
			{
				Succeeded = false,
				Errors = errors
			};
		}

		public static AuthResult Failure(
			string error)
		{
			return Failure(new[] { error });
		}
	}
}