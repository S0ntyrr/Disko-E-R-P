using Microsoft.AspNetCore.Mvc;
using Inventario.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace Inventario.Controllers
{
    public class ProductoController : Controller
    {
        private readonly AppDbContext _context;

        public ProductoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var productos = _context.Productos.ToList();
            return View(productos);
        }

        public IActionResult Details(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public IActionResult Edit(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Update(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        public IActionResult Delete(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}