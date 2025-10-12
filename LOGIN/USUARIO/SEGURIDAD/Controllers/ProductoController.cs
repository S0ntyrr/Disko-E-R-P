using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    // [Authorize] - Deshabilitado temporalmente para funcionar con localStorage
    public class ProductoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(AppDbContext context, ILogger<ProductoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var productos = _context.Productos.ToList();
                return View(productos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de productos");
                TempData["Error"] = "No se pudo cargar la lista de productos. Sistema funcionando en modo local.";
                return View(new List<Producto>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto == null) return NotFound();
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del producto {id}");
                TempData["Error"] = "No se pudo cargar el producto.";
                return RedirectToAction("Index");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Productos.Add(producto);
                    _context.SaveChanges();
                    TempData["Success"] = "Producto creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear producto");
                    TempData["Error"] = "No se pudo crear el producto. Sistema funcionando en modo local.";
                }
            }
            return View(producto);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto == null) return NotFound();
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar producto para editar {id}");
                TempData["Error"] = "No se pudo cargar el producto.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Productos.Update(producto);
                    _context.SaveChanges();
                    TempData["Success"] = "Producto actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar producto {producto.ProductoId}");
                    TempData["Error"] = "No se pudo actualizar el producto.";
                }
            }
            return View(producto);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto == null) return NotFound();
                return View(producto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar producto para eliminar {id}");
                TempData["Error"] = "No se pudo cargar el producto.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var producto = _context.Productos.Find(id);
                if (producto != null)
                {
                    _context.Productos.Remove(producto);
                    _context.SaveChanges();
                    TempData["Success"] = "Producto eliminado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar producto {id}");
                TempData["Error"] = "No se pudo eliminar el producto.";
            }
            return RedirectToAction("Index");
        }
    }
}

