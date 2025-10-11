using DiskoERP.Core.DTOs;
using DiskoERP.Core.Services.Interfaces;
using LOGIN.USUARIO.SEGURIDAD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiskoERP.Core.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly AppDbContext _context;

        public AuthService(IConfiguration configuration, ILogger<AuthService> logger, AppDbContext context)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
        }

        public async Task<LoginResultDto> IniciarSesion(LoginDto loginDto, string ipAddress, string userAgent)
        {
            // Usuario demo
            if (loginDto.Usuario == "admin" && loginDto.Password == "123456")
            {
                var usuario = new UsuarioDto
                {
                    UsuarioId = 1,
                    NombreCompleto = "Administrador Demo",
                    NombreUsuario = "admin",
                    Email = "admin@diskoerp.com",
                    Rol = "Administrador",
                    RolId = 1,
                    Estado = "Activo",
                    UltimoAcceso = DateTime.Now,
                    Permisos = new List<string> { "ADMIN_FULL", "USERS_MANAGE", "REPORTS_VIEW" }
                };

                var token = GenerarToken(usuario);

                return new LoginResultDto
                {
                    Exitoso = true,
                    Mensaje = "Inicio de sesión exitoso",
                    Token = token,
                    FechaExpiracion = DateTime.Now.AddHours(8),
                    Usuario = usuario
                };
            }

            await Task.Delay(100);
            return new LoginResultDto
            {
                Exitoso = false,
                Mensaje = "Usuario o contraseña incorrectos"
            };
        }

        public async Task<ResponseDto> CerrarSesion(string token)
        {
            await Task.Delay(100);
            return new ResponseDto { Exitoso = true, Mensaje = "Sesión cerrada correctamente" };
        }

        public async Task<ResponseDto> RecuperarPassword(RecuperarPasswordDto model, string ipAddress)
        {
            await Task.Delay(100);
            return new ResponseDto
            {
                Exitoso = true,
                Mensaje = "Se ha enviado un email con las instrucciones para recuperar tu contraseña"
            };
        }

        public async Task<ResponseDto> RestablecerPassword(RestablecerPasswordDto model)
        {
            await Task.Delay(100);
            return new ResponseDto
            {
                Exitoso = true,
                Mensaje = "Contraseña restablecida exitosamente"
            };
        }

        public async Task<bool> RegisterUserAsync(LoginDto model)
        {
            if (string.IsNullOrWhiteSpace(model.Usuario) || string.IsNullOrWhiteSpace(model.Password))
                return false;

            var salt = Guid.NewGuid().ToString();
            var hash = BCrypt.Net.BCrypt.HashPassword(model.Password + salt); // ✅ Uso correcto

            var usuario = new Usuario
            {
                NombreCompleto = model.Usuario,
                Email = model.Usuario + "@diskoerp.com",
                NombreUsuario = model.Usuario,
                PasswordSalt = salt,
                PasswordHash = hash,
                Estado = "Activo",
                RolId = 2
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> ValidateUserAsync(LoginDto model)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == model.Usuario && u.Estado == "Activo");

            if (usuario != null && BCrypt.Net.BCrypt.Verify(model.Password + usuario.PasswordSalt, usuario.PasswordHash)) // ✅ Uso correcto
                return usuario;

            return null;
        }

        private string GenerarToken(UsuarioDto usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "DiskoERP_SuperSecretKey_2025_MustBeAtLeast32CharactersLong!@#$%");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(8),
                Issuer = jwtSettings["Issuer"] ?? "DiskoERP",
                Audience = jwtSettings["Audience"] ?? "DiskoERPUsers",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
