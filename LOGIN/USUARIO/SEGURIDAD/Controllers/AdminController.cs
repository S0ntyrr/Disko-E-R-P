using Microsoft.AspNetCore.Mvc;
using SEGURIDAD.Models;

public class AdminController : Controller
{
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context) { _context = context; }

    public IActionResult Index()
    {
        var usuarios = _context.Usuarios.ToList();
        return View(usuarios);
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
        var usuario = _context.Usuarios.Find(id);
        return View(usuario);
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
        var usuario = _context.Usuarios.Find(id);
        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }
}