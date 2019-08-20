using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VSAND.Data.ViewModels;
using VSAND.Data.ViewModels.Teams;
using VSAND.Data.ViewModels.Users;

namespace VSAND.Services.Files
{
    public class ExcelService : IExcelService
    {
        NLog.ILogger log = LogManager.GetCurrentClassLogger();

        public List<ListItem<string>> ExcelSheetNames(string filePath)
        {
            var sheetList = new List<ListItem<string>>();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
            {
                //create the object for workbook part
                WorkbookPart workbookPart = doc.WorkbookPart;
                Sheets worksheets = workbookPart.Workbook.Sheets;
                foreach (var worksheet in worksheets)
                {
                    sheetList.Add(new ListItem<string>(worksheet.LocalName, worksheet.LocalName));
                }
            }
            return sheetList;
        }

        public DataTable ExcelSheetToDataTable(string filePath, string sheetName)
        {
            try
            {
                DataTable dtTable = new DataTable();
                //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
                using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePath, false))
                {
                    //create the object for workbook part
                    WorkbookPart workbookPart = doc.WorkbookPart;
                    Sheets workSheets = workbookPart.Workbook.GetFirstChild<Sheets>();
                    foreach (Sheet checkSheet in workSheets.OfType<Sheet>())
                    {
                        if (checkSheet.LocalName.Equals(sheetName))
                        {
                            //statement to get the worksheet object by using the sheet id
                            Worksheet theWorksheet = ((WorksheetPart)workbookPart.GetPartById(checkSheet.Id)).Worksheet;
                            SheetData workSheet = theWorksheet.GetFirstChild<SheetData>();

                            for (int rCnt = 0; rCnt < workSheet.ChildElements.Count(); rCnt++)
                            {
                                List<string> rowList = new List<string>();
                                for (int rCnt1 = 0; rCnt1 < workSheet.ElementAt(rCnt).ChildElements.Count(); rCnt1++)
                                {

                                    Cell thecurrentcell = (Cell)workSheet.ElementAt(rCnt).ChildElements.ElementAt(rCnt1);
                                    //statement to take the integer value
                                    string currentcellvalue = string.Empty;
                                    if (thecurrentcell.DataType != null)
                                    {
                                        if (thecurrentcell.DataType == CellValues.SharedString)
                                        {
                                            int id;
                                            if (Int32.TryParse(thecurrentcell.InnerText, out id))
                                            {
                                                SharedStringItem item = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
                                                if (item.Text != null)
                                                {
                                                    //first row will provide the column name.
                                                    if (rCnt == 0)
                                                    {
                                                        dtTable.Columns.Add(item.Text.Text);
                                                    }
                                                    else
                                                    {
                                                        rowList.Add(item.Text.Text);
                                                    }
                                                }
                                                else if (item.InnerText != null)
                                                {
                                                    currentcellvalue = item.InnerText;
                                                }
                                                else if (item.InnerXml != null)
                                                {
                                                    currentcellvalue = item.InnerXml;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (rCnt != 0)
                                        {
                                            // reserved for column values
                                            rowList.Add(thecurrentcell.InnerText);
                                        }
                                    }
                                }
                                if (rCnt != 0)
                                {
                                    // reserved for column values
                                    dtTable.Rows.Add(rowList.ToArray());
                                }

                            }
                        }
                    }
                    return dtTable;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex, ex.Message);
            }
            return null;
        }

        public object ExcelSheetToObj(string filePath, string sheetName)
        {
            DataTable dtTable = ExcelSheetToDataTable(filePath, sheetName);
            //Lets open the existing excel file and read through its content . Open the excel using openxml sdk
            string serializedResult = JsonConvert.SerializeObject(dtTable);
            dynamic result = JsonConvert.DeserializeObject(serializedResult);
            return result;
        }

        private WorksheetPart GetWorksheetFromSheetName(WorkbookPart workbookPart, string sheetName)
        {
            Sheet sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault(s => s.LocalName == sheetName);
            if (sheet == null)
            {
                throw new Exception(string.Format("Could not find sheet with name {0}", sheetName));
            }
            else
            {
                return workbookPart.GetPartById(sheet.Id) as WorksheetPart;
            }
        }

        private Cell ConstructCell(string value, CellValues dataType)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        public byte[] TeamRosterFile(List<TeamRoster> exportRoster, string fileName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    //Add a WorkbookPart to the document.
                    WorkbookPart workbookpart = document.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    //Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetpart.Worksheet = new Worksheet();

                    //Add Sheets to the Workbook.
                    Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());
                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet() { Id = workbookpart.GetIdOfPart(worksheetpart), SheetId = 1, Name = fileName };
                    sheets.Append(sheet);

                    workbookpart.Workbook.Save();

                    SheetData sheetData = worksheetpart.Worksheet.AppendChild(new SheetData());

                    Row row = new Row();
                    row.Append(
                        ConstructCell("Name", CellValues.String),
                        ConstructCell("Class", CellValues.String),
                        ConstructCell("Jersey Number", CellValues.String),
                        ConstructCell("Position1", CellValues.String),
                        ConstructCell("Position2", CellValues.String));

                    sheetData.AppendChild(row);

                    foreach (var ent in exportRoster)
                    {
                        row = new Row();
                        row.Append(
                            ConstructCell(ent.PlayerName, CellValues.String),
                            ConstructCell(ent.Class, CellValues.String),
                            ConstructCell(ent.JerseyNumber, CellValues.String),
                            ConstructCell(ent.Position.ToString(), CellValues.String),
                            ConstructCell(ent.Position2.ToString(), CellValues.String));
                        sheetData.AppendChild(row);
                    }

                    worksheetpart.Worksheet.Save();
                }
                return ms.ToArray();
            }
        }

        public byte[] SchoolMasterAccountFile(List<SchoolMasterAccount> exportMasterAccounts, string fileName)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    //Add a WorkbookPart to the document.
                    WorkbookPart workbookpart = document.AddWorkbookPart();
                    workbookpart.Workbook = new Workbook();

                    //Add a WorksheetPart to the WorkbookPart.
                    WorksheetPart worksheetpart = workbookpart.AddNewPart<WorksheetPart>();
                    worksheetpart.Worksheet = new Worksheet();

                    //Add Sheets to the Workbook.
                    Sheets sheets = workbookpart.Workbook.AppendChild(new Sheets());
                    // Append a new worksheet and associate it with the workbook.
                    Sheet sheet = new Sheet() { Id = workbookpart.GetIdOfPart(worksheetpart), SheetId = 1, Name = fileName };
                    sheets.Append(sheet);

                    workbookpart.Workbook.Save();

                    SheetData sheetData = worksheetpart.Worksheet.AppendChild(new SheetData());

                    Row row = new Row();
                    row.Append(
                        ConstructCell("School Id", CellValues.String),
                        ConstructCell("School Name", CellValues.String),
                        ConstructCell("Username", CellValues.String),
                        ConstructCell("Password", CellValues.String));

                    sheetData.AppendChild(row);

                    foreach (var ent in exportMasterAccounts)
                    {
                        row = new Row();
                        row.Append(
                            ConstructCell(ent.SchoolId.ToString(), CellValues.Number),
                            ConstructCell(ent.SchoolName, CellValues.String),
                            ConstructCell(ent.Username, CellValues.String));
                        sheetData.AppendChild(row);
                    }

                    worksheetpart.Worksheet.Save();
                }
                return ms.ToArray();
            }
        }

        //Read Excel data to generic list - overloaded version 1.
        public List<T> GetDataToList<T>(string filePath, Func<IList<string>, IList<string>, T> addRowData)
        {
            return GetDataToList<T>(filePath, "", addRowData);
        }

        //Read Excel data to generic list - overloaded version 2.
        public List<T> GetDataToList<T>(string filePath, string sheetName, Func<IList<string>, IList<string>, T> addRowData)
        {
            List<T> resultList = new List<T>();

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(filePath, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                Sheet sheet = null;
                WorksheetPart wsPart = null;

                // Find the worksheet.                
                if (sheetName == "")
                {
                    sheet = wbPart.Workbook.Descendants<Sheet>().FirstOrDefault();
                }
                else
                {
                    sheet = wbPart.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();
                }
                if (sheet != null)
                {
                    // Retrieve a reference to the worksheet part.
                    wsPart = (WorksheetPart)(wbPart.GetPartById(sheet.Id));
                }
                if (wsPart == null)
                {
                    document.Dispose();
                    throw new Exception("No worksheet.");
                }

                //List to hold custom column names for mapping data to columns (index-free).
                var columnNames = new List<string>();

                //List to hold column address letters for handling empty cell issue (handle empty cell issue).
                var columnLetters = new List<string>();

                //Iterate cells of custom header row.
                foreach (Cell cell in wsPart.Worksheet.Descendants<Row>().ElementAt(0))
                {
                    //Get custom column names.
                    //Remove spaces, symbols (except underscore), and make lower cases and for all values in columnNames list.                    
                    columnNames.Add(Regex.Replace(GetCellValue(document, cell), @"[^A-Za-z0-9_]", "").ToLower());

                    //Get built-in column names by extracting letters from cell references.
                    columnLetters.Add(GetColumnAddress(cell.CellReference));
                }

                //Used for sheet row data to be added through delegation.                
                var rowData = new List<string>();

                //Do data in rows
                string cellLetter = string.Empty;

                foreach (var row in GetUsedRows(document, wsPart))
                {
                    rowData.Clear();

                    //Iterate through prepared enumerable.
                    foreach (var cell in GetCellsForRow(row, columnLetters))
                    {
                        rowData.Add(GetCellValue(document, cell));
                    }

                    //Calls the delegated function to add it to the collection.
                    var oRetRow = addRowData(rowData, columnNames);
                    if (oRetRow != null)
                    {
                        resultList.Add(oRetRow);
                    }
                    
                }
            }
            return resultList;
        }

        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            if (cell == null) return null;
            string value = cell.InnerText;

            //Process values particularly for those data types.
            if (cell.DataType != null)
            {
                switch (cell.DataType.Value)
                {
                    //Obtain values from shared string table.
                    case CellValues.SharedString:
                        var sstPart = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
                        value = sstPart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
                        break;

                    //Optional boolean conversion.
                    case CellValues.Boolean:
                        value = value == "0" ? "FALSE" : "TRUE";
                        break;
                }
            }
            return value;
        }

        private IEnumerable<Row> GetUsedRows(SpreadsheetDocument document, WorksheetPart wsPart)
        {
            bool hasValue;
            //Iterate all rows except the first one.
            foreach (var row in wsPart.Worksheet.Descendants<Row>().Skip(1))
            {
                hasValue = false;
                foreach (var cell in row.Descendants<Cell>())
                {
                    //Find at least one cell with value for a row
                    if (!string.IsNullOrEmpty(GetCellValue(document, cell)))
                    {
                        hasValue = true;
                        break;
                    }
                }
                if (hasValue)
                {
                    //Return the row and keep iteration state.
                    yield return row;
                }
            }
        }

        private IEnumerable<Cell> GetCellsForRow(Row row, List<string> columnLetters)
        {
            int workIdx = 0;
            foreach (var cell in row.Descendants<Cell>())
            {
                //Get letter part of cell address.
                var cellLetter = GetColumnAddress(cell.CellReference);

                //Get column index of the matched cell.  
                int currentActualIdx = columnLetters.IndexOf(cellLetter);

                //Add empty cell if work index smaller than actual index.
                for (; workIdx < currentActualIdx; workIdx++)
                {
                    var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptyCell;
                }

                //Return cell with data from Excel row.
                yield return cell;
                workIdx++;

                //Check if it's ending cell but there still is any unmatched columnLetters item.   
                if (cell == row.LastChild)
                {
                    //Append empty cells to enumerable. 
                    for (; workIdx < columnLetters.Count(); workIdx++)
                    {
                        var emptyCell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                        yield return emptyCell;
                    }
                }
            }
        }

        private string GetColumnAddress(string cellReference)
        {
            //Create a regular expression to get column address letters.
            Regex regex = new Regex("[A-Za-z]+");
            Match match = regex.Match(cellReference);
            return match.Value;
        }
    }
}
