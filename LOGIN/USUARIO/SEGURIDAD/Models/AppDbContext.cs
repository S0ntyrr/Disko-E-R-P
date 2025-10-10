using Microsoft.EntityFrameworkCore;
using Empleados.Models;
using Proveedores.Models;
using Inventario.Models;
using Clientes.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace LOGIN.USUARIO.SEGURIDAD.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("TU_CONEXION_SQL");
        }
    }
}