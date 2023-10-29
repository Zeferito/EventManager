// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess.Repositories
{
    public class SalaRepository
    {
        private readonly DatabaseContext _context;

        public SalaRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Sala>> GetAllAsync()
        {
            return await _context.Salas.ToListAsync();
        }

        public List<Sala> GetAll()
        {
            return _context.Salas.ToList();
        }

        public async Task<Sala?> GetByIdAsync(int id)
        {
            return await _context.Salas.FindAsync(id);
        }

        public Sala? GetById(int id)
        {
            return _context.Salas.Find(id);
        }

        public async Task<Sala> CreateAsync(Sala sala)
        {
            _context.Salas.Add(sala);
            await _context.SaveChangesAsync();
            return sala;
        }

        public async Task<Sala> UpdateAsync(Sala sala)
        {
            _context.Entry(sala).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return sala;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sala = await _context.Salas.FindAsync(id);

            if (sala == null)
            {
                return false;
            }

            _context.Salas.Remove(sala);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Sala>> GetSalasWithEventosAsync()
        {
            return await _context.Salas.Include(s => s.Eventos).ToListAsync();
        }
    }
}
