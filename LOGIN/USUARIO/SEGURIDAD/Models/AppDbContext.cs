using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    // ...otros DbSet si los usas...
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}