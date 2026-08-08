using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/stream", (CancellationToken cancellationToken) => StreamWords(cancellationToken));

static async IAsyncEnumerable<string> StreamWords([EnumeratorCancellation]CancellationToken cancellationToken)
{
    var words = new[] { "Hello", " from", " Glimmer's", " asynchronous", " streaming", " backend!" };

    foreach (var word in words)
    {
        await Task.Delay(200, cancellationToken);
        yield return word;
    }
}

app.Run();
