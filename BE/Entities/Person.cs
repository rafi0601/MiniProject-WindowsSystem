﻿//BS"D

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace BE
{
    [Serializable]
    public struct Name // IMPROVEMENT insert into Person
    {
        public Name(string lastName, string firstName) : this()
        {
            LastName = lastName;
            FirstName = firstName;
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }


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
        string IKey.Key => ID;

        public string ID { get; /*private*/set; }
        public Name Name { get; set; } = new Name();
        public DateTime Birthdate { get; /*private*/set; }
        public Gender Gender { get; /*private*/ set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; } = new Address();
        public string Password { get; set; }

        public Person()
        { }

        public Person(string id, Name name, DateTime birthdate,
            Gender gender, string phoneNumber, Address address, string password)
        {
            ID = id;
            Name = name;
            Birthdate = birthdate;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Address = address;
            Password = password;
        }

        public Person(Person person)
            : this(person.ID, person.Name, person.Birthdate, person.Gender, person.PhoneNumber, person.Address, person.Password)
        { }

        [XmlIgnore] //CHECK if need it since no setter
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