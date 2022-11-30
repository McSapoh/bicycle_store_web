using bicycle_store_web;
using bicycle_store_web.Interfaces;
using bicycle_store_web.Repositories;
using bicycle_store_web.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Configuration;
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
        [InlineData(true, "Delete successful")]
        [InlineData(false, "Error while Deleting")]
        public void BicycleService_DeleteBicycle_ReturnIActionResult(bool Success, string Message)
        {
            // Arrangef.
            repository.Setup(x => x.Delete(new int())).Returns(Success);
            bicycleService = new BicycleService(repository.Object);
            var typeResult = new JsonResult(new { success = Success, message = Message });
            
            // Act.
            var result = (JsonResult)bicycleService.DeleteBicycle(new int());

            // Assert.
            Assert.NotNull(result);
            Assert.IsType<JsonResult>(result);
            result.Should().BeEquivalentTo(typeResult);
        }
    }
}
