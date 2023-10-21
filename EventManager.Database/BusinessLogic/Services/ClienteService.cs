// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class ClienteService
    {
        private readonly DatabaseContext _context;

        private readonly ClienteRepository _clienteRepository;

        public ClienteService(DatabaseContext context)
        {
            _context = context;
            _clienteRepository = new ClienteRepository(context);
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<Cliente?> CreateAsync(Cliente cliente)
        {
            return await _clienteRepository.CreateAsync(cliente);
        }

        public async Task<Cliente?> UpdateAsync(Cliente cliente)
        {
            return await _clienteRepository.UpdateAsync(cliente);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _clienteRepository.DeleteAsync(id);
        }
    }
}