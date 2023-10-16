// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.
using EventManager.Core.Database;
using EventManager.Core.Database.Models;
using EventManager.Core.Database.Services;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Utilities;

namespace EventManager.CLI.Views
{
    public class EventoView
    {
        private struct AgregableCantidad
        {
            public Agregable agregable;
            public int cantidad;
        }

        public void AgregarEvento()
        {
            Evento evento = new Evento();
            //Con el login establecer el id del usuario para los eventos que se creen
            evento.UsuarioId = 1;

            List<AgregableCantidad> agregableCantidades = new List<AgregableCantidad>();
            List<Cliente> clientes = new List<Cliente>();
            List<Empleado> empleados = new List<Empleado>();
            List<Sala> salas = new List<Sala>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Asignar Fecha Evento");
                Console.WriteLine("2. Agregar agregables de Evento");
                Console.WriteLine("3. Agregar Cliente a Evento");
                Console.WriteLine("4. Agregar Empleados para Evento");
                Console.WriteLine("5. Agregar sala de Evento");
                Console.WriteLine("6. Confirmar Evento");
                Console.WriteLine("7. Cancelar el Evento");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        evento.Fecha = AsignarFecha();
                        break;
                    case "2":
                        agregableCantidades = AgregarAgregableAEvento();
                        break;
                    case "3":
                        clientes = AsignarClienteAEvento();
                        break;
                    case "4":
                        empleados = AgregarEmpleadoAEvento();
                        break;
                    case "5":
                        salas = AgregarSalasAEvento();
                        break;
                    case "6":
                        Confirmar(evento, agregableCantidades, empleados, clientes, salas);
                        return;
                    case "7":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static DateTime AsignarFecha()
        {
            //Establecer fecha del evento
            DateTime fecha;
            while (true)
            {
                Console.WriteLine("Ingrese la fecha del evento");
                if (DateTime.TryParse(Console.ReadLine(), out fecha))
                {
                    break;
                }
                Console.WriteLine("Fecha invalida");
            }

            return fecha;
        }
        private List<AgregableCantidad> AgregarAgregableAEvento()
        {
            List<AgregableCantidad> agregableCantidades = new List<AgregableCantidad>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar agregables");
                Console.WriteLine("2. Confirmar");
                Console.WriteLine("3. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        agregableCantidades.Add(BuscarAgregable());
                        break;
                    case "2":
                        return agregableCantidades;
                    case "3":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private AgregableCantidad BuscarAgregable()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                AgregableService agregableService = new AgregableService(context);
                int agregableId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del agregable");
                    if (int.TryParse(Console.ReadLine(), out agregableId))
                    {
                        if (!agregableService.AgregableExistsAsync(agregableId).Result)
                        {
                            Console.WriteLine("Agregable no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                Agregable agregable = agregableService.GetAgregableByIdAsync(agregableId).Result;

                int cantidad;
                while (true)
                {
                    Console.WriteLine("Ingrese la cantidad del agregable");
                    if (int.TryParse(Console.ReadLine(), out cantidad))
                    {
                        if (cantidad > agregable.Total || cantidad <= 0)
                        {
                            Console.WriteLine("Cantidad invalida");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("Cantidad invalida");
                }
                return new AgregableCantidad { agregable = agregable, cantidad = cantidad };
            }
        }

        public List<Cliente> AsignarClienteAEvento()
        {
            List<Cliente> clientes = new List<Cliente>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Buscar Cliente");
                Console.WriteLine("2. Registrar Cliente");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        clientes.Add(BuscarCliente());
                        break;
                    case "2":
                        clientes.Add(RegistrarCliente());
                        break;
                    case "3":
                        return clientes;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
            return clientes;
        }

        private Cliente BuscarCliente()
        {
            //Buscar cliente
            using (DatabaseContext context = new DatabaseContext())
            {
                ClienteService clienteService = new ClienteService(context);

                int clienteId;
                while (true)
                {
                    Console.WriteLine("Ingrese id del cliente");
                    if (int.TryParse(Console.ReadLine(), out clienteId))
                    {
                        if (!clienteService.ClienteExistsAsync(clienteId).Result)
                        {
                            Console.WriteLine("Cliente no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("id invalido");
                }

                return clienteService.GetClienteByIdAsync(clienteId).Result;
            }
        }

        private Cliente RegistrarCliente()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ClienteService clienteService = new ClienteService(context);

                Console.WriteLine("Ingrese nombre del cliente");
                string nombre = Console.ReadLine();

                Console.WriteLine("Ingrese telefono del cliente");
                string telefono = Console.ReadLine();

                return clienteService
                    .CreateClienteAsync(new Cliente { Nombre = nombre, Telefono = telefono })
                    .Result;
            }
        }

        public List<Empleado> AgregarEmpleadoAEvento()
        {
            List<Empleado> empleados = new List<Empleado>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar empleado");
                Console.WriteLine("2. Confirmar");
                Console.WriteLine("3. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        empleados.Add(AgregarEmpleado());
                        break;
                    case "2":
                        return empleados;
                    case "3":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Empleado AgregarEmpleado()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                EmpleadoService agregableService = new EmpleadoService(context);

                int empleadoId;
                while (true)
                {
                    Console.WriteLine("Ingrese Id del empleado");
                    if (int.TryParse(Console.ReadLine(), out empleadoId))
                    {
                        break;
                    }
                    Console.WriteLine("Id invalida");
                }

                return agregableService.GetEmpleadoByIdAsync(empleadoId).Result;
            }
        }

        public List<Sala> AgregarSalasAEvento()
        {
            List<Sala> salas = new List<Sala>();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Buscar Sala");
                Console.WriteLine("2. Confirmar");
                Console.WriteLine("3. Cancelar");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        salas.Add(AgregarSala());
                        break;
                    case "2":
                        return salas;
                    case "3":
                        Console.WriteLine("Volviendo al menu anterior");
                        return null;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private Sala AgregarSala()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                SalaService salaService = new SalaService(context);

                int salaId;
                while (true)
                {
                    Console.WriteLine("Ingrese id de la sala");
                    if (int.TryParse(Console.ReadLine(), out salaId))
                    {
                        if (!salaService.SalaExistsAsync(salaId).Result)
                        {
                            Console.WriteLine("Sala no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("id invalido");
                }

                return salaService.GetSalaByIdAsync(salaId).Result;
            }
        }

        private void Confirmar(
            Evento evento,
            List<AgregableCantidad> agregableCantidades,
            List<Empleado> empleados,
            List<Cliente> clientes,
            List<Sala> salas
        )
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                EventoService eventoService = new EventoService(context);

                Evento eventoCreado = eventoService.CreateEventoAsync(evento).Result;

                for (int i = 0; i < agregableCantidades.Count; i++)
                {
                    eventoService
                        .EventoAddAgregableAsync(
                            eventoCreado.Id,
                            agregableCantidades[i].agregable.Id,
                            agregableCantidades[i].cantidad
                        )
                        .Wait();
                }

                foreach (Empleado empleado in empleados)
                {
                    eventoService.EventoAddEmpleadoAsync(eventoCreado.Id, empleado.Id).Wait();
                }

                foreach (Cliente cliente in clientes)
                {
                    eventoService.EventoAddClienteAsync(eventoCreado.Id, cliente.Id).Wait();
                }

                foreach (Sala sala in salas)
                {
                    eventoService.EventoAddSalaAsync(eventoCreado.Id, sala.Id).Wait();
                }
            }
        }
    }
}
