using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Builder
{
    class Car
    {
        public Car(Builder builder)
        {
            Id = builder.Id;
            Model = builder.Model;
            ModelFull = builder.ModelFull;
            Mark = builder.Mark;
            Certificate = builder.Certificate;
            IssuerId = builder.IssuerId;
            IssuePlace = builder.IssuePlace;
            IssueDate = builder.IssueDate;
            Owners = builder.Owners;
        }
        /*---*/
        public int Id { get; }
        public string Model { get; }
        public string ModelFull { get; }
        public string Mark { get; }
        public int Certificate { get; }
        public int IssuerId { get; }
        public string IssuePlace { get; }
        public DateTime IssueDate { get; }
        public IReadOnlyList<IOwner> Owners { get; }

        public class Builder
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
    }
}
