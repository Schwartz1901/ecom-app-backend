using User.API.DTOs;
using User.Domain.Aggregates.ValueObjects;

namespace User.API.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllAsync();
        Task<Guid> CreateUserAsync(CreateUserRequestDto createRequest);
        Task UpdateAsync(UpdateInformationDto updateInfo);
        Task DeleteAsync(Guid id);
    }
}
