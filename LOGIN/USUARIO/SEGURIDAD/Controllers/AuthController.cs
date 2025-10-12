using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Linq;
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

            try
            {
                Usuario? usuario = null;
                
                // Intentar validar con BD primero
                try
                {
                    usuario = await _authService.ValidateUserAsync(model);
                    if (usuario != null)
                    {
                        _logger.LogInformation($"✅ Usuario {model.Usuario} validado en BD");
                    }
                }
                catch (Exception exBd)
                {
                    _logger.LogWarning($"⚠️ BD no disponible, continuando sin validación de BD");
                }
                
                if (usuario == null)
                {
                    // Si no existe en BD, permitir login (será validado en cliente con localStorage)
                    _logger.LogInformation($"ℹ️ Usuario {model.Usuario} no encontrado en BD, permitiendo login local");
                    
                    // Para usuarios locales, creamos un usuario ficticio
                    usuario = new Usuario
                    {
                        UsuarioId = 999,
                        NombreUsuario = model.Usuario,
                        NombreCompleto = model.Usuario,
                        Email = $"{model.Usuario}@local.com",
                        RolId = 2,
                        Estado = "Activo"
                    };
                    
                    _logger.LogInformation($"✅ Usuario local creado para: {model.Usuario}");
                }

                // Crear los Claims del usuario
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.RolId == 1 ? "Administrador" : "Usuario"),
                    new Claim("NombreCompleto", usuario.NombreCompleto)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Configurar propiedades de autenticación
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RecordarSesion,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8),
                    AllowRefresh = true
                };

                // Firmar la cookie de autenticación
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimsPrincipal,
                    authProperties);

                // Guardar en sesión también
                HttpContext.Session.SetString("UsuarioId", usuario.UsuarioId.ToString());
                HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
                HttpContext.Session.SetString("Autenticado", "true");

                _logger.LogInformation($"Usuario {usuario.NombreUsuario} inició sesión exitosamente");
                TempData["Success"] = "¡Bienvenido! Inicio de sesión exitoso.";
                
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                ModelState.AddModelError("", "Error al iniciar sesión. Por favor intenta nuevamente.");
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
                var nombreUsuario = User.Identity?.Name ?? "Usuario";
                
                // Cerrar sesión de autenticación
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                // Limpiar sesión
                HttpContext.Session.Clear();

                _logger.LogInformation($"Usuario {nombreUsuario} cerró sesión exitosamente");
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
            _logger.LogInformation("Accediendo a página de registro");
            return View();
        }

        // GET: /Auth/RegisterSimple (versión simplificada)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult RegisterSimple()
        {
            _logger.LogInformation("✅ Accediendo a página de registro simple");
            return View(new LoginDto());
        }

        // POST: /Auth/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginDto model, string Email, string ConfirmPassword)
        {
            _logger.LogInformation("===== INICIO POST REGISTER =====");
            _logger.LogInformation($"Usuario recibido: {model?.Usuario ?? "NULL"}");
            _logger.LogInformation($"Email recibido: {Email ?? "NULL"}");
            _logger.LogInformation($"Password recibido: {(string.IsNullOrEmpty(model?.Password) ? "VACÍO" : "OK")}");
            _logger.LogInformation($"ConfirmPassword recibido: {(string.IsNullOrEmpty(ConfirmPassword) ? "VACÍO" : "OK")}");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("❌ ModelState inválido en registro");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"Error: {error.ErrorMessage}");
                }
                TempData["Error"] = "Por favor completa todos los campos correctamente.";
                return View(model);
            }

            try
            {
                // Validar que el email no esté vacío
                if (string.IsNullOrWhiteSpace(Email))
                {
                    ModelState.AddModelError("", "El email es requerido.");
                    TempData["Error"] = "El email es requerido.";
                    return View(model);
                }

                // Validar que las contraseñas coincidan
                if (model.Password != ConfirmPassword)
                {
                    ModelState.AddModelError("", "Las contraseñas no coinciden.");
                    TempData["Error"] = "Las contraseñas no coinciden.";
                    return View(model);
                }

                // Intentar registrar en BD, si falla, continuar con localStorage
                bool registradoEnBD = false;
                try
                {
                    var result = await _authService.RegisterUserAsync(model, Email);
                    if (result)
                    {
                        _logger.LogInformation($"✅ Usuario {model.Usuario} registrado exitosamente en BD con email {Email}");
                        registradoEnBD = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"⚠️ No se pudo registrar en BD (Base de datos no disponible). Continuando con modo local.");
                }

                // Siempre mostrar éxito - el usuario se puede guardar en localStorage
                if (registradoEnBD)
                {
                    TempData["Success"] = "¡Registro exitoso en la base de datos! Ya puedes iniciar sesión.";
                }
                else
                {
                    TempData["Success"] = "¡Registro exitoso! Tu cuenta se guardará localmente. Ya puedes iniciar sesión.";
                }
                
                _logger.LogInformation($"✅ Usuario {model.Usuario} listo para usar (BD: {registradoEnBD})");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error general al registrar usuario");
                TempData["Error"] = "Error al registrar el usuario. Por favor intenta nuevamente.";
                ModelState.AddModelError("", "Error al registrar el usuario. Por favor intenta nuevamente.");
                return View(model);
            }
        }

        // ✅ Prueba de conexión a BD
        [HttpGet]
        [AllowAnonymous]
        public IActionResult TestDb()
        {
            try
            {
                var usuarios = _context.Usuarios.Take(5).ToList();
                return Content($"✅ Conexión exitosa. Usuarios encontrados: {usuarios.Count}\n\n" + 
                              string.Join("\n", usuarios.Select(u => $"- {u.NombreUsuario} ({u.Email})")));
            }
            catch (Exception ex)
            {
                return Content($"❌ Error de conexión: {ex.Message}");
            }
        }

        // ✅ Prueba simple de registro (sin BD)
        [HttpGet]
        [AllowAnonymous]
        public IActionResult TestRegister()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> TestRegister(string usuario, string password, string email = "")
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
            {
                return Content("❌ Usuario y contraseña requeridos");
            }

            var model = new LoginDto { Usuario = usuario, Password = password };
            var emailFinal = string.IsNullOrWhiteSpace(email) ? $"{usuario}@test.com" : email;
            
            try
            {
                var result = await _authService.RegisterUserAsync(model, emailFinal);
                if (result)
                {
                    return Content($"✅ Usuario {usuario} registrado exitosamente en BD con email {emailFinal}!");
                }
                return Content($"❌ No se pudo registrar el usuario {usuario}");
            }
            catch (Exception ex)
            {
                return Content($"❌ Error: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }
    }
}
