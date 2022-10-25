using System;
using bicycle_store_web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace bicycle_store_web
{
    public partial class bicycle_storeContext : DbContext
    {
        //    public bicycle_storeContext() {}

        public bicycle_storeContext(DbContextOptions<bicycle_storeContext> options) : base(options) { }

        public virtual DbSet<Bicycle> Bicycles { get; set; }
        public virtual DbSet<BicycleOrder> BicycleOrders { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Producer> Producers { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<ShoppingCart> ShopingCarts { get; set; }
        public virtual DbSet<ShoppingCartOrder> ShoppingCartOrders { get; set; }
    }
}
