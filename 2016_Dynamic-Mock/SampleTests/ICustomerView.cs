using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.ComponentModel;

namespace SampleTests
{
    public class CUstomerViewModelTest
    {
        [TestMethod]
        public void Save_CustomerIsNotNull_GetsAddedToRepository()
        {
            // arrange
            Mock<IContainer> mockContainer = new Mock<IContainer>();
            Mock<ICustomerView> mockView = new Mock<ICustomerView>();

            CustomerViewModel viewModel = new CustomerViewModel(mockView.Object,
                mockContainer.Object);
            viewModel.CustomersRepository = new CustomersRepository();
            viewModel.Customer = new Mock<Customer>().Object;

            // act
            viewModel.Save();

            // assert
            Assert.IsTrue(viewModel.CustomersRepository.Count == 1);
        }
    }


    public class Customer
    {
    }

    public class CustomersRepository
    {
        public CustomersRepository()
        {
        }

        public int Count;
    }

    public class CustomerViewModel
    {
        public CustomerViewModel(object a, object b)
        {

        }

        public CustomersRepository CustomersRepository;

        public Customer Customer;

        internal void Save()
        {
            throw new NotImplementedException();
        }
    }

    public interface ICustomerView
    {
    }
}