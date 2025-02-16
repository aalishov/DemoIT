using System.ComponentModel.DataAnnotations;

namespace Books.Services.Contracts
{
    public class CreateBookViewModel
    {
       
        
            [Required]
            [MaxLength(64)]
            public string Name { get; set; }

            [MaxLength(255)]
            public string Description { get; set; }

            public IFormFile? ImageFile { get; set; }
        
    }
}