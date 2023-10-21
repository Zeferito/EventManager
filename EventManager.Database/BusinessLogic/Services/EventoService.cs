// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess;
using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class EventoService
    {
        private readonly DatabaseContext _context;

        private readonly EventoRepository _eventoRepository;

        public EventoService(DatabaseContext context)
        {
            _context = context;
            _eventoRepository = new EventoRepository(context);
        }

        public async Task<List<Evento>> GetAllAsync()
        {
            return await _eventoRepository.GetAllAsync();
        }

        public async Task<Evento?> GetByIdAsync(int id)
        {
            return await _eventoRepository.GetByIdAsync(id);
        }

        public async Task<Evento?> CreateAsync(Evento evento)
        {
            if (EventoOverlaps(evento))
            {
                Console.WriteLine("Cannot create Evento because it overlaps with an existing Evento.");
                return null;
            }

            if (ReservedAgregablesExceedTotal(evento))
            {
                Console.WriteLine("Cannot create Evento because reserved Agregables exceed the total available.");
                return null;
            }

            return await _eventoRepository.CreateAsync(evento);
        }

        public async Task<Evento?> UpdateAsync(Evento evento)
        {
            if (EventoOverlaps(evento))
            {
                Console.WriteLine("Cannot update Evento because it overlaps with an existing Evento.");
                return null;
            }

            if (ReservedAgregablesExceedTotal(evento))
            {
                Console.WriteLine("Cannot update Evento because reserved Agregables exceed the total available.");
                return null;
            }

            return await _eventoRepository.UpdateAsync(evento);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _eventoRepository.DeleteAsync(id);
        }

        public async Task<List<Evento>> GetEventosWithRelatedDataAsync()
        {
            return await _eventoRepository.GetEventosWithRelatedDataAsync();
        }

        private bool EventoOverlaps(Evento newEvento)
        {
            //TODO: Corregir este metodo para que se puedan reservar a futuro; Debe checar si empalma con una sala que no este disponible para cuando se registre
            return _context.Eventos
                .Any(e => e.Id != newEvento.Id &&
                          e.FechaInicio < newEvento.FechaTermino &&
                          e.FechaTermino > newEvento.FechaInicio);
        }

        private bool ReservedAgregablesExceedTotal(Evento newEvento)
        {
            //TODO: Checar que se liberen o que haya agregables disponibles para el momento de registrar un evento y que no solo se riga por el total, sino tambien por periodos
            var totalReservedAgregables = _context.EventoAgregables
                .Where(ea => ea.EventoId != newEvento.Id)
                .GroupBy(ea => ea.AgregableId)
                .Select(group => new
                {
                    AgregableId = group.Key,
                    ReservedTotal = group.Sum(ea => ea.CantidadReservada)
                })
                .ToList();

            foreach (EventoAgregable ea in newEvento.EventoAgregables)
            {
                var matchingReserved = totalReservedAgregables.FirstOrDefault(e => e.AgregableId == ea.AgregableId);
                if (matchingReserved != null &&
                    ea.CantidadReservada + matchingReserved.ReservedTotal > ea.Agregable.Total)
                {
                    // Reserved Agregables exceed the total available for at least one Agregable.
                    return true;
                }
            }

            // No Agregable exceeds the total available.
            return false;
        }
    }
}