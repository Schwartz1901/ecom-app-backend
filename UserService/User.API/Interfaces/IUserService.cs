using User.API.DTOs;

namespace User.API.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(Guid id);
        Task<List<UserDto>> GetAllAsync();
        Task CreateUserAsync(CreateUserRequestDto createRequest);
        Task UpdateAsync(UpdateInformationDto updateInfo);
        Task DeleteAsync(Guid id);
    }
}
