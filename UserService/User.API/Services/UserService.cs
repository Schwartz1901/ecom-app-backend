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

        public async Task<UserDto> GetByIdAsync(Guid id, string aid, string username, string email)
        {
            var userId = new UserId(id);
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                user = new UserAggregate(Guid.Parse(aid), username, email);
                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

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

        public async Task<Guid> CreateUserAsync(CreateUserRequestDto request)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                
                var newUser = new UserAggregate(request.ID, request.Username, request.Email);
                await _userRepository.CreateUserAsync(newUser);

                await _unitOfWork.CommitAsync();
                return newUser.Id.Value;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public  Task UpdateAsync(UpdateInformationDto updateInfo)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                
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

        public async Task<UserDto?> GetByAuthIdAsync(string aid)
        {
            var profile = await _userRepository.GetByAuthIdAsync(Guid.Parse(aid));
            if (profile == null) {
                throw new KeyNotFoundException("No user");
            }
            var userDto = new UserDto()
            {
                Username = profile.Username,
                Email = profile.Email,
                CreatedAt = profile.CreatedAt,
                IsActive = profile.IsActive,
                PhoneNumber = profile.PhoneNumber,
            };
            return userDto;
        }
    }
}
