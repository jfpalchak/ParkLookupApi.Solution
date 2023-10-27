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

  // POST: api/parks
  [HttpPost]
  public async Task<ActionResult<Park>> Post([FromBody] Park park)
  {
    _db.Parks.Add(park);
    await _db.SaveChangesAsync();
    return CreatedAtAction(nameof(GetPark), new { id = park.ParkId }, park);
  }

  // PUT: api/parks/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> Put(int id, [FromBody] Park park)
  {
    if (id != park.ParkId)
    {
      return BadRequest();
    }

    _db.Parks.Update(park);

    try
    {
      await _db.SaveChangesAsync();
    }
    catch(DbUpdateConcurrencyException)
    {
      if (!ParkExists(id))
      {
        return NotFound();
      }
      else
      {
        throw;
      }
    }

    return NoContent();
  }

  // Given park ID, check if it exists in DB;
  // if it exists, return true, otherwise return false
  private bool ParkExists(int id)
  {
    return _db.Parks.Any(p => p.ParkId == id);
  }

  

}