// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class EmpleadoService
    {
        private readonly DatabaseContext _context;

        private readonly EmpleadoRepository _empleadoRepository;

        public EmpleadoService(DatabaseContext context)
        {
            _context = context;
            _empleadoRepository = new EmpleadoRepository(context);
        }

        public async Task<List<Empleado>> GetAllAsync()
        {
            return await _empleadoRepository.GetAllAsync();
        }

        public async Task<Empleado?> GetByIdAsync(int id)
        {
            return await _empleadoRepository.GetByIdAsync(id);
        }

        public async Task<Empleado?> CreateAsync(Empleado empleado)
        {
            return await _empleadoRepository.CreateAsync(empleado);
        }

        public async Task<Empleado?> UpdateAsync(Empleado empleado)
        {
            return await _empleadoRepository.UpdateAsync(empleado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _empleadoRepository.DeleteAsync(id);
        }

        public async Task<List<Empleado>> GetEmpleadosWithEventosAsync()
        {
            return await _empleadoRepository.GetEmpleadosWithEventosAsync();
        }
    }
}