// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Views;
using EventManager.Core.Database;
using EventManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            foreach (string arg in args)
            {
                string[] split = arg.Split('=');

                string key = split[0];
                string val = split.Length > 1 ? split[1] : string.Empty;

                switch (key)
                {
                    case "--setup":
                        Setup();
                        break;
                }
            }

            while (true)
            {
                Console.WriteLine("\nEvento Menu —");
                Console.WriteLine("1. Agregar Evento");
                Console.WriteLine("2. Consultar Evento");
                Console.WriteLine("3. Actualizar Evento");
                Console.WriteLine("4. Eliminar Evento");
                Console.WriteLine("5. Salir");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        EventoAgregarView.ShowMenuAgregarEvento();
                        break;
                    case "2":
                        EventoConsultarView.ShowMenuConsultarEvento();
                        break;
                    case "3":
                        EventoActualizarView.ShowMenuActualizarEvento();
                        break;
                    case "4":
                        EventoEliminarView.ShowMenuEliminarEvento();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        public static Usuario? GetUser()
        {
            using DatabaseContext context = new DatabaseContext();

            const int id = 1;

            return context.Usuarios.Find(id);
        }

        private static void Setup()
        {
            using DatabaseContext context = new DatabaseContext();

            if (context.Usuarios.ToList().Count <= 0)
            {
                Usuario usuario = new Usuario
                {
                    Nombre = "admin",
                    Password = "password",
                    Tipo = Usuario.TipoUsuario.Encargado
                };
                context.Usuarios.Add(usuario);
            }

            if (context.Clientes.ToList().Count <= 0)
            {
                Cliente cliente = new Cliente
                {
                    Nombre = "Ivan Tapia",
                    Telefono = "1234567890"
                };
                context.Clientes.Add(cliente);
            }

            if (context.Empleados.ToList().Count <= 0)
            {
                Empleado empleado1 = new Empleado { Nombre = "Francisco" };
                Empleado empleado2 = new Empleado { Nombre = "Benson" };
                Empleado empleado3 = new Empleado { Nombre = "Mordecai" };
                context.Empleados.AddRange(empleado1, empleado2, empleado3);
            }

            if (context.Salas.ToList().Count <= 0)
            {
                Sala sala1 = new Sala { Nombre = "Infantil", Tipo = Sala.TipoSala.Salon };
                Sala sala2 = new Sala { Nombre = "Infantil", Tipo = Sala.TipoSala.Salon };
                Sala sala3 = new Sala { Nombre = "Infantil", Tipo = Sala.TipoSala.Salon };
                context.Salas.AddRange(sala1, sala2, sala3);
            }

            if (context.Agregables.ToList().Count <= 0)
            {
                Agregable agregable1 =
                    new Agregable
                    {
                        Nombre = "Bocina",
                        Tipo = Agregable.TipoAgregable.Equipo,
                        Total = 4
                    };
                Agregable agregable2 =
                    new Agregable
                    {
                        Nombre = "Proyector",
                        Tipo = Agregable.TipoAgregable.Equipo,
                        Total = 1
                    };
                Agregable agregable3 =
                    new Agregable
                    {
                        Nombre = "Plastilina",
                        Tipo = Agregable.TipoAgregable.Equipo,
                        Total = 10
                    };
                context.Agregables.AddRange(agregable1, agregable2, agregable3);
            }

            try
            {
                context.SaveChanges();
            }
            catch (DbUpdateException dub)
            {
                Console.WriteLine("Could not save changes: " + dub.Message);
                return;
            }

            Console.WriteLine("Finished setup.");
        }
    }
}