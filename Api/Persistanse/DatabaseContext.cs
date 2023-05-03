using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Persistanse
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            :base (options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ingridient> Ingridients { get; set; }
        public DbSet<FoodSlot> FoodSlots { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set;}
        public DbSet<IngridientComposition> IngridientCompositions { get; set; }
    }
}
