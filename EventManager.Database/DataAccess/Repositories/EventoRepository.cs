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


        public bool EventoOverlaps(Evento newEvento)
        {
            return _context.Eventos
                .Any(evento =>
                    // Don't let it check itself
                    evento.Id != newEvento.Id &&
                    // Check if period overlaps
                    evento.FechaInicio < newEvento.FechaTermino &&
                    evento.FechaTermino > newEvento.FechaInicio &&
                    // Check if a Sala is already reserved for another Evento that will occur during the same period
                    evento.Salas.Any(sala =>
                        newEvento.Salas.Any(newEventoSala => newEventoSala.Id == sala.Id)));
        }

        public bool ReservedAgregablesExceedTotal(Evento newEvento)
        {
            // Awgh.
            // This check should ONLY apply to Eventos that overlap with the new Evento, since otherwise all
            // Agregables should be available either way
            List<int> overlappingEventoIds = _context.Eventos
                .Where(e =>
                    e.Id != newEvento.Id &&
                    e.FechaInicio < newEvento.FechaTermino &&
                    e.FechaTermino > newEvento.FechaInicio)
                .Select(e => e.Id)
                .ToList();

            var totalReservedAgregables =
                _context.EventoAgregables
                    .Where(ea => overlappingEventoIds.Contains(ea.EventoId))
                    .GroupBy(ea => ea.AgregableId)
                    .Select(group => new
                    {
                        AgregableId = group.Key,
                        ReservedTotal = group.Sum(ea => ea.CantidadReservada)
                    })
                    .ToList();

            foreach (EventoAgregable eventoAgregable in newEvento.EventoAgregables)
            {
                var matchingReserved =
                    totalReservedAgregables.FirstOrDefault(e =>
                        e.AgregableId == eventoAgregable.AgregableId);

                if (matchingReserved != null)
                {
                    int reservedSum = eventoAgregable.CantidadReservada + matchingReserved.ReservedTotal;

                    if (reservedSum > eventoAgregable.Agregable.Total)
                    {
                        // Reserved Agregables exceed the total available for at least one Agregable.
                        return true;
                    }
                }
            }

            return false;
        }
    }
}