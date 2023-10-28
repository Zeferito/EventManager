// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess.Repositories
{
    public class ClienteRepository
    {
        private readonly DatabaseContext _context;

        public ClienteRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
        public List<Cliente> GetAll()
        {
            return _context.Clientes.ToList();
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }
        public Cliente? GetById(int id)
        {
            return _context.Clientes.Find(id);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public Cliente? Create(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return false;
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}