using Xunit;
using Bookong.Web.Services;
using Bookong.Web.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Bookong.Tests
{
    public class TeachingLogicTests
    {
        // Pomocná metoda - vyrobí data "jakože" z databáze
        private List<FolderDto> GetTestData()
        {
            return new List<FolderDto>
            {
                new FolderDto { Name = "Matematika", Field = "IT", CreatedBy = "Novak", CreatedAt = new DateTime(2023, 1, 10) },
                new FolderDto { Name = "Čeština", Field = "Lyceum", CreatedBy = "Svoboda", CreatedAt = new DateTime(2023, 1, 15) },
                new FolderDto { Name = "Angličtina", Field = "IT", CreatedBy = "Novak", CreatedAt = new DateTime(2023, 2, 1) }
            };
        }

        [Fact]
        public void Filter_BySearchTerm_ShouldReturnMatches()
        {
            // Arrange
            var logic = new TeachingLogicService();
            var data = GetTestData();

            // Act - Hledáme slovo "Matematika"
            var result = logic.FilterFolders(data, "Matematika", null, null, null, null);

            // Assert - Musí najít 1 výsledek a jmenovat se Matematika
            Assert.Single(result);
            Assert.Equal("Matematika", result.First().Name);
        }

        [Fact]
        public void Filter_ByAuthor_ShouldReturnSpecificAuthor()
        {
            // Arrange
            var logic = new TeachingLogicService();
            var data = GetTestData();

            // Act - Hledáme vše od učitele "Novak"
            var result = logic.FilterFolders(data, null, null, "Novak", null, null);

            // Assert - Musí najít 2 výsledky (Matiku a Angličtinu)
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void ValidateUrl_ValidHttp_ShouldReturnTrue()
        {
            var logic = new TeachingLogicService();

            // Zkoušíme platný odkaz
            bool result = logic.IsValidUrl("https://google.com");

            Assert.True(result); // Musí být TRUE
        }

        [Fact]
        public void ValidateUrl_InvalidString_ShouldReturnFalse()
        {
            var logic = new TeachingLogicService();

            // Zkoušíme nesmysl
            bool result = logic.IsValidUrl("tohle-neni-odkaz");

            Assert.False(result); // Musí být FALSE (chyba)
        }
    }
}
