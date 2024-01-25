using System.ComponentModel.DataAnnotations;

namespace Revas.Areas.Admin.ViewModels
{
    public class CreatePortfolioVM
    {
        [Required]
        public string Image { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
