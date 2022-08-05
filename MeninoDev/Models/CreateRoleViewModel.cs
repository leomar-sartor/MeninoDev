using System.ComponentModel.DataAnnotations;

namespace MeninoDev.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
