// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Core.Database;
using EventManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.CLI.Utils
{
    public class BibliotecaModelUtils
    {
        public static Evento? GetEventoById(bool eager)
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries?
            // TODO: Create cancellable loop for validation
            int eventoId = UserInputReader.ReadInt("Ingrese Id del Evento: ");

            Evento? evento;

            if (eager)
            {
                evento = context.Eventos
                    .Include(evt => evt.Usuario)
                    .Include(evt => evt.Salas)
                    .Include(evt => evt.EventoAgregables)
                    .ThenInclude(ea => ea.Agregable)
                    .Include(evt => evt.EventoEmpleados)
                    .ThenInclude(ee => ee.Empleado)
                    .Include(evt => evt.Clientes)
                    .FirstOrDefault(evt => evt.Id == eventoId);
            }
            else
            {
                evento = context.Eventos.Find(eventoId);
            }

            if (evento != null)
            {
                return evento;
            }

            Console.WriteLine("Evento no existe");
            return null;
        }

        public static void AddClienteToList(ICollection<Cliente> clientes)
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int clienteId = UserInputReader.ReadInt("Ingrese Id del Cliente: ");

            Cliente? cliente = context.Clientes.Find(clienteId);

            if (cliente == null)
            {
                Console.WriteLine("Cliente no existe");
                return;
            }

            clientes.Add(cliente);
        }

        public static void AddSalaToList(ICollection<Sala> salas)
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int salaId = UserInputReader.ReadInt("Ingrese Id de la Sala: ");

            Sala? sala = context.Salas.Find(salaId);

            if (sala == null)
            {
                Console.WriteLine("Sala no existe");
                return;
            }

            salas.Add(sala);
        }

        public static void AddEmpleadoToList(ICollection<Empleado> empleados)
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int empleadoId = UserInputReader.ReadInt("Ingrese Id del Empleado: ");

            Empleado? empleado = context.Empleados.Find(empleadoId);

            if (empleado == null)
            {
                Console.WriteLine("Empleado no existe");
                return;
            }

            empleados.Add(empleado);
        }

        public static void AddAgregableCantidadToList(ICollection<AgregableCantidad> agregableCantidades)
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int agregableId = UserInputReader.ReadInt("Ingrese Id del Agregable: ");

            Agregable? agregable = context.Agregables.Find(agregableId);

            if (agregable == null)
            {
                Console.WriteLine("Agregable no existe");
                return;
            }

            // TODO: Create cancellable loop for validation
            int cantidad = UserInputReader.ReadInt("Ingrese la cantidad del Agregable: ");

            if (cantidad > agregable.Total || cantidad <= 0)
            {
                Console.WriteLine("Cantidad invalida");
                return;
            }

            AgregableCantidad agregableCantidad = new AgregableCantidad
            {
                Agregable = agregable,
                Cantidad = cantidad
            };

            agregableCantidades.Add(agregableCantidad);
        }

        public static void RemoveClienteFromList(IList<Cliente> clientes)
        {
            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int clienteIndex = UserInputReader.ReadInt("Ingrese indice del Cliente: ");

            if (clienteIndex < 0 || clienteIndex >= clientes.Count)
            {
                Console.WriteLine("Indice invalido");
                return;
            }

            clientes.Remove(clientes[clienteIndex]);
        }

        public static void RemoveSalaFromList(IList<Sala> salas)
        {
            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int salaIndex = UserInputReader.ReadInt("Ingrese indice de la Sala: ");

            if (salaIndex < 0 || salaIndex >= salas.Count)
            {
                Console.WriteLine("Indice invalido");
                return;
            }

            salas.Remove(salas[salaIndex]);
        }

        public static void RemoveEmpleadoFromList(IList<Empleado> empleados)
        {
            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int empleadoIndex = UserInputReader.ReadInt("Ingrese indice del Empleado: ");

            if (empleadoIndex < 0 || empleadoIndex >= empleados.Count)
            {
                Console.WriteLine("Indice invalido");
                return;
            }

            empleados.Remove(empleados[empleadoIndex]);
        }

        public static void RemoveAgregableCantidadFromList(IList<AgregableCantidad> agregableCantidades)
        {
            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            int agregableIndex = UserInputReader.ReadInt("Ingrese indice del Agregable: ");

            if (agregableIndex < 0 || agregableIndex >= agregableCantidades.Count)
            {
                Console.WriteLine("Indice invalido");
                return;
            }

            int cantidadARemover = UserInputReader.ReadInt("Cantidad a remover: ");

            if (cantidadARemover >= agregableCantidades[agregableIndex].Cantidad)
            {
                agregableCantidades.Remove(agregableCantidades[agregableIndex]);
                return;
            }

            agregableCantidades[agregableIndex].Cantidad -= cantidadARemover;
        }
    }
}