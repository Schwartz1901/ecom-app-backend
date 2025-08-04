using User.API.DTOs;
using User.API.Interfaces;
using User.Domain.Aggregates;
using User.Domain.Aggregates.ValueObjects;
using User.Domain.Repositories;
using User.Domain.SeedWork;
using User.Infrastructure;

namespace User.API.Services
{
    public class UserService : IUserService 
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork) 
        {
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                
                var newUser = new UserAggregate(request.ID, request.Username, request.Email);
                await _userRepository.CreateUserAsync(newUser);

                var client = _httpClientFactory.CreateClient("CartService");

                var cartRequest = new { UserId = newUser.Id.Value };
                var response = await client.PostAsJsonAsync("Cart", cartRequest);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception("Cannot create cart " + error);
                }
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(UpdateInformationDto updateInfo)
        {

        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var client = _httpClientFactory.CreateClient("CartService");
                var response = await client.DeleteAsync($"Cart/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception("Failed to delete cart: " + error);
                };
                var userId = new UserId(id);
                await _userRepository.RemoveAsync(userId);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        

    }
}
