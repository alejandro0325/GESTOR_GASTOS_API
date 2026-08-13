namespace GESTOR_GASTOS.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Icono { get; set; } = "folder"; // Para usar íconos en el frontend
    }
}
