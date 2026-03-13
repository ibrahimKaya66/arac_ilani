using System.ClientModel;
using AracIlan.Uygulama.Servisler;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace AracIlan.Altyapi.Servisler;

/// <summary>
/// AI servisi - OpenAI veya Groq (ücretsiz) ile teknik özellik ve expertiz analizi.
/// AI:Provider = "Groq" veya "OpenAI". Key yoksa null döner.
/// </summary>
public class AIServisi(IConfiguration config, IHostEnvironment ortam, ILogger<AIServisi> log) : IAIServisi
{
    private readonly string _provider = config["AI:Provider"] ?? "Groq";
    private readonly string? _groqKey = config["AI:Groq:ApiKey"] ?? config["Groq:ApiKey"];
    private readonly string? _openAiKey = config["AI:OpenAI:ApiKey"] ?? config["OpenAI:ApiKey"];

    private const string GroqTextModel = "llama-3.1-70b-versatile";
    private const string GroqVisionModel = "meta-llama/llama-4-scout-17b-16e-instruct";
    private const string OpenAIModel = "gpt-4o";
    private const string GroqEndpoint = "https://api.groq.com/openai/v1";

    private (OpenAIClient Client, string Model) GetTextClient()
    {
        if (_provider.Equals("Groq", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(_groqKey))
        {
            var opts = new OpenAIClientOptions { Endpoint = new Uri(GroqEndpoint) };
            return (new OpenAIClient(new ApiKeyCredential(_groqKey), opts), GroqTextModel);
        }
        if (!string.IsNullOrWhiteSpace(_openAiKey))
            return (new OpenAIClient(new ApiKeyCredential(_openAiKey)), OpenAIModel);
        return (null!, null!);
    }

    private (OpenAIClient Client, string Model) GetVisionClient()
    {
        if (_provider.Equals("Groq", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(_groqKey))
        {
            var opts = new OpenAIClientOptions { Endpoint = new Uri(GroqEndpoint) };
            return (new OpenAIClient(new ApiKeyCredential(_groqKey), opts), GroqVisionModel);
        }
        if (!string.IsNullOrWhiteSpace(_openAiKey))
            return (new OpenAIClient(new ApiKeyCredential(_openAiKey)), OpenAIModel);
        return (null!, null!);
    }

    public async Task<string?> TeknikOzellikUretAsync(string markaAd, string modelAd, string motorAd, int? motorHacmi, string yakitTipi, int? guc, int uretimYili, CancellationToken iptal = default)
    {
        var (client, model) = GetTextClient();
        if (client == null) return null;

        try
        {
            var chatClient = client.GetChatClient(model);
            var prompt = $"{markaAd} {modelAd} aracı için teknik özellikleri JSON formatında üret. " +
                $"Motor: {motorAd}, Hacim: {motorHacmi ?? 0}cc, Yakıt: {yakitTipi}, Güç: {guc ?? 0} HP, Yıl: {uretimYili}. " +
                "Sadece JSON döndür - başka metin yok.";

            var response = await chatClient.CompleteChatAsync(
                [new UserChatMessage(prompt)],
                new ChatCompletionOptions { Temperature = 0.3f },
                iptal);

            var icerik = response.Value.Content[0].Text;
            return string.IsNullOrWhiteSpace(icerik) ? null : icerik.Trim();
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "AI teknik özellik üretme hatası");
            return null;
        }
    }

    public async Task<string?> ExpertizGorselAnalizAsync(string? gorselYolu, Stream? gorselStream, CancellationToken iptal = default)
    {
        var (client, model) = GetVisionClient();
        if (client == null) return null;

        try
        {
            byte[]? bytes = null;
            if (gorselStream != null && gorselStream.CanRead)
            {
                using var ms = new MemoryStream();
                await gorselStream.CopyToAsync(ms, iptal);
                bytes = ms.ToArray();
            }
            else if (!string.IsNullOrEmpty(gorselYolu))
            {
                var fizikselYol = gorselYolu.StartsWith("/uploads/", StringComparison.OrdinalIgnoreCase)
                    ? Path.Combine(ortam.ContentRootPath, "uploads", gorselYolu.Replace("/uploads/", "").Replace("/", Path.DirectorySeparatorChar.ToString()))
                    : gorselYolu;
                if (File.Exists(fizikselYol))
                    bytes = await File.ReadAllBytesAsync(fizikselYol, iptal);
            }

            if (bytes == null || bytes.Length == 0) return null;

            var chatClient = client.GetChatClient(model);
            var prompt = "Bu araç expertiz görselini analiz et. Hangi parçalar orijinal, hangileri boyalı veya değiştirilmiş belirt. " +
                "JSON formatında döndür. Sadece JSON - başka metin yok.";

            var contentParts = new List<ChatMessageContentPart>
            {
                ChatMessageContentPart.CreateTextPart(prompt),
                ChatMessageContentPart.CreateImagePart(new BinaryData(bytes), "image/jpeg")
            };

            var userMessage = ChatMessage.CreateUserMessage(contentParts);
            var response = await chatClient.CompleteChatAsync(
                [userMessage],
                new ChatCompletionOptions { Temperature = 0.2f },
                iptal);

            var icerik = response.Value.Content[0].Text;
            return string.IsNullOrWhiteSpace(icerik) ? null : icerik.Trim();
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "AI expertiz analizi hatası");
            return null;
        }
    }
}
