namespace GameCenter.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; } = 1;
        private int itemsPerPage = 10;
        private readonly int maxItemsPerPage = 50;

        public int ItemsPerPage
        {
            get
            {
                return itemsPerPage;
            }
            set
            {
                itemsPerPage = (value > maxItemsPerPage) ? maxItemsPerPage : value;
            }
        }
    }
}