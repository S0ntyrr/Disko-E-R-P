using Microsoft.AspNetCore.Mvc;
using Proveedores.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace Proveedores.Controllers
{
    public class ProveedorController : Controller
    {
        private readonly AppDbContext _context;

        public ProveedorController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var proveedores = _context.Proveedores.ToList();
            return View(proveedores);
        }

        public IActionResult Details(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Add(proveedor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        public IActionResult Edit(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost]
        public IActionResult Edit(Proveedor proveedor)
        {
            if (ModelState.IsValid)
            {
                _context.Proveedores.Update(proveedor);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedor);
        }

        public IActionResult Delete(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor == null) return NotFound();
            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var proveedor = _context.Proveedores.Find(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}