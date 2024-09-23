using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ChapAppSignalR.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Email address is required")]
        public string EmailAddress { get; set; }


        [Required, DataType(DataType.Password)]
        public string Password { get; set; }


        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? Country { get; set; }


    }
}
