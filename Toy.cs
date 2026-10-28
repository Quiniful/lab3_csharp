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
