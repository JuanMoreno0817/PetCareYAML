using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL.Entities;
using System.Diagnostics.Metrics;


namespace PetCareWebAPI.DAL
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Adopter> Adopters { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Psichologist> Psichologists { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<AdoptionForm> AdoptionForms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Adopter>().HasIndex(a => a.Identification).IsUnique();
            modelBuilder.Entity<Adopter>().ToTable("Adopter");

            modelBuilder.Entity<Psichologist>().HasIndex(p => p.Identification).IsUnique();
            modelBuilder.Entity<Psichologist>().ToTable("Psichologist");

            modelBuilder.Entity<Vet>().HasIndex(v => v.Identification).IsUnique();
            modelBuilder.Entity<Vet>().ToTable("Vet");
        }
    }
}
