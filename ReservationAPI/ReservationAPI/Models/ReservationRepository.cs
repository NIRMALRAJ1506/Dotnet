using Microsoft.EntityFrameworkCore;
using ReservationAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationAPI.Repositories
{
    public class ReservationRepository : IRepository<Reservation>
    {
        private readonly ReservationContext _context;

        public ReservationRepository(ReservationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservation.ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservation.FindAsync(id);
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _context.Reservation.AddAsync(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            var existingReservation = await _context.Reservation.FindAsync(reservation.Id);
            if (existingReservation != null)
            {
                _context.Entry(existingReservation).CurrentValues.SetValues(reservation);
                await _context.SaveChangesAsync();
            }
        }


        public async Task DeleteAsync(int id)
        {
            var reservation = await _context.Reservation.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservation.Remove(reservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}
