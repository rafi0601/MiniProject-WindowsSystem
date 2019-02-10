//BS"D

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace BE
{

    //public struct Id
    //{
    //    public string ID { get; set; }
    //}

    [Serializable]
    [DebuggerDisplay("ID={ID}, Name={Name}")]
    public abstract partial class Person : IKey //: DependencyObject    
    {
        string IKey.Key => ID;

        private string _iD; public string ID { get => _iD; /*private*/set => _iD = value.PadLeft(totalWidth: 9, paddingChar: '0'); }
        public PersonName Name { get; set; } = new PersonName();
        public DateTime Birthdate { get; /*private*/set; } = new DateTime();
        public Gender Gender { get; /*private*/ set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; } = new Address();
        public string Password { get; set; }


        public Person()
        { }

        public Person(string id, PersonName name, DateTime birthdate,
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

        //public override bool Equals(object obj) => this.ID == (obj as Person)?.ID;
        //
        //public override int GetHashCode() => base.GetHashCode();


    }

    //public class PersonComparer : IEqualityComparer<Person>
    //{
    //    public bool Equals(Person x, Person y) 
    //        => x is null && y is null ? true : x.ID == y.ID;
    //
    //    public int GetHashCode(Person obj)
    //        => obj.GetHashCode();//return obj.ID.GetHashCode();
    //
    //}
}