using ClosedXML.Excel;
using ECommerce.Business.Excel;
using ECommerce.Models;

namespace Ecommerce.Tests
{
    public class ProductExcelServiceTests
    {
        [Fact]
        public void ReadProducts_ReturnsProducts_WhenExcelDataIsValid()
        {
            // Arrange
            using var stream = new MemoryStream();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");

                worksheet.Cell(1, 1).Value = "Name";
                worksheet.Cell(1, 2).Value = "Description";
                worksheet.Cell(1, 3).Value = "Price";
                worksheet.Cell(1, 4).Value = "CategoryId";

                worksheet.Cell(2, 1).Value = "Laptop";
                worksheet.Cell(2, 2).Value = "Dell Laptop";
                worksheet.Cell(2, 3).Value = 55000;
                worksheet.Cell(2, 4).Value = 1;

                workbook.SaveAs(stream);
            }

            stream.Position = 0;

            var excelService = new ProductExcelService();

            // Act
            var result = excelService.ReadProducts(stream);

            // Assert
            Assert.Single(result);
            Assert.Equal("Laptop", result[0].Name);
            Assert.Equal("Dell Laptop", result[0].Description);
            Assert.Equal(55000, result[0].Price);
            Assert.Equal(1, result[0].CategoryId);
        }
    }
}