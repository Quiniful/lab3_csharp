# Лабораторная работа 3 — Массивы и файлы

Отчёт по лабораторной работе. Вариант 6.


**Выполнил:** Козарчук Никита  
**Группа:** ИТ-2  
**Год:** 2026

## Запуск программы

Требуется [.NET SDK 10](https://dotnet.microsoft.com/download/dotnet/10.0).

```sh
git clone https://github.com/Quiniful/lab3_csharp.git
cd lab3_csharp
dotnet run --project Lab3.csproj
```

Либо откройте `Lab3.slnx` в Rider или Visual Studio с поддержкой .NET 10.
Пункты меню 1–8 соответствуют заданиям 1.6–8.6, команда 0 завершает программу.
Исходники находятся в корне репозитория. Рабочие файлы создаются в каталоге `data`
текущего рабочего каталога и перезаписываются при повторном выполнении задания.

## Цель работы

Освоить обработку двумерных массивов, перегрузку операторов, работу с бинарными и текстовыми файлами, структурами и XML-сериализацией. Разработать один консольный проект с восемью задачами варианта 6 и проверкой входных данных.

## Состав проекта

| Файл | Назначение |
| --- | --- |
| [Matrix.cs](Matrix.cs) | Конструкторы и методы заданий 1–3; единственное поле int[,] _values |
| [FileTasks.cs](FileTasks.cs) | Статические методы заданий 4–8 |
| [Toy.cs](Toy.cs) | Структура игрушки с развёрнутыми свойствами |
| [ConsoleInput.cs](ConsoleInput.cs) | Чтение и проверка данных |
| [Program.cs](Program.cs) | Меню и взаимодействие с пользователем |

Среда выполнения: .NET 10, консольное приложение C#. Каждому типу соответствует отдельный файл. Автосвойства, LINQ и лямбда-выражения не используются. Классы, методы и свойства именуются в PascalCase; переменные — в camelCase; закрытые поля — в _camelCase. Отступ равен четырём пробелам, скобки располагаются на отдельных строках, длина строки исходного кода не превышает 65 символов.

## Задание 1 Заполнение массивов

Условие 1.6: первую матрицу n × m заполнить с клавиатуры по столбцам сверху вниз; вторую квадратную матрицу — случайными четырёхзначными числами из чётных цифр; третью — по образцу для произвольного n. Заполнение реализовано тремя перегруженными конструкторами Matrix.

Первый конструктор Matrix(rows, columns) выполняет внешний цикл по столбцам и внутренний по строкам. Для размера 2 × 3 ввод 1, 2, 3, 4, 5, 6 даёт:


```text
1 3 5
2 4 6
```

Конструктор Matrix(size, random) сначала выбирает ненулевую чётную цифру тысяч из 2, 4, 6, 8. Затем трижды дописывает цифру из 0, 2, 4, 6, 8. Поэтому каждое число четырёхзначное и все его цифры чётные.


```text
int number = 2 * random.Next(1, 5);
for (digit = 0; digit < 3; digit++)
{
    number = number * 10 + 2 * random.Next(0, 5);
}
```

Для третьей матрицы выбрано правило, точно воспроизводящее образец: ниже главной диагонали стоят нули; диагонали, параллельные главной, заполняются попеременно по возрастанию и убыванию. При нумерации i и j с нуля: если i > j, значение равно 0; если j − i чётно, значение равно i + 1; иначе — n − j. На главной диагонали находятся числа от 1 до n.


```text
n = 5
1 4 1 2 1
0 2 3 2 1
0 0 3 2 3
0 0 0 4 1
0 0 0 0 5
```

Матрицы выводятся методом ToString в табличной форме. Ширина столбца определяется по длине самого длинного числа, включая знак минуса.

## Задание 2 Кратчайший путь

Условие 2.6: найти самый короткий путь между максимальным и минимальным элементами двумерного массива. Разрешены движения вправо, влево, вверх, вниз и по диагонали.

Препятствий нет. За один шаг каждая координата меняется не более чем на единицу. Для клеток (r₁, c₁) и (r₂, c₂) минимальное число шагов равно max(|r₁ − r₂|, |c₁ − c₂|). Сначала нужно двигаться по диагонали, пока различаются обе координаты, затем — по оставшейся координате. Так достигается указанная нижняя граница.

Метод FindShortestPath находит значения минимума и максимума, собирает координаты всех их вхождений и выбирает пару с наименьшим расстоянием. Если все элементы равны, путь состоит из одной клетки, а число шагов равно нулю. При равенстве расстояний сохраняется первая найденная пара при обходе по строкам.


```text
int distance = Math.Max(
    Math.Abs(high.X - low.X),
    Math.Abs(high.Y - low.Y));

while (start != finish)
{
    start.X += Math.Sign(finish.X - start.X);
    start.Y += Math.Sign(finish.Y - start.Y);
    path.Add(start);
}
```

В Point координата X хранит номер строки, Y — номер столбца. В интерфейсе выводятся индексы с нуля и значения посещённых элементов.

Контрольный пример:


```text
9 4 5
5 5 0

Путь: [0, 0] -> [1, 1] -> [1, 2]
Количество шагов: 2
```

Для N элементов и p максимумов, q минимумов сложность поиска составляет O(N + pq), дополнительная память — O(N). При ограничении интерфейса 30 × 30 перебор пар невелик. Корректность длины пути проверена сравнением с независимым поиском в ширину на 100 случайных матрицах.

## Задание 3 Матричное выражение

Условие 3.6: вычислить Aᵀ − (B + Cᵀ), перегрузив необходимые операторы. В классе Matrix реализованы операторы + и −, метод Transpose и статический метод CalculateExpression.


```text
public static Matrix CalculateExpression(
    Matrix a, Matrix b, Matrix c)
{
    ArgumentNullException.ThrowIfNull(a);
    ArgumentNullException.ThrowIfNull(b);
    ArgumentNullException.ThrowIfNull(c);
    return a.Transpose() - (b + c.Transpose());
}
```

Для A и C размера r × c матрица B должна иметь размер c × r. Размер результата равен c × r. Пользователь вводит r и c один раз; размеры всех трёх матриц согласуются автоматически. Поэтому допустимы и прямоугольные матрицы. Перед сложением и вычитанием проверяется совпадение размеров.

Операторы создают новый объект и не изменяют исходные матрицы. Операции выполняются в checked-контексте: если результат не помещается в Int32, программа сообщает о переполнении и возвращается в меню.

Контрольные данные:


```text
A = 1 2     B = 5 6     C = 2 0
    3 4         7 8         1 3

A^T = 1 3       C^T = 2 1
      2 4             0 3

B + C^T = 7  7
          7 11

A^T - (B + C^T) = -6 -4
                   -5 -7
```

Ввод матриц выполняется по столбцам. Для воспроизведения примера выберите пункт 3, размеры 2 и 2, затем введите A: 1, 3, 2, 4; B: 5, 7, 6, 8; C: 2, 1, 0, 3.

## Проверка матричного выражения онлайн

В онлайн-калькулятор Matrixcalc введены те же матрицы и выражение A^T-(B+C^T). Получен результат [-6, -4; -5, -7], совпадающий с выводом программы.


![Проверка выражения Aᵀ − (B + Cᵀ) в онлайн-калькуляторе](screenshots/online-calculator.png)

Рисунок 1 — проверка выражения варианта 6 на сайте https://matrixcalc.org/ru/.

## Задание 4 Бинарный файл чисел

Условие 4.6: переписать в другой файл последовательного доступа элементы исходного файла, кратные k. Исходный файл заполняется случайными числами отдельным методом FillBinary.

Числа записываются с помощью BinaryWriter.Write(int), по четыре байта на элемент, без заголовка. Метод CopyMultiples последовательно читает исходный файл с помощью BinaryReader и записывает подходящие числа, сохраняя порядок. Чтение завершается при достижении конца потока.


```text
while (reader.BaseStream.Position
    < reader.BaseStream.Length)
{
    int value = reader.ReadInt32();
    if ((long)value % k == 0)
    {
        writer.Write(value);
    }
}
```

Нулевой делитель отклоняется при вводе и в методе обработки. Отрицательный k допускается. Приведение к long перед вычислением остатка позволяет корректно обработать пару int.MinValue и −1. Ноль считается кратным любому ненулевому k.

Количество элементов и границы генерации вводятся с клавиатуры. Диапазон включителен с обеих сторон; для верхней границы int.MaxValue используется Random.NextInt64(min, (long)max + 1), чтобы не возникало переполнения.

Контрольный пример:


```text
Исходные числа: -6 -1 0 3 4 9
k = -3
Результат: -6 0 3 9
```

Файлы: data/numbers.bin и data/multiples.bin. Для пустого исходного файла создаётся пустой результат. Все потоки закрываются конструкцией using.

## Задание 5 Структуры и XML

Условие 5.6: файл содержит название игрушки, стоимость и возрастные границы. Определить стоимость самого дорогого конструктора. Для данного задания обязательны структура и XML-сериализация.

Структура Toy содержит поля _name, _price, _minAge, _maxAge и публичные свойства с явно написанными get и set. Для стоимости используется decimal. Публичные свойства позволяют XmlSerializer записать и восстановить массив структур; автосвойств нет.

Отдельный метод FillToys сохраняет массив Toy[] в два файла: бинарный toys.bin и XML toys.xml. Бинарный формат: количество записей Int32, затем для каждой записи строка имени, цена Decimal и две возрастные границы Int32. После записи оба файла читаются; XML действительно десериализуется, и цена конструктора определяется по восстановленным объектам.


```text
XmlSerializer serializer = new XmlSerializer(
    typeof(Toy[]));
using FileStream stream = File.Create(xmlPath);
serializer.Serialize(stream, toys);
```

При поиске имя сравнивается со словом «конструктор» без учёта регистра и крайних пробелов. Названия вроде «Конструктор LEGO» в этом варианте не считаются точным совпадением. Возрастные границы сохраняются, но не участвуют в поиске максимальной цены.

| Название | Цена | Возраст |
| --- | --- | --- |
| Конструктор | 100,00 | 2–5 |
| Мяч | 999,00 | 0–10 |
| конструктор | 250,50 | 6–12 |

Результат чтения обоих файлов: самый дорогой конструктор стоит 250,50 руб. Если конструктора нет, выводится отдельное сообщение. Конструктор с нулевой стоимостью считается найденным. Для этого метод возвращает bool, а найденную цену — через out.

Проверяются непустое название, цена не меньше нуля и не более двух знаков после запятой, возраст от 0 до 150 лет, верхняя возрастная граница не меньше нижней. Разрешён ввод цены через точку или запятую.

## Задание 6 Сумма по индексам

Условие 6.6: в текстовом файле хранятся целые числа по одному в строке. Найти сумму элементов, равных своему индексу. Индексация начинается с нуля. Исходный файл заполняется случайными данными отдельным методом.

Метод FillText получает perLine = 1 и записывает числа в UTF-8. Метод SumAtIndex последовательно читает строки, переводит каждую строку в Int32 и сравнивает значение со счётчиком index. Сумма накапливается в long, чтобы вместить сумму большого числа значений Int32.


```text
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
```

Контрольный файл:


```text
0
1
9
3
4
```

Совпадают элементы с индексами 0, 1, 3 и 4. Их сумма: 0 + 1 + 3 + 4 = 8. Пустой файл даёт сумму 0. Некорректная числовая строка вызывает сообщение об ошибке формата файла.

Проверка через интерфейс со случайным заполнением: количество 5, диапазон от 3 до 3. Файл содержит пять троек, совпадение есть только при индексе 3, поэтому сумма равна 3. Одинаковые границы позволяют воспроизвести результат.

Файл: data/index.txt. Сложность обработки O(N), дополнительная память O(1), кроме буфера чтения строки.

## Задание 7 Произведение кратных элементов

Условие 7.6: в текстовом файле хранятся целые числа по несколько в строке. Вычислить произведение элементов, кратных заданному k. Исходный файл заполняется случайными данными отдельным методом.

Метод FillText получает число элементов в строке от 2 до 100. В последней строке может быть меньше элементов. ProductMultiples читает файл по строкам, разделяет каждую строку по пробельным символам и последовательно обрабатывает числа. Нулевой k отклоняется.


```text
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
```

Для результата применяется BigInteger из System.Numerics, поэтому произведение не ограничено диапазоном long. Начальное произведение равно 1. Дополнительный признак found позволяет отличить отсутствие подходящих чисел от результата, равного 1. Ноль является кратным k и обнуляет произведение.

Контрольный пример:


```text
Файл:
2 -3 6
9 5

k = 3
Произведение: (-3) * 6 * 9 = -162
```

При отсутствии кратных элементов выводится «Кратных элементов нет». Для воспроизводимой проверки через меню: количество 4, границы 6 и 6, по 2 числа в строке, k = 3. Ожидаемый результат: 6⁴ = 1296.

Файл: data/product.txt. Файл не загружается целиком в память; используется текущая строка и её токены. Объём памяти для BigInteger зависит от длины результата.

## Задание 8 Строки без русских букв

Условие 8.6: переписать в другой текстовый файл строки, в которых нет русских букв. Исходные строки вводятся с клавиатуры и сохраняются в отдельный текстовый файл.

Метод CopyWithoutRussian читает строки по очереди. Для каждого символа проверяется принадлежность диапазону от «А» до «я», а буквы «Ё» и «ё» проверяются отдельно. В Unicode они расположены вне указанного диапазона. Если русская буква найдена, строка не копируется.


```text
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
```

Пустые строки, строки с цифрами, латинскими буквами и знаками препинания сохраняются, поскольку не содержат русских букв. Проверяются именно буквы русского алфавита, а не все символы кириллицы.

| Строка | Решение |
| --- | --- |
| Hello | Скопировать |
| Ёж | Исключить |
| ёж | Исключить |
| 123 | Скопировать |
| Пустая строка | Скопировать |
| é | Скопировать |

Файлы: data/lines.txt и data/without_russian.txt. Порядок строк сохраняется, оба файла записываются в UTF-8. Если исходный файл пуст, итоговый файл также пуст.

## Проверка программы и вывод

Проект успешно собран командой dotnet build: ошибок 0, предупреждений 0. Проверка `dotnet format whitespace --verify-no-changes` завершилась успешно. Выполнено 27 автоматических проверок алгоритмов, форматов файлов и ограничений исходного кода, а также 11 запусков консольного интерфейса.

| Проверка | Результат |
| --- | --- |
| Образец n = 5 и ввод по столбцам | Совпадают с заданием |
| 900 случайных чисел | Четырёхзначные, все цифры чётные |
| Путь на 100 случайных матрицах | Длина совпала с поиском в ширину |
| Матричное выражение | Совпало с Matrixcalc |
| Несовместимые размеры и переполнение | Ошибки обнаружены |
| Кратность и Int32.MinValue / −1 | Обработаны корректно |
| XML и бинарный файл игрушек | Все поля восстановлены |
| Пустые данные и нулевая цена | Обработаны корректно |
| Произведение за пределами Int64 | Вычислено с BigInteger |
| Ё и ё, пустые строки | Отфильтрованы по условию |
| Автосвойства, LINQ, лямбды | Не обнаружены в синтаксическом дереве |

В консольных сценариях проверены все восемь пунктов меню, неверная команда, нулевой размер, нечисловой ввод, число вне диапазона Int32, неверная цена, несогласованные возрастные границы, нулевой k и завершение входного потока. При ошибке ввод повторяется; при завершении потока программа корректно заканчивает работу.

Вывод: выполнены все восемь задач варианта 6 в одном проекте. Освоены перегруженные конструкторы, операторы матриц, последовательное чтение файлов, сериализация структуры в XML и проверка пользовательского ввода.

Запуск: открыть Lab3.slnx в Rider или Visual Studio либо выполнить dotnet run --project Lab3.csproj из каталога проекта. Рабочие файлы создаются в подкаталоге data текущего рабочего каталога. Повторный запуск задания перезаписывает его рабочие файлы.

Источники: lab3_csharp.pdf — условия лабораторной работы; Microsoft Learn — Общие соглашения о коде C# (https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/coding-style/coding-conventions); Matrixcalc — проверка матричного выражения (https://matrixcalc.org/ru/).

## Проверка кодстайла

Код сверен с [соглашениями Microsoft](https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/coding-style/coding-conventions)
и [правилами именования](https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/coding-style/identifier-names).
Сохранён стиль предыдущей лабораторной: явные типы, полная запись `new`, блочные
пространства имён, развёрнутые свойства, `_camelCase` для закрытых полей,
скобки Allman, отступ 4 пробела и длина строки до 65 символов.
Поле массива объявлено `readonly`; для коротких строк с подстановками
используется интерполяция. Автосвойств, LINQ и лямбда-выражений нет.

```sh
dotnet build Lab3.csproj
dotnet format Lab3.csproj whitespace --verify-no-changes --no-restore
dotnet format Lab3.csproj style --verify-no-changes --no-restore --severity info --diagnostics IDE0044 IDE0059 IDE0060
```

[Результаты выполненных проверок](Проверки.txt).

## Краткие пояснения для защиты

- Конструкторы перегружены: имя одно, наборы параметров разные.
- Транспонирование меняет местами индексы строки и столбца.
- Операторы `+` и `-` возвращают новые матрицы, не изменяя исходные.
- `XmlSerializer` сохраняет публичные свойства структуры; `get` и `set` написаны вручную.
- `using` закрывает файловый поток и при штатном завершении, и при исключении.
- `TryParse` проверяет ввод без исключения при неверном формате.
- Признак `found` позволяет отличить отсутствие результата от нулевой цены или произведения, равного единице.

## Полный исходный код


<details>
<summary>Matrix.cs</summary>

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Lab3
{
    internal class Matrix
    {
        private readonly int[,] _values;

        public Matrix(int rows, int columns)
        {
            CheckSize(rows, columns);
            _values = new int[rows, columns];
            int column;
            int row;
            for (column = 0; column < columns; column++)
            {
                for (row = 0; row < rows; row++)
                {
                    _values[row, column] = ConsoleInput.ReadInt(
                        $"[{row}, {column}]: ");
                }
            }
        }

        public Matrix(int size, Random random)
        {
            CheckSize(size, size);
            ArgumentNullException.ThrowIfNull(random);
            _values = new int[size, size];
            int row;
            int column;
            for (row = 0; row < size; row++)
            {
                for (column = 0; column < size; column++)
                {
                    int number = 2 * random.Next(1, 5);
                    int digit;
                    for (digit = 0; digit < 3; digit++)
                    {
                        number = number * 10
                            + 2 * random.Next(0, 5);
                    }
                    _values[row, column] = number;
                }
            }
        }

        public Matrix(int size)
        {
            CheckSize(size, size);
            _values = new int[size, size];
            int row;
            int column;
            for (row = 0; row < size; row++)
            {
                for (column = row; column < size; column++)
                {
                    if ((column - row) % 2 == 0)
                    {
                        _values[row, column] = row + 1;
                    }
                    else
                    {
                        _values[row, column] = size - column;
                    }
                }
            }
        }

        public Matrix(int[,] values)
        {
            ArgumentNullException.ThrowIfNull(values);
            CheckSize(values.GetLength(0), values.GetLength(1));
            _values = (int[,])values.Clone();
        }

        public int Rows
        {
            get
            {
                return _values.GetLength(0);
            }
        }

        public int Columns
        {
            get
            {
                return _values.GetLength(1);
            }
        }

        public int this[int row, int column]
        {
            get
            {
                return _values[row, column];
            }
        }

        private static void CheckSize(int rows, int columns)
        {
            if (rows <= 0 || columns <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(rows), "Размеры должны быть > 0.");
            }
        }

        public static Matrix CalculateExpression(
            Matrix a, Matrix b, Matrix c)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            ArgumentNullException.ThrowIfNull(c);
            return a.Transpose() - (b + c.Transpose());
        }

        public Matrix Transpose()
        {
            int[,] result = new int[Columns, Rows];
            int row;
            int column;
            for (row = 0; row < Rows; row++)
            {
                for (column = 0; column < Columns; column++)
                {
                    result[column, row] = _values[row, column];
                }
            }
            return new Matrix(result);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            return Combine(a, b, false);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            return Combine(a, b, true);
        }

        private static Matrix Combine(
            Matrix a, Matrix b, bool subtract)
        {
            ArgumentNullException.ThrowIfNull(a);
            ArgumentNullException.ThrowIfNull(b);
            if (a.Rows != b.Rows || a.Columns != b.Columns)
            {
                throw new ArgumentException(
                    "Размеры матриц должны совпадать.");
            }
            int[,] result = new int[a.Rows, a.Columns];
            int row;
            int column;
            for (row = 0; row < a.Rows; row++)
            {
                for (column = 0; column < a.Columns; column++)
                {
                    if (subtract)
                    {
                        result[row, column] = checked(
                            a[row, column] - b[row, column]);
                    }
                    else
                    {
                        result[row, column] = checked(
                            a[row, column] + b[row, column]);
                    }
                }
            }
            return new Matrix(result);
        }

        public List<Point> FindShortestPath()
        {
            int min = _values[0, 0];
            int max = min;
            int row;
            int column;
            for (row = 0; row < Rows; row++)
            {
                for (column = 0; column < Columns; column++)
                {
                    min = Math.Min(min, _values[row, column]);
                    max = Math.Max(max, _values[row, column]);
                }
            }
            List<Point> minima = new List<Point>();
            List<Point> maxima = new List<Point>();
            for (row = 0; row < Rows; row++)
            {
                for (column = 0; column < Columns; column++)
                {
                    Point point = new Point(row, column);
                    if (_values[row, column] == min)
                    {
                        minima.Add(point);
                    }
                    if (_values[row, column] == max)
                    {
                        maxima.Add(point);
                    }
                }
            }
            Point start = maxima[0];
            Point finish = minima[0];
            int best = int.MaxValue;
            foreach (Point high in maxima)
            {
                foreach (Point low in minima)
                {
                    int distance = Math.Max(
                        Math.Abs(high.X - low.X),
                        Math.Abs(high.Y - low.Y));
                    if (distance < best)
                    {
                        best = distance;
                        start = high;
                        finish = low;
                    }
                }
            }
            List<Point> path = new List<Point>();
            path.Add(start);
            while (start != finish)
            {
                start.X += Math.Sign(finish.X - start.X);
                start.Y += Math.Sign(finish.Y - start.Y);
                path.Add(start);
            }
            return path;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            int width = 1;
            int row;
            int column;
            foreach (int value in _values)
            {
                width = Math.Max(width, value.ToString().Length);
            }
            for (row = 0; row < Rows; row++)
            {
                for (column = 0; column < Columns; column++)
                {
                    if (column > 0)
                    {
                        result.Append(' ');
                    }
                    result.Append(_values[row, column]
                        .ToString().PadLeft(width));
                }
                result.AppendLine();
            }
            return result.ToString();
        }
    }
}
```

</details>

<details>
<summary>Toy.cs</summary>

```csharp
using System;

namespace Lab3
{
    public struct Toy
    {
        private string _name;
        private decimal _price;
        private int _minAge;
        private int _maxAge;

        public Toy(string name, decimal price,
            int minAge, int maxAge)
        {
            if (string.IsNullOrWhiteSpace(name) || price < 0
                || minAge < 0 || maxAge < minAge)
            {
                throw new ArgumentException(
                    "Некорректные сведения об игрушке.");
            }
            _name = name.Trim();
            _price = price;
            _minAge = minAge;
            _maxAge = maxAge;
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
            }
        }

        public int MinAge
        {
            get
            {
                return _minAge;
            }
            set
            {
                _minAge = value;
            }
        }

        public int MaxAge
        {
            get
            {
                return _maxAge;
            }
            set
            {
                _maxAge = value;
            }
        }
    }
}
```

</details>

<details>
<summary>FileTasks.cs</summary>

```csharp
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
```

</details>

<details>
<summary>ConsoleInput.cs</summary>

```csharp
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
```

</details>

<details>
<summary>Program.cs</summary>

```csharp
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
```

</details>
