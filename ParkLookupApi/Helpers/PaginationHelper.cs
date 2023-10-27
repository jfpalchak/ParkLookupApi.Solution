using Microsoft.EntityFrameworkCore;
using ParkLookupApi.Wrappers;
using ParkLookupApi.Filters;

namespace ParkLookupApi.Helpers;

public class PaginationHelper
{
  public static async Task<PagedResponse<List<T>>> CreateAsync<T>(IQueryable<T> source, PaginationFilter filter)
  {
    var count = await source.CountAsync();
    var items = await source.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

    PagedResponse<List<T>> response = new PagedResponse<List<T>>(items, count, filter.PageNumber, filter.PageSize);
    
    return response;
  }
}