using Empleados.Models;
using LOGIN.USUARIO.SEGURIDAD.Models;

namespace LOGIN.USUARIO.SEGURIDAD.Empleados.Services
{
    public class EmpleadoService
    {
        private readonly AppDbContext _context;
        public EmpleadoService(AppDbContext context)
        {
            _context = context;
        }

        public List<Empleado> GetAll() => _context.Empleados.ToList();
        public Empleado? GetById(int id) => _context.Empleados.Find(id);
        public void Add(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            _context.SaveChanges();
        }
        public void Update(Empleado empleado)
        {
            _context.Empleados.Update(empleado);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var empleado = _context.Empleados.Find(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                _context.SaveChanges();
            }
        }
    }
}