using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.Models;
using SocialApp.API.Helpers;
using System.Linq;

namespace SocialApp.API.Data
{
    public class SocialAppRepository : ISocialAppRepository
    {
        private readonly DataContext _context;
        public SocialAppRepository(DataContext context)
        {
            _context = context;

        }
        
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            // Note need to convert it with AsQueryable to be able to use the "Where"
            // clause for filtering
            var users = _context.Users.Include(p => p.Photos).AsQueryable();
            users = users.Where( u => (u.Gender == userParams.Gender && u.Id != userParams.UserId));

            // TODO shouldn't use magic numbers like this! Make it a static readonly
            // as mentioned here: https://stackoverflow.com/a/8446525
            if (userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                users = users.Where( u => (
                    u.DateOfBirth.CalculateAge() >= userParams.MinAge && 
                    u.DateOfBirth.CalculateAge() <= userParams.MaxAge));
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}