using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.ImmutableInterface
{
    class RealEntity: IRealEntity
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

    interface IRealEntity
    {
        int Id { get; }
        string FirstName { get; }
        string SecondName { get; }
        string Role { get; }
        int Age { get; }
        int Pin { get; }
        string Address { get; }
        DateTime BirthDay { get; }
    }
}
