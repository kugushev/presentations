using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Constructor
{
    class RealEntity
    {
        public RealEntity(int id, string firstName, string secondName, string role, int age, int pin, string address, DateTime birthDay)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            Role = role;
            Age = age;
            Pin = pin;
            Address = address;
            BirthDay = birthDay;
        }

        public int Id { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string Role { get; }
        public int Age { get; }
        public int Pin { get; }
        public string Address { get; }
        public DateTime BirthDay { get; }
    }
}
