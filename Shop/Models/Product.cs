using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Shop.Models
{
    public class Product
    {    [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required][DataType(DataType.Text)]
        public string BriefDescription { get; set; }
        [Required][DataType(DataType.MultilineText)]
        public string FullDescription { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [DataType(DataType.Upload)]
        [DisplayName("Image")]
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

   
}
