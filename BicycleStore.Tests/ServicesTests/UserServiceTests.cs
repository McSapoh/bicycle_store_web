using bicycle_store_web;
using bicycle_store_web.Enums;
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
        private Mock<IShoppingCartService> shoppingCartService;
        private Mock<IUserRepository> repository;
        public User User { get; set; }
        public UserServiceTests()
        {
            repository = new Mock<IUserRepository>();
            shoppingCartService = new Mock<IShoppingCartService>();
            User = null;
        }

        [Fact]
        public void GetUserId_ReturnInt()
        {
            var ExpectedResult = 5;

            repository.Setup(x => x.GetUserId(It.IsAny<string>())).Returns(ExpectedResult);
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.GetUserId("");

            // Assert.
            Assert.IsType<int>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Fact]
        public void GetUserRole_ReturnString()
        {
            var ExpectedResult = "Role";

            repository.Setup(x => x.GetUserRole(It.IsAny<int>())).Returns(ExpectedResult);
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.GetUserRole(new int());

            // Assert.
            Assert.IsType<string>(Result);
            Assert.Equal(ExpectedResult, Result);
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
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.GetById(new int());

            // Assert.
            Result.Should().BeEquivalentTo(User);
        }

        [Fact]
        public void GetUsers_ReturnList()
        {
            // Arrange.
            var List = new List<User>();

            repository.Setup(x => x.GetAll()).Returns(List);
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.GetUsers();

            // Assert.
            Assert.IsType<List<User>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(true, null)]
        [InlineData(false, "SuperAdmin")]
        [InlineData(false, "Admin")]
        [InlineData(false, "User")]
        public void ChangePermisions_ReturnBool(bool UserIsNull, string UserRole)
        {
            // Arrange.
            if (!UserIsNull)
                User = new User() { Role = UserRole };

            repository.Setup(x => x.Update(It.IsAny<User>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(User);
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.ChangePermisions(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(!UserIsNull, Result);
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
            shoppingCartService.Setup(x => x.CreateShopingCart(It.IsAny<int>())).Verifiable();
            userService = new UserService(shoppingCartService.Object, repository.Object);

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
            userService = new UserService(shoppingCartService.Object, repository.Object);

            // Act.
            var Result = userService.DeleteUser(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }
    }
}
