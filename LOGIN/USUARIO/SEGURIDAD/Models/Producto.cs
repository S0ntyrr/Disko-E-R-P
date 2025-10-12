namespace LOGIN.USUARIO.SEGURIDAD.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public bool Activo { get; set; } = true;
    }
}

