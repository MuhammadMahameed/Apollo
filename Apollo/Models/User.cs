using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Apollo.Models
{
    public enum UserType
    { 
        Client,
        Admin,
    }
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Range(18,99,ErrorMessage ="Must be at least 18 YO")]
        public int Age { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",ErrorMessage = "Please enter correct email address")]
        [Display(Name = "Email Address")]
        public string EmailAdress  { get; set; }
        public UserType RoleType { get; set; } = UserType.Client;

        internal object First()
        {
            throw new NotImplementedException();
        }
    }
}
