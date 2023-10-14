// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.Services
{
    public class SalaService
    {
        private readonly DatabaseContext _context;

        public SalaService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Sala>> GetSalasAsync()
        {
            return await _context.Salas.ToListAsync();
        }

        public async Task<Sala> GetSalaByIdAsync(int id)
        {
            return await _context.Salas.FindAsync(id);
        }

        public async Task<Sala> CreateSalaAsync(Sala Sala)
        {
            _context.Salas.Add(Sala);
            await _context.SaveChangesAsync();
            return Sala;
        }

        public async Task<Sala> UpdateSalaAsync(int id, Sala Sala)
        {
            _context.Entry(Sala).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Sala;
        }

        public async Task<Sala> DeleteSalaAsync(int id)
        {
            var Sala = await _context.Salas.FindAsync(id);
            _context.Salas.Remove(Sala);
            await _context.SaveChangesAsync();
            return Sala;
        }

        public async Task<bool> SalaExistsAsync(int id)
        {
            return await _context.Salas.AnyAsync(s => s.Id == id);
        }
    }
}