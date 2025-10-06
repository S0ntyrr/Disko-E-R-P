using System.ComponentModel.DataAnnotations;

namespace DiskoERP.Core.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Password { get; set; } = string.Empty;

        public bool RecordarSesion { get; set; } = false;
    }

    public class RecuperarPasswordDto
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; } = string.Empty;
    }

    public class RestablecerPasswordDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "La nueva contraseña es requerida")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres")]
        public string NuevaPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirma la contraseña")]
        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmarPassword { get; set; } = string.Empty;
    }

    public class ResponseDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public object? Data { get; set; }
    }

    public class LoginResultDto
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime FechaExpiracion { get; set; }
        public UsuarioDto Usuario { get; set; } = new();
    }

    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int RolId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public DateTime? UltimoAcceso { get; set; }
        public List<string> Permisos { get; set; } = new();
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
