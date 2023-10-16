// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Views;
using EventManager.Core.Database;
using EventManager.Core.Database.Models;
using EventManager.Core.Database.Services;

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
                Console.WriteLine("5. Agregar sala de Evento");
                Console.WriteLine("6. Confirmar Evento");
                Console.WriteLine("7. Cancelar el Evento");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        eventoView.AgregarEvento();
                        break;
                    case "2":
                        break;
                    case "3":
                        break;
                    case "4":
                        break;
                    case "5":
                        break;
                    case "6":
                        return;
                    case "7":
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
