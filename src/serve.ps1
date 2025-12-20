param(
    [int]$Port = 5500,
    [string]$Root = "c:\Users\BESTCOMP.cz\Desktop\projekt-4ep-bookong\src"
)
$listener = New-Object System.Net.HttpListener
$prefix = "http://localhost:$Port/"
$listener.Prefixes.Add($prefix)
$listener.Start()
Write-Host "Serving $Root on $prefix" -ForegroundColor Green

function Get-ContentType($ext){
    switch ($ext.ToLower()) {
        ".html" { "text/html; charset=utf-8" }
        ".css"  { "text/css" }
        ".js"   { "application/javascript" }
        ".json" { "application/json" }
        ".png"  { "image/png" }
        ".jpg"  { "image/jpeg" }
        ".jpeg" { "image/jpeg" }
        ".svg"  { "image/svg+xml" }
        ".ico"  { "image/x-icon" }
        default  { "application/octet-stream" }
    }
}

try {
    while ($listener.IsListening) {
        $ctx = $listener.GetContext()
        $req = $ctx.Request
        $res = $ctx.Response

        $path = $req.Url.AbsolutePath.TrimStart('/')
        if ([string]::IsNullOrWhiteSpace($path)) { $path = "qrcode-generator.html" }
        $fsPath = Join-Path $Root $path

        if (-not (Test-Path $fsPath)) {
            $res.StatusCode = 404
            $bytes = [System.Text.Encoding]::UTF8.GetBytes("Not Found")
            $res.OutputStream.Write($bytes,0,$bytes.Length)
            $res.Close()
            continue
        }

        try {
            $bytes = [System.IO.File]::ReadAllBytes($fsPath)
            $ext = [System.IO.Path]::GetExtension($fsPath)
            $res.ContentType = Get-ContentType $ext
            $res.Headers.Add("Cache-Control","no-cache, no-store")
            $res.OutputStream.Write($bytes,0,$bytes.Length)
            $res.Close()
        } catch {
            $res.StatusCode = 500
            $err = [System.Text.Encoding]::UTF8.GetBytes("Server error")
            $res.OutputStream.Write($err,0,$err.Length)
            $res.Close()
        }
    }
} finally {
    $listener.Stop()
    $listener.Close()
}
