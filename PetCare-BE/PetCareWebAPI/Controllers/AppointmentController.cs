using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;

namespace PetCareWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public AppointmentController(DataBaseContext context)
        {
            _context = context;
        }

        [HttpGet, ActionName("Get")]
        [Route("GetAppointments")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointments()
        {
            var appointments = await _context.Appointments
                                                        .Include(a => a.Adopter)
                                                        .Include(a => a.Psichologist)
                                                        .ToListAsync();
            if (appointments == null) return NotFound();
            return appointments;
        }

        [HttpGet, ActionName("Get")]
        [Route("GetAppointment/{id}")]
        public async Task<ActionResult<Appointment>> GetAppointmentByIdentification(Guid id)
        {
            var appointment = await _context.Appointments.Include(a => a.Adopter)
                                                        .Include(a =>a.Psichologist)
                                                        .FirstOrDefaultAsync(a =>                                              a.IDAppointment == id);

            if (appointment == null) return NotFound("Appointment not found");

            return appointment;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Create")]
        [Route("CreateAppointment")]
        public async Task<ActionResult<Appointment>> CreateAppointment(Appointment appointment)
        {
            try
            {
                appointment.IDAppointment = Guid.NewGuid();
                _context.Entry(appointment.Adopter).State = EntityState.Unchanged;
                _context.Entry(appointment.Psichologist).State = EntityState.Unchanged;

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("Appointment {0} ya existe", appointment.IDAppointment));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(appointment);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditAppointment/{id}")]
        public async Task<ActionResult<Appointment>> EditAppointment(Guid id, Appointment appointment)
        {
            try
            {
                if (id != appointment.IDAppointment) return NotFound("Appointment not found");

                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("Appointment {0} ya existe", appointment.IDAppointment));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(appointment);
        }

        [HttpDelete, ActionName("Delete")]
        [Route("DeleteAppointment/{id}")]
        public async Task<IActionResult> DeleteAppointment(Guid id)
        {
            var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.IDAppointment == id);

            if (appointment == null) return NotFound("Appointment not found");

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok(String.Format("La cita {0} fue eliminada con éxito!", appointment.IDAppointment));
        }
    }
}
