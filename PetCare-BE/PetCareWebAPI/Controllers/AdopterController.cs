using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;
using PetCareWebAPI.DTOs;

namespace PetCareWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdopterController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public AdopterController(DataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetAdopters")]
        public async Task<ActionResult<IEnumerable<Adopter>>> GetAdopters()
        {
            var adopters = await _context.Adopters.ToListAsync();
            if (adopters == null) return NotFound();
            return adopters;
        }

        [HttpGet, ActionName("Get")]
        [Route("GetAdopter/{id}")]
        public async Task<ActionResult<Adopter>> GetAdopterByIdentification(int id)
        {
            var adopter = await _context.Adopters.FirstOrDefaultAsync(a => a.Identification == id);

            if (adopter == null) return NotFound("Adopter not found");

            return adopter;
        }

        [HttpGet, ActionName("Post")]
        [Route("GetAdopterByEmail/{email}")]
        public async Task<ActionResult<Adopter>> GetAdopterByEmail(string email)
        {
            var adopter = await _context.Adopters.FirstOrDefaultAsync(a => a.Email == email);

            if (adopter == null) return null;

            return adopter;
        }

        [HttpPost, ActionName("Create")]
        [Route("CreateAdopter")]
        public async Task<ActionResult<Adopter>> CreateAdopter(Adopter adopter)
        {
            try
            {
                _context.Adopters.Add(adopter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", adopter.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(adopter);
        }

        [HttpPut, ActionName("Edit")]
        [Route("EditAdopter/{id}")]
        public async Task<IActionResult> EditAdopter(int id, Adopter adopter)
        {
            try
            {
                if (id != adopter.Identification) return NotFound("Adopter not found");

                _context.Adopters.Update(adopter);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", adopter.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(adopter);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeleteAdopter/{id}")]
        public async Task<IActionResult> DeleteAdopter(int id)
        {
            var adopter = await _context.Adopters.FirstOrDefaultAsync(a => a.Identification == id);

            if (adopter == null) return NotFound("Adopter not found");

            _context.Adopters.Remove(adopter);
            await _context.SaveChangesAsync();

            return Ok(String.Format("La persona {0} fue eliminada con éxito!", adopter.Name));
        }
    }
}
