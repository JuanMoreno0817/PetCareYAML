using System.ComponentModel.DataAnnotations;

namespace PetCareWebAPI.DAL.Entities
{
    public class MedicalRecord
    {
        [Key]
        public Guid IdMedicalRe { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Description { get; set; }
        public Vet Vet { get; set; } //FK
    }
}
