// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

using EventManager.CLI.Utils;
using EventManager.Core.Models;

namespace EventManager.CLI.Views
{
    public class EventoConsultarView
    {
        public static void ShowMenuConsultarEvento()
        {
            while (true)
            {
                Console.WriteLine("\nConsultar Evento Menu —");
                Console.WriteLine("1. Buscar");
                Console.WriteLine("2. Cancelar");

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Evento? evento = BibliotecaModelUtils.GetEventoById(true);

                        if (evento != null)
                        {
                            Console.WriteLine(evento.ToString());
                            return;
                        }

                        break;
                    case "2":
                        Console.WriteLine("Volviendo al menu anterior");
                        return;
                    default:
                        Console.WriteLine("Invalid option selected");
                        break;
                }
            }
        }
    }
}