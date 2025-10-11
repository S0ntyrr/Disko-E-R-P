using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LOGIN.USUARIO.SEGURIDAD.Models;
using DiskoERP.Core.DTOs;
using DiskoERP.Core.Services.Interfaces;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _context;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, AppDbContext context)
        {
            _authService = authService;
            _logger = logger;
            _context = context;
        }

        // GET: /Auth/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Dashboard");

            ViewData["ReturnUrl"] = returnUrl;
            return View(); // ✅ Usa la vista relativa /Views/Auth/Login.cshtml
        }

        // POST: /Auth/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _authService.ValidateUserAsync(model);
            if (usuario != null)
            {
                // 🔹 Aquí podrías manejar la cookie o sesión según tu lógica
                TempData["Success"] = "Inicio de sesión exitoso.";
                return RedirectToAction("Index", "Dashboard");
            }

            ModelState.AddModelError("", "Credenciales incorrectas.");
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
                    await _authService.CerrarSesion(token);

                Response.Cookies.Delete("AuthToken");
                HttpContext.Session.Clear();

                _logger.LogInformation("Usuario cerró sesión exitosamente");
                TempData["Success"] = "Sesión cerrada correctamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cerrar sesión");
                TempData["Error"] = "Ocurrió un error al cerrar sesión.";
            }

            return RedirectToAction("Login");
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
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var ipAddress = ObtenerDireccionIP();
                var resultado = await _authService.RecuperarPassword(model, ipAddress);

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

        // ✅ Helper para obtener IP del cliente
        private string ObtenerDireccionIP()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                ipAddress = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            return ipAddress ?? "Desconocida";
        }

        // ✅ Verificar sesión activa
        [HttpGet]
        [Authorize]
        public IActionResult ValidarSesion()
        {
            return Json(new { exitoso = true, autenticado = User.Identity?.IsAuthenticated == true });
        }

        // ✅ Obtener usuario autenticado
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Auth/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterUserAsync(model);
            if (result)
            {
                TempData["Success"] = "Usuario registrado correctamente.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Error al registrar el usuario.");
            return View(model);
        }

        // ✅ Prueba de conexión a BD
        [HttpGet]
        public IActionResult TestDb()
        {
            var usuarios = _context.Usuarios.Take(1).ToList();
            return Content($"Usuarios encontrados: {usuarios.Count}");
        }
    }
}
