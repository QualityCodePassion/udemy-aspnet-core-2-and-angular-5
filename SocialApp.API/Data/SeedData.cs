using System.Collections.Generic;
using Newtonsoft.Json;
using SocialApp.API.Models;

namespace SocialApp.API.Data
{
    public class SeedData
    {
        private readonly DataContext _context;
        public SeedData(DataContext context)
        {
            _context = context;
        }

        public void SeedUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChangesAsync();

            // Seed data
            var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            foreach (var user in users)
            {
                (user.PasswordHash, user.PasswordSalt) = AuthRepository.CreatePasswordHash("1234");
                user.Username = user.Username.ToLower();

                _context.Users.Add(user);
            }

            _context.SaveChanges();
        }
    }
}