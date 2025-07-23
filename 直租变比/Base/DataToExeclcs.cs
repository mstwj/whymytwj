using System;

namespace 直租变比.Base
{
    public class DataToExeclcs
    {

        public void ExportDataTableToExcel(System.Data.DataTable dataTable, string excelFilePath)
        {
            /*
            //ExportDataTableToExcel(dataGrid.ToDataTable(), exePath);
            //string excelFilePath = Assembly.GetExecutingAssembly().Location;
            //System.Data.DataTable dataTable = dataGrid.ToDataTable();

            Application excelApp = new Application();
            Workbook workbook = excelApp.Workbooks.Add(Type.Missing);
            Worksheet worksheet = null;

            if (workbook != null && dataTable != null)
            {
                worksheet = (Worksheet?)workbook.Sheets["Sheet1"];
                if (worksheet == null)
                {
                    worksheet = (Worksheet?)workbook.Sheets.Add();
                    worksheet.Name = "Sheet1";
                }

                for (int i = 1; i <= dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i] = dataTable.Columns[i - 1].ColumnName;
                }

                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dataTable.Rows[i][j].ToString();
                    }
                }
            }

            if (excelFilePath != null && excelFilePath != string.Empty)
            {
                try
                {
                    //worksheet.SaveAs(excelFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    worksheet.SaveAs(excelFilePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, XlSaveAsAccessMode.xlExclusive, Type.Missing,
                        Type.Missing, Type.Missing);

                }
                catch
                {
                    // Handle exception
                }
            }

            workbook.Close(false, Type.Missing, Type.Missing);
            excelApp.Quit();

            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            worksheet = null;
            workbook = null;
            excelApp = null;

            GC.Collect();
            */
        }

    }
}
