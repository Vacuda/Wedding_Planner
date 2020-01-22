using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [NotMapped]
        public string Login_Email{get;set;}

        [Required]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Login_Password{get;set;}
    }
}