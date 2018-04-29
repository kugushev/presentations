using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Builder
{
    class RealEntity
    {
        public RealEntity(Builder builder)
        {
            Id = builder.Id;
            FirstName = builder.FirstName;
            SecondName = builder.SecondName;
            Role = builder.Role;
            Age = builder.Age;
            Pin = builder.Pin;
            Address = builder.Address;
            BirthDay = builder.BirthDay;
        }
        /*---*/
        public int Id { get; }
        public string FirstName { get; }
        public string SecondName { get; }
        public string Role { get; }
        public int Age { get; }
        public int Pin { get; }
        public string Address { get; }
        public DateTime BirthDay { get; }

        public class Builder
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string Role { get; set; }
            public int Age { get; set; }
            public int Pin { get; set; }
            public string Address { get; set; }
            public DateTime BirthDay { get; set; }
        }
    }
}
