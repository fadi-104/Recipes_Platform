using Core.Exceptions;
using DomainLayer.Requests;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;


namespace BusinessLogicLayer.Services.AiService
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> GetAiResponseAsync(List<string> prompt)
        {
            string apiKey = _configuration["Gemini:ApiKey"];
            string model = "gemini-2.5-flash";

            string url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

            var promptText = string.Join(", ", prompt);
            var body = new
            {
                system_instruction = new
                {
                    parts = new[]
                {
                    new { text = "أنت مساعد متخصص فقط في إنشاء وصفات بناءً على المكونات. " +
                                 "إذا طُلب منك أي شيء آخر خارج الطبخ، رد بعبارة: 'أنا مختص فقط بالوصفات.' " +
                                 "يجب أن يكون ردك دائمًا بصيغة JSON كالتالي: " +
                                 "{ 'title': '...', 'ingredients': ['...'], 'steps': ['...'] }" }
                }
                },
                contents = new[]
                {
                new
                {
                    role = "user",
                    parts = new[]
                    {
                        new { text = promptText }
                    }
                }
            }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}\n{errorBody}");
            }

            string respJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(respJson);
            var root = doc.RootElement;

            if (root.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0 &&
                candidates[0].TryGetProperty("content", out var contentObj) &&
                contentObj.TryGetProperty("parts", out var partsArr) &&
                partsArr.GetArrayLength() > 0 &&
                partsArr[0].TryGetProperty("text", out var textEl))
            {
                return textEl.GetString() ?? "";
            }

            return "";

        }

        public async Task<string> SendToN8N(string prompt)
        {
            var url = "https://fadiyousef107.app.n8n.cloud/webhook/c2a89d13-5d67-4619-9799-292ae4dc9810";

            var body = new
            {
                    text = prompt
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();

        }
        
    }
}
