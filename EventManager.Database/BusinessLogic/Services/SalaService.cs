// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class SalaService
    {
        private readonly DatabaseContext _context;

        private readonly SalaRepository _salaRepository;

        public SalaService(DatabaseContext context)
        {
            _context = context;
            _salaRepository = new SalaRepository(context);
        }

        public async Task<List<Sala>> GetAllAsync()
        {
            return await _salaRepository.GetAllAsync();
        }

        public async Task<Sala?> GetByIdAsync(int id)
        {
            return await _salaRepository.GetByIdAsync(id);
        }

        public async Task<Sala?> CreateAsync(Sala sala)
        {
            return await _salaRepository.CreateAsync(sala);
        }

        public async Task<Sala?> UpdateAsync(Sala sala)
        {
            return await _salaRepository.UpdateAsync(sala);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _salaRepository.DeleteAsync(id);
        }

        public async Task<List<Sala>> GetSalasWithEventosAsync()
        {
            return await _salaRepository.GetSalasWithEventosAsync();
        }
    }
}