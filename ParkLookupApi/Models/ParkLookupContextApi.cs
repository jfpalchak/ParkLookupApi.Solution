using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ParkLookupApi.Models;

public class ParkLookupApiContext : IdentityDbContext<ApplicationUser>
{
  public DbSet<Park> Parks { get; set; }

  public ParkLookupApiContext(DbContextOptions<ParkLookupApiContext> options) : base(options) { }
  
  protected override void OnModelCreating(ModelBuilder builder)
  {
    base.OnModelCreating(builder);

    builder.Entity<Park>()
      .HasData(
        new Park 
        { 
          ParkId = 1, 
          Name = "Acadia National Park", 
          Location = "Maine", 
          Description = "Today, Acadia preserves about 40,000 acres of Atlantic coast shoreline, mixed hardwood and spruce/fir forest, mountains, and lakes, as well as several offshore islands.", 
          Category = "National Park" 
        },
        new Park 
        { 
          ParkId = 2, 
          Name = "Badlands National Park", 
          Location = "South Dakota", 
          Description = "Prairie grasslands support bison, bighorn sheep, deer, pronghorn antelope, swift fox, and black-footed ferrets.", 
          Category = "National Park" 
        },
        new Park 
        { 
          ParkId = 3, 
          Name = "Cape Kiwanda", 
          Location = "Oregon", 
          Description = "This sandstone headland just north of Pacific City offers one of the best viewpoints on the coast for witnessing the ocean's power. The landmark is one of three along the Three Capes Scenic Route (along with Cape Meares and Cape Lookout).", 
          Category = "State Park" 
        }
      );
  }
}