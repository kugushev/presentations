using Demo.ImmutableInterface.BusinessLogic;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Demo.ImmutableInterface.Presentation
{
    public class CarController
    {
        private readonly ICarService service;
        private readonly ICarRepository carRepository;
        public CarController(ICarService service)
        {
            this.service = service;
        }

        public void PostCar(Car car)
        {
            service.RegisterCar(car);
        }

        public IReadOnlyList<ICar> GetCar(string model)
        {
            return carRepository.Filter(c => c.Model == model);
        }
    }

    public class CarControllerTests
    {
        [Fact]
        public void PostCar_Test()
        {
            // arrange
            string json = @"
{
	""model"": ""Kia Rio"",
    ""issueDate"": ""2015.12.1"",
	""certificate"": ""ABCD12345"",
	""owners"": [{
		""name"": ""Aleksandr Kugushev""
    }]
}";
            var service = Substitute.For<ICarService>();
            var controller = new CarController(service);

            // act
            controller.PostCar(JsonConvert.DeserializeObject<Car>(json));

            // assert
            service.Received().RegisterCar(Arg.Is<ICar>(c => c.Model == "Kia Rio"));
        }
    }
}
