// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Views;
using EventManager.Core.Database;
using EventManager.Core.Database.Models;
using EventManager.Core.Database.Services;
using Org.BouncyCastle.Tls;

namespace EventManager.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (OperatingSystem.IsWindows()) { }

            // Back up the cwd
            string cwd = Environment.CurrentDirectory;

            foreach (string arg in args)
            {
                string[] split = arg.Split('=');

                string key = split[0];
                string val = split.Length > 1 ? split[1] : string.Empty;

                switch (key)
                {
                    case "--hello":
                        Console.WriteLine("Hi!");
                        break;
                }
            }

            //SetUp();
            RunMenu();
            Console.WriteLine("Hello, World!");
        }

        private static void RunMenu()
        {
            EventoView eventoView = new EventoView();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar Evento");
                Console.WriteLine("2. Consultar Evento");
                Console.WriteLine("3. Actualizar Evento");
                Console.WriteLine("4. Eliminar Evento");
                Console.WriteLine("5. Confirmar Evento");
                Console.WriteLine("6. Cancelar el Evento");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        eventoView.AgregarEvento();
                        break;
                    case "2":
                        Evento evento = eventoView.ConsultarEvento();
                        if (evento != null)
                        {
                            Console.WriteLine(evento.ToString());
                        }
                        break;
                    case "3":
                        break;
                    case "4":
                        eventoView.EliminarEvento();
                        break;
                    case "5":
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void SetUp()
        {
            CrearUsuario();
            CrearCliente();
            CrearEmpleado();
            CrearSalas();
            CrearAgregables();
        }

        private static void CrearSalas()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                SalaService salaService = new SalaService(context);
                salaService
                    .CreateSalaAsync(new Sala { Nombre = "Infantil", Tipo = Sala.TipoSala.SALON })
                    .Wait();
                salaService
                    .CreateSalaAsync(new Sala { Nombre = "Infernal", Tipo = Sala.TipoSala.SALON })
                    .Wait();
                salaService
                    .CreateSalaAsync(new Sala { Nombre = "Lectura", Tipo = Sala.TipoSala.SALON })
                    .Wait();
            }
        }

        private static void CrearAgregables()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                AgregableService agregableService = new AgregableService(context);
                agregableService
                    .CreateAgregableAsync(
                        new Agregable
                        {
                            Nombre = "Bocina",
                            Tipo = Agregable.TipoAgregable.EQUIPO,
                            Total = 4
                        }
                    )
                    .Wait();
                agregableService
                    .CreateAgregableAsync(
                        new Agregable
                        {
                            Nombre = "Proyector",
                            Tipo = Agregable.TipoAgregable.EQUIPO,
                            Total = 1
                        }
                    )
                    .Wait();
                agregableService
                    .CreateAgregableAsync(
                        new Agregable
                        {
                            Nombre = "Plastilina",
                            Tipo = Agregable.TipoAgregable.EQUIPO,
                            Total = 10
                        }
                    )
                    .Wait();
            }
        }

        private static void CrearEmpleado()
        {
            //Crear Usuario
            using (DatabaseContext context = new DatabaseContext())
            {
                EmpleadoService empleadoService = new EmpleadoService(context);
                empleadoService.CreateEmpleadoAsync(new Empleado { Nombre = "Francisco" }).Wait();
                empleadoService.CreateEmpleadoAsync(new Empleado { Nombre = "Benson" }).Wait();
                empleadoService.CreateEmpleadoAsync(new Empleado { Nombre = "Mordecai" }).Wait();
            }
        }

        private static void CrearUsuario()
        {
            //Crear Usuario
            using (DatabaseContext context = new DatabaseContext())
            {
                UsuarioService usuarioService = new UsuarioService(context);
                usuarioService
                    .CreateUsuarioAsync(
                        new Usuario
                        {
                            Nombre = "admin",
                            Password = "password",
                            Tipo = Usuario.TipoUsuario.ENCARGADA
                        }
                    )
                    .Wait();
            }
        }

        private static void CrearCliente()
        {
            //Crear Cliente
            using (DatabaseContext context = new DatabaseContext())
            {
                ClienteService clienteService = new ClienteService(context);
                clienteService
                    .CreateClienteAsync(
                        new Cliente { Nombre = "Ivan Tapia", Telefono = "1234567890" }
                    )
                    .Wait();
            }
        }
    }
}
