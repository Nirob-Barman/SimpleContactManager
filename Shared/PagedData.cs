namespace SimpleContactManager.Shared
{
    public class PagedData<T>
    {
        public int TotalCount { get; set; }
        public int CurrentPageDataCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T>? Data { get; set; }
    }
}
