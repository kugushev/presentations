using Demo.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.ImmutableInterface.BusinessLogic
{
    class CarFactory
    {
        private readonly ICarService carService;
        public CarFactory(ICarService carService)
        {
            this.carService = carService;
        }

        public ICar CreateCar(string model, DateTime issueDate, 
            string certificate)
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
