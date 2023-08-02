namespace MayNghien.Models.Response.Base
{
    public class SearchResponse<T>
    {
        public List<T> Data { get; set; }
        public long CurrentPage { get; set; }
        public long TotalPages { get; set; }
        public long RowsPerPage { get; set; }
        public long TotalRows { get; set; }
    }
}
