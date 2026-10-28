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
            using BinaryWriter writer = new BinaryWriter(
                File.Create(path));
            int index;
            for (index = 0; index < count; index++)
            {
                writer.Write((int)random.NextInt64(
                    min, (long)max + 1));
            }
        }

        public static void CopyMultiples(
            string source, string target, int k)
        {
            CheckDivisor(k);
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(source));
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
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(path));
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
            using BinaryReader reader = new BinaryReader(
                File.OpenRead(path), Encoding.UTF8);
            int count = reader.ReadInt32();
            if (count < 0 || count > 10000)
            {
                throw new InvalidDataException(
                    "Некорректное число игрушек.");
            }
            Toy[] toys = new Toy[count];
            int index;
            for (index = 0; index < count; index++)
            {
                toys[index] = new Toy(reader.ReadString(),
                    reader.ReadDecimal(), reader.ReadInt32(),
                    reader.ReadInt32());
            }
            return toys;
        }

        public static Toy[] ReadToysXml(string path)
        {
            XmlSerializer serializer = new XmlSerializer(
                typeof(Toy[]));
            using FileStream stream = File.OpenRead(path);
            Toy[] toys = (Toy[])serializer.Deserialize(stream);
            foreach (Toy toy in toys)
            {
                if (string.IsNullOrWhiteSpace(toy.Name)
                    || toy.Price < 0 || toy.MinAge < 0
                    || toy.MaxAge < toy.MinAge)
                {
                    throw new InvalidDataException(
                        "Некорректная игрушка в XML.");
                }
            }
            return toys;
        }

        public static bool FindConstructorPrice(
            Toy[] toys, out decimal price)
        {
            bool found = false;
            price = 0;
            foreach (Toy toy in toys)
            {
                if (string.Equals(toy.Name.Trim(),
                    "конструктор",
                    StringComparison.OrdinalIgnoreCase))
                {
                    if (!found || toy.Price > price)
                    {
                        price = toy.Price;
                        found = true;
                    }
                }
            }
            return found;
        }

        public static void FillText(string path, int count,
            int min, int max, int perLine, Random random)
        {
            using StreamWriter writer = new StreamWriter(
                path, false, Encoding.UTF8);
            int index;
            for (index = 0; index < count; index++)
            {
                int value = (int)random.NextInt64(
                    min, (long)max + 1);
                writer.Write(value);
                if ((index + 1) % perLine == 0
                    || index == count - 1)
                {
                    writer.WriteLine();
                }
                else
                {
                    writer.Write(' ');
                }
            }
        }

        public static long SumAtIndex(string path)
        {
            using StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();
            long index = 0;
            long sum = 0;
            while (line != null)
            {
                int value = ParseNumber(line);
                if (value == index)
                {
                    sum = checked(sum + value);
                }
                index++;
                line = reader.ReadLine();
            }
            return sum;
        }

        public static bool ProductMultiples(
            string path, int k, out BigInteger product)
        {
            CheckDivisor(k);
            product = BigInteger.One;
            bool found = false;
            using StreamReader reader = new StreamReader(path);
            string line = reader.ReadLine();
            while (line != null)
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
                line = reader.ReadLine();
            }
            return found;
        }

        public static void FillLines(
            string path, string[] lines)
        {
            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        public static void CopyWithoutRussian(
            string source, string target)
        {
            using StreamReader reader = new StreamReader(source);
            using StreamWriter writer = new StreamWriter(
                target, false, Encoding.UTF8);
            string line = reader.ReadLine();
            while (line != null)
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
                line = reader.ReadLine();
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
