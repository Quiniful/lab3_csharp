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
                ArgumentException.ThrowIfNullOrWhiteSpace(value);
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
            ArgumentException.ThrowIfNullOrWhiteSpace(_name);
            CheckPrice(_price);
            CheckAgeRange(_minAge, _maxAge);
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
            if (minAge < 0 || maxAge > 150 || maxAge < minAge)
            {
                throw new ArgumentException(
                    "Возраст должен быть 0-150, min <= max.");
            }
        }
    }
}
