using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Birth Date")]
        [PersonalData]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; } = DateTime.Parse("01.01.1970");
        public byte[]? UserAvatar { get; set; }
        //public IEnumerable<string>? Roles { get; set; }
        // Foreign keys for client type of users
        public IEnumerable<Reservation>? Reservations { get; set; }
        // Foreign keys for employee type of users

        [Display(Name = "Company")]
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
        public IEnumerable<Pickup>? Pickups { get; set; }
        public IEnumerable<Delivery>? Deliveries { get; set; }
    }
}
