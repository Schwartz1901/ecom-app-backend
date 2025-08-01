using User.API.DTOs;
using User.API.Interfaces;
using User.Domain.Aggregates;
using User.Domain.Aggregates.ValueObjects;
using User.Domain.Repositories;
using User.Infrastructure;

namespace User.API.Services
{
    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        public UserService(IUserRepository userRepository, IHttpClientFactory httpClientFactory) 
        {
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var userId = new UserId(id);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };

        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var results = await _userRepository.GetAllAsync();
            return results.Select(user => new UserDto
            {
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            }).ToList();
        }

        public async Task CreateUserAsync(CreateUserRequestDto request)
        {
            var newUser = new UserAggregate(request.Username, request.Email);
            var userId = await _userRepository.CreateUserAsync(newUser);

            var client = _httpClientFactory.CreateClient("CartService");

            var cartRequest = new { UserId = newUser.Id.Value };
            var response = await client.PostAsJsonAsync("Cart", cartRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Cannot create cart" + response.Content);
            }
        }

        public async Task UpdateAsync(UpdateInformationDto updateInfo)
        {

        }

        public async Task DeleteAsync(Guid id)
        {

        }

    }
}
