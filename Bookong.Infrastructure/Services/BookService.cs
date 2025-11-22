using OfficeOpenXml;
using Bookong.Domain.Entities;

namespace Bookong.Infrastructure.Services
{
    public class BookService
    {
        private readonly string file1 = "Data/Školní_knihovna_2023-06-23_komplet UC i ZACI.xlsx";
        private readonly string file2 = "Data/Knihovna Buky_ucitelska.xls";

        public BookService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public List<Book> LoadBooks()
        {
            var books = new List<Book>();
            LoadFromExcel(file1, books);
            LoadFromExcel(file2, books);
            return books;
        }

        private void LoadFromExcel(string path, List<Book> books)
        {
            using var package = new ExcelPackage(new FileInfo(path));
            var sheet = package.Workbook.Worksheets[0];

            int row = 2; // první řádek je hlavička
            while (!string.IsNullOrEmpty(sheet.Cells[row, 1].Text))
            {
                books.Add(new Book
                {
                    Nazev = sheet.Cells[row, 1].Text,
                    Stoleti = sheet.Cells[row, 2].Text,
                    Autor = sheet.Cells[row, 3].Text,
                    ISBN = sheet.Cells[row, 4].Text
                });

                row++;
            }
        }
    }
}
