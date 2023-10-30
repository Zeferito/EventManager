// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Database.DataAccess.Repositories;
using EventManager.Database.Models.Entities;

namespace EventManager.Database.BusinessLogic.Services
{
    public partial class EventoService
    {
        private readonly EventoRepository _eventoRepository;

        public EventoService(EventoRepository eventoRepository)
        {
            _eventoRepository = eventoRepository;
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

        public Evento? Create(Evento evento)
        {
            // if (EventoOverlaps(evento))
            // {
            //     Console.WriteLine("Cannot create Evento because it overlaps with an existing Evento.");
            //     return null;
            // }

            // if (ReservedAgregablesExceedTotal(evento))
            // {
            //     Console.WriteLine("Cannot create Evento because reserved Agregables exceed the total available.");
            //     return null;
            // }

            return _eventoRepository.Create(evento);
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
            return _eventoRepository.EventoOverlaps(newEvento);
        }

        private bool ReservedAgregablesExceedTotal(Evento newEvento)
        {
            return _eventoRepository.ReservedAgregablesExceedTotal(newEvento);
        }
    }
}