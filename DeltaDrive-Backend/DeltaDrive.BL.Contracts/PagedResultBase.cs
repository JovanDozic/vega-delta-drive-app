namespace DeltaDrive.BL.Contracts
{
    public class PagedResultBase
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
