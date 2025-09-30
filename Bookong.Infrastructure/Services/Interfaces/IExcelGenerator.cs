namespace Bookong.Infrastructure.Services.Interfaces
{
    public interface IExcelGenerator
    {
        byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName) where T : class;
    }
}
