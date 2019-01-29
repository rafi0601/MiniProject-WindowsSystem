//BS"D

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BE
{
    [Serializable]
    public struct Name // IMPROVEMENT insert into Person
    {
        private string _lastName;
        private string _firstName;

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Last name mustn't be null or empty or consists only white spaces", nameof(value));

                _lastName = value;
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("First name mustn't be null or empty or consists only white spaces", nameof(value));

                _firstName = value;
            }
        }

        public override string ToString()
        {
            return LastName + " " + FirstName;
        }
    }

    //public struct Id
    //{
    //    public string ID { get; set; }
    //}

    [Serializable]
    [DebuggerDisplay("ID={ID}, Name={Name}")]
    public abstract class Person : IKey //: DependencyObject    
    {
        private string _iD;
        private DateTime _birthdate;
        private string _phoneNumber;

        public string Key => ID;

        public string ID
        {
            get => _iD;
            set
            {
                if (_iD != default)
                    throw new Exception();

                if (string.IsNullOrWhiteSpace(value))
                    throw new CustomException(true, new ArgumentNullException("ID mustn't be null or empty or consists only white spaces"));

                if (value.Length > 9 || !uint.TryParse(value, out uint temp))
                    throw new ArgumentException("ID is not valid");

                while (value.Length < 9) // Pading zeroes to the begining
                    value = "0" + value;

                int counter = 0, incNum;
                for (int i = 0; i < 9; i++)
                {
                    incNum = (value[i] - '0') * (i % 2 + 1);
                    counter += incNum > 9 ? incNum - 9 : incNum;
                }

                if (counter % 10 != 0)
                    throw new ArgumentException("ID is not valid");

                _iD = value;
            }
        }
        public Name Name { get; set; } = new Name();
        public DateTime Birthdate
        {
            get => _birthdate;
            set
            {
                //if (_birthdate == default)
                //    throw new Exception();

                if (value > DateTime.Today || value.Year < DateTime.Today.Year - 120)
                    throw new ArgumentException("The Birthdate date is illogical", nameof(value));

                _birthdate = value;
            }
        }
        public Gender Gender { get; /*private*/ set; }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentNullException("Phone number mustn't be null or empty or consists only white spaces", nameof(value));

                if (!(ulong.TryParse(value, out ulong tmp) &&
                    value.Length == 10 && value.StartsWith("05") ||
                    value.Length == 13 && value.StartsWith("+9725")))
                    throw new ArgumentException("The phone number is not valid", nameof(value));

                _phoneNumber = value;
            }
        }
        public Address Address { get; set; } = new Address();

        //public string ID { get; /*private*/ set; }
        //public Name Name { get; set; } = new Name();
        //public DateTime Birthdate { get; /*private*/ set; }
        //public Gender Gender { get; /*private*/ set; }
        //public string PhoneNumber { get; set; }
        //public Address Address { get; set; } = new Address();

        public Person()
        { }

        public Person(string id, Name name, DateTime birthdate,
            Gender gender, string phoneNumber, Address address)
        {
            ID = id;
            Name = name;
            Birthdate = birthdate;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public Person(Person person)
            : this(person.ID, person.Name, person.Birthdate, person.Gender, person.PhoneNumber, person.Address)
        { }

        public uint AgeInYears
        {
            get
            {
                int age = DateTime.Today.Year - Birthdate.Year;
                return (uint)(Birthdate.AddYears(age) > DateTime.Today ? --age : age);
            }
        }


        public override string ToString()
        {
            return Name.ToString() + ", " + Gender + ", " + AgeInYears;
            //return this.ToStringProperty();
        }

        //      public bool Equals(Person x, Person y)
        //      {
        //          return true;
        //      }
        //
        //      public int GetHashCode(Person obj)
        //      {
        //          throw new NotImplementedException();
        //      }
    }
}