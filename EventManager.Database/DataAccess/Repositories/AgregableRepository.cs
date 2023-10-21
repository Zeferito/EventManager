// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess.Repositories
{
    public class AgregableRepository
    {
        private readonly DatabaseContext _context;

        public AgregableRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Agregable>> GetAllAsync()
        {
            return await _context.Agregables.ToListAsync();
        }

        public async Task<Agregable?> GetByIdAsync(int id)
        {
            return await _context.Agregables.FindAsync(id);
        }

        public async Task<Agregable> CreateAsync(Agregable agregable)
        {
            _context.Agregables.Add(agregable);
            await _context.SaveChangesAsync();
            return agregable;
        }

        public async Task<Agregable> UpdateAsync(Agregable agregable)
        {
            _context.Entry(agregable).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return agregable;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var agregable = await _context.Agregables.FindAsync(id);

            if (agregable == null)
            {
                return false;
            }

            _context.Agregables.Remove(agregable);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}