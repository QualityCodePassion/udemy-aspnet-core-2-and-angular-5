using System.Collections.Generic;
using System.Threading.Tasks;
using SocialApp.API.Models;
using SocialApp.API.Helpers;

namespace SocialApp.API.Data
{
    public interface ISocialAppRepository
    {
         void Add<T>(T entity) where T: class;

         void Delete<T>(T entity) where T: class;

         Task<bool> SaveAll();

         Task<PagedList<User>> GetUsers(UserParams userParams);

         Task<User> GetUser(int id);
    }
}