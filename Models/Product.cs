using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomApi.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public long Price { get; set; }

        public string? PictureUrl { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }

        [Required]
        public int QuantityInStock { get; set; }
    }
}
