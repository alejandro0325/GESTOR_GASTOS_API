namespace GESTOR_GASTOS.Entities
{
	public class Transaction
	{
		public int Id { get; set; }
		public decimal Monto { get; set; }
		public string Tipo { get; set; } = "Gasto";
		public DateTime Fecha { get; set; } = DateTime.Now;
		public string Descripcion { get; set; } = string.Empty;
		public int CategoriaId { get; set; }
	}
}
