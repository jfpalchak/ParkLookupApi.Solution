using Microsoft.EntityFrameworkCore;
using ParkLookupApi.Wrappers;

namespace ParkLookupApi.Helpers;

public class PaginationHelper
{
  public static async Task<PagedResponse<List<T>>> CreateAsync<T>(IQueryable<T> source, int pageNumber, int pageSize)
  {
    var count = await source.CountAsync();
    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

    PagedResponse<List<T>> response = new PagedResponse<List<T>>(items, count, pageNumber, pageSize);
    
    return response;
  }
}