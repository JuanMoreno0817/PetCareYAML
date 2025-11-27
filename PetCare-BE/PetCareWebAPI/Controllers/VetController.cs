using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;

namespace PetCareWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class VetController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public VetController(DataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetVets")]
        public async Task<ActionResult<IEnumerable<Vet>>> GetVets()
        {
            var vet = await _context.Vets.ToListAsync();
            if (vet == null) return NotFound();
            return Ok(vet);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetVet/{id}")]
        public async Task<ActionResult<Vet>> GetVetByIdentification(int id)
        {
            var vet = await _context.Vets.FirstOrDefaultAsync(v => v.Identification == id);

            if (vet == null) return NotFound("Vet not found");

            return Ok(vet);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Create")]
        [Route("CreateVet")]
        public async Task<ActionResult<Vet>> CreateVet(Vet vet)
        {
            try
            {
                _context.Vets.Add(vet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", vet.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(vet);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditVet/{id}")]
        public async Task<IActionResult> EditVet(int id, Vet vet)
        {
            try
            {
                if (id != vet.Identification) return NotFound("Vet not found");

                _context.Vets.Update(vet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", vet.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(vet);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeleteVet/{id}")]
        public async Task<IActionResult> DeleteVet(int id)
        {
            var vet = await _context.Vets.FirstOrDefaultAsync(v => v.Identification == id);

            if (vet == null) return NotFound("Vet not found");

            _context.Vets.Remove(vet);
            await _context.SaveChangesAsync();

            return Ok(String.Format("El veterinario {0} fue eliminada con éxito!", vet.Name));
        }
    }
}
