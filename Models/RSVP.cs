using System.ComponentModel.DataAnnotations;

namespace Wedding_Planner.Models
{
    public class RSVP
    {
        
        [Key]
        public int RSVPId {get;set;}
        

        [Required]
        public int WeddingId {get;set;}


        [Required]
        public int UserId {get;set;}
        

        //Relationships and Navigations

        public Wedding Wedding {get;set;}


        public User User {get;set;}



    }
}