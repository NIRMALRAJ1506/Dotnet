using Microsoft.EntityFrameworkCore;
namespace ReservationAPI.Models
{
    public class ReservationContext : DbContext
    {
        public ReservationContext(DbContextOptions<ReservationContext> options):base(options) 
        {

        }

        public DbSet<Reservation> Reservation {  get; set; }
    }
}
