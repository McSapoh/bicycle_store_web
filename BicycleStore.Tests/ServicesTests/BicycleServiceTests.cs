using bicycle_store_web;
using bicycle_store_web.Interfaces;
using bicycle_store_web.Repositories;
using bicycle_store_web.Services;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Configuration;
using Xunit;

namespace BicycleStore.Tests
{
    public class BicycleServiceTests
    {
        //private readonly BicycleService bicycleService;
        //public BicycleServiceTests()
        //{
        //    var IRepo = new Mock<IGenericRepository<Bicycle>>();
        //    IRepo.Setup(x => x.Delete(1)).Returns(true);
        //    bicycleService = new BicycleService(IRepo.Object);

        //}
        //[Theory]
        //[InlineData(1)]
        //public void BicycleService_DeleteBicycle_ReturnIActionResult(int id)
        //{
        //    var result = bicycleService.DeleteBicycle(id);
        //    Assert.NotNull(result);
        //}
    }
}
