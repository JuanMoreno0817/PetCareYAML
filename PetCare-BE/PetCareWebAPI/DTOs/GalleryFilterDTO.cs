using PetCareWebAPI.DAL.Entities;

namespace PetCareWebAPI.DTOs
{
    public class GalleryFilterDTO
    {
        public int? AgeMin { get; set; }
        public int? AgeMax { get; set; }
        public Pet.Types? Tipo { get; set; }
        public Pet.Genero? genero { get; set; }
    }
}
