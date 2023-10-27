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
  public async Task<ActionResult<IEnumerable<Park>>> GetAll([FromQuery] string category, [FromQuery] string location)
  {
    IQueryable<Park> query = _db.Parks.AsQueryable();

    if (category != null)
    {
      query = query.Where(p => p.Category.Contains(category));
    }

    if (location != null)
    {
      query = query.Where(p => p.Location.Contains(location));
    }

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
  [Authorize]
  [HttpPost]
  public async Task<ActionResult<Park>> Post([FromBody] Park park)
  {
    _db.Parks.Add(park);
    await _db.SaveChangesAsync();
    return CreatedAtAction(nameof(GetPark), new { id = park.ParkId }, park);
  }

  // PUT: api/parks/{id}
  [Authorize]
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

  // DELETE: api/parks/{id}
  [Authorize]
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePark(int id)
  {
    Park thisPark = await _db.Parks.FindAsync(id);

    if (thisPark == null)
    {
      return NotFound();
    }

    _db.Parks.Remove(thisPark);
    await _db.SaveChangesAsync();

    return NoContent();
  }

  // GET: api/parks/random
  [HttpGet("random")]
  public async Task<ActionResult<Park>> Random()
  {
    IQueryable<Park> query = _db.Parks.AsQueryable();
    // Get the total number of Parks currently in database.
    int count = query.Count();

    // Generate a random number that is >= 0 and < number of parks.
    int index = new Random().Next(count);
    // Skip the random number of elements in the database and grab the 
    return await query.Skip(index).FirstOrDefaultAsync();
  }

  // GET: api/parks/search
  [HttpGet("search")]
  public async Task<ActionResult<IEnumerable<Park>>> Search([FromQuery] string searchString)
  {
    IQueryable<Park> query = _db.Parks.AsQueryable();

    if (!String.IsNullOrEmpty(searchString))
    {
      query = query.Where(p => p.Name.Contains(searchString) 
                            || p.Location.Contains(searchString) 
                            || p.Category.Contains(searchString)
      );
    }

    return await query.ToListAsync();
  }

}