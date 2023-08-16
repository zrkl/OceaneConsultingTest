using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using JobPortal.DBContext;

namespace JobPortal.DBContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<CandidatEntity> Cabdidate { get; set; }
        public DbSet<OfferEntity> Offer { get; set; }
    }
}
