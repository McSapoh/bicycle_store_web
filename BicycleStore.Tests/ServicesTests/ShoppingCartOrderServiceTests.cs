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
    public class ShoppingCartOrderServiceTests
    {
        private ShoppingCartOrderService shoppingCartOrderService;
        private readonly Mock<IBicycleService> _bicycleService;
        private readonly Mock<IShoppingCartOrderRepository> repository;
        public ShoppingCartOrder ShoppingCartOrder { get; set; }
        public Bicycle Bicycle { get; set; }

        public ShoppingCartOrderServiceTests()
        {
            _bicycleService = new Mock<IBicycleService>();
            repository = new Mock<IShoppingCartOrderRepository>();
            ShoppingCartOrder = null;
            Bicycle = null;
        }

        [Fact]
        public void GetShoppingCartOrders_ReturnList()
        {
            // Arrange.
            var List = new List<ShoppingCartOrder>();

            repository.Setup(x => x.GetAll(It.IsAny<int>())).Returns(List);
            shoppingCartOrderService = new ShoppingCartOrderService(repository.Object, _bicycleService.Object);

            // Act.
            var Result = shoppingCartOrderService.GetShoppingCartOrders(new int());

            // Assert.
            Assert.IsType<List<ShoppingCartOrder>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(true, false, true, true)]
        [InlineData(true, true, false, false)]
        public void SaveShoppingCartOrder_ReturnBool(bool ShoppingCartIsNotNull, bool ShoppingCartOrderIsNotNull, 
            bool IsEnoughtQuantity, bool ExpectedResult)
        {
            // Arrange.
            if (ShoppingCartIsNotNull)
            {
                ShoppingCartOrder = new ShoppingCartOrder();
                Bicycle = new Bicycle();

                if (ShoppingCartOrderIsNotNull)
                    repository.Setup(x => x.Update(It.IsAny<ShoppingCartOrder>())).Verifiable();
                else
                    repository.Setup(x => x.Create(It.IsAny<ShoppingCartOrder>())).Verifiable();

                if (IsEnoughtQuantity)
                {
                    Bicycle.Quantity = 1;
                    ShoppingCartOrder.Quantity = 0;
                }
                else
                {
                    Bicycle.Quantity = 0;
                    ShoppingCartOrder.Quantity = 1;
                }

                repository.Setup(x => x.CheckExistence(It.IsAny<int>(), It.IsAny<int>())).Returns(ShoppingCartOrderIsNotNull);
                repository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<int>())).Returns(ShoppingCartOrder);
                _bicycleService.Setup(x => x.GetBicycle(It.IsAny<int>())).Returns(Bicycle);
            }


            repository.Setup(x => x.CheckExistence(It.IsAny<int>())).Returns(ShoppingCartIsNotNull);
            shoppingCartOrderService = new ShoppingCartOrderService(repository.Object, _bicycleService.Object);

            // Act.
            var Result = shoppingCartOrderService.SaveShoppingCartOrder(new int(), new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }


        [Theory]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void DeleteShoppingCartOrder_ReturnBool(bool ShoppingCartOrderIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!ShoppingCartOrderIsNull)
                ShoppingCartOrder = new ShoppingCartOrder();

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(ShoppingCartOrder);
            shoppingCartOrderService = new ShoppingCartOrderService(repository.Object, _bicycleService.Object);

            // Act.
            var Result = shoppingCartOrderService.DeleteShoppingCartOrder(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}
