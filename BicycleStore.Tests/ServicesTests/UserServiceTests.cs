using bicycle_store_web;
using bicycle_store_web.Interfaces;
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
    public class UserServiceTests
    {
        private UserService userService;
        private IShoppingCartService shoppingCartService;
        private Mock<IUserRepository> repository;
        public User User { get; set; }
        public UserServiceTests()
        {
            repository = new Mock<IUserRepository>();
            shoppingCartService = new Mock<IShoppingCartService>().Object;
            User = null;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetUser_ReturnUser(bool UserIsNull)
        {
            // Arrange.
            if (!UserIsNull)
                User = new User();

            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(User);
            userService = new UserService(shoppingCartService, repository.Object);

            // Act.
            var Result = userService.GetById(new int());

            // Assert.
            Result.Should().BeEquivalentTo(User);
        }

        [Fact]
        public void GetBicycles_ReturnList()
        {
            // Arrange.
            var List = new List<User>();

            repository.Setup(x => x.GetAll()).Returns(List);
            userService = new UserService(shoppingCartService, repository.Object);

            // Act.
            var Result = userService.GetUsers();

            // Assert.
            Assert.IsType<List<User>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveUser_ReturnBool(bool UserIsNull, bool ExpectedResult)
        {
            // Arrange.
            IFormFile Photo = null;

            if (!UserIsNull)
                User = new User();

            repository.Setup(x => x.Create(It.IsAny<User>())).Verifiable();
            repository.Setup(x => x.Update(It.IsAny<User>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(User);
            userService = new UserService(shoppingCartService, repository.Object);

            // Act.
            var Result = userService.SaveUser(new User(), Photo);

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void DeleteUser_ReturnBool(bool UserIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!UserIsNull)
                User = new User();

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(User);
            userService = new UserService(shoppingCartService, repository.Object);

            // Act.
            var Result = userService.DeleteUser(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}
