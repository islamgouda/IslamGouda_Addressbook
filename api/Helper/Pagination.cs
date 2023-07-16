namespace api.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Count = count;

            Data = data;

        }
        public int PageIndex { get; set; }
        public int PageSize { get; set; } 
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }
    }

    public class PageSearch
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public SearchItems? data { get; set; }
    }
    public class SearchItems
    {
        public string Name { get; set; }
    }
}
