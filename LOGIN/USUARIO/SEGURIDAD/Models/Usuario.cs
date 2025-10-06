public class Usuario
{
    public int UsuarioId { get; set; }
    public string NombreCompleto { get; set; }
    public string Email { get; set; }
    public string NombreUsuario { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public int RolId { get; set; }
    public string Estado { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int? CreadoPor { get; set; }
}