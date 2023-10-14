// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.Core.Database;
using EventManager.Core.Database.Models;
using EventManager.Core.Database.Services;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Tls;
using Org.BouncyCastle.Utilities;

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

            Console.WriteLine("Hello, World!");
            RunMenu();
        }

        private static void RunMenu()
        {
            // Back up the cwd
            string cwd = Environment.CurrentDirectory;

            SetUp();

            while (true)
            {
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. Agregar Evento");
                Console.WriteLine("2. Agregar agregables de Evento");
                Console.WriteLine("3. Actualizar Evento");
                Console.WriteLine("4. Eliminar Evento");
                Console.WriteLine("5. Exit");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AgregarEvento();
                        break;
                    case "2":
                        AgregarAgregableAEvento();
                        break;
                    case "3":
                        Console.WriteLine("You selected Option 3");
                        break;
                    case "4":
                        Console.WriteLine("You selected Option 4");
                        break;
                    case "5":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static void SetUp()
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

        private static void AgregarEvento()
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

            //Crear evento
            using (DatabaseContext context = new DatabaseContext())
            {
                EventoService eventoService = new EventoService(context);
                eventoService.CreateEventoAsync(new Evento { Fecha = fecha, UsuarioId = 1 }).Wait();
            }
        }

        private static void AgregarAgregableAEvento()
        {
            //Crear agregable
            using (DatabaseContext context = new DatabaseContext())
            {
                EventoService eventoService = new EventoService(context);
                //Establecer id del evento
                int eventoId;
                while (true)
                {
                    Console.WriteLine("Ingrese id del evento");
                    if (int.TryParse(Console.ReadLine(), out eventoId))
                    {
                        if (!eventoService.EventoExistsAsync(eventoId).Result)
                        {
                            Console.WriteLine("Evento no existe");
                            continue;
                        }
                        break;
                    }
                    Console.WriteLine("id invalido");
                }

                AgregableService agregableService = new AgregableService(context);

                Console.WriteLine("Ingrese nombre del agregable");
                string nombre = Console.ReadLine();

                Agregable.TipoAgregable tipo;
                while (true)
                {
                    Console.WriteLine(
                        "Ingrese el tipo del agregable (0: MATERIAL, 1: EQUIPO, 2: MOBILIARIO)"
                    );

                    if (Enum.TryParse(Console.ReadLine(), out tipo))
                    {
                        break;
                    }
                    Console.WriteLine("Tipo invalido");
                }
                int cantidad;
                while(true){
                    Console.WriteLine("Ingrese la cantidad del agregable");
                    if(int.TryParse(Console.ReadLine(), out cantidad)){
                        break;
                    }
                    Console.WriteLine("Cantidad invalida");
                }
                Agregable agregable = agregableService
                    .CreateAgregableAsync(new Agregable { Nombre = nombre, Tipo = tipo, Cantidad = cantidad})
                    .Result;

                agregableService.AgregableAddEventoAsync(agregable.Id, eventoId).Wait();
            }
        }
    }
}
