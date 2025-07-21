using EcomAPI.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ChatbotController(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
    {
        var huggingFaceApiKey = _config["HuggingFace:ApiKey"];
        var model = "abc"; 
        var url = $"https://api-inference.huggingface.co/models/{model}";

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url);
        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", huggingFaceApiKey);
        httpRequest.Content = new StringContent(
            JsonSerializer.Serialize(new { inputs = request.Prompt }),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.SendAsync(httpRequest);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, new { error = content });

        // HuggingFace returns a list of objects with generated_text
        var result = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(content);
        var answer = result?.FirstOrDefault()?["generated_text"] ?? "No response";

        return Ok(new ChatResponseDto { Response = answer });
    }
}