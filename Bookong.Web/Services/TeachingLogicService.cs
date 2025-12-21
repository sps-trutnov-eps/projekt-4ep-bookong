using System;
using System.Collections.Generic;
using System.Linq;
using Bookong.Web.Models;

namespace Bookong.Web.Services
{
    public class TeachingLogicService
    {
        // Tuhle metodu bude volat Frontend, aby získal vyfiltrovaná data
        public List<FolderDto> FilterFolders(
            IEnumerable<FolderDto> folders,
            string searchTerm,
            string filterField,
            string filterAuthor,
            DateTime? filterFrom,
            DateTime? filterTo)
        {
            if (folders == null) return new List<FolderDto>();

            var query = folders.AsEnumerable();

            // 1. Fulltext hledání (hledá v názvu, oboru i autorovi)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(f =>
                    (f.Name ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (f.Field ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    (f.CreatedBy ?? "").Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Filtrování podle konkrétních polí
            if (!string.IsNullOrWhiteSpace(filterField))
            {
                query = query.Where(f => (f.Field ?? "").Contains(filterField, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(filterAuthor))
            {
                query = query.Where(f => (f.CreatedBy ?? "").Contains(filterAuthor, StringComparison.OrdinalIgnoreCase));
            }

            // 3. Filtrování podle data (od - do)
            if (filterFrom.HasValue)
            {
                query = query.Where(f => f.CreatedAt.Date >= filterFrom.Value.Date);
            }

            if (filterTo.HasValue)
            {
                query = query.Where(f => f.CreatedAt.Date <= filterTo.Value.Date);
            }

            return query.ToList();
        }

        // Validace, zda je URL odkaz platný (aby tam nikdo nenapsal nesmysl)
        public bool IsValidUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;

            // Kontrola, zda je to http nebo https odkaz
            bool result = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }
    }
}
