using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SocialApp.API.Models;

namespace SocialApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }

        public async Task<User> Login(string username, string password)
        {
            // TODO Upto here (9mins into vid)
            var user = await _context.Users.FirstOrDefaultAsync( x => x.Username == username );

            if(username == null)
                return null;
            
            if(!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt) )
                return null;

            // If the above test all pass, authentication is verified
            return user;
        }

        public async Task<User> Register(User user, string password)
        {
             (user.PasswordHash, user.PasswordSalt) = CreatePasswordHash(password);

             await _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();

             return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for( int i = 0 ; i < computeHash.Length; i++ )
                {
                    if(computeHash[i] != passwordHash[i] )
                        return false;
                }

                return true;
            }
        }

        private (byte[],byte[]) CreatePasswordHash(string password)
        {
            using( var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordSalt, passwordHash);
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if( await _context.Users.AnyAsync( x => x.Username == username) )
                return true;
            else
                return false;
        }
    }
}