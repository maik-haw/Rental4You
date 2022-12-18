using Microsoft.AspNetCore.Identity;
namespace Rental4You.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [PersonalData]
        public DateTime BirthDate { get; set; }
        public byte[]? UserAvatar { get; set; }
        //public IEnumerable<string>? Roles { get; set; }
        // Foreign keys for client type of users
        public IEnumerable<Reservation>? Reservations { get; set; }
        // Foreign keys for employee type of users
        public int? CompanyId { get; set; }
        public Company? Company { get; set; }
        public IEnumerable<Pickup>? Pickups { get; set; }
        public IEnumerable<Delivery>? Deliveries { get; set; }
    }
}
