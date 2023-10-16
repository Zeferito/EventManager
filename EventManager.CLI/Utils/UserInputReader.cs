// Copyright (c) Miguel Angel De La Rosa Martínez, Alec Demian Santana Celaya, Jaime Valdez Tanori, Martin Ricardo Yocupicio Ramos. Licensed under the MIT Licence.
// See the LICENSE file in the repository root for full license text.

namespace EventManager.CLI.Utils
{
    public class UserInputReader
    {
        public static string ReadString(string prompt = "Enter a string: ")
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }

                Console.WriteLine("Invalid input.");
            }
        }

        public static int ReadInt(string prompt = "Enter an integer: ")
        {
            int result;

            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer.");
                    continue;
                }

                break;
            }

            return result;
        }

        public static DateTime ReadDateTime(string prompt = "Enter a date and time (e.g., yyyy-MM-dd HH:mm:ss): ")
        {
            DateTime result;

            while (true)
            {
                Console.WriteLine(prompt);
                string? input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || !DateTime.TryParse(input, out result))
                {
                    Console.WriteLine("Invalid input. Please enter a valid date and time.");
                    continue;
                }

                break;
            }

            return result;
        }
    }
}