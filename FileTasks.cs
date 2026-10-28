using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace Lab3
{
    internal static class FileTasks
    {
        public static void FillBinary(string path, int count,
            int min, int max, Random random)
        {
            CheckPath(path);
            CheckGeneration(count, min, max, random);
            using BinaryWriter writer = new BinaryWriter(
                File.Create(path));
            for (int index = 0; index < count; index++)
            {
                writer.Write((int)random.NextInt64(
                    min, (long)max + 1));
            }
        }

        public static void CopyMultiples(
            string source, string target, int k)
        {
            CheckDivisor(k);
            CheckDifferentPaths(source, target);
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(source));
            CheckBinaryLength(reader.BaseStream.Length);
            using BinaryWriter writer = new BinaryWriter(
                File.Create(target));
            while (reader.BaseStream.Position
                < reader.BaseStream.Length)
            {
                int value = reader.ReadInt32();
                if ((long)value % k == 0)
                {
                    writer.Write(value);
                }
            }
        }

        public static void PrintBinary(string path)
        {
            CheckPath(path);
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(path));
            CheckBinaryLength(reader.BaseStream.Length);
            while (reader.BaseStream.Position
                < reader.BaseStream.Length)
            {
                Console.Write($"{reader.ReadInt32()} ");
            }
            Console.WriteLine();
        }

        public static void FillToys(
            string binaryPath, string xmlPath, Toy[] toys)
        {
            CheckDifferentPaths(binaryPath, xmlPath);
            CheckToys(toys);
            using (BinaryWriter writer = new BinaryWriter(
                File.Create(binaryPath), Encoding.UTF8))
            {
                writer.Write(toys.Length);
                foreach (Toy toy in toys)
                {
                    writer.Write(toy.Name);
                    writer.Write(toy.Price);
                    writer.Write(toy.MinAge);
                    writer.Write(toy.MaxAge);
                }
            }
            XmlSerializer serializer = new XmlSerializer(
                typeof(Toy[]));
            using FileStream stream = File.Create(xmlPath);
            serializer.Serialize(stream, toys);
        }

        public static Toy[] ReadToysBinary(string path)
        {
            CheckPath(path);
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(path), Encoding.UTF8);
            int count = reader.ReadInt32();
            if (count < 0 || count > 10000)
            {
                throw new InvalidDataException(
                    "Некорректное число игрушек.");
            }
            Toy[] toys = new Toy[count];
            for (int index = 0; index < count; index++)
            {
                try
                {
                    toys[index] = new Toy(reader.ReadString(),
                        reader.ReadDecimal(), reader.ReadInt32(),
                        reader.ReadInt32());
                }
                catch (ArgumentException error)
                {
                    throw new InvalidDataException(
                        "Некорректная игрушка в файле.", error);
                }
            }
            return toys;
        }

        public static Toy[] ReadToysXml(string path)
        {
            CheckPath(path);
            XmlSerializer serializer = new XmlSerializer(
                typeof(Toy[]));
            using FileStream stream = File.OpenRead(path);
            try
            {
                Toy[] toys = (Toy[])serializer.Deserialize(
                    stream);
                CheckToys(toys);
                return toys;
            }
            catch (InvalidOperationException error)
            {
                throw new InvalidDataException(
                    "Некорректный XML игрушек.", error);
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(
                    "Некорректные данные игрушек.", error);
            }
        }

        public static bool FindConstructorPrice(
            Toy[] toys, out decimal price)
        {
            CheckToys(toys);
            bool found = false;
            price = 0;
            foreach (Toy toy in toys)
            {
                if (string.Equals(toy.Name.Trim(),
                    "конструктор",
                    StringComparison.OrdinalIgnoreCase)
                    && (!found || toy.Price > price))
                {
                    price = toy.Price;
                    found = true;
                }
            }
            return found;
        }

        public static void FillText(string path, int count,
            int min, int max, int perLine, Random random)
        {
            CheckPath(path);
            CheckGeneration(count, min, max, random);
            if (perLine < 1 || perLine > 100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(perLine),
                    "Чисел в строке должно быть 1-100.");
            }
            using StreamWriter writer = new StreamWriter(
                path, false, Encoding.UTF8);
            for (int index = 0; index < count; index++)
            {
                int value = (int)random.NextInt64(
                    min, (long)max + 1);
                writer.Write(value);
                bool endOfLine = (index + 1) % perLine == 0
                    || index == count - 1;
                writer.Write(
                    endOfLine ? Environment.NewLine : " ");
            }
        }

        public static long SumAtIndex(string path)
        {
            CheckPath(path);
            using StreamReader reader = new StreamReader(path);
            string line;
            long index = 0;
            long sum = 0;
            while ((line = reader.ReadLine()) != null)
            {
                int value = ParseNumber(line);
                if (value == index)
                {
                    sum = checked(sum + value);
                }
                index++;
            }
            return sum;
        }

        public static bool ProductMultiples(
            string path, int k, out BigInteger product)
        {
            CheckDivisor(k);
            CheckPath(path);
            product = BigInteger.One;
            bool found = false;
            using StreamReader reader = new StreamReader(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split((char[])null,
                    StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    int value = ParseNumber(part);
                    if ((long)value % k == 0)
                    {
                        product *= value;
                        found = true;
                    }
                }
            }
            return found;
        }

        public static void FillLines(
            string path, string[] lines)
        {
            CheckPath(path);
            ArgumentNullException.ThrowIfNull(lines);
            CheckCount(lines.Length);
            foreach (string line in lines)
            {
                if (line == null)
                {
                    throw new ArgumentException(
                        "Строка не может быть null.",
                        nameof(lines));
                }
            }
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        public static void CopyWithoutRussian(
            string source, string target)
        {
            CheckDifferentPaths(source, target);
            using StreamReader reader = new StreamReader(source);
            using StreamWriter writer = new StreamWriter(
                target, false, Encoding.UTF8);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                bool hasRussian = false;
                foreach (char symbol in line)
                {
                    if ((symbol >= 'А' && symbol <= 'я')
                        || symbol == 'Ё' || symbol == 'ё')
                    {
                        hasRussian = true;
                        break;
                    }
                }
                if (!hasRussian)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private static void CheckGeneration(
            int count, int min, int max, Random random)
        {
            CheckCount(count);
            ArgumentNullException.ThrowIfNull(random);
            if (min > max)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(max),
                    "Верхняя граница меньше нижней.");
            }
        }

        private static void CheckCount(int count)
        {
            if (count < 0 || count > 10000)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(count),
                    "Количество должно быть от 0 до 10000.");
            }
        }

        private static void CheckToys(Toy[] toys)
        {
            ArgumentNullException.ThrowIfNull(toys);
            CheckCount(toys.Length);
            foreach (Toy toy in toys)
            {
                toy.Validate();
            }
        }

        private static void CheckPath(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
        }

        private static void CheckDifferentPaths(
            string source, string target)
        {
            CheckPath(source);
            CheckPath(target);
            if (string.Equals(Path.GetFullPath(source),
                Path.GetFullPath(target),
                StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Файлы ввода и вывода должны различаться.");
            }
        }

        private static void CheckBinaryLength(long length)
        {
            if (length % sizeof(int) != 0)
            {
                throw new InvalidDataException(
                    "Длина файла не кратна размеру Int32.");
            }
        }

        private static int ParseNumber(string text)
        {
            int value;
            if (!int.TryParse(text, out value))
            {
                throw new InvalidDataException(
                    "Файл содержит некорректное целое число.");
            }
            return value;
        }

        private static void CheckDivisor(int k)
        {
            if (k == 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(k), "k не может быть нулём.");
            }
        }
    }
}
