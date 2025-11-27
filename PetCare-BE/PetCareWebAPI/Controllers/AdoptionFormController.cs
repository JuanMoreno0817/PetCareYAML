using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;
using PetCareWebAPI.Response;

namespace PetCareWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdoptionFormController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public AdoptionFormController(DataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetAdoptionForms")]
        public async Task<ActionResult<IEnumerable<AdoptionForm>>> GetAdoptionForms()
        {
            var adoptionForms = await _context.AdoptionForms.ToListAsync();
            if (adoptionForms == null) return NotFound();
            return adoptionForms;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetAdoptionForm/{id}")]
        public async Task<ActionResult<AdoptionForm>> GetAdoptionFormByIdentification(string id)
        {
            var adoptionForms = await _context.AdoptionForms.Include(a => a.Adopter)
                                                        .Include(a => a.Pet)
                                                        .FirstOrDefaultAsync(a => a.Adopter.Name == id);

            if (adoptionForms == null) return NotFound("Adoption form not found");

            return adoptionForms;
        }

        [HttpGet, ActionName("Get")]
        [Route("GetAdoptionByAdopter/{id}")]
        public async Task<ActionResult<IEnumerable<AdoptionForm>>> GetAdoptionByAdopter(int id)
        {
            var adoptionForms = await _context.AdoptionForms.Include(a => a.Adopter)
                                                            .Include(a => a.Pet)
                                                            .Where(a => a.Adopter.Identification == id)
                                                            .ToListAsync();

            if (adoptionForms == null) return NotFound("Adoption form not found");

            return adoptionForms;
        }

        [HttpPost]
        [Route("CreateAdoptionForm")]
        public async Task<ActionResult<AdoptionForm>> CreateAdoptionForm([FromBody] AdoptionForm adoptionForm)
        {
            try
            {
                adoptionForm.CreateDate = DateTime.Now;
                adoptionForm.IdForm = Guid.NewGuid();

                _context.Entry(adoptionForm.Adopter).State = EntityState.Unchanged;
                _context.Entry(adoptionForm.Pet).State = EntityState.Unchanged;

                _context.AdoptionForms.Add(adoptionForm);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
                    return Conflict("Ya existe un formulario de adopción con este identificador único.");
                else
                    return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(adoptionForm);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditAdoptionForm/{id}")]
        public async Task<IActionResult> EditAdoptionForm(Guid id, AdoptionForm adoptionForm)
        {
            try
            {
                if (id != adoptionForm.IdForm) return NotFound("Adoption form not found");

                _context.AdoptionForms.Update(adoptionForm);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", adoptionForm.IdForm));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(adoptionForm);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeleteAdoptionForm/{id}")]
        public async Task<IActionResult> DeleteAdoptionForm(Guid id)
        {
            var adoptionForm = await _context.AdoptionForms.FirstOrDefaultAsync(a => a.IdForm == id);

            if (adoptionForm == null) return NotFound("Adoption form not found");

            _context.AdoptionForms.Remove(adoptionForm);
            await _context.SaveChangesAsync();

            return Ok(String.Format("El formulario de adopción {0} fue eliminado con éxito!", adoptionForm.IdForm));
        }
    }
}
