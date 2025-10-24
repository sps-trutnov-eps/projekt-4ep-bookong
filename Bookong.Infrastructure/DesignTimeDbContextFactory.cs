using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Bookong.Infrastructure.Data;

namespace Bookong.Infrastructure
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BookongDbContext>
    {
        public BookongDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();

            // search upward for Bookong.Web folder
            string? webProjectPath = null;
            var searchDir = basePath;
            for (int i = 0; i < 6; i++)
            {
                var candidate = Path.Combine(searchDir, "Bookong.Web");
                if (Directory.Exists(candidate))
                {
                    webProjectPath = candidate;
                    break;
                }

                var parent = Directory.GetParent(searchDir);
                if (parent == null) break;
                searchDir = parent.FullName;
            }

            // fallback to relative ../Bookong.Web
            if (webProjectPath == null)
            {
                var fallback = Path.GetFullPath(Path.Combine(basePath, "..", "Bookong.Web"));
                if (Directory.Exists(fallback)) webProjectPath = fallback;
            }

            if (webProjectPath == null)
            {
                // last resort: use basePath
                webProjectPath = basePath;
            }

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var builder = new ConfigurationBuilder()
                .SetBasePath(webProjectPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{env}.json", optional: true);

            IConfiguration configuration = builder.Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Fallback to a sensible default if configuration is missing (helps local dev)
            if (string.IsNullOrEmpty(connectionString))
            {
                // Try to read appsettings from parent working directory if it exists
                var altPath = Path.GetFullPath(Path.Combine(basePath, "..", "Bookong.Web", "appsettings.json"));
                if (File.Exists(altPath))
                {
                    var altConfig = new ConfigurationBuilder()
                        .AddJsonFile(altPath, optional: false)
                        .Build();
                    connectionString = altConfig.GetConnectionString("DefaultConnection");
                }
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                // Final fallback to localdb default used in template
                connectionString = "Server=(localdb)\\mssqllocaldb;Database=BookongDb;Trusted_Connection=True;";
            }

            var optionsBuilder = new DbContextOptionsBuilder<BookongDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BookongDbContext(optionsBuilder.Options);
        }
    }
}