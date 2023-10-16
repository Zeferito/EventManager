// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Core.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Core.Database.Services
{
    public class EventoService
    {
        private readonly DatabaseContext _context;

        public EventoService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<Evento>> GetEventosAsync()
        {
            return await _context.Eventos.ToListAsync();
        }

        public async Task<Evento> GetEventoByIdAsync(int id)
        {
            return await _context.Eventos.FindAsync(id);
        }

        public async Task<Evento?> GetEventoByIdEagerAsync(int id)
        {
            var evento = await _context.Eventos
                .Include(evento => evento.Usuario)
                .Include(evento => evento.Salas)
                .Include(evento => evento.EventoAgregables)
                .ThenInclude(ea => ea.Agregable)
                .Include(evento => evento.EventoEmpleados)
                .ThenInclude(ee => ee.Empleado)
                .Include(evento => evento.Clientes)
                .FirstOrDefaultAsync(evento => evento.Id == id);

            return evento;
        }

        /**
        public async Task<Evento> GetEventoByDateAsync(Datetime date)
        {
            return await _context.Eventos.FindAsync(id);
        }
        **/

        public async Task<Evento> CreateEventoAsync(Evento evento)
        {
            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();

            return evento;
        }

        public async Task<Evento> UpdateEventoAsync(int id, Evento evento)
        {
            _context.Entry(evento).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return evento;
        }

        public async Task<Evento> DeleteEventoAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();

            return evento;
        }

        public async Task<bool> EventoExistsAsync(int id)
        {
            return await _context.Eventos.AnyAsync(e => e.Id == id);
        }

        public async Task<EventoAgregable> EventoAddAgregableAsync(
            int eventoId,
            int agregableId,
            int cantidad
        )
        {
            var evento = _context.Eventos.FindAsync(eventoId).Result;
            var agregable = _context.Agregables.FindAsync(agregableId).Result;

            if (evento != null && agregable != null)
            {
                if (!evento.EventoAgregables.Any(ea => ea.AgregableId == agregableId))
                {
                    var eventoAgregable = new EventoAgregable
                    {
                        Evento = evento,
                        Agregable = agregable,
                        Cantidad = cantidad
                    };

                    await _context.EventoAgregables.AddAsync(eventoAgregable);
                    await _context.SaveChangesAsync();
                    return eventoAgregable;
                }
            }
            return null;
        }

        public async Task<EventoEmpleado> EventoAddEmpleadoAsync(int eventoId, int empleadoId)
        {
            var evento = _context.Eventos.FindAsync(eventoId).Result;
            var empleado = _context.Empleados.FindAsync(empleadoId).Result;

            if (evento != null && empleado != null)
            {
                if (!evento.EventoEmpleados.Any(ee => ee.EmpleadoId == empleadoId))
                {
                    var eventoEmpleado = new EventoEmpleado
                    {
                        Evento = evento,
                        Empleado = empleado
                    };

                    await _context.EventoEmpleados.AddAsync(eventoEmpleado);
                    await _context.SaveChangesAsync();
                    return eventoEmpleado;
                }
            }
            return null;
        }

        public async Task<Evento> EventoAddClienteAsync(int eventoId, int clienteId)
        {
            var evento = _context.Eventos.FindAsync(eventoId).Result;
            var cliente = _context.Clientes.FindAsync(clienteId).Result;

            if (evento != null && cliente != null)
            {
                if (!evento.Clientes.Any(c => c.Id == clienteId))
                {
                    evento.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                    return evento;
                }
            }
            return null;
        }

        public async Task<Evento> EventoAddSalaAsync(int eventoId, int salaId)
        {
            var evento = _context.Eventos.FindAsync(eventoId).Result;
            var sala = _context.Salas.FindAsync(salaId).Result;

            if (evento != null && sala != null)
            {
                if (!evento.Salas.Any(s => s.Id == salaId))
                {
                    evento.Salas.Add(sala);
                    await _context.SaveChangesAsync();
                    return evento;
                }
            }
            return null;
        }
    }
}
