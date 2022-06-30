using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Shop.ModelHelpers
{
    public class GetImgHelper
    {
        public string Title { get; set; }
        [Required]
        public IFormFile? ImgH { get; set; }
        
    }
}
