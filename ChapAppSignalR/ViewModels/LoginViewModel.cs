using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace ChapAppSignalR.ViewModels
{
    public class LoginViewModel
    {
        //you want to put validation to view model you rarely put validation on your actual domain models
        [Display(Name ="Email Address")] 
        [Required(ErrorMessage ="Email address is required")]
        public string EmailAddress { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
