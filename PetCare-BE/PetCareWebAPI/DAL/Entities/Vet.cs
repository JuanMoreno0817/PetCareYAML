using System.ComponentModel.DataAnnotations;

namespace PetCareWebAPI.DAL.Entities
{
    public class Vet : Person
    {
        [Display(Name = "Especialización")]
        public string Specialization { get; set; }
        public int AgeExperiencie { get; set; }
    }
}
