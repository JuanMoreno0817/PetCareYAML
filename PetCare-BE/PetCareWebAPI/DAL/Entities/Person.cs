using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetCareWebAPI.Enums;

namespace PetCareWebAPI.DAL.Entities
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] //No identity
        [Display(Name = "Documento")]
        public int Identification { get; set; }

        [Display(Name = "Tipo de Documento")]
        public string TypeOfDocument { get; set; }

        [Display(Name = "Nombres")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Name { get; set; }

        [Display(Name = "Apellidos")]
        [MaxLength(20, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Lastname { get; set; }

        [Display(Name = "Celular")]
        [MaxLength(10, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Cellphone { get; set; }

        [Display(Name = "Dirección")]
        [MaxLength(30, ErrorMessage = "The {0} field can not have more than {1} characters.")]
        public string Address { get; set; }

        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Fecha Nacimiento")]
        public DateTime Borndate { get; set; }

        public UserType userType { get; set; }

        public string AuthId { get; set; } = null;
    }
}
