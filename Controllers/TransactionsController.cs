using Microsoft.AspNetCore.Mvc;
using GESTOR_GASTOS.Entities;

namespace GESTOR_GASTOS.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
	// Lista estática en memoria para simular la base de datos
	private static List<Transaction> _transactions = new List<Transaction>
	{
		new Transaction { Id = 1, Monto = 150000, Tipo = "Ingreso", Descripcion = "Nómina", CategoriaId = 1 },
		new Transaction { Id = 2, Monto = 45000, Tipo = "Gasto", Descripcion = "Mercado", CategoriaId = 2 }
	};

	// GET: api/transactions
	[HttpGet]
	public ActionResult<IEnumerable<Transaction>> Get()
	{
		return Ok(_transactions);
	}

	// POST: api/transactions
	[HttpPost]
	public ActionResult<Transaction> Create([FromBody] Transaction transaction)
	{
		transaction.Id = _transactions.Count + 1;
		_transactions.Add(transaction);
		return CreatedAtAction(nameof(Get), new { id = transaction.Id }, transaction);
	}
}

}
