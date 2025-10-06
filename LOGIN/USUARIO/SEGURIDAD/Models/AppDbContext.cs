using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    // Puedes agregar DbSet<Rol>, DbSet<Permiso>, DbSet<RolesPermisos> si los usas en el CRUD

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}