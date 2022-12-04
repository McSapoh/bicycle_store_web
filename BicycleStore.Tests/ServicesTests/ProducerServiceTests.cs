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
    public class ProducesServiceTests
    {
        private ProducerService producerService;
        private Mock<IProducerRepository> repository;
        public Producer Producer;
        public ProducesServiceTests()
        {
            repository = new Mock<IProducerRepository>();
            Producer = null;
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetProducer_ReturnProducer(bool ProducerIsNull)
        {
            // Arrange.
            if (!ProducerIsNull)
                Producer = new Producer();

            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Producer);
            producerService = new ProducerService(repository.Object);

            // Act.
            var Result = producerService.GetById(new int());

            // Assert.
            Result.Should().BeEquivalentTo(Producer);
        }

        [Fact]
        public void GetProducers_ReturnList()
        {
            // Arrange.
            var List = new List<Producer>();

            repository.Setup(x => x.GetAll()).Returns(List);
            producerService = new ProducerService(repository.Object);

            // Act.
            var Result = producerService.GetProducers();

            // Assert.
            Assert.IsType<List<Producer>>(Result);
            Result.Should().BeEquivalentTo(List);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void SaveProducer_ReturnBool(bool ProducerIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!ProducerIsNull)
                Producer = new Producer();

            repository.Setup(x => x.Create(It.IsAny<Producer>())).Verifiable();
            repository.Setup(x => x.Update(It.IsAny<Producer>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Producer);
            producerService = new ProducerService(repository.Object);

            // Act.
            var Result = producerService.SaveProducer(new Producer());

            // Assert.
            Assert.IsType<bool>(Result);
            Assert.Equal(ExpectedResult, Result);
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(true, true)]
        public void DeleteProducer_ReturnBool(bool ProducerIsNull, bool ExpectedResult)
        {
            // Arrange.
            if (!ProducerIsNull)
                Producer = new Producer();

            repository.Setup(x => x.Delete(It.IsAny<int>())).Verifiable();
            repository.Setup(x => x.GetById(It.IsAny<int>())).Returns(Producer);
            producerService = new ProducerService(repository.Object);

            // Act.
            var Result = producerService.DeleteProducer(new int());

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
            producerService = new ProducerService(repository.Object);

            // Act.
            var Result = producerService.GetSelectList();

            // Assert.
            Assert.IsType<SelectList>(Result);
            Result.Should().BeEquivalentTo(SelectList);
        }
    }
}
