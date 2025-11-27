using Microsoft.EntityFrameworkCore;
using PetCareWebAPI.DAL;
using PetCareWebAPI.DAL.Entities;
using PetCareWebAPI.DTOs;

namespace PetCareWebAPI.Controllers
{
    public class LoginServices
    {
        private readonly DataBaseContext _context;

        public LoginServices(DataBaseContext context) 
        {
            _context = context;
        }

        public async Task<Person?> GetPerson(LogInDTO person) 
        {
            return await _context.Persons.
                SingleOrDefaultAsync(x => x.Email == person.Email && x.Password == person.Password);
        }
    }
}
