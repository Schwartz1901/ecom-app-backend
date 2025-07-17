using DocumentAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalAPIController
    {
        private readonly IExternalAPIService _externalAPIService;
        public ExternalAPIController(IExternalAPIService externalAPIService) 
        {
            _externalAPIService = externalAPIService;
        }
        [HttpGet("randomUser")]
        public async Task<IActionResult> GetRandomUser()
        {
            var user = await _externalAPIService.GetRandomUserAsync();
            return Ok(user);
        }
    }
}
