using System.ComponentModel.DataAnnotations;

namespace PetCareWebAPI.DAL.Entities
{
    public class Appointment
    {
        [Key]
        public Guid IDAppointment { get; set; }
        public DateTime AppointmentDate { get; set; }
        public Adopter Adopter { get; set; } //FK
        public Psichologist Psichologist { get; set; } //FK
    }
}
