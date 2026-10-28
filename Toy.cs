using System;
using System.Xml.Serialization;

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
            _name = name;
            _price = price;
            _minAge = minAge;
            _maxAge = maxAge;
            Validate();
            _name = name.Trim();
        }

        [XmlElement(Order = 0)]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                CheckName(value);
                _name = value.Trim();
            }
        }

        [XmlElement(Order = 1)]
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                CheckPrice(value);
                _price = value;
            }
        }

        [XmlElement(Order = 3)]
        public int MinAge
        {
            get
            {
                return _minAge;
            }
            set
            {
                CheckAgeRange(value, _maxAge);
                _minAge = value;
            }
        }

        [XmlElement(Order = 2)]
        public int MaxAge
        {
            get
            {
                return _maxAge;
            }
            set
            {
                CheckAgeRange(_minAge, value);
                _maxAge = value;
            }
        }

        public void SetAgeRange(int minAge, int maxAge)
        {
            CheckAgeRange(minAge, maxAge);
            _minAge = minAge;
            _maxAge = maxAge;
        }

        public void Validate()
        {
            CheckName(_name);
            CheckPrice(_price);
            CheckAgeRange(_minAge, _maxAge);
        }

        private static void CheckName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Название не может быть пустым.",
                    nameof(name));
            }
        }

        private static void CheckPrice(decimal price)
        {
            if (price < 0 || decimal.Round(price, 2) != price)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Цена должна быть >= 0, до двух знаков.");
            }
        }

        private static void CheckAgeRange(
            int minAge, int maxAge)
        {
            if (minAge < 0 || minAge > 150)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minAge),
                    "Возраст должен быть 0-150.");
            }
            if (maxAge < minAge || maxAge > 150)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxAge),
                    "Верхний возраст должен быть min-150.");
            }
        }
    }
}
