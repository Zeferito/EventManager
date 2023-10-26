// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess.Repositories
{
    public class EmpleadoRepository
    {
        private readonly DatabaseContext _context;

        public EmpleadoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Empleado>> GetAllAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public List<Empleado> GetAll()
        {
            return _context.Empleados.ToList();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public Empleado? GetById(int id)
        {
            return _context.Empleados.Find(id);
        }

        public async Task<Empleado> CreateAsync(Empleado empleado)
        {
            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<Empleado> UpdateAsync(Empleado empleado)
        {
            _context.Entry(empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return empleado;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);

            if (empleado == null)
            {
                return false;
            }

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Empleado>> GetEmpleadosWithEventosAsync()
        {
            return await _context.Empleados.Include(e => e.Eventos).ToListAsync();
        }
    }
}
