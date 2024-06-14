using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace SharedKernel.Extensions
{
    public static class ConvertorExtensions
    {
        public static MemoryStream ToExcel<T>(this List<T> list, string sheetName = "report")
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet(sheetName);

            var headerRow = sheet.CreateRow(0);
            int headerColumnIndex = 0;

            var properties = typeof(T).GetProperties();


            foreach (var property in properties)
            {
                headerRow.CreateCell(headerColumnIndex++).SetCellValue(property.Name);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var row = sheet.CreateRow(i + 1);
                var currentValue = list[i];

                int dataColumnIndex = 0;
                foreach (var property in properties)
                {
                    row.CreateCell(dataColumnIndex++).SetCellValue(property.GetValue(currentValue)?.ToString());
                }
            }

            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream, true);
            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
