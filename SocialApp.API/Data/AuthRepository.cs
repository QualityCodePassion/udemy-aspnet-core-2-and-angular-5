using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialApp.API.Models;

namespace SocialApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<AuthRepository> _logger;

        public AuthRepository(DataContext context,
        ILogger<AuthRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

    public async Task<User> Login(string username, string password)
    {
        // Get the Stored User details from the DataContext
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);

        // TODO Should implement "max retries" type algorithm to prevent a brute
        // force attack!

        if (user == null)
            return null;

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
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
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != passwordHash[i])
                    return false;
            }

            return true;
        }
    }

    public static (byte[], byte[]) CreatePasswordHash(string password)
    {
        // (My note) that since "HMACSHA512" derives from the "IDisposable" class, 
        // we should dispose of it as quickly as possible by utilizing the
        // "using statement" below
        // See: http://www.blackwasp.co.uk/usingstatement.aspx
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            //_logger.LogDebug (1, "passwordSalt: {passwordSalt}", passwordSalt);
            //_logger.LogDebug (1, "passwordHash: {passwordHash}", passwordHash);

            return (passwordHash, passwordSalt);
        }
    }

    public async Task<bool> UserExists(string username)
    {
        if (await _context.Users.AnyAsync(x => x.Username == username))
            return true;
        else
            return false;
    }
}
}