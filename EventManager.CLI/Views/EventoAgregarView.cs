// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Utils;
using EventManager.Core.Database;
using EventManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.CLI.Views
{
    public class EventoAgregarView
    {
        public static void ShowMenuAgregarEvento()
        {
            string? nombre = null;
            string? descripcion = null;
            DateTime fechaInicio = new DateTime();
            DateTime fechaTermino = new DateTime();

            List<Cliente> clientes = new List<Cliente>();
            List<Sala> salas = new List<Sala>();
            List<Empleado> empleados = new List<Empleado>();
            List<AgregableCantidad> agregableCantidades = new List<AgregableCantidad>();

            while (true)
            {
                Console.WriteLine("\nAgregar Evento Menu —");
                Console.WriteLine("1. Asignar Nombre");
                Console.WriteLine("2. Asignar Descripcion");
                Console.WriteLine("3. Asignar Periodo");
                Console.WriteLine("4. Asignar Cliente");
                Console.WriteLine("5. Asignar Salas");
                Console.WriteLine("6. Asignar Empleados");
                Console.WriteLine("7. Asignar Agregables");
                Console.WriteLine("8. Confirmar");
                Console.WriteLine("9. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        nombre = UserInputReader.ReadString("Ingrese el nombre del Evento: ");
                        break;
                    case "2":
                        descripcion = UserInputReader.ReadString("Ingrese la descripcion del Evento: ");
                        break;
                    case "3":
                        // TODO: Add period validation
                        fechaInicio = UserInputReader.ReadDateTime("Ingrese la fecha y hora de inicio del Evento: ");
                        fechaTermino = UserInputReader.ReadDateTime("Ingrese la fecha y hora de termino del Evento: ");
                        break;
                    case "4":
                        ShowSubMenuClientes(clientes);
                        break;
                    case "5":
                        ShowSubMenuSalas(salas);
                        break;
                    case "6":
                        ShowSubMenuEmpleados(empleados);
                        break;
                    case "7":
                        ShowSubMenuAgregableCantidades(agregableCantidades);
                        break;
                    case "8":
                        if (SaveEvento(nombre, descripcion, fechaInicio, fechaTermino, clientes, salas, empleados,
                                agregableCantidades))
                        {
                            return;
                        }

                        break;
                    case "9":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void ShowSubMenuClientes(List<Cliente> clientes)
        {
            List<Cliente> newClientesState = new List<Cliente>();
            newClientesState.AddRange(clientes);

            using DatabaseContext context = new DatabaseContext();

            while (true)
            {
                Console.WriteLine("\nClientes Submenu —");
                Console.WriteLine("1. Agregar");
                Console.WriteLine("2. Registrar");
                Console.WriteLine("3. Remover");
                Console.WriteLine("4. Confirmar");
                Console.WriteLine("5. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BibliotecaModelUtils.AddClienteToList(newClientesState);
                        break;
                    case "2":
                        string nombre = UserInputReader.ReadString("Ingrese nombre del cliente: ");
                        string telefono = UserInputReader.ReadString("Ingrese telefono del cliente: ");

                        Cliente cliente = new Cliente { Nombre = nombre, Telefono = telefono };

                        Cliente insertedCliente = context.Clientes.Add(cliente).Entity;

                        if (insertedCliente == null)
                        {
                            Console.WriteLine("No se ha podido guardar el Cliente");
                            break;
                        }

                        newClientesState.Add(insertedCliente);
                        break;
                    case "3":
                        BibliotecaModelUtils.RemoveClienteFromList(newClientesState);
                        break;
                    case "4":
                        clientes.Clear();
                        clientes.AddRange(newClientesState);
                        return;
                    case "5":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void ShowSubMenuSalas(List<Sala> salas)
        {
            List<Sala> newSalasState = new List<Sala>();
            newSalasState.AddRange(salas);

            while (true)
            {
                Console.WriteLine("\nSalas Submenu —");
                Console.WriteLine("1. Agregar");
                Console.WriteLine("2. Remover");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BibliotecaModelUtils.AddSalaToList(newSalasState);
                        break;
                    case "2":
                        BibliotecaModelUtils.RemoveSalaFromList(newSalasState);
                        break;
                    case "3":
                        salas.Clear();
                        salas.AddRange(newSalasState);
                        return;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void ShowSubMenuEmpleados(List<Empleado> empleados)
        {
            List<Empleado> newEmpleadosState = new List<Empleado>();
            newEmpleadosState.AddRange(empleados);

            while (true)
            {
                Console.WriteLine("\nEmpleados Submenu —");
                Console.WriteLine("1. Agregar");
                Console.WriteLine("2. Remover");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BibliotecaModelUtils.AddEmpleadoToList(newEmpleadosState);
                        break;
                    case "2":
                        BibliotecaModelUtils.RemoveEmpleadoFromList(newEmpleadosState);
                        break;
                    case "3":
                        empleados.Clear();
                        empleados.AddRange(newEmpleadosState);
                        return;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void ShowSubMenuAgregableCantidades(List<AgregableCantidad> agregableCantidades)
        {
            List<AgregableCantidad> newAgregableCantidadesState = new List<AgregableCantidad>();
            newAgregableCantidadesState.AddRange(agregableCantidades);

            while (true)
            {
                Console.WriteLine("\nAgregables Submenu —");
                Console.WriteLine("1. Agregar");
                Console.WriteLine("2. Remover");
                Console.WriteLine("3. Confirmar");
                Console.WriteLine("4. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        BibliotecaModelUtils.AddAgregableCantidadToList(newAgregableCantidadesState);
                        break;
                    case "2":
                        BibliotecaModelUtils.RemoveAgregableCantidadFromList(newAgregableCantidadesState);
                        break;
                    case "3":
                        agregableCantidades.Clear();
                        agregableCantidades.AddRange(newAgregableCantidadesState);
                        return;
                    case "4":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static bool SaveEvento(
            string? nombre,
            string? descripcion,
            DateTime fechaInicio,
            DateTime fechaTermino,
            List<Cliente> clientes,
            List<Sala> salas,
            List<Empleado> empleados,
            List<AgregableCantidad> agregableCantidades
        )
        {
            if (nombre == null || descripcion == null)
            {
                return false;
            }

            using DatabaseContext context = new DatabaseContext();

            Usuario? usuario = Program.GetUser();

            if (usuario == null)
            {
                return false;
            }

            Evento evento = new Evento
            {
                Nombre = nombre,
                Descripcion = descripcion,
                FechaInicio = fechaInicio,
                FechaTermino = fechaTermino,
                Usuario = usuario
            };

            Evento insertedEvento = context.Eventos.Attach(evento).Entity;

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException dub)
            {
                Console.WriteLine("Could not save changes: " + dub);
                return false;
            }

            foreach (Cliente cliente in clientes)
            {
                cliente.Evento = insertedEvento;

                // Cliente exists?
                if (context.Clientes.Any(c => c.Id == cliente.Id))
                {
                    context.Entry(cliente).State = EntityState.Modified;
                }
                else
                {
                    // Attach new Cliente
                    context.Clientes.Attach(cliente);
                }
            }

            foreach (Sala sala in salas)
            {
                sala.Evento = insertedEvento;
                context.Entry(sala).State = EntityState.Modified;
            }

            foreach (Empleado empleado in empleados)
            {
                EventoEmpleado eventoEmpleado = new EventoEmpleado
                {
                    Evento = insertedEvento,
                    Empleado = empleado
                };

                context.EventoEmpleados.Attach(eventoEmpleado);
            }

            foreach (AgregableCantidad agregableCantidad in agregableCantidades)
            {
                EventoAgregable eventoAgregable = new EventoAgregable
                {
                    Evento = insertedEvento,
                    Agregable = agregableCantidad.Agregable,
                    Cantidad = agregableCantidad.Cantidad
                };

                context.EventoAgregables.Attach(eventoAgregable);
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException dub)
            {
                Console.WriteLine("Could not save changes: " + dub);
                return false;
            }

            return true;
        }
    }
}