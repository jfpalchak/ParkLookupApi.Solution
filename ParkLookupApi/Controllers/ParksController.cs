using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ParkLookupApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ParksController : ControllerBase
{
  private readonly ParkLookupApiContext _db;

  public ParksController(ParkLookupApiContext db)
  {
    _db = db;
  }


}