namespace Empleados.Models
{
    public class Empleado
    {
        public int EmpleadoId { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public bool Activo { get; set; }
    }
}