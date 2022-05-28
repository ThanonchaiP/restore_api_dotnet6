using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    [Table("BasketItems")]
    public class BasketItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Quantity { get; set; }

        //navigation properties
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int BasketId { get; set; }
        public Basket Basket { get; set; } = null!;

    }
}