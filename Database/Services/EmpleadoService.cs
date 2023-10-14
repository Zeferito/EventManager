// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.Services
{
    public class EmpleadoService
    {
        private readonly DatabaseContext _context;

        public EmpleadoService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Empleado>> GetEmpleadosAsync()
        {
            return await _context.Empleados.ToListAsync();
        }

        public async Task<Empleado> GetEmpleadoByIdAsync(int id)
        {
            return await _context.Empleados.FindAsync(id);
        }

        public async Task<Empleado> CreateEmpleadoAsync(Empleado Empleado)
        {
            _context.Empleados.Add(Empleado);
            await _context.SaveChangesAsync();
            return Empleado;
        }

        public async Task<Empleado> UpdateEmpleadoAsync(int id, Empleado Empleado)
        {
            _context.Entry(Empleado).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Empleado;
        }

        public async Task<Empleado> DeleteEmpleadoAsync(int id)
        {
            var Empleado = await _context.Empleados.FindAsync(id);
            _context.Empleados.Remove(Empleado);
            await _context.SaveChangesAsync();
            return Empleado;
        }

        public async Task<bool> AgregableExistsAsync(int id)
        {
            return await _context.Agregables.AnyAsync(e => e.Id == id);
        }
    }
}