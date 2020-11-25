using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Shop.ViewModels
{
    public class ProductViewmodel
    {   [Key]
            public int ProductId { get; set; }
            [Required]
            public string Name { get; set; }
            [Required][DataType(DataType.Text)]
            public string BriefDescription { get; set; }
            [Required][DataType(DataType.MultilineText)]
            public string FullDescription { get; set; }
            [Required]
            public float Price { get; set; }
            [Required]
            public string Brand { get; set; }
            [Required][DataType(DataType.Upload)]
            [NotMapped]
            [DisplayName("Upload Image")]
            public IFormFile Img { get; set; }
        

    }
}
