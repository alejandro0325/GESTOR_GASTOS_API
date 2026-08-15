using GESTOR_GASTOS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using GESTOR_GASTOS.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GESTOR_GASTOS.Services;

var builder = WebApplication.CreateBuilder(args);


// Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseMySql(
		connectionString,
		ServerVersion.AutoDetect(connectionString)
	)
);

// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngular", policy =>
	{
		policy
			.WithOrigins("http://localhost:4200")
			.AllowAnyMethod()
			.AllowAnyHeader();
	});
});

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity
builder.Services
	.AddIdentity<ApplicationUser, IdentityRole>(options =>
	{
		options.Password.RequireDigit = true;
		options.Password.RequireLowercase = true;
		options.Password.RequireUppercase = false;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequiredLength = 6;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>()
	.AddDefaultTokenProviders();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");

var secret = jwtSettings.GetValue<string>("Secret");

var key = Encoding.UTF8.GetBytes(secret);

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = "JwtBearer";
		options.DefaultChallengeScheme = "JwtBearer";
	})
	.AddJwtBearer("JwtBearer", options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,

			ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
			ValidAudience = jwtSettings.GetValue<string>("Audience"),

			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
// Authorization
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(
		"CanCreateTransaction",
		policy => policy.RequireRole("User", "Admin")
	);

	options.AddPolicy(
		"CanEditTransaction",
		policy => policy.RequireRole("Admin")
	);

	options.AddPolicy(
		"CanDeleteTransaction",
		policy => policy.RequireRole("Admin")
	);
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAngular");

// Authentication
app.UseAuthentication();

// Authorization
app.UseAuthorization();

// Controllers
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	try
	{
		var config = services.GetRequiredService<IConfiguration>();

		await SeedData.InitializeAsync(services, config);
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();

		logger.LogError(
			ex,
			"Error inicializando datos de seed."
		);
	}
}

app.Run();