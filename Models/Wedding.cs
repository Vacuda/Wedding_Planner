using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wedding_Planner.Models
{
    public class Wedding
    {
        
        [Key]
        public int WeddingId {get;set;}


        [Required]
        [Display(Name="Person One")]
        public string PlayerOne {get;set;}


        [Required]
        [Display(Name="Person Two")]
        public string PlayerTwo {get;set;}


        [Required]
        [FutureDate]
        public DateTime Date {get;set;}


        [Required]
        public string Address {get;set;}


        public DateTime Updated_At{get;set;} = DateTime.Now;

        public DateTime Created_At{get;set;} = DateTime.Now;


        [Required]
        public int  UserId {get;set;}


        //Relationships and Navigations

        public List<RSVP> RSVPs {get;set;} = new List<RSVP>();

        
        public User Created_By_User {get;set;}


        //Displays

        [NotMapped]
        public int Number_Guests {
            get{
                return RSVPs.Count;
            }
        }


        [NotMapped]
        public String Date_Display {
            get{
                return Date.ToString("MMM d, yyyy");
            }
        }

    }
}