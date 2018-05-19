using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.ImmutableInterface.BusinessLogic
{
    class CarFactory
    {
        public ICar CreateCar(string model, DateTime issueDate, 
            int certificate)
        {
            var car = new Car
            {
                Model = model,
                IssueDate = issueDate,
                Certificate = certificate
            };
            
            return car;
        }
    }
}
