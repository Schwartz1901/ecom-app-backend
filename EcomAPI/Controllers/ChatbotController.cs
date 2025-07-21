using EcomAPI.Controllers.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public ChatbotController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> Ask([FromBody] ChatRequestDto request)
    {
        var apiKey = _config["Groq:ApiKey"];
        var url = "https://api.groq.com/openai/v1/chat/completions";

        var payload = new
        {
            model = "llama-3.1-8b-instant", 
            messages = new[]
            {
                new { role = "user", content = request.Prompt }
            }
        };

        var httpClient = _httpClientFactory.CreateClient();

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
        };

        httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var response = await httpClient.SendAsync(httpRequest);
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return StatusCode((int)response.StatusCode, new { error = content });

        using var jsonDoc = JsonDocument.Parse(content);
        var answer = jsonDoc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        return Ok(new ChatResponseDto { Response = answer?.Trim() ?? "No response" });
    }
}
