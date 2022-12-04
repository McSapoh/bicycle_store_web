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
    public class TypeServiceTests
    {
        private TypeService typeService;
        private Mock<ITypeRepository> repository;
        public bicycle_store_web.Type Type;
        public TypeServiceTests()
        {
            repository = new Mock<ITypeRepository>();
            Type = null;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetType_ReturnType(bool TypeIsNull)
        {
            // Arrange.
            if (!TypeIsNull)
                Type = new bicycle_store_web.Type();

            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Type);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.GetById(new int());

            // Assert.
            Result.Should().BeEquivalentTo(Type);
        }

        [Fact]
        public void GetTypes_ReturnList()
        {
            // Arrange.
            var List = new List<bicycle_store_web.Type>();

            repository.Setup(x => x.GetAll()).Returns(List);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.GetTypes();

            // Assert.
            Assert.IsType<List<bicycle_store_web.Type>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveType_ReturnBool(bool TypeIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!TypeIsNull)
                Type = new bicycle_store_web.Type();

            repository.Setup(x => x.Create(It.IsAny<bicycle_store_web.Type> ())).Verifiable();
            repository.Setup(x => x.Update(It.IsAny<bicycle_store_web.Type>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Type);
            typeService = new TypeService(repository.Object);

            // Act.
            var Result = typeService.SaveType(new bicycle_store_web.Type());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void DeleteType_ReturnBool(bool TypeIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!TypeIsNull)
                Type = new bicycle_store_web.Type();

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Type);
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
