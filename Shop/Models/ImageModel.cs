using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public byte[] Image { get; set; }
        [Required]
        public Product Product { get; set; }

    }
}
