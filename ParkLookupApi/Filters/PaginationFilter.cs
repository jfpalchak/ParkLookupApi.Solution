
namespace ParkLookupApi.Filters;

public class PaginationFilter
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }

  // If no parameters are given, set defaults:
  public PaginationFilter()
  {
    this.PageNumber = 1;
    this.PageSize = 10;
  }

  // Minimum possible page number is set to 1.
  // Maximum possible page size is set to 10.
  public PaginationFilter(int pageNumber, int pageSize)
  {
    this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
    this.PageSize = pageSize > 10 || pageSize < 0 ? 10 : pageSize;
  }
}