using Microsoft.AspNetCore.Mvc;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    public class InventarioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InventarioController> _logger;

        public InventarioController(AppDbContext context, ILogger<InventarioController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Inventario/Dashboard
        public IActionResult Dashboard()
        {
            return View();
        }

        // GET: Inventario/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: Inventario/Categorias
        public IActionResult Categorias()
        {
            return View();
        }

        // GET: Inventario/Movimientos
        public IActionResult Movimientos()
        {
            return View();
        }

        // GET: Inventario/Movimientos/Create
        public IActionResult MovimientosCreate()
        {
            return View();
        }
    }
}
