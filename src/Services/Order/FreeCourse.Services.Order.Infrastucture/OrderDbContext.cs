using FreeCourse.Services.Order.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Infrastucture
{
    public class OrderDbContext : DbContext
    {
        public const string DEFAULT_SCHEME = "ordering";

        public OrderDbContext(DbContextOptions<OrderDbContext> options): base(options)
        {

        }

        public DbSet<Domain.OrderAggregate.Order> Orders { get; set; }
        public DbSet<Domain.OrderAggregate.OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.OrderAggregate.Order>().ToTable("Orders", DEFAULT_SCHEME);

            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().ToTable("OrderItems", DEFAULT_SCHEME);
            modelBuilder.Entity<Domain.OrderAggregate.OrderItem>().Property(x=>x.Price).HasColumnType("decimal(18,2)");
            
            modelBuilder.Entity<Domain.OrderAggregate.Order>().OwnsOne(x => x.Address).WithOwner();

            base.OnModelCreating(modelBuilder);
        }
    }
}
