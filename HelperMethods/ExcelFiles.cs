using OfficeOpenXml;

namespace JobPortal_New.HelperMethods
{
    public class ExcelFiles
    {
        public static string ExportToExcel<T>(List<T> data, string sheetName)
        {
            var fileInfo = new FileInfo($"{sheetName}.xlsx");
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            using (var package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);

                // Write headers
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].Name == "Password" || properties[i].Name == "ConfirmPassword" || properties[i].Name == "role" || properties[i]== null)
                    {
                        continue;
                    }
                    worksheet.Cells[1, i + 1].Value = properties[i].Name;
                }

                // Write data
                for (int row = 0; row < data.Count; row++)
                {
                    var item = data[row];
                    for (int col = 0; col < properties.Length; col++)
                    {
                        var value = properties[col].GetValue(item);
                        if (value == null)
                        {
                            continue;
                        }
                        if (value is DateTime dateTimeValue)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dateTimeValue;
                            worksheet.Cells[row + 2, col + 1].Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
                        }
                        else
                        {
                            worksheet.Cells[row + 2, col + 1].Value = value;
                        }
                    }
                }

                package.Save();
            }

            return fileInfo.FullName;

        }
    }
}
