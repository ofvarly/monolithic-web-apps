using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieReviewApi.Models;
using MovieReviewApi.DTOs;


namespace MovieReviewApi.Interfaces
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();

        Task<User> GetUserByIdAsync(string id);

        Task UpdateUserAsync(User user);

        Task DeleteUserAsync(User user);
    }
}