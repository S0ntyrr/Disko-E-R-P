using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    // [Authorize] - Deshabilitado temporalmente para funcionar con localStorage
    public class ProveedorController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProveedorController> _logger;

        public ProveedorController(AppDbContext context, ILogger<ProveedorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var proveedores = _context.Proveedores.ToList();
                return View(proveedores);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de proveedores");
                TempData["Error"] = "No se pudo cargar la lista de proveedores. Sistema funcionando en modo local.";
                return View(new List<Proveedor>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var proveedor = _context.Proveedores.Find(id);
                if (proveedor == null) return NotFound();
                return View(proveedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del proveedor {id}");
                TempData["Error"] = "No se pudo cargar el proveedor.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Proveedores.Add(proveedor);
                    _context.SaveChanges();
                    TempData["Success"] = "Proveedor creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear proveedor");
                    TempData["Error"] = "No se pudo crear el proveedor. Sistema funcionando en modo local.";
                }
            }
            return View(proveedor);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var proveedor = _context.Proveedores.Find(id);
                if (proveedor == null) return NotFound();
                return View(proveedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar proveedor para editar {id}");
                TempData["Error"] = "No se pudo cargar el proveedor.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Proveedores.Update(proveedor);
                    _context.SaveChanges();
                    TempData["Success"] = "Proveedor actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar proveedor {proveedor.ProveedorId}");
                    TempData["Error"] = "No se pudo actualizar el proveedor.";
                }
            }
            return View(proveedor);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var proveedor = _context.Proveedores.Find(id);
                if (proveedor == null) return NotFound();
                return View(proveedor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar proveedor para eliminar {id}");
                TempData["Error"] = "No se pudo cargar el proveedor.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var proveedor = _context.Proveedores.Find(id);
                if (proveedor != null)
                {
                    _context.Proveedores.Remove(proveedor);
                    _context.SaveChanges();
                    TempData["Success"] = "Proveedor eliminado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar proveedor {id}");
                TempData["Error"] = "No se pudo eliminar el proveedor.";
            }
            return RedirectToAction("Index");
        }
    }
}

