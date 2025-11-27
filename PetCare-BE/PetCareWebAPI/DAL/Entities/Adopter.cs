using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace PetCareWebAPI.DAL.Entities
{
    public class Adopter : Person
    {
        [Display(Name = "Ocupación")]
        public string Ocupation { get; set; }
        public HouseTypes? HouseType { get; set; }

        [Display(Name = "Ingresos")]
        public double? MoneyIncome { get; set; }

        public enum HouseTypes
        {
            Casa,
            Apartmento,
            Finca,
            Cabaña
        }

    }
}
