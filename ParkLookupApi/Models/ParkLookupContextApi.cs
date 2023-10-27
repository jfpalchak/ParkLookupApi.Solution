
namespace ParkLookupApi.Models;

public class ParkLookupApiContext : IdentityDbContext<ApplicationUser>
{
  public DbSet<Park> Parks { get; set; }

  public ParkLookupApiContext(DbContextOptions<ParkLookupAPiContext> options) : base(options) { }
  
}