using CleanAuthCore.Context;
using CleanAuthCore.Entities;
using CleanAuthCore.Repositories.Interfaces;
using CleanAuthCore.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanAuthCore.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await GetUserByUserNameAsync(username);
            if (user == null) return false;
            return PasswordHasher.VerifyPassword(password, user.PasswordHash);
        }
    }
}
