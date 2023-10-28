// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class ClienteService
    {
        private readonly ClienteRepository _clienteRepository;

        public ClienteService(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }
        public List<Cliente> GetAll()
        {
            return _clienteRepository.GetAll();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }
        public Cliente? GetById(int id)
        {
            return _clienteRepository.GetById(id);
        }

        public async Task<Cliente?> CreateAsync(Cliente cliente)
        {
            return await _clienteRepository.CreateAsync(cliente);
        }

        public Cliente? Create(Cliente cliente)
        {
            return _clienteRepository.Create(cliente);
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