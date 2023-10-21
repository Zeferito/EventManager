// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Database.DataAccess.Repositories
{
    public class EventoRepository
    {
        private readonly DatabaseContext _context;

        public EventoRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Evento>> GetAllAsync()
        {
            return await _context.Eventos.ToListAsync();
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await _context.Eventos.FindAsync(id);
        }

        public async Task<Evento> CreateAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<Evento> UpdateAsync(Evento evento)
        {
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return evento;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);

            if (evento == null)
            {
                return false;
            }

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Evento>> GetEventosWithRelatedDataAsync()
        {
            return await _context.Eventos
                .Include(e => e.Usuario)
                .Include(e => e.Clientes)
                .Include(e => e.Salas)
                .Include(e => e.Empleados)
                .Include(e => e.EventoAgregables)
                .ThenInclude(ea => ea.Agregable)
                .ToListAsync();
        }
    }
}