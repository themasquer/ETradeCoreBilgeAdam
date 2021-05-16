namespace AppCore.Business.Models.Paging
{
    public class PageModel
    {
        public int PageNumber { get; set; } = 1;
        public int RecordsPerPageCount { get; set; } = 10;
        public int RecordsCount { get; set; }
    }
}
