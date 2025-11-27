using PetCareWebAPI.DAL.Entities;
using System.Data.SqlTypes;

namespace PetCareWebAPI.DAL
{
    public class SeederDB
    {
        private readonly DataBaseContext _context;

        public SeederDB(DataBaseContext context)
        {
            _context = context;
        }

        public async Task SeederAsync() 
        {
            await _context.Database.EnsureCreatedAsync();

            await PopulateAdopterAsync();
            await PopulatePsichologistAsync();
            await PopulateVetAsync();
            await PopulateMedicalRecordAsync();
            await PopulatePetAsync();

            var adopter1 = _context.Adopters.FirstOrDefault(p => p.Identification == 12345);
            var adopter2 = _context.Adopters.FirstOrDefault(p => p.Identification == 67890);
            var adopter3 = _context.Adopters.FirstOrDefault(p => p.Identification == 24680);

            await PopulateAdoptionFormAsync(adopter1,adopter2,adopter3);
            await PopulateAppointmentAsync(adopter1,adopter2,adopter3);
        }

        private async Task PopulatePetAsync()
        {
            if (!_context.Pets.Any())
            {
                _context.Pets.Add(new Pet
                {
                    Name = "Firulais",
                    Age = 1,
                    Color = "Negro",
                    Race = "Criollo",
                    Description = "Amoroso, juguetón y leal.",
                    Height = 50,
                    Weight = 20,
                    Status = 0,
                    genero = 0,
                    Tipo = (Pet.Types)1                    
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Pelusa",
                    Age = 3,
                    Color = "Negro con Blanco",
                    Race = "Border Collie",
                    Description = "Amoroso, juguetón y muy inteligente.",
                    Height = 50,
                    Weight = 20,
                    Status = 0,
                    genero = (Pet.Genero)1,
                    Tipo = (Pet.Types)1
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Max",
                    Age = 2,
                    Color = "Dorado",
                    Race = "Labrador Retriever",
                    Description = "Juguetón, amigable y fácil de entrenar.",
                    Height = 60,
                    Weight = 30,
                    Status = 0,
                    genero = 0,
                    Tipo = (Pet.Types)1
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Buddy",
                    Age = 4,
                    Color = "Atigrado",
                    Race = "Bulldog Inglés",
                    Description = "Leal, valiente y cariñoso.",
                    Height = 40,
                    Weight = 25,
                    Status = 0,
                    genero = (Pet.Genero)1,
                    Tipo = (Pet.Types)1
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Sol",
                    Age = 1,
                    Color = "Dorado",
                    Race = "Golden Retriever",
                    Description = "Amigable, inteligente y activo.",
                    Height = 55,
                    Weight = 27,
                    Status = 0,
                    genero = 0,
                    Tipo = (Pet.Types)1
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Milo",
                    Age = 2,
                    Color = "Blanco",
                    Race = "Persa",
                    Description = "Elegante, tranquilo y amoroso.",
                    Height = 25,
                    Weight = 5,
                    Status = 0,
                    genero = (Pet.Genero)1,
                    Tipo = 0
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Nina",
                    Age = 1,
                    Color = "Punto de lince",
                    Race = "Siames",
                    Description = "Vocal, inteligente y afectuoso.",
                    Height = 20,
                    Weight = 4,
                    Status = 0,
                    genero = 0,
                    Tipo = 0
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Simba",
                    Age = 3,
                    Color = "Atigrado",
                    Race = "Maine Coon",
                    Description = "Amigable, activo y juguetón.",
                    Height = 30,
                    Weight = 6,
                    Status = 0,
                    genero = (Pet.Genero)1,
                    Tipo = 0
                });


                _context.Pets.Add(new Pet
                {
                    Name = "Oreo",
                    Age = 1,
                    Color = "Negro y blanco",
                    Race = "Criollo",
                    Description = "Tranquilo, cariñoso y curioso.",
                    Height = 22,
                    Weight = 4,
                    Status = 0,
                    genero = 0,
                    Tipo = 0
                });

                _context.Pets.Add(new Pet
                {
                    Name = "Tigre",
                    Age = 2,
                    Color = "Atigrado",
                    Race = "Bengalí",
                    Description = "Energético, juguetón y enérgico.",
                    Height = 28,
                    Weight = 5,
                    Status = 0,
                    genero = (Pet.Genero)1,
                    Tipo = 0
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateAdopterAsync() 
        {
            if (!_context.Adopters.Any()) 
            {
                _context.Adopters.Add(new Adopter
                {
                    Identification = 12345,
                    TypeOfDocument = "CC",
                    Name = "Luis",
                    Lastname = "Gutierrez",
                    Cellphone = "3004398765",
                    Address = "Cll 56AA",
                    Email = "luisgutierrez@correo.com",
                    Borndate = new DateTime(2000, 05, 25),
                    HouseType = 0,
                    MoneyIncome = 3000000,
                    Ocupation = "Teacher",
                    Password = "12345",
                    userType = (Enums.UserType)1,
                    AuthId = null
                });

                _context.Adopters.Add(new Adopter
                {
                    Identification = 67890,
                    TypeOfDocument = "CC",
                    Name = "Maria",
                    Lastname = "Lopez",
                    Cellphone = "3001234567",
                    Address = "Cll 12",
                    Email = "maria@correo.com",
                    Borndate = new DateTime(1995, 10, 15),
                    HouseType = (Adopter.HouseTypes)1,
                    MoneyIncome = 5000000,
                    Ocupation = "Engineer",
                    Password = "12345",
                    userType = (Enums.UserType)1,
                    AuthId = null
                });

                _context.Adopters.Add(new Adopter
                {
                    Identification = 24680,
                    TypeOfDocument = "CC",
                    Name = "Laura",
                    Lastname = "Perez",
                    Cellphone = "3109876543",
                    Address = "Cra 456",
                    Email = "laura@correo.com",
                    Borndate = new DateTime(1990, 3, 10),
                    HouseType = (Adopter.HouseTypes)3,
                    MoneyIncome = 6000000,
                    Ocupation = "Artist",
                    Password = "12345",
                    userType = (Enums.UserType)1,
                    AuthId = null
                });

                _context.Adopters.Add(new Adopter
                {
                    Identification = 54321,
                    TypeOfDocument = "CC",
                    Name = "Ana",
                    Lastname = "Martinez",
                    Cellphone = "3194321290",
                    Address = "Cll 45",
                    Email = "ana@correo.com",
                    Borndate = new DateTime(1988, 12, 5),
                    HouseType = (Adopter.HouseTypes)2,
                    MoneyIncome = 4000000,
                    Ocupation = "Doctor",
                    Password = "12345",
                    userType = (Enums.UserType)1,
                    AuthId = null
                });

                _context.Adopters.Add(new Adopter
                {
                    Identification = 98765,
                    TypeOfDocument = "CC",
                    Name = "Carlos",
                    Lastname = "Garcia",
                    Cellphone = "3204598765",
                    Address = "Av 43",
                    Email = "carlos@correo.com",
                    Borndate = new DateTime(1976, 7, 20),
                    HouseType = (Adopter.HouseTypes)2,
                    MoneyIncome = 2800000,
                    Ocupation = "Nurse",
                    Password = "12345",
                    userType = (Enums.UserType)1,
                    AuthId = null
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulatePsichologistAsync() 
        {
            if (!_context.Psichologists.Any()) 
            {
                _context.Psichologists.Add(new Psichologist 
                {
                    Identification = 76321,
                    TypeOfDocument = "CC",
                    Name = "Andrés",
                    Lastname = "Zuluaga",
                    Cellphone = "3208765123",
                    Address = "Av 4",
                    Email = "andres@correo.com",
                    Borndate = new DateTime(1980, 2, 22),
                    AgeExperiencie = 10,
                    ProfessionalCard = "00001",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                _context.Psichologists.Add(new Psichologist
                {
                    Identification = 66623,
                    TypeOfDocument = "CC",
                    Name = "Jairo",
                    Lastname = "Mosquera",
                    Cellphone = "3202356789",
                    Address = "Cll 40",
                    Email = "jairo@correo.com",
                    Borndate = new DateTime(1990, 12, 24),
                    AgeExperiencie = 7,
                    ProfessionalCard = "00002",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                _context.Psichologists.Add(new Psichologist
                {
                    Identification = 62323,
                    TypeOfDocument = "CC",
                    Name = "Manuela",
                    Lastname = "Mosquera",
                    Cellphone = "3235678943",
                    Address = "Cll 30",
                    Email = "manuela@correo.com",
                    Borndate = new DateTime(1996, 12, 2),
                    AgeExperiencie = 7,
                    ProfessionalCard = "00003",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateVetAsync()
        {
            if (!_context.Vets.Any()) 
            {
                _context.Vets.Add(new Vet
                {
                    Identification = 31241,
                    TypeOfDocument = "CC",
                    Name = "Karen",
                    Lastname = "Osorio",
                    Cellphone = "3008705123",
                    Address = "Cra 43",
                    Email = "karen@correo.com",
                    Borndate = new DateTime(1986, 5, 10),
                    AgeExperiencie = 9,
                    Specialization = "Cirugía",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                _context.Vets.Add(new Vet
                {
                    Identification = 34145,
                    TypeOfDocument = "CC",
                    Name = "Mariangel",
                    Lastname = "Cardenas",
                    Cellphone = "3109876523",
                    Address = "Cll 56",
                    Email = "mariangel@correo.com",
                    Borndate = new DateTime(1992, 10, 7),
                    AgeExperiencie = 6,
                    Specialization = "Fisioterapia",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                _context.Vets.Add(new Vet
                {
                    Identification = 34555,
                    TypeOfDocument = "CC",
                    Name = "Marcela",
                    Lastname = "Muñoz",
                    Cellphone = "3176523123",
                    Address = "Av 5",
                    Email = "marcela@correo.com",
                    Borndate = new DateTime(1980, 2, 10),
                    AgeExperiencie = 6,
                    Specialization = "Cirugía",
                    Password = "12345",
                    userType = 0,
                    AuthId = null
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateMedicalRecordAsync() 
        {
            if (!_context.MedicalRecords.Any()) 
            {
                var vet = _context.Vets.FirstOrDefault(v => v.Identification == 34145);
                _context.MedicalRecords.Add(new MedicalRecord
                {
                    IdMedicalRe = new Guid(),
                    CreateDate = DateTime.Now,
                    Description = "Revisión inicial, se encuentra en óptimas condiciones, se le aplicaron las 2 primeras vacunas y se desparasito",
                    Vet = vet,
                });

                _context.MedicalRecords.Add(new MedicalRecord
                {
                    IdMedicalRe = new Guid(),
                    CreateDate = DateTime.Now,
                    Description = "Revisión inicial, se encuentra en óptimas condiciones y se desparasito",
                    Vet = vet
                });

                _context.MedicalRecords.Add(new MedicalRecord
                {
                    IdMedicalRe = new Guid(),
                    CreateDate = DateTime.Now,
                    Description = "Revisión inicial, se encuentra deshidratado, se desparasito y en 5 horas se le aplicara suero.",
                    Vet = vet
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateAdoptionFormAsync(Adopter adopter1, Adopter adopter2, Adopter adopter3) 
        {
            if (!_context.AdoptionForms.Any()) 
            {
                var pet1 = _context.Pets.FirstOrDefault(p => p.IdPet == 1);
                var pet4 = _context.Pets.FirstOrDefault(p => p.IdPet == 4);
                var pet10 = _context.Pets.FirstOrDefault(p => p.IdPet == 10);

                _context.AdoptionForms.Add(new AdoptionForm
                {
                    IdForm = new Guid(),
                    CreateDate = DateTime.Now,
                    Adopter = adopter1,
                    Pet = pet1
                });

                _context.AdoptionForms.Add(new AdoptionForm
                {
                    IdForm = new Guid(),
                    CreateDate = DateTime.Now,
                    Adopter = adopter2,
                    Pet = pet10
                });

                _context.AdoptionForms.Add(new AdoptionForm
                {
                    IdForm = new Guid(),
                    CreateDate = DateTime.Now,
                    Adopter = adopter3,
                    Pet = pet4
                });

                await _context.SaveChangesAsync();
            }
        }

        private async Task PopulateAppointmentAsync(Adopter adopter1, Adopter adopter2, Adopter adopter3) 
        {
            if (!_context.Appointments.Any()) 
            {
                var psichologist = _context.Psichologists.FirstOrDefault(p => p.Identification == 76321);
                _context.Appointments.Add(new Appointment 
                {
                    IDAppointment = new Guid(),
                    Adopter = adopter1,
                    Psichologist = psichologist,
                    AppointmentDate = new DateTime(2024, 05, 25, 14, 30,0)
                });

                _context.Appointments.Add(new Appointment
                {
                    IDAppointment = new Guid(),
                    Adopter = adopter2,
                    Psichologist = psichologist,
                    AppointmentDate = new DateTime(2024, 05, 26, 14, 30, 0)
                });

                _context.Appointments.Add(new Appointment
                {
                    IDAppointment = new Guid(),
                    Adopter = adopter3,
                    Psichologist = psichologist,
                    AppointmentDate = new DateTime(2024, 05, 27, 14, 30, 0)
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
