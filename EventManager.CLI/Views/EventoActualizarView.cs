// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Utils;
using EventManager.Core.Database;
using EventManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.CLI.Views
{
    public class EventoActualizarView
    {
        public static void ShowMenuActualizarEvento()
        {
            using DatabaseContext context = new DatabaseContext();

            // TODO: Display list of available entries
            // TODO: Create cancellable loop for validation
            Console.WriteLine(context.Eventos.ToList());
            int eventoId = UserInputReader.ReadInt("Ingrese Id del Evento: ");

            Evento? evento = context.Eventos.Find(eventoId);

            if (evento == null)
            {
                return;
            }

            List<EventoEmpleado> eventoEmpleados = evento.EventoEmpleados;
            List<Empleado> empleados = new List<Empleado>();

            foreach (EventoEmpleado eventoEmpleado in eventoEmpleados)
            {
                empleados.Add(eventoEmpleado.Empleado);
            }

            while (true)
            {
                Console.WriteLine("Actualizar Evento Menu —");
                Console.WriteLine("1. Actualizar Nombre");
                Console.WriteLine("2. Actualizar Descripcion");
                Console.WriteLine("3. Actualizar Periodo");
                Console.WriteLine("4. Actualizar Cliente");
                Console.WriteLine("5. Actualizar Salas");
                Console.WriteLine("6. Confirmar");
                Console.WriteLine("7. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        evento.Nombre = UserInputReader.ReadString("Ingrese el nombre del Evento: ");
                        break;
                    case "2":
                        evento.Descripcion = UserInputReader.ReadString("Ingrese la descripcion del Evento: ");
                        break;
                    case "3":
                        // TODO: Add period validation
                        evento.FechaInicio =
                            UserInputReader.ReadDateTime("Ingrese la fecha y hora de inicio del Evento: ");
                        evento.FechaTermino =
                            UserInputReader.ReadDateTime("Ingrese la fecha y hora de termino del Evento: ");
                        break;
                    case "4":
                        ShowSubMenuClientes(evento.Clientes);
                        break;
                    case "5":
                        ShowSubMenuSalas(evento.Salas);
                        break;
                    case "6":
                        if (UpdateEvento(evento))
                        {
                            return;
                        }

                        break;
                    case "7":
                        Console.WriteLine("Goodbye!");
                        return;
                    case "8":
                        // TODO: Find out how to do this shit
                        ShowSubMenuEmpleados(empleados);
                        Console.WriteLine("Actualizar Empleados. Not supported yet. Sorry xP");
                        break;
                    case "9":
                        // TODO: Find out how to do this shit
                        // ShowSubMenuAgregableCantidades(agregableCantidades);
                        Console.WriteLine("Actualizar Agregables. Not supported yet. Sorry xP");
                        break;
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

                        newClientesState.Add(cliente);
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

        private static bool UpdateEvento(Evento evento)
        {
            using DatabaseContext context = new DatabaseContext();

            context.Entry(evento).State = EntityState.Modified;

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