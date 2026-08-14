using GESTOR_GASTOS.Entities;
using Microsoft.EntityFrameworkCore;

namespace GESTOR_GASTOS.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}
