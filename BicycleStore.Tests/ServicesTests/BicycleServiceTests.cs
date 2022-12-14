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
using System.Linq;
using Xunit;

namespace BicycleStore.Tests.ServicesTests
{
    public class BicycleServiceTests
    {
        private BicycleService bicycleService;
        private Mock<IBicycleRepository> repository;
        public Bicycle Bicycle { get; set; }
        public BicycleServiceTests()
        {
            repository = new Mock<IBicycleRepository>();
            Bicycle = null;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetBicycle_ReturnBicycle(bool BicycleIsNull)
        {
            // Arrange.
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

        [Fact]
        public void GetBicyclesWithoutPhoto_ReturnAnonymousList()
        {
            // Arrange.
            //var a = (Id: 1, Name: "Contessa Active 60", WheelDiameter: 29, Price: 16800, Quantity: 0, TypeId: 1, CountryId: 1, ProducerId: 1);
            var a = (Id: 1, Name: "Contessa Active 60");
            var List = new[] { a }.ToList();

            repository.Setup(x => x.GetAllWithoutPhoto()).Returns(List);
            bicycleService = new BicycleService(repository.Object);

            // Act.
            var Result = bicycleService.GetBicyclesWithoutPhoto();

            // Assert.
            Assert.IsType<List<(int Id, string Name)>>(Result);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveBicycle_ReturnBool(bool BicycleIsNull, bool ExpectedResult)
        {
            // Arrange.
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
