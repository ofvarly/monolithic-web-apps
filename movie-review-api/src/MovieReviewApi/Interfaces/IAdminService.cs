using MovieReviewApi.DTOs;

namespace MovieReviewApi.Interfaces
{
    public interface IAdminService
    {
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();

        Task<UserReadDto> GetUserByIdAsync(string id);
        
        Task<UserReadDto> UpdateUserAsync(string id, RegisterDto userDto);
        
        Task DeleteUserAsync(string id);
    }
}