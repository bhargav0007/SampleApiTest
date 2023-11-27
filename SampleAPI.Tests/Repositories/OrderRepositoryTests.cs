using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SampleAPI.Controllers;
using SampleAPI.Entities;
using SampleAPI.Repositories;
using SampleAPI.Requests;

namespace SampleAPI.Tests.Repositories
{
    public class OrderRepositoryTests
    {
        private readonly Mock<IOrderRepository> _mockOrderRepository;


        public OrderRepositoryTests()
        {
            _mockOrderRepository = new Mock<IOrderRepository>();
        }

        [Fact]
        public async void GetAllOrders()
        {
            //Arrange
            List<Order> expectedOrders = new List<Order> {
                new Order { Id = Guid.NewGuid(), DescriptionName = "Mobile", EntryDate = DateTime.Now, Status = true },
                new Order { Id = Guid.NewGuid(), DescriptionName = "Laptop", EntryDate = DateTime.Now, Status = true }
            };

            var mockDbSet = new Mock<DbSet<Order>>();

            var mockDbContext = new Mock<SampleApiDbContext>();
            mockDbContext.Setup(c => c.Orders).Returns(mockDbSet.Object);

            var ordersRepository = new OrderRepository(mockDbContext.Object);

            //Act
            var orderResult = await ordersRepository.GetRecentOrders();

            // Assert
            Assert.NotNull(orderResult);
            Assert.Equal(expectedOrders.Count, orderResult.Count);
        }
        [Fact]
        public async void GetRecentOrders()
        {
            //Arrange
            List<Order> expectedOrders = new List<Order> {
                new Order { Id = Guid.NewGuid(), DescriptionName = "Mobile", EntryDate = DateTime.Now.AddDays(-2), Status = true },
                new Order { Id = Guid.NewGuid(), DescriptionName = "Laptop", EntryDate = DateTime.Now, Status = true },
                new Order { Id = Guid.NewGuid(), DescriptionName = "SmartWatch", EntryDate = DateTime.Now.AddDays(-3), Status = true },
            };

            var mockDbSet = new Mock<DbSet<Order>>();

            var mockDbContext = new Mock<SampleApiDbContext>();
            mockDbContext.Setup(c => c.Orders).Returns(mockDbSet.Object);

            var ordersRepository = new OrderRepository(mockDbContext.Object);

            //Act
            var orderResult = await ordersRepository.GetRecentOrders();

            // Assert
            Assert.NotNull(orderResult);
            Assert.Equal(1, orderResult.Count);
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
            var mockDbSet = new Mock<DbSet<Order>>();

            var mockDbContext = new Mock<SampleApiDbContext>();
            mockDbContext.Setup(c => c.Orders).Returns(mockDbSet.Object);

            var ordersRepository = new OrderRepository(mockDbContext.Object);

            //Act
            await ordersRepository.AddNewOrder(expectedNewOrders);
        }

        [Fact]
        public async void AddNewOrdersWithNull()
        {
            try
            {
                //Arrange
                AddOrderDto expectedNewOrders = null;
                var mockDbSet = new Mock<DbSet<Order>>();

                var mockDbContext = new Mock<SampleApiDbContext>();
                mockDbContext.Setup(c => c.Orders).Returns(mockDbSet.Object);

                var ordersRepository = new OrderRepository(mockDbContext.Object);

                //Act
                await ordersRepository.AddNewOrder(expectedNewOrders);
            }
            catch (NullReferenceException ex)
            {
                Assert.True(true);
            }

        }
    }
}