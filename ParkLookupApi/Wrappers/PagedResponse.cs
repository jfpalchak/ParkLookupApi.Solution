namespace ParkLookupApi.Wrappers;

public class PagedResponse<T> : Response<T>
{
  public int PageNumber { get; set; }
  public int PageSize { get; set; }
  public int TotalPages { get; set; }
  public bool HasPreviousPage => PageNumber > 1;
  public bool HasNextPage => PageNumber < TotalPages;

  public PagedResponse(T data, int count, int pageNumber, int pageSize)
  {
    this.PageNumber = pageNumber;
    this.PageSize = pageSize;
    this.TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    this.Data = data;
    this.Message = null;
    this.Succeeded = true;
    this.Errors = null;
  }
}