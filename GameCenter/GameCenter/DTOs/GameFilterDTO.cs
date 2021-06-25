namespace GameCenter.DTOs
{
    public class GameFilterDTO
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public PaginationDTO pagination 
        { 
            get { return new PaginationDTO() { Page = Page, ItemsPerPage = ItemsPerPage}; }
        }
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool NewlyReleases { get; set; }
        public bool UpcomingReleases { get; set; }    
        public string OrderingField { get; set; }
        public bool AscendingOrder{ get; set; } = true;
    }
}