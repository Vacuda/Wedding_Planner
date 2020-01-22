using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Models
{
    public class User
    {
        [Key]
        public int UserId{get;set;}

        [Required]
        [Display(Name="First Name")]
        [MinLength(2, ErrorMessage="Must be atleast 2 Characters Long")]
        public string FirstName{get;set;}

        [Required]
        [Display(Name="Last Name")]
        [MinLength(2, ErrorMessage="Must be atleast 2 Characters Long")]
        public string LastName{get;set;}

        [Required]
        [Display(Name="Email Address")]
        [EmailAddress]
        public string Email{get;set;}

        [Required]
        [Display(Name="Password")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password too short!")]
        public string Password{get;set;}

        public DateTime Updated_At{get;set;} = DateTime.Now;
        public DateTime Created_At{get;set;} = DateTime.Now;

        [Required]
        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [NotMapped]
        [Compare("Password", ErrorMessage="Passwords don't match.")]
        public string Confirm_Password{get;set;}

        

        //Relationships and Navigation


        public List<RSVP> RSVPs {get;set;}
        

        public List<Wedding> Weddings_Created {get;set;}



    }
}