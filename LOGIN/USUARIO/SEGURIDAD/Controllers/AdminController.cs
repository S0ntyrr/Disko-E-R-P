using Microsoft.AspNetCore.Mvc;
using LOGIN.USUARIO.SEGURIDAD.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace LOGIN.USUARIO.SEGURIDAD.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AdminController> _logger;
        
        public AdminController(AppDbContext context, ILogger<AdminController> logger) 
        { 
            _context = context;
            _logger = logger;
        }

        // Panel principal de administración
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Index()
        {
            try
            {
                var usuarios = _context.Usuarios.ToList();
                return View(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuarios");
                return View(new List<Usuario>());
            }
        }

    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(Usuario usuario)
    {
        usuario.FechaCreacion = DateTime.Now;
        usuario.Estado = "Activo";
        usuario.PasswordSalt = Guid.NewGuid().ToString();
        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash + usuario.PasswordSalt);
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        try
        {
            var usuario = _context.Usuarios.Find(id);
            // Si no se encuentra en BD, retorna la vista vacía (funcionará con localStorage)
            return View(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al cargar usuario para editar {id}");
            // Aún así retorna la vista para que funcione con localStorage
            return View();
        }
    }

    [HttpPost]
    public IActionResult Edit(Usuario usuario)
    {
        var original = _context.Usuarios.Find(usuario.UsuarioId);
        if (original != null)
        {
            original.NombreCompleto = usuario.NombreCompleto;
            original.Email = usuario.Email;
            original.NombreUsuario = usuario.NombreUsuario;
            original.RolId = usuario.RolId;
            original.Estado = usuario.Estado;
            if (!string.IsNullOrEmpty(usuario.PasswordHash))
            {
                original.PasswordSalt = Guid.NewGuid().ToString();
                original.PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuario.PasswordHash + original.PasswordSalt);
            }
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        try
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario");
        }
        return RedirectToAction("Index");
    }
    }
}