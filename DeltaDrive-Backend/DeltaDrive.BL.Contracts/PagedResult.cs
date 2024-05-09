namespace DeltaDrive.BL.Contracts
{
    public class PagedResult<TEntity> : PagedResultBase where TEntity : class
    {
        public IList<TEntity> Results { get; set; }

        public PagedResult()
        {
            Results = [];
        }
    }
}
