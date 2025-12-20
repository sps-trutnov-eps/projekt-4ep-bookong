using Microsoft.Extensions.FileProviders;
using System.Collections.Concurrent;
using System.Text.Encodings.Web;
using QRCoder;
using System.Threading;
using OfficeOpenXml;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5001");
var app = builder.Build();

// Zjistíme přesnou cestu, kde aplikace právě "stojí"
var rootPath = AppContext.BaseDirectory; 
// Pokud spouštíte přes dotnet run, aktuální složka je:
var currentDir = Directory.GetCurrentDirectory();

Console.WriteLine($"Aplikace běží v: {currentDir}");
// EPPlus 8: nastavení licence (nekomerční použití)
ExcelPackage.License.SetNonCommercialPersonal("BooKong Dev");
var dataDir = Path.GetFullPath(Path.Combine(currentDir, "..", "Data"));

// Nastavení statických souborů pro aktuální složku
var defaultFiles = new DefaultFilesOptions {
    FileProvider = new PhysicalFileProvider(currentDir),
    RequestPath = ""
};
defaultFiles.DefaultFileNames.Clear();
defaultFiles.DefaultFileNames.Add("qrcode-generator.html");
app.UseDefaultFiles(defaultFiles);

app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(currentDir),
    RequestPath = ""
});

// Ruční cesta pro otestování, jestli server vůbec žije
app.MapGet("/test", () => "Server funguje, zkuste /qrcode-generator.html");

// Přesměrování z hlavní stránky
// Základní stránka i fallback pro SPA
app.MapGet("/", () => Results.Redirect("/qrcode-generator.html"));
app.MapFallbackToFile("qrcode-generator.html");

// Načtení knih z Excel souborů ve složce Data
string Norm(string s) => s.Trim().ToLowerInvariant()
    .Replace("á","a").Replace("č","c").Replace("ď","d").Replace("é","e")
    .Replace("ě","e").Replace("í","i").Replace("ň","n").Replace("ó","o")
    .Replace("ř","r").Replace("š","s").Replace("ť","t").Replace("ú","u")
    .Replace("ů","u").Replace("ý","y").Replace("ž","z");

List<Book> LoadBooksFromData()
{
    var result = new List<Book>();
    if (!Directory.Exists(dataDir)) return result;

    foreach (var file in Directory.EnumerateFiles(dataDir))
    {
        var ext = Path.GetExtension(file).ToLowerInvariant();
        if (ext != ".xlsx" && ext != ".xls") continue;

        using var package = new ExcelPackage(new FileInfo(file));
        foreach (var ws in package.Workbook.Worksheets)
        {
            if (ws.Dimension == null) continue;
            int startRow = ws.Dimension.Start.Row;
            int endRow = ws.Dimension.End.Row;
            int startCol = ws.Dimension.Start.Column;
            int endCol = ws.Dimension.End.Column;

            var map = new Dictionary<string,int>();
            for (int c = startCol; c <= endCol; c++)
            {
                var header = ws.Cells[startRow, c].Text?.Trim();
                if (string.IsNullOrEmpty(header)) continue;
                var h = Norm(header);
                map[h] = c;
            }

            int TitleCol = -1, AuthorCol = -1, IsbnCol = -1, PublisherCol = -1, YearCol = -1, CenturyCol = -1;
            foreach (var kv in map)
            {
                var h = kv.Key; var c = kv.Value;
                if (TitleCol < 0 && (h.Contains("nazev") || h.Contains("title") || h.Contains("kniha") || h.Contains("titul"))) TitleCol = c;
                if (AuthorCol < 0 && (h.Contains("autor") || h.Contains("author"))) AuthorCol = c;
                if (IsbnCol < 0 && h.Contains("isbn")) IsbnCol = c;
                if (PublisherCol < 0 && (h.Contains("nakladatel") || h.Contains("nakladatelstvi") || h.Contains("publisher"))) PublisherCol = c;
                if (YearCol < 0 && (h.Contains("rok") || h.Contains("year"))) YearCol = c;
                if (CenturyCol < 0 && (h.Contains("stoleti") || h.Contains("century"))) CenturyCol = c;
            }

            for (int r = startRow + 1; r <= endRow; r++)
            {
                string title = TitleCol > 0 ? ws.Cells[r, TitleCol].Text?.Trim() ?? string.Empty : string.Empty;
                if (string.IsNullOrWhiteSpace(title)) continue;

                var b = new Book
                {
                    Title = title,
                    Author = AuthorCol > 0 ? ws.Cells[r, AuthorCol].Text?.Trim() : null,
                    Isbn = IsbnCol > 0 ? ws.Cells[r, IsbnCol].Text?.Trim() : null,
                    Publisher = PublisherCol > 0 ? ws.Cells[r, PublisherCol].Text?.Trim() : null,
                };

                if (YearCol > 0 && int.TryParse(ws.Cells[r, YearCol].Text?.Trim(), out var year)) b.Year = year;
                if (CenturyCol > 0 && int.TryParse(ws.Cells[r, CenturyCol].Text?.Trim(), out var cent)) b.Century = cent;

                result.Add(b);
            }
        }
    }

    return result;
}

// API: vrátí knihy načtené ze složky Data
app.MapGet("/api/data/books", (int? limit) => {
    var list = LoadBooksFromData();
    list = list
        .OrderBy(b => b.Title ?? "")
        .Take(limit.HasValue && limit.Value > 0 ? limit.Value : 200)
        .ToList();
    return Results.Json(list);
});

// API: vygeneruje QR kód pro libovolný textový payload
app.MapGet("/api/qrcode", (string payload) => {
    using var qrGen = new QRCodeGenerator();
    using var qrData = qrGen.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
    using var png = new PngByteQRCode(qrData);
    var bytes = png.GetGraphic(20);
    return Results.File(bytes, "image/png");
});

// --- VAŠE LOGIKA ---
var store = new ConcurrentDictionary<int, Book>();
int counter = 0;

app.MapPost("/api/books", async (HttpRequest req) => {
    var b = await req.ReadFromJsonAsync<Book>() ?? new Book();
    if (string.IsNullOrWhiteSpace(b.Title)) return Results.BadRequest(new { message = "Název je povinný." });
    var id = Interlocked.Increment(ref counter);
    b.Id = id;
    store[id] = b;
    var qrcodeUrl = $"{req.Scheme}://{req.Host}/api/books/{id}/qrcode";
    return Results.Json(new { id, qrcodeUrl });
});

app.MapGet("/api/books/{id:int}/qrcode", (int id, HttpRequest req) => {
    if (!store.TryGetValue(id, out var b)) return Results.NotFound();
    var payload = $"{req.Scheme}://{req.Host}/books/{id}";
    using var qrGen = new QRCodeGenerator();
    using var qrData = qrGen.CreateQrCode(payload, QRCodeGenerator.ECCLevel.Q);
    using var png = new PngByteQRCode(qrData);
    var bytes = png.GetGraphic(20);
    return Results.File(bytes, "image/png");
});

app.MapGet("/books/{id:int}", (int id) => {
    if (!store.TryGetValue(id, out var b)) return Results.NotFound("Kniha nenalezena.");
    string Esc(string? s) => HtmlEncoder.Default.Encode(s ?? "");
    return Results.Content($"<html><body><h1>{Esc(b.Title)}</h1></body></html>", "text/html");
});

app.Run();

internal class Book {
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Isbn { get; set; }
    public string? Publisher { get; set; }
    public int? Year { get; set; }
    public int? Century { get; set; }
}