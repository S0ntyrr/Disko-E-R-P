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
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /Auth/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var usuario = await _authService.ValidateUserAsync(model);
            if (usuario != null)
            {
                // Autenticación exitosa
                return RedirectToAction("Index", "Dashboard");
            }
            ModelState.AddModelError("", "Credenciales incorrectas");
            return View(model);
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
            return Json(new { exitoso = true, autenticado = User.Identity?.IsAuthenticated == true });
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

        // GET: /Auth/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        public async Task<IActionResult> Register(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                // Lógica para guardar el usuario en la base de datos
                var result = await _authService.RegisterUserAsync(model);
                if (result)
                    return RedirectToAction("Login");
                ModelState.AddModelError("", "Error al registrar usuario");
            }
            return View(model);
        }
    }
}
