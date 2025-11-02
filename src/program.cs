using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using QRCoder;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5000");
var app = builder.Build();

var baseDir = Directory.GetCurrentDirectory();
var srcFolder = Path.Combine(baseDir, "src");
if (!Directory.Exists(srcFolder)) Directory.CreateDirectory(srcFolder);

app.UseDefaultFiles(new DefaultFilesOptions {
    FileProvider = new PhysicalFileProvider(srcFolder),
    RequestPath = ""
});
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(srcFolder),
    RequestPath = ""
});

// in-memory store + counter
var store = new ConcurrentDictionary<int, Book>();
int counter = 0;

// POST /api/books -> vrátí { id, qrcodeUrl }
app.MapPost("/api/books", async (HttpRequest req) =>
{
    var b = await req.ReadFromJsonAsync<Book>() ?? new Book();
    if (string.IsNullOrWhiteSpace(b.Title))
        return Results.BadRequest(new { message = "Název je povinný." });

    var id = Interlocked.Increment(ref counter);
    b.Id = id;
    store[id] = b;

    var qrcodeUrl = $"{req.Scheme}://{req.Host}/api/books/{id}/qrcode";
    return Results.Json(new { id, qrcodeUrl });
});

// GET /api/books/{id}/qrcode -> PNG QR s odkazem na /books/{id}
app.MapGet("/api/books/{id:int}/qrcode", (int id, HttpRequest req) =>
{
    if (!store.TryGetValue(id, out var b))
        return Results.NotFound();

    var payload = $"{req.Scheme}://{req.Host}/books/{id}";

    using var qrGen = new QRCodeGenerator();
    using var qrData = qrGen.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
    using var png = new PngByteQRCode(qrData);
    var bytes = png.GetGraphic(pixelsPerModule: 20); // velikost upravit podle potřeby

    return Results.File(bytes, "image/png");
});

// jednoduchý HTML detail pro /books/{id}
app.MapGet("/books/{id:int}", (int id) =>
{
    if (!store.TryGetValue(id, out var b))
        return Results.NotFound("Kniha nenalezena.");

    string Esc(string? s) => HtmlEncoder.Default.Encode(s ?? "");
    var html = $@"<!doctype html>
<html lang=""cs""><head><meta charset=""utf-8""><meta name=""viewport"" content=""width=device-width,initial-scale=1"">
<title>{Esc(b.Title)}</title></head>
<body style=""font-family:Segoe UI,Arial,sans-serif;padding:20px"">
  <h1>{Esc(b.Title)}</h1>
  <p><strong>Autor:</strong> {Esc(b.Author)}</p>
  <p><strong>ISBN:</strong> {Esc(b.Isbn)}</p>
  <p><small>Id: {b.Id}</small></p>
</body></html>";
    return Results.Content(html, "text/html");
});

app.Run();

internal class Book
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    public string? Publisher { get; set; }
    public string? Literature { get; set; }
    public string? Year { get; set; }
    public int? Century { get; set; }
    public string? Shelf { get; set; }
    public string? Note { get; set; }
}