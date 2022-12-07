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
    public class ShoppingCartServiceTests
    {
        private ShoppingCartService shoppingCartService;
        private readonly Mock<IShoppingCartOrderService> _shoppingCartOrderService;
        private readonly Mock<IShoppingCartRepository> repository;

        public ShoppingCartServiceTests()
        {
            _shoppingCartOrderService = new Mock<IShoppingCartOrderService>();
            repository = new Mock<IShoppingCartRepository>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddToShoppingCart_ReturnBool(bool ExpectedResult)
        {
            // Arrange.
            _shoppingCartOrderService.Setup(x => x.SaveShoppingCartOrder(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(ExpectedResult);
            shoppingCartService = new ShoppingCartService(_shoppingCartOrderService.Object, repository.Object);

            // Act.
            var Result = shoppingCartService.AddToShoppingCart(new int(), new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Fact]
        public void GetShoppingCart_ReturnList()
        {
            // Arrange.
            var List = new List<ShoppingCartOrder>();

            _shoppingCartOrderService.Setup(x => x.GetShoppingCartOrders(It.IsAny<int>())).Returns(List);
            shoppingCartService = new ShoppingCartService(_shoppingCartOrderService.Object, repository.Object);

            // Act.
            var Result = shoppingCartService.GetShoppingCart(new int());

            // Assert.
            Assert.IsType<List<ShoppingCartOrder>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Fact]
        public void GetShoppingCartId_ReturnString()
        {
            var ExpectedResult = 5;

            repository.Setup(x => x.GetShoppingCartId(It.IsAny<int>())).Returns(ExpectedResult);
            shoppingCartService = new ShoppingCartService(_shoppingCartOrderService.Object, repository.Object);

            // Act.
            var Result = shoppingCartService.GetShoppingCartId(new int());

            // Assert.
            Assert.IsType<int>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RemoveFromShoppingCart_ReturnBool(bool ExpectedResult)
        {
            // Arrange.
            _shoppingCartOrderService.Setup(x => x.DeleteShoppingCartOrder(It.IsAny<int>()))
                .Returns(ExpectedResult);
            shoppingCartService = new ShoppingCartService(_shoppingCartOrderService.Object, repository.Object);

            // Act.
            var Result = shoppingCartService.RemoveFromShoppingCart(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}
