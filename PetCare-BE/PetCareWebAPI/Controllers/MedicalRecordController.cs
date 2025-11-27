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
    public class MedicalRecordController : ControllerBase
    {
        private readonly DataBaseContext _context;

        public MedicalRecordController(DataBaseContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet, ActionName("Get")]
        [Route("GetMedicalRecords")]
        public async Task<ActionResult<IEnumerable<MedicalRecord>>> GetMedicalRecords()
        {
            var medicalRecords = await _context.MedicalRecords.ToListAsync();
            if (medicalRecords == null) return NotFound();
            return medicalRecords;
        }

        [HttpGet, ActionName("Get")]
        [Route("GetMedicalRecord/{id}")]
        public async Task<ActionResult<MedicalRecord>> GetMedicalRecordByIdentification(Guid id)
        {
            var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(a => a.IdMedicalRe == id);

            if (medicalRecord == null) return NotFound("Medical record not found");

            return medicalRecord;
        }

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Create")]
        [Route("CreateMedicalRecord")]
        public async Task<ActionResult<MedicalRecord>> CreateMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                medicalRecord.CreateDate = DateTime.Now;
                medicalRecord.IdMedicalRe = Guid.NewGuid();
                _context.Entry(medicalRecord.Vet).State = EntityState.Unchanged;

                _context.MedicalRecords.Add(medicalRecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", medicalRecord.IdMedicalRe));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(medicalRecord);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditMedicalRecord")]
        public async Task<IActionResult> EditMedicalRecord(MedicalRecord medicalRecord)
        {
            try
            {
                medicalRecord.UpdateDate = DateTime.Now;
                _context.MedicalRecords.Update(medicalRecord);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", medicalRecord.IdMedicalRe));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(medicalRecord);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeleteMedicalRecord/{id}")]
        public async Task<IActionResult> DeleteMedicalRecord(Guid id)
        {
            var medicalRecord = await _context.MedicalRecords.FirstOrDefaultAsync(a => a.IdMedicalRe == id);

            if (medicalRecord == null) return NotFound("Medical record not found");

            _context.MedicalRecords.Remove(medicalRecord);
            await _context.SaveChangesAsync();

            return Ok(medicalRecord);
        }
    }
}
