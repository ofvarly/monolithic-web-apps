using Microsoft.EntityFrameworkCore;
using MovieReviewApi.Interfaces;
using MovieReviewApi.Models;
using MovieReviewApi.Data;

namespace MovieReviewApi.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == id) ??
                    throw new KeyNotFoundException("User not found");
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}