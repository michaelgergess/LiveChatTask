using Microsoft.EntityFrameworkCore;

namespace DTOs.Paginated
{
    public class PaginatedList<T> :List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; set; }
        public PaginatedList(List<T> items,int count,int pageIndex,int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling( count /(double) pageSize);
            this.AddRange(items);
        }
        public bool PreviousPage
        {
            get
            {
                return (PageIndex >1);
            }
        }
        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source,int pageIndex,int pageSize)
        {
            pageIndex = Math.Max(pageIndex, 1); 
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync().ConfigureAwait(false);
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

    
    }
}
