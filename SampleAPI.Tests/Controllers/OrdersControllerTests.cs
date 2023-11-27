using FluentAssertions;
using FluentAssertions.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;

namespace SampleAPI.Tests.Controllers
{
    public class OrdersControllerTests
    {

        private readonly Mock<IOrderRepository> _mockOrderRepository;

        public OrdersControllerTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
        }


        [Fact]
        public async void GetRecentOrders()
        {
            //Arrange
            List<Order> expectedOrders = new List<Order> {
                new Order { Id = Guid.NewGuid(), DescriptionName = "Mobile", EntryDate = DateTime.Now, Status = true },
                new Order { Id = Guid.NewGuid(), DescriptionName = "Laptop", EntryDate = DateTime.Now, Status = true }
            };

            _mockOrderRepository.Setup(x => x.GetRecentOrders()).ReturnsAsync(expectedOrders);

            var ordersController = new OrdersController(_mockOrderRepository.Object);

            //Act
            var orderResult = await ordersController.GetOrders();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(orderResult.Result);
            var orders = Assert.IsType<List<Order>>(okResult.Value);
            var orderCount = orders.Count;
            Assert.Equal(expectedOrders.Count, orderCount);
        }

        [Fact]
        public async void AddNewOrders()
        {
            //Arrange
            AddOrderDto expectedNewOrders = new AddOrderDto
            {
                DescriptionName = "Mobile",
                EntryDate = DateTime.Now.AddDays(-3)
            };

            _mockOrderRepository.Setup(x => x.AddNewOrder(expectedNewOrders));

            var ordersController = new OrdersController(_mockOrderRepository.Object);

            //Act
            ActionResult actionResult = await ordersController.AddNewOrder(expectedNewOrders);

            // Assert
            OkObjectResult okResult = Assert.IsType<OkObjectResult>(actionResult);

        }

        [Fact]
        public async void AddNewOrderWithNull()
        {
            //Arrange
            AddOrderDto? expectedNewOrders = null;
           

            _mockOrderRepository.Setup(x => x.AddNewOrder(expectedNewOrders));

            var ordersController = new OrdersController(_mockOrderRepository.Object);

            //Act
            ActionResult actionResult = await ordersController.AddNewOrder(expectedNewOrders);

            // Assert
            BadRequestObjectResult badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.Equal("Order object is null", badRequestResult.Value);

            _mockOrderRepository.Verify(service => service.AddNewOrder(It.IsAny<AddOrderDto>()), Times.Never);
        }
    }
}
