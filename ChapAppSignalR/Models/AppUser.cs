using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ChapAppSignalR.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        //public string? Phone { get; set; }
        public string? IdentityNumber { get; set; }
        public string? Country { get; set; }
        

        public ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    }
}
