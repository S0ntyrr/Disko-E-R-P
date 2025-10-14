using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LOGIN.USUARIO.SEGURIDAD.Models;
using Microsoft.EntityFrameworkCore;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    // [Authorize] - Deshabilitado temporalmente para funcionar con localStorage
    public class ClienteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(AppDbContext context, ILogger<ClienteController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var clientes = _context.Clientes.ToList();
                return View(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener lista de clientes");
                TempData["Error"] = "No se pudo cargar la lista de clientes. Sistema funcionando en modo local.";
                return View(new List<Cliente>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);
                // Si no se encuentra en BD, retorna la vista vacía (funcionará con localStorage)
                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener detalles del cliente {id}");
                // Aún así retorna la vista para que funcione con localStorage
                return View();
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Clientes.Add(cliente);
                    _context.SaveChanges();
                    TempData["Success"] = "Cliente creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear cliente");
                    TempData["Error"] = "No se pudo crear el cliente. Sistema funcionando en modo local.";
                }
            }
            return View(cliente);
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);
                // Si no se encuentra en BD, retorna la vista vacía (funcionará con localStorage)
                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar cliente para editar {id}");
                // Aún así retorna la vista para que funcione con localStorage
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Clientes.Update(cliente);
                    _context.SaveChanges();
                    TempData["Success"] = "Cliente actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al actualizar cliente {cliente.ClienteId}");
                    TempData["Error"] = "No se pudo actualizar el cliente.";
                }
            }
            return View(cliente);
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);
                if (cliente == null) return NotFound();
                return View(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar cliente para eliminar {id}");
                TempData["Error"] = "No se pudo cargar el cliente.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var cliente = _context.Clientes.Find(id);
                if (cliente != null)
                {
                    _context.Clientes.Remove(cliente);
                    _context.SaveChanges();
                    TempData["Success"] = "Cliente eliminado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar cliente {id}");
                TempData["Error"] = "No se pudo eliminar el cliente.";
            }
            return RedirectToAction("Index");
        }
    }
}

