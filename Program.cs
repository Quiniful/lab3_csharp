using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Numerics;
using System.Text;

namespace Lab3
{
    internal class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Program program = new Program();
            try
            {
                program.Run();
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Ввод завершён.");
            }
        }

        private void Run()
        {
            Directory.CreateDirectory("data");
            int command = -1;
            while (command != 0)
            {
                Console.WriteLine("Лабораторная 3. Вариант 6");
                Console.WriteLine("1 - Заполнение матриц");
                Console.WriteLine("2 - Кратчайший путь");
                Console.WriteLine("3 - Матричное выражение");
                Console.WriteLine("4 - Бинарный файл чисел");
                Console.WriteLine("5 - Игрушки и XML");
                Console.WriteLine("6 - Сумма по индексам");
                Console.WriteLine("7 - Произведение кратных");
                Console.WriteLine("8 - Строки без русских букв");
                Console.WriteLine("0 - Выход");
                command = ConsoleInput.ReadRange(
                    "Команда: ", 0, 8);
                try
                {
                    Execute(command);
                }
                catch (OverflowException)
                {
                    Console.WriteLine(
                        "Переполнение! Введите меньшие числа.");
                }
                catch (IOException error)
                {
                    if (error is EndOfStreamException)
                    {
                        throw;
                    }
                    Console.WriteLine(error.Message);
                }
                catch (UnauthorizedAccessException error)
                {
                    Console.WriteLine(error.Message);
                }
            }
        }

        private void Execute(int command)
        {
            switch (command)
            {
                case 1:
                    ShowMatrices();
                    break;
                case 2:
                    ShowPath();
                    break;
                case 3:
                    ShowExpression();
                    break;
                case 4:
                case 6:
                case 7:
                    WorkWithNumbers(command);
                    break;
                case 5:
                    WorkWithToys();
                    break;
                case 8:
                    WorkWithLines();
                    break;
            }
        }

        private Matrix ReadMatrix(string name)
        {
            Console.WriteLine($"Матрица {name}");
            int rows = ConsoleInput.ReadRange(
                "Число строк (1-30): ", 1, 30);
            int columns = ConsoleInput.ReadRange(
                "Число столбцов (1-30): ", 1, 30);
            return new Matrix(rows, columns);
        }

        private void ShowMatrices()
        {
            Matrix first = ReadMatrix("1, ввод по столбцам");
            int size = ConsoleInput.ReadRange(
                "Размер двух квадратных матриц (1-30): ", 1, 30);
            Random random = new Random();
            Matrix second = new Matrix(size, random);
            Matrix third = new Matrix(size);
            Console.WriteLine("Первая матрица:");
            Console.WriteLine(first);
            Console.WriteLine("Четырёхзначные числа:");
            Console.WriteLine(second);
            Console.WriteLine("Заполнение по образцу:");
            Console.WriteLine(third);
        }

        private void ShowPath()
        {
            Matrix matrix = ReadMatrix("для поиска пути");
            Console.WriteLine(matrix);
            List<Point> path = matrix.FindShortestPath();
            Console.WriteLine(
                $"Количество шагов: {path.Count - 1}");
            Console.WriteLine("Путь от максимума к минимуму:");
            foreach (Point point in path)
            {
                int value = matrix[point.X, point.Y];
                Console.WriteLine(
                    $"[{point.X}, {point.Y}] = {value}");
            }
        }

        private void ShowExpression()
        {
            int rows = ConsoleInput.ReadRange(
                "Строк в A и C (1-30): ", 1, 30);
            int columns = ConsoleInput.ReadRange(
                "Столбцов в A и C (1-30): ", 1, 30);
            Console.WriteLine("Введите A по столбцам:");
            Matrix a = new Matrix(rows, columns);
            Console.WriteLine("Введите B по столбцам:");
            Matrix b = new Matrix(columns, rows);
            Console.WriteLine("Введите C по столбцам:");
            Matrix c = new Matrix(rows, columns);
            Console.WriteLine("A:");
            Console.WriteLine(a);
            Console.WriteLine("B:");
            Console.WriteLine(b);
            Console.WriteLine("C:");
            Console.WriteLine(c);
            Matrix result = Matrix.CalculateExpression(a, b, c);
            Console.WriteLine("A^T - (B + C^T):");
            Console.WriteLine(result);
        }

        private void WorkWithNumbers(int task)
        {
            int count = ConsoleInput.ReadRange(
                "Количество чисел (0-10000): ", 0, 10000);
            int min = ConsoleInput.ReadInt("Нижняя граница: ");
            int max = ConsoleInput.ReadRange(
                "Верхняя граница: ", min, int.MaxValue);
            Random random = new Random();
            if (task == 4)
            {
                int k = ConsoleInput.ReadDivisor();
                FileTasks.FillBinary("data/numbers.bin",
                    count, min, max, random);
                FileTasks.CopyMultiples("data/numbers.bin",
                    "data/multiples.bin", k);
                Console.WriteLine("Исходные числа:");
                FileTasks.PrintBinary("data/numbers.bin");
                Console.WriteLine("Кратные k:");
                FileTasks.PrintBinary("data/multiples.bin");
            }
            else if (task == 6)
            {
                FileTasks.FillText("data/index.txt", count,
                    min, max, 1, random);
                Console.WriteLine(File.ReadAllText(
                    "data/index.txt"));
                long sum = FileTasks.SumAtIndex(
                    "data/index.txt");
                Console.WriteLine($"Сумма: {sum}");
            }
            else
            {
                int perLine = ConsoleInput.ReadRange(
                    "Чисел в строке (2-100): ", 2, 100);
                int k = ConsoleInput.ReadDivisor();
                FileTasks.FillText("data/product.txt", count,
                    min, max, perLine, random);
                Console.WriteLine(File.ReadAllText(
                    "data/product.txt"));
                BigInteger product;
                bool found = FileTasks.ProductMultiples(
                    "data/product.txt", k, out product);
                if (found)
                {
                    Console.WriteLine(
                        $"Произведение: {product}");
                }
                else
                {
                    Console.WriteLine("Кратных элементов нет.");
                }
            }
        }

        private void WorkWithToys()
        {
            int count = ConsoleInput.ReadRange(
                "Количество игрушек (0-10000): ", 0, 10000);
            Toy[] toys = new Toy[count];
            int index;
            for (index = 0; index < count; index++)
            {
                string name = ConsoleInput.ReadText(
                    "Название: ");
                decimal price = ConsoleInput.ReadPrice();
                int minAge = ConsoleInput.ReadRange(
                    "Минимальный возраст: ", 0, 150);
                int maxAge = ConsoleInput.ReadRange(
                    "Максимальный возраст: ", minAge, 150);
                toys[index] = new Toy(
                    name, price, minAge, maxAge);
            }
            FileTasks.FillToys("data/toys.bin",
                "data/toys.xml", toys);
            Toy[] binary = FileTasks.ReadToysBinary(
                "data/toys.bin");
            Toy[] xml = FileTasks.ReadToysXml("data/toys.xml");
            Console.WriteLine("Результат из бинарного файла:");
            PrintPrice(binary);
            Console.WriteLine(
                "Результат после XML-десериализации:");
            PrintPrice(xml);
        }

        private void PrintPrice(Toy[] toys)
        {
            decimal price;
            if (FileTasks.FindConstructorPrice(toys, out price))
            {
                Console.WriteLine(
                    $"Самый дорогой конструктор: {price} руб.");
            }
            else
            {
                Console.WriteLine("Конструктор не найден.");
            }
        }

        private void WorkWithLines()
        {
            int count = ConsoleInput.ReadRange(
                "Количество строк (0-10000): ", 0, 10000);
            string[] lines = new string[count];
            int index;
            for (index = 0; index < count; index++)
            {
                lines[index] = ConsoleInput.ReadLine("Строка: ");
            }
            FileTasks.FillLines("data/lines.txt", lines);
            FileTasks.CopyWithoutRussian("data/lines.txt",
                "data/without_russian.txt");
            Console.WriteLine("Строки без русских букв:");
            Console.Write(File.ReadAllText(
                "data/without_russian.txt"));
        }
    }
}
