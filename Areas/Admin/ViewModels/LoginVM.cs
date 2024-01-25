using System.ComponentModel.DataAnnotations;

namespace Revas.Areas.Admin.ViewModels
{
    public class LoginVM
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string UsernameOrEmail { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }
    }
}
