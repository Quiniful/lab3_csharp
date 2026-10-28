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
