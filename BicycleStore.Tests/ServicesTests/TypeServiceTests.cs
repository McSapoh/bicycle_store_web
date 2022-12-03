using bicycle_store_web;
using bicycle_store_web.Interfaces;
using bicycle_store_web.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace BicycleStore.Tests.ServicesTests
{
    public class TypeServiceTests
    {
        private TypeService typeService;
        private Mock<ITypeRepository> repository;
        public TypeServiceTests()
        {
            repository = new Mock<ITypeRepository>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetBicycle_ReturnBicycle(bool TypeIsNull)
        {
            // Arrange.
            Type Type = null;

            if (!TypeIsNull)
                Type = new Mock<Type>().Object;

            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Type);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.GetType(new int());

            // Assert.
            Result.Should().BeEquivalentTo(Type);
        }

        [Fact]
        public void GetBicycles_ReturnList()
        {
            // Arrange.
            var List = new List<Type>();

            repository.Setup(x => x.GetAll()).Returns(List);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.GetBicycles();

            // Assert.
            Assert.IsType<List<Type>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveBicycle_ReturnBool(bool TypeIsNull, bool ExpectedResult)
        {
            // Arrange.
            Type Bicycle = null;

            if (!TypeIsNull)
                Bicycle = new Type();

            repository.Setup(x => x.Create(It.IsAny<Bicycle>())).Verifiable();
            repository.Setup(x => x.Update(It.IsAny<Bicycle>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Bicycle);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.SaveType(new Type());

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
            Type Bicycle = null;

            if (!BicycleIsNull)
                Bicycle = new Mock<Type>().Object;

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Bicycle);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.DeleteType(new int());

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
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.GetSelectList();

            // Assert.
            Assert.IsType<SelectList>(Result);
            Result.Should().BeEquivalentTo(SelectList);
        }
    }
}
