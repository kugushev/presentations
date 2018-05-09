using Demo.Other;
using System;
using System.Collections.Generic;

namespace Samples.ImmutableInterface
{
    class Car: ICar
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string ModelFull { get; set; }
        public string Mark { get; set; }
        public int Certificate { get; set; }
        public int IssuerId { get; set; }
        public string IssuePlace { get; set; }
        public DateTime IssueDate { get; set; }
        public IReadOnlyList<IOwner> Owners { get; set; }
    }

    interface ICar
    {
        int Id { get; }
        string Model { get; }
        string ModelFull { get; }
        string Mark { get; }
        int Certificate { get; }
        int IssuerId { get; }
        string IssuePlace { get; }
        DateTime IssueDate { get; }
        IReadOnlyList<IOwner> Owners { get; }
    }
}
