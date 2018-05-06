using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.ImmutableInterface.BusinessLogic
{
    public interface ICar
    {
        string Model { get; }
        DateTime IssueDate { get; }
        string Certificate { get; }
        IReadOnlyList<IOwner> Owners { get; }
    }

    public class Car : ICar
    {
        public string Model { get; set; }

        public DateTime IssueDate { get; set; }

        public string Certificate { get; set; }

        public List<Owner> Owners { get; set; }
        IReadOnlyList<IOwner> ICar.Owners => Owners;
    }
}
