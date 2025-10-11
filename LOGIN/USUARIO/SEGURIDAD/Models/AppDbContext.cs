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
        // ✅ Constructor requerido por AddDbContext en program.cs
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // ✅ Tablas del sistema
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        // ✅ Configuraciones opcionales (si necesitas reglas personalizadas)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ejemplo de configuraciones personalizadas (opcional)
            modelBuilder.Entity<Usuario>()
                .HasKey(u => u.UsuarioId);

            modelBuilder.Entity<Usuario>()
                .Property(u => u.NombreCompleto)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
