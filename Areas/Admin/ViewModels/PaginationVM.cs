namespace Revas.Areas.Admin.ViewModels
{
    public class PaginationVM<T> where T : class, new()
    {
        public List<T> Items { get; set; }
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int Limit { get; set; }

    }
}
