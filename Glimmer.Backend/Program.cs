using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var ollamaUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
var ollamaModel = builder.Configuration["Ollama:Model"] ?? "qwen2.5-coder";

app.MapGet("/stream", (CancellationToken cancellationToken) => StreamWords(null, cancellationToken));

app.MapPost("/chat", async (ChatRequest request, HttpContext context, IHttpClientFactory clientFactory, CancellationToken token) =>
{
    context.Response.ContentType = "text/plain; charset=utf-8";

    var httpClient = clientFactory.CreateClient();

    var ollamaPayload = new
    {
        model = ollamaModel,
        messages = new[]
        {
            new { role = "user", content = request.Message }
        },
        stream = true
    };

    var content = new StringContent(
        System.Text.Json.JsonSerializer.Serialize(ollamaPayload),
        System.Text.Encoding.UTF8,
        "application/json"
    );

    using var ollamaRequest = new HttpRequestMessage(HttpMethod.Post, $"{ollamaUrl}/api/chat") { Content = content };

    using var response = await httpClient.SendAsync(ollamaRequest, HttpCompletionOption.ResponseHeadersRead, token);
    response.EnsureSuccessStatusCode();

    using var stream = await response.Content.ReadAsStreamAsync(token);
    using var reader = new System.IO.StreamReader(stream);

    while (!token.IsCancellationRequested)
    {
        var line = await reader.ReadLineAsync(token);
        if (line is null) break;

        if (string.IsNullOrWhiteSpace(line)) continue;

        // Parse the JSON chunk returned by Ollama
        using var doc = System.Text.Json.JsonDocument.Parse(line);
        if (doc.RootElement.TryGetProperty("message", out var messageProp) &&
            messageProp.TryGetProperty("content", out var contentProp))
        {
            var wordPiece = contentProp.GetString();
            if (!string.IsNullOrEmpty(wordPiece))
            {
                await context.Response.WriteAsync(wordPiece, token);
                await context.Response.Body.FlushAsync(token);
            }
        }
    }
});

app.MapPost("/chat2", async (ChatRequest request, HttpContext context, CancellationToken token) =>
{
    context.Response.ContentType = "text/plain; charset=utf-8";

    string[] words = request.Message.Split(' ');
    var wordsList = words.ToList();

    for (int i = 0; i < wordsList.Count; i++)
    {
        var wordToStream = i < wordsList.Count - 1 ? wordsList[i] + " " : wordsList[i];
 
        await Task.Delay(200, token);
        await context.Response.WriteAsync(wordToStream, token);
        await context.Response.Body.FlushAsync(token);
    }
});

static async IAsyncEnumerable<string> StreamWords(IEnumerable<string>? wordsToStream, [EnumeratorCancellation]CancellationToken cancellationToken)
{
    var words = wordsToStream ?? [ "Hello", "from", "Glimmer's", "asynchronous", "streaming", "backend!" ];

    var wordsList = words.ToList();
    for (int i = 0; i < wordsList.Count; i++)
    {
        var wordToStream = i < wordsList.Count - 1 ? wordsList[i] + " " : wordsList[i];
        await Task.Delay(200, cancellationToken);
        yield return wordToStream;
    }
}


app.Run();

record ChatRequest(string Message);
