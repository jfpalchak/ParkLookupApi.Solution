using System.ComponentModel.DataAnnotations;

namespace ParkLookupApi.Models;

public class Park
{
  public int ParkId { get; set; }

  [Required]
  public string Name { get; set; }
  [Required]
  public string Location { get; set; }
  public string Description { get; set; }
  
  [Required]
  public string Category { get; set; }


}