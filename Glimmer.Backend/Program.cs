using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/stream", (CancellationToken cancellationToken) => StreamWords(null, cancellationToken));

app.MapPost("/chat", async (ChatRequest request, HttpContext context, CancellationToken token) =>
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
