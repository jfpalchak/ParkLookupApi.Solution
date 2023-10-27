using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkLookupApi.Models;

namespace ParkLookupApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParksController : ControllerBase
{
  private readonly ParkLookupApiContext _db;
  private readonly UserManager<ApplicationUser> _userManager;

  public ParksController(ParkLookupApiContext db, UserManager<ApplicationUser> userManager)
  {
    _db = db;
    _userManager = userManager;
  }

  // GET: api/parks/
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Park>>> Get()
  {
    IQueryable<Park> query = _db.Parks.AsQueryable();

    return await query.ToListAsync();
  }

  // GET: api/parks/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Park>> GetPark(int id)
  {
    Park thisPark = await _db.Parks.FindAsync(id);

    if (thisPark == null)
    {
      return NotFound();
    }

    return thisPark;
  }

}