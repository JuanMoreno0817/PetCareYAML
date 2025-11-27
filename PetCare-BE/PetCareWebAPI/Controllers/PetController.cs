using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;
using PetCareWebAPI.DTOs;
using PetCareWebAPI.Services;

namespace PetCareWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
        private readonly DataBaseContext _context;
        private readonly IKafkaProducer _kafka; 
        private readonly IConfiguration _cfg;

        public PetController(DataBaseContext context, IKafkaProducer kafka, IConfiguration cfg) // ← inyecta
        {
            _context = context;
            _kafka = kafka; // servicio para enviar mensajes a Kafka
            _cfg = cfg; // acceso a configuración (Kafka:Topic, BootstrapServers, etc.)
        }

        [HttpGet, ActionName("Get")]
        [Route("GetPets")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPets()
        {
            var Pet = await _context.Pets.ToListAsync();
            if (Pet == null) return NotFound();
            return Ok(Pet);
        }

        //Trae la mascota con el nombre asociado
        [HttpGet, ActionName("Get")]
        [Route("GetPetByName/{name}")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPetsByName(string name)
        {
            var Pet = await _context.Pets.FirstOrDefaultAsync(p => p.Name == name);
            if (Pet == null) return null;
            return Ok(Pet);
        }

        [HttpGet, ActionName("Get")]
        [Route("GetPetByIdentification/{id}")]
        public async Task<ActionResult<Pet>> GetPetByIdentification(int id)
        {
            var Pet = await _context.Pets.FirstOrDefaultAsync(v => v.IdPet == id);

            if (Pet == null) return NotFound("Pet not found");

            return Ok(Pet);
        }

        //Trae una mascota que contenga la cadena que envíe el usuario
        [HttpGet, ActionName("Get")]
        [Route("GetPetsByNameString/{name}")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetPetByNameString(string name)
        {
            var Pet = await _context.Pets.Where(v => v.Name.Contains(name)).ToListAsync();

            if (Pet == null) return Ok(Enumerable.Empty<Pet>());

            return Ok(Pet);
        }

        [HttpPost]
        [Route("GetFilteredPets")]
        public async Task<ActionResult<IEnumerable<Pet>>> GetFilteredPets([FromBody] GalleryFilterDTO filters)
        {
            var query = _context.Pets.AsQueryable();

            if (filters.Tipo.HasValue)
            {
                query = query.Where(p => p.Tipo == filters.Tipo);
            }

            if (filters.genero.HasValue)
            {
                query = query.Where(p => p.genero == filters.genero);
            }

            if (filters.AgeMin.HasValue)
            {
                query = query.Where(p => p.Age >= filters.AgeMin);
            }

            if (filters.AgeMax.HasValue)
            {
                query = query.Where(p => p.Age <= filters.AgeMax);
            }

            var pets = await query.ToListAsync();

            if (!pets.Any())
            {
                return null;
            }

            return Ok(pets);
        }

        [Authorize(Policy = "Admin")]
        [HttpPost, ActionName("Create")]
        [Route("CreatePet")]
        public async Task<ActionResult<Pet>> CreatePet(Pet Pet)
        {
            try
            {
                _context.Pets.Add(Pet);
                await _context.SaveChangesAsync();

                // 2) Publica el evento/log a Kafka SOLO si se guardó bien
                var payload = new
                {
                    eventType = "PetCreated",
                    petId = Pet.IdPet, 
                    name = Pet.Name,
                    occurredAt = DateTimeOffset.UtcNow,
                    actor = User?.Identity?.Name ?? "system"
                };

                var topic = _cfg["Kafka:Topic"];
                await _kafka.PublishAsync(topic, payload, key: Pet.IdPet.ToString()); // particiona por PetId
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", Pet.Name));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(Pet);
        }

        [Authorize(Policy = "Admin")]
        [HttpPut, ActionName("Edit")]
        [Route("EditPet/{id}")]
        public async Task<ActionResult<Pet>> EditPet(int id, Pet Pet)
        {
            try
            {
                if (id != Pet.IdPet) return NotFound("Pet not found");

                _context.Pets.Update(Pet);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException.Message.Contains("duplicate"))
                    return Conflict(String.Format("{0} ya existe", Pet.IdPet));
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }

            return Ok(Pet);
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete, ActionName("Delete")]
        [Route("DeletePet/{id}")]
        public async Task<IActionResult> DeletePet(int id)
        {
            var Pet = await _context.Pets.FirstOrDefaultAsync(v => v.IdPet == id);

            if (Pet == null) return NotFound("Pet not found");

            _context.Pets.Remove(Pet);
            await _context.SaveChangesAsync();

            return Ok(String.Format("La mascota {0} fue eliminada con éxito!", Pet.Name));
        }
    }
}
