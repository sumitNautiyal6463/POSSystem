using Microsoft.EntityFrameworkCore;
using POSSystem.DAL;

namespace POSSystem.POSDBContext
{
    public class POSContext : DbContext
    {
        public POSContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
    }
}
