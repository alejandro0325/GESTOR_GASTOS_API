using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GESTOR_GASTOS.Entities;
using GESTOR_GASTOS.Data;

namespace GESTOR_GASTOS.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TransactionsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public TransactionsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Transaction>>> Get()
		{
			var transactions = await _context.Transactions.ToListAsync();
			return Ok(transactions);
		}

		[HttpPost]
		public async Task<ActionResult<Transaction>> Create([FromBody] Transaction transaction)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Transactions.Add(transaction);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
		}
	}
}
