// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Utils;
using EventManager.Core.Database;
using EventManager.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.CLI.Views
{
    public class EventoEliminarView
    {
        public static void ShowMenuEliminarEvento()
        {
            Evento? selectedEvento = null;

            while (true)
            {
                Console.WriteLine("\nEliminar Evento Menu —");
                Console.WriteLine("1. Seleccionar");
                Console.WriteLine("2. Eliminar");
                Console.WriteLine("3. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        selectedEvento = BibliotecaModelUtils.GetEventoById(true);
                        break;
                    case "2":
                        if (DeleteEvento(selectedEvento))
                        {
                            return;
                        }

                        break;
                    case "3":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }

        private static bool DeleteEvento(Evento? evento)
        {
            if (evento == null)
            {
                return false;
            }

            using DatabaseContext context = new DatabaseContext();

            context.Eventos.Remove(evento);

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