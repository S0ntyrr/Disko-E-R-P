using DiskoERP.Core.DTOs;

namespace DiskoERP.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResultDto> IniciarSesion(LoginDto loginDto, string ipAddress, string userAgent);
        Task<ResponseDto> CerrarSesion(string token);
        Task<ResponseDto> RecuperarPassword(RecuperarPasswordDto model, string ipAddress);
        Task<ResponseDto> RestablecerPassword(RestablecerPasswordDto model);
    }
}
