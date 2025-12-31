using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace Core.Utilities.ExternalServices
{
    public class LlmClient : ILlmClient
    {
        private readonly HttpClient _httpClient;

        public LlmClient(string siteUrl = null, string siteName = null)
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY")
              ?? throw new ArgumentNullException("API key gerekli!");

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            if (!string.IsNullOrEmpty(siteUrl))
                _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", siteUrl);

            if (!string.IsNullOrEmpty(siteName))
                _httpClient.DefaultRequestHeaders.Add("X-Title", siteName);
        }

        public async Task<string> SendMessageAsync(string prompt, string systemPrompt = null, string model = "xiaomi/mimo-v2-flash:free", Dictionary<string, object> extraBody = null)
        {
            var messageList = new List<object>();

            if (!string.IsNullOrEmpty(systemPrompt))
            {
                messageList.Add(new { role = "system", content = systemPrompt });
            }

            messageList.Add(new { role = "user", content = prompt });

            var requestBody = new Dictionary<string, object>
            {
                ["model"] = model,
                ["messages"] = messageList
            };

            if (extraBody != null)
            {
                foreach (var kvp in extraBody)
                    requestBody[kvp.Key] = kvp.Value;
            }

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            var llmResponse = JsonSerializer.Deserialize<LlmResponse>(
                responseJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            var finalContent = llmResponse?.Choices?[0]?.Message?.Content ?? string.Empty;

            if (finalContent.Contains("assistantfinal"))
            {
                int idx = finalContent.IndexOf("assistantfinal") + "assistantfinal".Length;
                finalContent = finalContent.Substring(idx).Trim();
            }

            return finalContent;
        }
    }

    
    public class LlmResponse
    {
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        public LlmMessage Message { get; set; }
    }

    public class LlmMessage
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
