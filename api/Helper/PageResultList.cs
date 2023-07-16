namespace api.Helper
{
    public class PageResultList<T> where T : class
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public List<T> Data { get; set; } = new List<T>();
    }
}
