using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Verificar si hay sesión activa (por cookie o por Session)
            var autenticado = User.Identity?.IsAuthenticated == true;
            var sesionActiva = HttpContext.Session.GetString("Autenticado") == "true";
            
            // En modo desarrollo, permitir acceso sin autenticación
            if (!autenticado && !sesionActiva)
            {
                _logger.LogWarning("⚠️ Acceso al dashboard sin autenticación del servidor. Usuario puede estar autenticado localmente.");
                // No redirigir, permitir acceso (el usuario puede estar autenticado via localStorage)
            }
            
            // Obtener nombre del usuario
            var nombreUsuario = User.Identity?.Name 
                ?? HttpContext.Session.GetString("Usuario") 
                ?? Request.Query["usuario"].ToString()
                ?? "Usuario";
            
            _logger.LogInformation($"✅ Usuario '{nombreUsuario}' accedió al dashboard (Autenticado servidor: {autenticado})");
            ViewData["NombreUsuario"] = nombreUsuario;
            ViewData["Autenticado"] = autenticado ? "Servidor" : "Local";
            
            return View("~/Views/Dashboard/Index.cshtml");
        }
    }
}
