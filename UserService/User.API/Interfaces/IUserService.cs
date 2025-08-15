using User.API.DTOs;
using User.Domain.Aggregates.ValueObjects;

namespace User.API.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id, string aid, string username,string email);
        Task<UserDto?> GetByAuthIdAsync(string aid);
        Task<List<UserDto>> GetAllAsync();
        Task<Guid> CreateUserAsync(CreateUserRequestDto createRequest);
        Task UpdateAsync(UpdateInformationDto updateInfo);
        Task DeleteAsync(Guid id);
    }
}
