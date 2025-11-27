using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareWebAPI.DAL.Entities
{
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdPet { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Edad")]
        public int Age { get; set; }

        [Display(Name = "Color")]
        public string Color { get; set; }

        [Display(Name = "Raza")]
        public string Race { get; set; }

        [Display(Name = "Peso")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Weight { get; set; }

        [Display(Name = "Altura")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public double Height { get; set; }
        public string Description { get; set; }
        public Genero genero { get; set; }
        public AdoptionStatus Status { get; set; }
        public Types Tipo { get; set; }
        public int? IdMedicalRecord { get; set; } //FK 

        public enum AdoptionStatus 
        {
            Unadopted,
            Adopted
        }

        public enum Genero 
        {
            Hembra,
            Macho
        }

        public enum Types 
        { 
            Gato,
            Perro
        }
    }
}
