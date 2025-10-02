using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DiskoERP.Core.DTOs;
using DiskoERP.Core.Services.Interfaces;
using System.Security.Claims;

namespace DiskoERP.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        // GET: /Auth/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            // Si el usuario ya está autenticado, redirigir al dashboard
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = null)
        {
            try
            {
                // Validar el modelo
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // Obtener información del cliente
                var ipAddress = ObtenerDireccionIP();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Intentar iniciar sesión
                var resultado = await _authService.IniciarSesion(model, ipAddress, userAgent);

                if (!resultado.Exitoso)
                {
                    // Si el login falló, mostrar mensaje de error
                    TempData["Error"] = resultado.Mensaje;
                    ModelState.AddModelError(string.Empty, resultado.Mensaje);
                    
                    _logger.LogWarning($"Intento de login fallido para usuario: {model.Usuario} desde IP: {ipAddress}");
                    
                    return View(model);
                }

                // Login exitoso - guardar token en cookie
                Response.Cookies.Append("AuthToken", resultado.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = resultado.FechaExpiracion
                });

                // Guardar información del usuario en sesión
                HttpContext.Session.SetString("UsuarioId", resultado.Usuario.UsuarioId.ToString());
                HttpContext.Session.SetString("NombreUsuario", resultado.Usuario.NombreCompleto);
                HttpContext.Session.SetString("Rol", resultado.Usuario.Rol);

                _logger.LogInformation($"Usuario {resultado.Usuario.NombreUsuario} inició sesión exitosamente desde IP: {ipAddress}");

                TempData["Success"] = $"¡Bienvenido, {resultado.Usuario.NombreCompleto}!";

                // Redirigir a la URL de retorno o al dashboard
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado durante el login");
                TempData["Error"] = "Ocurrió un error inesperado. Por favor, intenta nuevamente.";
                return View(model);
            }
        }

        // GET: /Auth/Logout
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Cookies["AuthToken"];

                if (!string.IsNullOrEmpty(token))
                {
                    await _authService.CerrarSesion(token);
                }

                // Limpiar cookies y sesión
                Response.Cookies.Delete("AuthToken");
                HttpContext.Session.Clear();

                _logger.LogInformation("Usuario cerró sesión exitosamente");

                TempData["Success"] = "Sesión cerrada correctamente.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión");
                return RedirectToAction("Login");
            }
        }

        // GET: /Auth/RecuperarPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RecuperarPassword()
        {
            return View();
        }

        // POST: /Auth/RecuperarPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RecuperarPassword(RecuperarPasswordDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var ipAddress = ObtenerDireccionIP();
                var resultado = await _authService.RecuperarPassword(model, ipAddress);

                // Siempre mostramos el mismo mensaje por seguridad
                TempData["Success"] = resultado.Mensaje;
                
                _logger.LogInformation($"Solicitud de recuperación de contraseña para: {model.Email} desde IP: {ipAddress}");

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar recuperación de contraseña");
                TempData["Error"] = "Ocurrió un error. Por favor, intenta nuevamente.";
                return View(model);
            }
        }

        // GET: /Auth/RestablecerPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RestablecerPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                TempData["Error"] = "Token inválido.";
                return RedirectToAction("Login");
            }

            var model = new RestablecerPasswordDto { Token = token };
            return View(model);
        }

        // POST: /Auth/RestablecerPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestablecerPassword(RestablecerPasswordDto model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var resultado = await _authService.RestablecerPassword(model);

                if (!resultado.Exitoso)
                {
                    TempData["Error"] = resultado.Mensaje;
                    return View(model);
                }

                _logger.LogInformation("Contraseña restablecida exitosamente");

                TempData["Success"] = "Contraseña restablecida exitosamente. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al restablecer contraseña");
                TempData["Error"] = "Ocurrió un error. Por favor, intenta nuevamente.";
                return View(model);
            }
        }

        // GET: /Auth/AccesoDenegado
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccesoDenegado()
        {
            return View();
        }

        // Método auxiliar para obtener la dirección IP del cliente
        private string ObtenerDireccionIP()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            
            // Si está detrás de un proxy, obtener la IP real
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }

            return ipAddress ?? "Desconocida";
        }

        // API: Validar si el usuario está autenticado
        [HttpGet]
        [Authorize]
        public IActionResult ValidarSesion()
        {
            return Json(new { exitoso = true, autenticado = User.Identity.IsAuthenticated });
        }

        // API: Obtener información del usuario actual
        [HttpGet]
        [Authorize]
        public IActionResult ObtenerUsuarioActual()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nombreUsuario = User.FindFirstValue(ClaimTypes.Name);
            var rol = User.FindFirstValue(ClaimTypes.Role);

            return Json(new 
            { 
                usuarioId, 
                nombreUsuario, 
                rol,
                autenticado = true 
            });
        }
    }
}