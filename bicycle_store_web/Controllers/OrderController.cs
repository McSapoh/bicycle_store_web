﻿using bicycle_store_web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bicycle_store_web.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly bicycle_storeContext _db;
        private readonly BicycleService bicycleService;
        private readonly UserService userService;
        private readonly OrderService orderService;
        public OrderController(ILogger<OrderController> logger, bicycle_storeContext context,
            BicycleService bicycleService, UserService userService, OrderService orderService)
        {
            _logger = logger;
            _db = context;
            this.bicycleService = bicycleService;
            this.userService = userService;
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateOrder() => orderService.CreateOrder(userService.GetUserId(User.Identity.Name));
    }
}