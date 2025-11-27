using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetCareWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AllMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Identification = table.Column<int>(type: "int", nullable: true),
                    TypeOfDocument = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Lastname = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Cellphone = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Borndate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userType = table.Column<int>(type: "int", nullable: true),
                    AuthId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Identification);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    IdPet = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Race = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    genero = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    IdMedicalRecord = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.IdPet);
                });

            migrationBuilder.CreateTable(
                name: "Adopter",
                columns: table => new
                {
                    Identification = table.Column<int>(type: "int", nullable: true),
                    Ocupation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HouseType = table.Column<int>(type: "int", nullable: true),
                    MoneyIncome = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adopter", x => x.Identification);
                    table.ForeignKey(
                        name: "FK_Adopter_Persons_Identification",
                        column: x => x.Identification,
                        principalTable: "Persons",
                        principalColumn: "Identification",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Psichologist",
                columns: table => new
                {
                    Identification = table.Column<int>(type: "int", nullable: true),
                    ProfessionalCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeExperiencie = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Psichologist", x => x.Identification);
                    table.ForeignKey(
                        name: "FK_Psichologist_Persons_Identification",
                        column: x => x.Identification,
                        principalTable: "Persons",
                        principalColumn: "Identification",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vet",
                columns: table => new
                {
                    Identification = table.Column<int>(type: "int", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeExperiencie = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vet", x => x.Identification);
                    table.ForeignKey(
                        name: "FK_Vet_Persons_Identification",
                        column: x => x.Identification,
                        principalTable: "Persons",
                        principalColumn: "Identification",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdoptionForms",
                columns: table => new
                {
                    IdForm = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdopterIdentification = table.Column<int>(type: "int", nullable: true),
                    PetIdPet = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdoptionForms", x => x.IdForm);
                    table.ForeignKey(
                        name: "FK_AdoptionForms_Adopter_AdopterIdentification",
                        column: x => x.AdopterIdentification,
                        principalTable: "Adopter",
                        principalColumn: "Identification");
                    table.ForeignKey(
                        name: "FK_AdoptionForms_Pets_PetIdPet",
                        column: x => x.PetIdPet,
                        principalTable: "Pets",
                        principalColumn: "IdPet");
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    IDAppointment = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdopterIdentification = table.Column<int>(type: "int", nullable: true),
                    PsichologistIdentification = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.IDAppointment);
                    table.ForeignKey(
                        name: "FK_Appointments_Adopter_AdopterIdentification",
                        column: x => x.AdopterIdentification,
                        principalTable: "Adopter",
                        principalColumn: "Identification");
                    table.ForeignKey(
                        name: "FK_Appointments_Psichologist_PsichologistIdentification",
                        column: x => x.PsichologistIdentification,
                        principalTable: "Psichologist",
                        principalColumn: "Identification");
                });

            migrationBuilder.CreateTable(
                name: "MedicalRecords",
                columns: table => new
                {
                    IdMedicalRe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VetIdentification = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalRecords", x => x.IdMedicalRe);
                    table.ForeignKey(
                        name: "FK_MedicalRecords_Vet_VetIdentification",
                        column: x => x.VetIdentification,
                        principalTable: "Vet",
                        principalColumn: "Identification");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adopter_Identification",
                table: "Adopter",
                column: "Identification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionForms_AdopterIdentification",
                table: "AdoptionForms",
                column: "AdopterIdentification");

            migrationBuilder.CreateIndex(
                name: "IX_AdoptionForms_PetIdPet",
                table: "AdoptionForms",
                column: "PetIdPet");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AdopterIdentification",
                table: "Appointments",
                column: "AdopterIdentification");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PsichologistIdentification",
                table: "Appointments",
                column: "PsichologistIdentification");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalRecords_VetIdentification",
                table: "MedicalRecords",
                column: "VetIdentification");

            migrationBuilder.CreateIndex(
                name: "IX_Psichologist_Identification",
                table: "Psichologist",
                column: "Identification",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vet_Identification",
                table: "Vet",
                column: "Identification",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdoptionForms");

            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "MedicalRecords");

            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropTable(
                name: "Adopter");

            migrationBuilder.DropTable(
                name: "Psichologist");

            migrationBuilder.DropTable(
                name: "Vet");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
