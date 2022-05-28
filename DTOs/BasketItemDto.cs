namespace EcomApi.DTOs
{
    public class BasketItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public long Price { get; set; }
        public string? PictureUrl { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public int Quantity { get; set; }
    }
}