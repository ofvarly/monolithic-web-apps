using MovieReviewApi.DTOs;
using MovieReviewApi.Interfaces;
using AutoMapper;

namespace MovieReviewApi.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;

        public AdminService(IAdminRepository adminRepository, IMapper mapper)
        {
            _adminRepository = adminRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            var users = await _adminRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserReadDto>>(users);
        }

        public async Task<UserReadDto> GetUserByIdAsync(string id)
        {
            var user = await _adminRepository.GetUserByIdAsync(id);
            return _mapper.Map<UserReadDto>(user);
        }

        public async Task<UserReadDto> UpdateUserAsync(string id, RegisterDto userDto)
        {
            var user = await _adminRepository.GetUserByIdAsync(id);

            _mapper.Map(userDto, user);
            await _adminRepository.UpdateUserAsync(user);

            return _mapper.Map<UserReadDto>(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _adminRepository.GetUserByIdAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            await _adminRepository.DeleteUserAsync(user);
        }
    }
}