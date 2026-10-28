using System;
using System.Globalization;
using System.IO;

namespace Lab3
{
    internal static class ConsoleInput
    {
        public static string ReadLine(string message)
        {
            Console.Write(message);
            string input = Console.ReadLine();
            if (input == null)
            {
                throw new EndOfStreamException();
            }
            return input;
        }

        public static string ReadText(string message)
        {
            while (true)
            {
                string input = ReadLine(message).Trim();
                if (input.Length > 0)
                {
                    return input;
                }
                Console.WriteLine("Ошибка! Строка пустая.");
            }
        }

        public static int ReadInt(string message)
        {
            while (true)
            {
                string input = ReadLine(message).Trim();
                int result;
                if (int.TryParse(input, out result))
                {
                    return result;
                }
                Console.WriteLine("Введите целое число Int32.");
            }
        }

        public static int ReadRange(
            string message, int min, int max)
        {
            while (true)
            {
                int result = ReadInt(message);
                if (result >= min && result <= max)
                {
                    return result;
                }
                Console.WriteLine(
                    $"Введите число от {min} до {max}");
            }
        }

        public static int ReadDivisor()
        {
            while (true)
            {
                int result = ReadInt("Введите k (не ноль): ");
                if (result != 0)
                {
                    return result;
                }
                Console.WriteLine("k не может быть нулём.");
            }
        }

        public static decimal ReadPrice()
        {
            while (true)
            {
                string input = ReadLine("Цена в рублях: ");
                input = input.Trim().Replace(',', '.');
                decimal result;
                if (decimal.TryParse(input,
                    NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture, out result)
                    && result >= 0
                    && decimal.Round(result, 2) == result)
                {
                    return result;
                }
                Console.WriteLine(
                    "Введите цену >= 0, до двух знаков дроби.");
            }
        }
    }
}
