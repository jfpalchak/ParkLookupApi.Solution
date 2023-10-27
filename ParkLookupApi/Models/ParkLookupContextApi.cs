using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ParkLookupApi.Models;

public class ParkLookupApiContext : IdentityDbContext<ApplicationUser>
{
  public DbSet<Park> Parks { get; set; }

  public ParkLookupApiContext(DbContextOptions<ParkLookupApiContext> options) : base(options) { }
  
}