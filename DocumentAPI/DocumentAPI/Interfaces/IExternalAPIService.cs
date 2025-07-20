
using DocumentAPI.DTOs;
namespace DocumentAPI.Interfaces
{
    public interface IExternalAPIService
    {
        Task<Result> GetRandomUserAsync();
    }
}
