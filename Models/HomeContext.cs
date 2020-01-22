using Microsoft.EntityFrameworkCore;
 
namespace Wedding_Planner.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<RSVP> RSVPs {get;set;}
        public DbSet<Wedding> Weddings {get;set;}

        
    }
}