using ClosedXML.Excel;
using ECommerce.Models;
using System.Globalization;

namespace ECommerce.Business.Excel
{
    public class ProductExcelService
    {
        public List<Product> ReadProducts(Stream stream)
        {
            var products = new List<Product>();

            using var workbook = new XLWorkbook(stream);

            var worksheet = workbook.Worksheet(1);
            var headerRow = worksheet.Row(1);

            var headerMap = headerRow
                .CellsUsed()
                .ToDictionary(
                    c => NormalizeHeader(c.GetString()),
                    c => c.Address.ColumnNumber,
                    StringComparer.OrdinalIgnoreCase);

            int nameColumn = GetRequiredColumn(headerMap, "name");
            int descriptionColumn = GetRequiredColumn(headerMap, "description");
            int priceColumn = GetRequiredColumn(headerMap, "price");
            int categoryIdColumn = GetRequiredColumn(headerMap, "categoryid", "category id");

            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                // Ignore completely empty rows
                if (row.CellsUsed().Count() == 0)
                {
                    continue;
                }

                int rowNumber = row.RowNumber();

                string name = row.Cell(nameColumn).GetString().Trim();
                string description = row.Cell(descriptionColumn).GetString().Trim();

                // Validate Name
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InvalidDataException(
                        $"Row {rowNumber}: Product name is required.");
                }

                // Validate Description
                if (string.IsNullOrWhiteSpace(description))
                {
                    throw new InvalidDataException(
                        $"Row {rowNumber}: Description is required.");
                }

                // Validate Price
                if (!TryReadDecimal(row.Cell(priceColumn), out decimal price))
                {
                    var rawPrice = row.Cell(priceColumn).GetFormattedString();
                    throw new InvalidDataException(
                        $"Row {rowNumber}: Price must be a valid number. Value found: '{rawPrice}'.");
                }

                if (price <= 0)
                {
                    throw new InvalidDataException(
                        $"Row {rowNumber}: Price must be greater than 0.");
                }

                // Validate CategoryId
                if (!TryReadInt(row.Cell(categoryIdColumn), out int categoryId))
                {
                    throw new InvalidDataException(
                        $"Row {rowNumber}: CategoryId must be a valid number.");
                }

                if (categoryId <= 0)
                {
                    throw new InvalidDataException(
                        $"Row {rowNumber}: CategoryId must be greater than 0.");
                }

                var product = new Product
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    CategoryId = categoryId
                };

                products.Add(product);
            }

            return products;
        }

        private static string NormalizeHeader(string header)
        {
            return new string(header.Where(c => !char.IsWhiteSpace(c)).ToArray()).ToLowerInvariant();
        }

        private static int GetRequiredColumn(
            Dictionary<string, int> headerMap,
            params string[] headers)
        {
            foreach (var header in headers)
            {
                var normalized = NormalizeHeader(header);
                if (headerMap.TryGetValue(normalized, out int column))
                {
                    return column;
                }
            }

            throw new InvalidDataException($"Missing required column: {headers[0]}.");
        }

        private static bool TryReadDecimal(IXLCell cell, out decimal value)
        {
            if (cell.TryGetValue<decimal>(out value))
            {
                return true;
            }

            var text = cell.GetString().Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                text = cell.GetFormattedString().Trim();
            }

            text = text.Replace("₹", string.Empty)
                       .Replace("$", string.Empty)
                       .Replace(",", string.Empty)
                       .Trim();

            return decimal.TryParse(
                text,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out value)
                || decimal.TryParse(
                    text,
                    NumberStyles.Number,
                    CultureInfo.CurrentCulture,
                    out value);
        }

        private static bool TryReadInt(IXLCell cell, out int value)
        {
            if (cell.TryGetValue<int>(out value))
            {
                return true;
            }

            return int.TryParse(cell.GetString().Trim(), out value);
        }
    }
}