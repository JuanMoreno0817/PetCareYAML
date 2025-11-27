using System.ComponentModel.DataAnnotations;

namespace PetCareWebAPI.DAL.Entities
{
    public class AdoptionForm
    {
        [Key]
        public Guid IdForm { get; set; }
        public DateTime? CreateDate { get; set; }
        public Adopter Adopter { get; set; } //FK
        public Pet Pet { get; set; } //FK
    }
}
