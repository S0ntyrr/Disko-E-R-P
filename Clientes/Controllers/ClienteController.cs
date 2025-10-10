using Microsoft.AspNetCore.Mvc;
using Clientes.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace Clientes.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;

        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var clientes = _context.Clientes.ToList();
            return View(clientes);
        }

        public IActionResult Details(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Clientes.Update(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cliente = _context.Clientes.Find(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}