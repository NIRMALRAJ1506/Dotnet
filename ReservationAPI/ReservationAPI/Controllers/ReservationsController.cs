using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationAPI.Models;
using ReservationAPI.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] // Require authentication for all actions
    public class ReservationsController : ControllerBase
    {
        private readonly IRepository<Reservation> _reservationRepository;

        public ReservationsController(IRepository<Reservation> reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            var reservations = await _reservationRepository.GetAllAsync();
            if (reservations == null || !reservations.Any())
            {
                return NotFound("No reservations found.");
            }

            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationRepository.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateReservation([FromBody] Reservation reservation)
        {
            if (reservation == null)
            {
                return BadRequest("Reservation is null.");
            }

            await _reservationRepository.AddAsync(reservation);

            return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (reservation == null || reservation.Id != id)
            {
                return BadRequest("Invalid reservation data.");
            }

            try
            {
                var existingReservation = await _reservationRepository.GetByIdAsync(id);
                if (existingReservation == null)
                {
                    return NotFound("Reservation not found.");
                }

                await _reservationRepository.UpdateAsync(reservation);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception here (optional)
                return StatusCode(500, "An error occurred while updating the reservation.");
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var existingReservation = await _reservationRepository.GetByIdAsync(id);
            if (existingReservation == null)
            {
                return NotFound();
            }

            await _reservationRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}
