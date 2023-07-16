using OfficeOpenXml;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq.Expressions;
using System.Reflection;

namespace api.Helper
{
    public static class FileExcel
    {
        public static IList<T> ImportExcelToEntity<T>(IFormFile formFile) where T : class, new()
        {
            try
            {
                var entitiesList = new List<T>();
                List<string> propertiesNames = new List<string>(ExtractPropertiesName<T>(EntityCreator<T>()));
                // var columnsNames = typeof(T).GetProperties();
                //var DisplayNames = columnsNames.Select(p => p.GetCustomAttributes(typeof(DisplayNameAttribute), false).OfType<DisplayNameAttribute>().FirstOrDefault()?.DisplayName ?? p.Name).ToArray();
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage(formFile.OpenReadStream()))
                {
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                    {
                        return null;
                    }
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    bool currentColExists = false;
                    string currentPropName = string.Empty;
                    for (int row = 1; row < rowCount; row++)
                    {
                        T currentEntityVM;
                        currentEntityVM = EntityCreator<T>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            currentPropName = worksheet.Cells[1, col]?.Value?.ToString().Trim();
                            if (currentPropName != null)
                            {
                                currentColExists = ColumnNameExistis(currentPropName, propertiesNames) ? true : false;
                                if (currentColExists)
                                {
                                    AssignValue(ref currentEntityVM, currentPropName.Replace(" ", ""), worksheet.Cells[row + 1, col]?.Text?.Trim());
                                }
                            }
                        }
                        if (Valid(currentEntityVM))
                        {
                            entitiesList.Add(currentEntityVM);
                        }
                    }
                }
                return entitiesList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static byte[]? ExportEntityToByte<T>(IList<T> dataList) where T : class, new()
        {
            if (dataList == null || dataList.Count() == 0)
            {
                return null;
            }
            var columnsNames = typeof(T).GetProperties().ToList();
            if (columnsNames == null || columnsNames.Count() == 0)
            {
                return null;
            }
            var DisplayColumnsNames = columnsNames.Select(p => new
            {
                DisplayName = p.GetCustomAttributes(typeof(DisplayNameAttribute), false)
                .OfType<DisplayNameAttribute>()
                .FirstOrDefault()?.DisplayName
                ?? null,
                columnName = p.Name
            })
                .ToList();
            byte[]? fileData = null;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            using (var p = new ExcelPackage())
            {
                try
                {
                    ExcelWorksheet ws = p.Workbook.Worksheets.Add("sheet1");
                    // Add headers to the worksheet
                    for (int a = 0; a < DisplayColumnsNames.Count; a++)
                    {
                        if (DisplayColumnsNames[a].DisplayName != null)
                        {
                            ws.Cells[1, a + 1].Value = DisplayColumnsNames[a].DisplayName;
                        }
                    }
                    ws.InsertRow(2, 1);
                    // Add data rows to the worksheet
                    for (int i = 0; i < dataList.Count; i++)
                    {
                        var row = dataList[i];
                        for (int j = 0; j < columnsNames.Count; j++)
                        {
                            // this columns added Only For Change Any Styles For Row
                            if (row != null && columnsNames[j].GetValue(row) != null && columnsNames[j].Name == "IsValidColumn")
                            {
                                if (!(bool)(columnsNames[j]?.GetValue(row)))// if the given line is false 
                                {
                                    ws.Row(i + 2).Style.Font.Color.SetColor(Color.Red);
                                }
                                else
                                {
                                    ws.Row(i + 2).Style.Font.Color.SetColor(Color.Green);
                                }
                            }
                            else if (row != null && columnsNames != null && columnsNames[j].GetValue(row) != null && columnsNames[j].GetValue(row).ToString() != string.Empty)
                            {
                                // Check if columns have display or not  //
                                if (DisplayColumnsNames[j].DisplayName != null)
                                {
                                    ws.Cells[i + 2, j + 1].Value = columnsNames[j].GetValue(dataList[i]).ToString();
                                }
                            }
                            else
                            {
                                ws.Cells[i + 2, j + 1].Value = "";
                            }
                        }
                    }
                    ws.DeleteRow(dataList.Count + 3, 5000);
                    using (var stream = new MemoryStream())
                    {
                        p.SaveAs(stream);
                        // To have the byte array read from the begining
                        stream.Seek(0, SeekOrigin.Begin);
                        fileData = stream.ToArray();
                    }
                    return fileData;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public static IEnumerable<string> ExtractPropertiesName<T>(T entityVM) where T : class, new()
        {
            return entityVM
                .GetType()
                .GetProperties()
                .ToList()
                .Select(propInfo => propInfo.Name);
        }
        public static bool ColumnNameExistis(string targetColName, IEnumerable<string> propsName)
        {
            return targetColName == null ? false : propsName.Contains(targetColName.Replace(" ", "").Trim());
        }
        public static T EntityCreator<T>() where T : class, new()
        {
            try
            {
                Func<T> Creator = Expression
                    .Lambda<Func<T>>
                    (
                    Expression
                    .New(typeof(T)
                    .GetConstructor(Type.EmptyTypes)
                )).Compile();
                return Creator();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }
        public static bool Valid<T>(T entity) where T : class, new()
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(entity);
            return Validator.TryValidateObject(entity, context, results);
        }
        public static bool AssignValue<T>(ref T entityVM, string propName, object value) where T : class, new()
        {
            try
            {
                Type type = entityVM.GetType();
                PropertyInfo propInfo = type.GetProperty(propName);
                Type propInfoType = propInfo.PropertyType;
                propInfo.SetValue(entityVM, Convert.ChangeType(value, propInfoType));
                return true;
            }
            catch (Exception ex)
            {
                // throw new AmenException(ex.Message);
                return true;
            }
        }
    }
}
