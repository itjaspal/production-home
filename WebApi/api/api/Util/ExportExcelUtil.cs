using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace api.Util
{
    public class ExportExcel<T> where T : class, new()
    {
        public static void export(IEnumerable<T> resultData, Stream stream)
        {
            DataTableMapper<T> mapper = new DataTableMapper<T>();
            DataTable retTable = mapper.Map(resultData.ToList());
            export(retTable, stream);
        }

        public static void export(DataTable retTable, Stream stream)
        {
            Workbook workbook = new Workbook();
            //Initailize worksheet
            Worksheet sheet = workbook.Worksheets[0];

            sheet.InsertDataTable(retTable, true, 2, 1);

            //set column format to Text
            foreach (var cols in sheet.Columns)
            {
                foreach (var cells in cols.CellList)
                {
                    cells.Text = cells.DisplayedText;
                }
            }

            //Sets body style
            CellStyle oddStyle = workbook.Styles.Add("oddStyle");
            oddStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
            oddStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
            oddStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
            oddStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
            oddStyle.KnownColor = ExcelColors.LightTurquoise;

            CellStyle evenStyle = workbook.Styles.Add("evenStyle");
            evenStyle.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
            evenStyle.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
            evenStyle.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
            evenStyle.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
            evenStyle.KnownColor = ExcelColors.None;

            foreach (CellRange range in sheet.AllocatedRange.Rows)
            {
                if (range.Row % 2 == 0)
                    range.CellStyleName = evenStyle.Name;
                else
                    range.CellStyleName = oddStyle.Name;
            }

            //Sets header style
            CellStyle styleHeader = sheet.Rows[0].Style;
            styleHeader.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
            styleHeader.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
            styleHeader.Borders[BordersLineType.EdgeTop].LineStyle = LineStyleType.Thin;
            styleHeader.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
            styleHeader.VerticalAlignment = VerticalAlignType.Center;
            styleHeader.KnownColor = ExcelColors.LightBlue;
            styleHeader.Font.KnownColor = ExcelColors.White;
            styleHeader.Font.IsBold = true;

            //sheet.Columns[sheet.AllocatedRange.LastColumn - 1].Style.NumberFormat = "\"$\"#,##0";
            //sheet.Columns[sheet.AllocatedRange.LastColumn - 2].Style.NumberFormat = "\"$\"#,##0";

            sheet.AllocatedRange.AutoFitColumns();
            sheet.AllocatedRange.AutoFitRows();

            sheet.Rows[0].RowHeight = 20;

            //workbook.SaveToFile(@"" + filename, ExcelVersion.Version2010);
            workbook.SaveToStream(stream, FileFormat.Version2010);

            //System.Diagnostics.Process.Start(workbook.FileName);
        }

        public static List<T> import(string filename)
        {
            //Create a new workbook
            Workbook workbook = new Workbook();
            //Load a file and imports its data
            workbook.LoadFromFile(filename);
            return import(workbook);
        }

        public static List<T> import(Stream stream)
        {
            //Create a new workbook
            Workbook workbook = new Workbook();
            //Load a file and imports its data
            workbook.LoadFromStream(stream);
            return import(workbook);
        }

        private static List<T> import(Workbook workbook)
        {
            //Initialize worksheet
            Worksheet sheet = workbook.Worksheets[0];
            // get the data source that the grid is displaying data for
            var resultData = sheet.ExportDataTable();

            DataTableMapper<T> mapper = new DataTableMapper<T>();
            List<T> resultObj = mapper.Map(resultData);

            return resultObj;
        }
    }
}