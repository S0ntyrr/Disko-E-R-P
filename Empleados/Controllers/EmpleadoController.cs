using Microsoft.AspNetCore.Mvc;
using Empleados.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace Empleados.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly AppDbContext _context;
        public EmpleadoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View(_context.Empleados.ToList());

        public IActionResult Details(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Add(empleado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Edit(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost]
        public IActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Empleados.Update(empleado);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empleado);
        }

        public IActionResult Delete(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado == null) return NotFound();
            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}