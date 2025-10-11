using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    // Temporalmente sin restricción de login para ver el dashboard
    [AllowAnonymous]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(ILogger<DashboardController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Acceso al dashboard");
            return View("~/Views/Dashboard/Index.cshtml");
        }
    }
}
