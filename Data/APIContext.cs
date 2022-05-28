using Microsoft.EntityFrameworkCore;
using EcomApi.Models;

namespace EcomApi.Data
{
    public class APIContext : DbContext
    {
        public APIContext(DbContextOptions<APIContext> options)
            : base(options)
        {
        }

        public DbSet<EcomApi.Models.Product> Product { get; set; } = null!;

        public DbSet<EcomApi.Models.Basket> Basket { get; set; } = null!;
    }
}
