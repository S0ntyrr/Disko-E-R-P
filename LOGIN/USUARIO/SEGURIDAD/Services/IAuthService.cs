using DiskoERP.Core.DTOs;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace DiskoERP.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> IniciarSesion(LoginDto loginDto, string ipAddress, string userAgent);
        Task<ResponseDto> CerrarSesion(string token);
        Task<ResponseDto> RecuperarPassword(RecuperarPasswordDto model, string ipAddress);
        Task<ResponseDto> RestablecerPassword(RestablecerPasswordDto model);

        // Métodos adicionales implementados en AuthService
        Task<bool> RegisterUserAsync(LoginDto model);
        Task<Usuario?> ValidateUserAsync(LoginDto model);
    }
}
