// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class AgregableService
    {
        private readonly AgregableRepository _agregableRepository;

        public AgregableService(AgregableRepository agregableRepository)
        {
            _agregableRepository = agregableRepository;
        }

        public async Task<List<Agregable>> GetAllAsync()
        {
            return await _agregableRepository.GetAllAsync();
        }

         public List<Agregable> GetAll()
        {
            return _agregableRepository.GetAll();
        }

        public async Task<Agregable?> GetByIdAsync(int id)
        {
            return await _agregableRepository.GetByIdAsync(id);
        }

        public Agregable? GetById(int id)
        {
            return _agregableRepository.GetById(id);
        }

        public async Task<Agregable?> CreateAsync(Agregable agregable)
        {
            return await _agregableRepository.CreateAsync(agregable);
        }

        public async Task<Agregable?> UpdateAsync(Agregable agregable)
        {
            return await _agregableRepository.UpdateAsync(agregable);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _agregableRepository.DeleteAsync(id);
        }
    }
}