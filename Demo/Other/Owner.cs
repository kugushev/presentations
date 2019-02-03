using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Other
{
    public interface IOwner
    {
        string Name { get; }
    }
    public class Owner : IOwner
    {
        public string Name { get; set; }
    }
    
}
