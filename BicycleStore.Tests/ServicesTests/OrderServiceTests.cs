using bicycle_store_web;
using bicycle_store_web.Enums;
using bicycle_store_web.Interfaces;
using bicycle_store_web.Models;
using bicycle_store_web.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace BicycleStore.Tests.ServicesTests
{
    public class OrderServiceTests
    {
        private OrderService orderService;
        private readonly Mock<IShoppingCartService> shoppingCartService;
        private readonly Mock<IShoppingCartOrderService> _shoppingCartOrderService;
        private readonly Mock<IOrderRepository> _orderRepo;
        private readonly Mock<IBicycleService> bicycleService;
        private readonly Mock<IBicycleRepository> _bicycleRepo;
        private readonly Mock<IBicycleOrderRepository> _bicycleOrderRepo;
        private readonly Mock<IShoppingCartOrderRepository> _shoppingCartOrderRepo;
        public Order Order { get; set; }
        public OrderServiceTests()
        {
            shoppingCartService = new Mock<IShoppingCartService>();
            _shoppingCartOrderService = new Mock<IShoppingCartOrderService>();
            _orderRepo = new Mock<IOrderRepository>();
            bicycleService = new Mock<IBicycleService>();
            _bicycleRepo = new Mock<IBicycleRepository>();
            _bicycleOrderRepo = new Mock<IBicycleOrderRepository>();
            _shoppingCartOrderRepo = new Mock<IShoppingCartOrderRepository>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void GetAdminOrders_ReturnList(int QuantityOfOrders)
        {
            // Arrange.
            var UserOrders = new List<Order>();
            for (int i = 1; i < QuantityOfOrders; i++)
                UserOrders.Add(new Order() { OrderId = i });

            var bicycleOrders = new List<BicycleOrder>() { new BicycleOrder() };
            _orderRepo.Setup(x => x.GetAll()).Returns(UserOrders);
            _bicycleOrderRepo.Setup(x => x.GetAll(It.IsAny<int>())).Returns(bicycleOrders);

            var ExpectedResult = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                foreach (var bicycleOrder in bicycleOrders)
                {
                    ExpectedResult.Add(bicycleOrder);
                }
            }

            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object,
                _bicycleOrderRepo.Object
            );
            // Act.
            var Result = orderService.GetAdminOrders();

            //// Assert.
            Assert.IsType<List<BicycleOrder>>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        public void GetUserOrders_ReturnList(int QuantityOfOrders)
        {
            // Arrange.
            var UserOrders = new List<Order>();
            for (int i = 1; i < QuantityOfOrders; i++)
                UserOrders.Add(new Order() { OrderId = i });

            var bicycleOrders = new List<BicycleOrder>() { new BicycleOrder() };
            _orderRepo.Setup(x => x.GetAll(It.IsAny<int>())).Returns(UserOrders);
            _bicycleOrderRepo.Setup(x => x.GetAll(It.IsAny<int>())).Returns(bicycleOrders);

            var ExpectedResult = new List<BicycleOrder>();
            foreach (var userOrder in UserOrders)
            {
                foreach (var bicycleOrder in bicycleOrders)
                {
                    ExpectedResult.Add(bicycleOrder);
                }
            }

            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object,
                _bicycleOrderRepo.Object
            );
            // Act.
            var Result = orderService.GetUserOrders(new int());

            //// Assert.
            Assert.IsType<List<BicycleOrder>>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(1, 5, 5)]
        [InlineData(25, 10, 250)]
        public void GetTotalCost_ReturnInt(int Quantity, uint Price, int ExpectedResult)
        {
            // Arrange.
            var List = new List<ShoppingCartOrder>()
            {
                new ShoppingCartOrder() { Quantity = Quantity, Bicycle = new Bicycle() { Price = Price } }
            };
            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object,
                _bicycleOrderRepo.Object
            );

            // Act.
            var Result = orderService.GetTotalCost(List);

            // Assert.
            Assert.IsType<int>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(true)]
        public void CreateOrder_ReturnsBool(bool ExpectedResult)
        {
            // Arrange.
            var List = new List<ShoppingCartOrder>()
            {
                new ShoppingCartOrder() { Quantity = 1, Bicycle = new Bicycle() { Price = 1 } }
            };
            shoppingCartService.Setup(x => x.GetShoppingCartId(It.IsAny<int>())).Returns(new int());
            shoppingCartService.Setup(x => x.GetShoppingCart(It.IsAny<int>())).Returns(List);
            _orderRepo.Setup(x => x.GetMaxId()).Returns(new int());
            bicycleService.Setup(x => x.GetBicycle(It.IsAny<int>())).Returns(new Bicycle() { Quantity = UInt16.MaxValue});
            _orderRepo.Setup(x => x.Create(It.IsAny<Order>())).Verifiable();
            _bicycleOrderRepo.Setup(x => x.Create(It.IsAny<BicycleOrder>())).Verifiable();
            _bicycleRepo.Setup(x => x.Create(It.IsAny<Bicycle>())).Verifiable();
            shoppingCartService.Setup(x => x.ClearShoppingCart(It.IsAny<int>())).Verifiable();

            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object,
                _bicycleOrderRepo.Object
            );

            // Act.
            var Result = orderService.CreateOrder(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, null, false)]
        [InlineData(true, "Sended", true)]
        public void SendOrder_ReturnBool(bool OrderIsNotNull, string Status, bool ExpectedResult)
        {
            // Arrange.
            if (OrderIsNotNull)
                Order = new Order() { Status = Status };
            _orderRepo.Setup(x => x.Update(It.IsAny<Order>())).Verifiable();
            _orderRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(Order);
            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object,
                _bicycleOrderRepo.Object
            );

            // Act.
            var Result = orderService.SendOrder(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, null, false)]
        [InlineData(true, "Received", true)]
        public void ConfirmReceipt_ReturnBool(bool OrderIsNotNull, string Status, bool ExpectedResult)
        {
            // Arrange.
            if (OrderIsNotNull)
                Order = new Order() { Status = Status };
            _orderRepo.Setup(x => x.Update(It.IsAny<Order>())).Verifiable();
            _orderRepo.Setup(x => x.GetById(It.IsAny<int>())).Returns(Order);
            orderService = new OrderService(
                bicycleService.Object, shoppingCartService.Object, _orderRepo.Object, _bicycleRepo.Object, 
                _bicycleOrderRepo.Object
            );

            // Act.
            var Result = orderService.ConfirmReceipt(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}
