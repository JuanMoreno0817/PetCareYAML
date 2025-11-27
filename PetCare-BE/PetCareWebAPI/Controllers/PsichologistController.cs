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
    public class PsichologistController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public PsichologistController(DataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetPsichologists")]
        public async Task<ActionResult<IEnumerable<Psichologist>>> GetPsichologists()
        {
            var Psichologist = await _context.Psichologists.ToListAsync();
            if (Psichologist == null) return NotFound();
            return Ok(Psichologist);
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetPsichologist/{id}")]
        public async Task<ActionResult<Psichologist>> GetPsichologistByIdentification(int id)
        {
            var Psichologist = await _context.Psichologists.FirstOrDefaultAsync(v => v.Identification == id);

            if (Psichologist == null) return NotFound("Psichologist not found");

            return Ok(Psichologist);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Create")]
        [Route("CreatePsichologist")]
        public async Task<ActionResult<Psichologist>> CreatePsichologist(Psichologist Psichologist)
        {
            try
            {
                _context.Psichologists.Add(Psichologist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", Psichologist.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(Psichologist);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditPsichologist/{id}")]
        public async Task<IActionResult> EditPsichologist(int id, Psichologist Psichologist)
        {
            try
            {
                if (id != Psichologist.Identification) return NotFound("Psichologist not found");

                _context.Psichologists.Update(Psichologist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", Psichologist.Identification));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(Psichologist);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeletePsichologist/{id}")]
        public async Task<IActionResult> DeletePsichologist(int id)
        {
            var Psichologist = await _context.Psichologists.FirstOrDefaultAsync(v => v.Identification == id);

            if (Psichologist == null) return NotFound("Psichologist not found");

            _context.Psichologists.Remove(Psichologist);
            await _context.SaveChangesAsync();

            return Ok(String.Format("El psicólogo {0} fue eliminada con éxito!", Psichologist.Name));
        }
    }
}
