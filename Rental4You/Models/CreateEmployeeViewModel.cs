using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Rental4You.Models
{
    public class CreateEmployeeViewModel
    {
        public string? Id { get; set; }
        public string Role { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "E-Mail")]
        public string EMail { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Password { get; set; }

        [Display(Name = "Birthdate")]
        public DateTime BirthDate { get; set; }

        //[Display(Name = "Profile picture")]
        public FileUpload UserAvatar { get; set; }

        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        public class FileUpload
        {
            [Required]
            [Display(Name = "File")]
            public IFormFile FormFile { get; set; }
        }
    }
}
