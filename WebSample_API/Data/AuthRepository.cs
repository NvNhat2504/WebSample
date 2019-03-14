using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebSample_API.Models;

namespace WebSample_API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<User> Login(string userName, string passWord)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
            if (user == null)
                return null;
            else if (!VerifyPasswordHash(passWord, user.PasswordHash, user.PasswordSalt))
                return null;
            return user;
        }

        private bool VerifyPasswordHash(string passWord, byte[] passwordHash, byte[] passwordSalt)
        {
            byte[] computeHash = null;
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord));
            }
            return computeHash.SequenceEqual(passwordHash);
        }

        public async Task<User> Register(User user, string passWord)
        {
            byte[] passWordHash, passWordSalt;
            CreatePasswordHash(passWord, out passWordHash, out passWordSalt);
            user.PasswordHash = passWordHash;
            user.PasswordSalt = passWordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string passWord, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(passWord));
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}