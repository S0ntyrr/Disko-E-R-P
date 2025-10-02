using DiskoERP.DTOs;

namespace DiskoERP.Services
{
    public interface IUsuarioService
    {
        Task<ResponseDto> CrearUsuario(CrearUsuarioDto dto, int usuarioCreadorId);
        Task<ResponseDto> ActualizarUsuario(ActualizarUsuarioDto dto, int usuarioModificadorId);
        Task<ResponseDto> ObtenerUsuarioPorId(int usuarioId);
        Task<PaginacionDto<UsuarioListaDto>> ObtenerUsuarios(FiltroUsuariosDto filtro);
        Task<ResponseDto> CambiarEstadoUsuario(int usuarioId, string nuevoEstado, int usuarioModificadorId);
        Task<ResponseDto> EliminarUsuario(int usuarioId);
    }
}

// Services/UsuarioService.cs
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using DiskoERP.Data;
using DiskoERP.DTOs;
using DiskoERP.Models;

namespace DiskoERP.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DiskoERPContext _context;

        public UsuarioService(DiskoERPContext context)
        {
            _context = context;
        }

        public async Task<ResponseDto> CrearUsuario(CrearUsuarioDto dto, int usuarioCreadorId)
        {
            try
            {
                // Validar que el rol existe
                var rolExiste = await _context.Roles.AnyAsync(r => r.RolId == dto.RolId && r.Estado == "Activo");
                if (!rolExiste)
                {
                    return new ResponseDto { Exitoso = false, Mensaje = "El rol seleccionado no es válido" };
                }

                // Generar salt y hash para la contraseña
                var salt = GenerarSalt();
                var hash = GenerarHash(dto.Password, salt);

                // Crear el nuevo usuario
                var nuevoUsuario = new Usuario
                {
                    NombreCompleto = dto.NombreCompleto,
                    Email = dto.Email,
                    NombreUsuario = dto.NombreUsuario,
                    PasswordHash = hash,
                    PasswordSalt = salt,
                    RolId = dto.RolId,
                    Estado = "Activo",
                    FechaCreacion = DateTime.Now,
                    CreadoPor = usuarioCreadorId
                };

                _context.Usuarios.Add(nuevoUsuario);
                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    Exitoso = true,
                    Mensaje = "Usuario creado exitosamente",
                    Data = new { UsuarioId = nuevoUsuario.UsuarioId }
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Exitoso = false, Mensaje = $"Error al crear usuario: {ex.Message}" };
            }
        }

        public async Task<ResponseDto> ActualizarUsuario(ActualizarUsuarioDto dto, int usuarioModificadorId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

                if (usuario == null)
                {
                    return new ResponseDto { Exitoso = false, Mensaje = "Usuario no encontrado" };
                }

                // Verificar si el email cambió y no está duplicado
                if (usuario.Email != dto.Email)
                {
                    var emailDuplicado = await _context.Usuarios
                        .AnyAsync(u => u.Email == dto.Email && u.UsuarioId != dto.UsuarioId);

                    if (emailDuplicado)
                    {
                        return new ResponseDto { Exitoso = false, Mensaje = "El email ya está en uso por otro usuario" };
                    }
                }

                // Actualizar campos
                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Email = dto.Email;
                usuario.RolId = dto.RolId;
                usuario.Estado = dto.Estado;
                usuario.FechaModificacion = DateTime.Now;
                usuario.ModificadoPor = usuarioModificadorId;

                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    Exitoso = true,
                    Mensaje = "Usuario actualizado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Exitoso = false, Mensaje = $"Error al actualizar usuario: {ex.Message}" };
            }
        }

        public async Task<ResponseDto> ObtenerUsuarioPorId(int usuarioId)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Rol)
                        .ThenInclude(r => r.Permisos)
                            .ThenInclude(rp => rp.Permiso)
                    .FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);

                if (usuario == null)
                {
                    return new ResponseDto { Exitoso = false, Mensaje = "Usuario no encontrado" };
                }

                var usuarioDto = new UsuarioDto
                {
                    UsuarioId = usuario.UsuarioId,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    NombreUsuario = usuario.NombreUsuario,
                    Rol = usuario.Rol.NombreRol,
                    RolId = usuario.RolId,
                    Estado = usuario.Estado,
                    UltimoAcceso = usuario.UltimoAcceso,
                    Permisos = usuario.Rol.Permisos.Select(rp => rp.Permiso.CodigoPermiso).ToList()
                };

                return new ResponseDto
                {
                    Exitoso = true,
                    Mensaje = "Usuario obtenido exitosamente",
                    Data = usuarioDto
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Exitoso = false, Mensaje = $"Error al obtener usuario: {ex.Message}" };
            }
        }

        public async Task<PaginacionDto<UsuarioListaDto>> ObtenerUsuarios(FiltroUsuariosDto filtro)
        {
            try
            {
                var query = _context.Usuarios
                    .Include(u => u.Rol)
                    .AsQueryable();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(filtro.Busqueda))
                {
                    query = query.Where(u =>
                        u.NombreCompleto.Contains(filtro.Busqueda) ||
                        u.Email.Contains(filtro.Busqueda) ||
                        u.NombreUsuario.Contains(filtro.Busqueda)
                    );
                }

                if (filtro.RolId.HasValue)
                {
                    query = query.Where(u => u.RolId == filtro.RolId.Value);
                }

                if (!string.IsNullOrEmpty(filtro.Estado))
                {
                    query = query.Where(u => u.Estado == filtro.Estado);
                }

                // Contar total de registros
                var totalRegistros = await query.CountAsync();

                // Aplicar paginación
                var usuarios = await query
                    .OrderByDescending(u => u.FechaCreacion)
                    .Skip((filtro.Pagina - 1) * filtro.RegistrosPorPagina)
                    .Take(filtro.RegistrosPorPagina)
                    .Select(u => new UsuarioListaDto
                    {
                        UsuarioId = u.UsuarioId,
                        NombreCompleto = u.NombreCompleto,
                        Email = u.Email,
                        NombreUsuario = u.NombreUsuario,
                        Rol = u.Rol.NombreRol,
                        Estado = u.Estado,
                        FechaCreacion = u.FechaCreacion,
                        UltimoAcceso = u.UltimoAcceso
                    })
                    .ToListAsync();

                var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)filtro.RegistrosPorPagina);

                return new PaginacionDto<UsuarioListaDto>
                {
                    Items = usuarios,
                    TotalRegistros = totalRegistros,
                    PaginaActual = filtro.Pagina,
                    TotalPaginas = totalPaginas,
                    RegistrosPorPagina = filtro.RegistrosPorPagina
                };
            }
            catch (Exception ex)
            {
                return new PaginacionDto<UsuarioListaDto>
                {
                    Items = new List<UsuarioListaDto>(),
                    TotalRegistros = 0,
                    PaginaActual = 1,
                    TotalPaginas = 0,
                    RegistrosPorPagina = filtro.RegistrosPorPagina
                };
            }
        }

        public async Task<ResponseDto> CambiarEstadoUsuario(int usuarioId, string nuevoEstado, int usuarioModificadorId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);

                if (usuario == null)
                {
                    return new ResponseDto { Exitoso = false, Mensaje = "Usuario no encontrado" };
                }

                usuario.Estado = nuevoEstado;
                usuario.FechaModificacion = DateTime.Now;
                usuario.ModificadoPor = usuarioModificadorId;

                // Si se inactiva, cerrar todas las sesiones activas
                if (nuevoEstado == "Inactivo" || nuevoEstado == "Bloqueado")
                {
                    var sesionesActivas = await _context.SesionesUsuario
                        .Where(s => s.UsuarioId == usuarioId && s.Estado == "Activa")
                        .ToListAsync();

                    foreach (var sesion in sesionesActivas)
                    {
                        sesion.Estado = "Cerrada";
                        sesion.FechaCierre = DateTime.Now;
                    }
                }

                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    Exitoso = true,
                    Mensaje = $"Usuario {(nuevoEstado == "Activo" ? "activado" : "desactivado")} exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Exitoso = false, Mensaje = $"Error al cambiar estado: {ex.Message}" };
            }
        }

        public async Task<ResponseDto> EliminarUsuario(int usuarioId)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(usuarioId);

                if (usuario == null)
                {
                    return new ResponseDto { Exitoso = false, Mensaje = "Usuario no encontrado" };
                }

                // Soft delete - solo cambiar estado
                usuario.Estado = "Inactivo";
                usuario.FechaModificacion = DateTime.Now;

                await _context.SaveChangesAsync();

                return new ResponseDto
                {
                    Exitoso = true,
                    Mensaje = "Usuario eliminado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ResponseDto { Exitoso = false, Mensaje = $"Error al eliminar usuario: {ex.Message}" };
            }
        }

        // Métodos auxiliares privados
        private string GenerarSalt()
        {
            var bytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return Convert.ToBase64String(bytes);
        }

        private string GenerarHash(string password, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            
            var combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);
            
            using (var sha256 = SHA256.Create())
            {
                var hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
} Validar que el email no exista
                var emailExiste = await _context.Usuarios
                    .AnyAsync(u => u.Email == dto.Email);

                if (emailExiste)
                {
                    return new ResponseDto 
                    { 
                        Exitoso = false, 
                        Mensaje = "Ya existe un usuario con ese email" 
                    };
                }

                // Validar que el nombre de usuario no exista
                var usuarioExiste = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == dto.NombreUsuario);

                if (usuarioExiste)
                {
                    return new ResponseDto 
                    { 
                        Exitoso = false, 
                        Mensaje = "El nombre de usuario ya está en uso" 
                    };
                }

                //