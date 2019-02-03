using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Samples.Constructor
{
    class Car
    {
        public Car(int id, string model, string modelFull, string mark, int certificate, int issuerId,
            string issuePlace, DateTime issueDate, IReadOnlyList<IOwner> owners)
        {
            Id = id;
            Model = model;
            ModelFull = modelFull;
            Mark = mark;
            Certificate = certificate;
            IssuerId = issuerId;
            IssuePlace = issuePlace;
            IssueDate = issueDate;
            Owners = owners;
        }
        /*…*/
        public int Id { get; }
        public string Model { get; }
        public string ModelFull { get; }
        public string Mark { get; }
        public int Certificate { get; }
        public int IssuerId { get; }
        public string IssuePlace { get; }
        public DateTime IssueDate { get; }
        public IReadOnlyList<IOwner> Owners { get; }
    }
}
