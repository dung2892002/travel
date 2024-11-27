namespace Travel.Core.DTOs
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public OverallReview? OverallReview {  get; set; } 
    }
}
