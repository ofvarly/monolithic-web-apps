namespace MovieReviewApi.DTOs
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 1; // Varsayılan sayfa numarası
        public int PageSize { get; set; } = 5;  // Varsayılan sayfa boyutu
    }
}