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

namespace BicycleStore.Tests
{
    public class BicycleServiceTests
    {
        private BicycleService bicycleService;
        private Mock<IBicycleRepository> repository;
        public BicycleServiceTests()
        {
            repository = new Mock<IBicycleRepository>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetBicycle_ReturnBicycle(bool BicycleIsNull)
        {
            // Arrange.
            Bicycle Bicycle = null;

            if (!BicycleIsNull)
                Bicycle = new Mock<Bicycle>().Object;

            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Bicycle);
            bicycleService = new BicycleService(repository.Object);

            // Act.
            var Result = bicycleService.GetBicycle(new int());

            // Assert.
            Result.Should().BeEquivalentTo(Bicycle);
        }

        [Fact]
        public void GetBicycles_ReturnList()
        {
            // Arrange.
            var List = new List<Bicycle>();

            repository.Setup(x => x.GetAll()).Returns(List);
            bicycleService = new BicycleService(repository.Object);

            // Act.
            var Result = bicycleService.GetBicycles();

            // Assert.
            Assert.IsType<List<Bicycle>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveBicycle_ReturnBool(bool BicycleIsNull, bool ExpectedResult)
        {
            // Arrange.
            Bicycle Bicycle = null;
            IFormFile Photo = null;

            if (!BicycleIsNull)
                Bicycle = new Bicycle();

            repository.Setup(x => x.Create(It.IsAny<Bicycle>())).Verifiable();
            repository.Setup(x => x.Update(It.IsAny<Bicycle>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Bicycle);
            bicycleService = new BicycleService(repository.Object);

            // Act.
            var Result = bicycleService.SaveBicycle(new Bicycle(), Photo);

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void DeleteBicycle_ReturnBool(bool BicycleIsNull, bool ExpectedResult)
        {
            // Arrange.
            Bicycle Bicycle = null;

            if (!BicycleIsNull)
                Bicycle = new Mock<Bicycle>().Object;

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Bicycle);
            bicycleService = new BicycleService(repository.Object);
            
            // Act.
            var Result = bicycleService.DeleteBicycle(new int());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Fact]
        public void GetSelectList_ReturnSelectList()
        {
            // Arrange.
            var SelectList = new SelectList(new Mock<IEnumerable>().Object);

            repository.Setup(x => x.GetSelectList()).Returns(SelectList);
            bicycleService = new BicycleService(repository.Object);

            // Act.
            var Result = bicycleService.GetSelectList();

            // Assert.
            Assert.IsType<SelectList>(Result);
            Result.Should().BeEquivalentTo(SelectList);
        }
    }
}
