// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Core.Database.Services
{
    public class AgregableService
    {
        private readonly DatabaseContext _context;

        public AgregableService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Agregable>> GetAgregablesAsync()
        {
            return await _context.Agregables.ToListAsync();
        }

        public async Task<Agregable> GetAgregableByIdAsync(int id)
        {
            return await _context.Agregables.FindAsync(id);
        }

        public async Task<Agregable> CreateAgregableAsync(Agregable Agregable)
        {
            _context.Agregables.Add(Agregable);
            await _context.SaveChangesAsync();
            return Agregable;
        }

        public async Task<Agregable> UpdateAgregableAsync(int id, Agregable Agregable)
        {
            _context.Entry(Agregable).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Agregable;
        }

        public async Task<Agregable> DeleteAgregableAsync(int id)
        {
            var Agregable = await _context.Agregables.FindAsync(id);
            _context.Agregables.Remove(Agregable);
            await _context.SaveChangesAsync();
            return Agregable;
        }

        public async Task<bool> AgregableExistsAsync(int id)
        {
            return await _context.Agregables.AnyAsync(a => a.Id == id);
        }

        public async Task<EventoAgregable> AgregableAddEventoAsync(int eventoId, int agregableId)
        {
            var evento = _context.Eventos.FindAsync(eventoId).Result;
            var agregable = _context.Agregables.FindAsync(agregableId).Result;

            if (evento != null && agregable != null)
            {
                if (!evento.EventoAgregables.Any(ab => ab.AgregableId == agregableId))
                {
                    var eventoAgregable = new EventoAgregable
                    {
                        Evento = evento,
                        Agregable = agregable
                    };

                    await _context.EventoAgregables.AddAsync(eventoAgregable);
                    await _context.SaveChangesAsync();
                    return eventoAgregable;
                }
            }
            return null;
        }
    }
}
