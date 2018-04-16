using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Drawing;       //microsoft Excel 12.0 object in references-> COM tab

namespace ComputerizedLibrarySystem
{
    public class ExcelConsole
    {

        //Create COM Objects. Create a COM object for everything that is referenced
        private Excel.Application xlApp;
        private Excel.Workbook xlWorkbook;
        private Excel._Worksheet xlWorksheet;
        private Excel.Range xlRange;

        public ExcelConsole(string fileName) {
            this.xlApp = new Excel.Application();
            //Excel.workbook xlworkbook = xlapp.workbooks.open(@"c:\b programmer\c#\excelappincsharp\the excel wrk bk.xlsx");
            this.xlWorkbook = xlApp.Workbooks.Open(@"" + fileName);
            this.xlWorksheet = xlWorkbook.Sheets[1];
            this.xlRange = xlWorksheet.UsedRange;
        }

        public IList<Book> getBookExcelFile()
        {

            IList<Book> excelBooks = new List<Book>();
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 1; i <= rowCount; i++)
            {
                //foreach line create a new book
                Book newBook = new Book();
                for (int j = 1; j <= colCount; j++)
                {
                    
                    //new line
                    if (j == 1)//col 1 for Accession No
                    {
                        Console.Write("\r\n");
                        
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                                if (xlRange.Cells[i, j].Value2.ToString() == "ACCESSION NO" || xlRange.Cells[i, j].Value2.ToString() == "LIST OF BOOKS IN THE LIBRARY")
                            continue;
                        //generate accession no
                    }
                    if (j == 2)//col 2 for Author Name
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null && xlRange.Cells[i, j].Value2.ToString() != "AUTHOR")
                            newBook.AuthorName = xlRange.Cells[i, j].Value2.ToString();
                        else
                        {
                            newBook.AuthorName = "N/A";
                            continue; //there must be author name
                        }
                    }
                    if (j == 3)//col 3 for Book Title
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null && xlRange.Cells[i, j].Value2.ToString() != "TITLE")
                            newBook.BookTitle = xlRange.Cells[i, j].Value2.ToString();
                        else
                        {
                            newBook.BookTitle = "N/A";
                            continue; //there must be Book Title
                        }
                    }
                    if (j == 4)//col 4 for No of Copies
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            newBook.NoOfCopies = xlRange.Cells[i, j].Value2.ToString();
                        else
                            newBook.NoOfCopies = "N/A";
                    }
                    if (j == 5)//col 5 for Place of Publication
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            newBook.PlaceOfPublication = xlRange.Cells[i, j].Value2.ToString();
                        else
                            newBook.PlaceOfPublication = "N/A";
                    }
                    if (j == 6)//col 6 for Publisher
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            newBook.Publisher = xlRange.Cells[i, j].Value2.ToString();
                        else
                            newBook.Publisher = "N/A";
                    }
                    if (j == 7)//col 7 for Year of Publication
                    {
                        if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            newBook.DateOfPublished = xlRange.Cells[i, j].Value2.ToString();
                        else
                            newBook.DateOfPublished = "N/A";
                    }
                    

                    //write the value to the console
                    if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                        Console.Write(xlRange.Cells[i, j].Value2.ToString() + "\t");
                }
                //End of line and add the book to list for upload
                excelBooks.Add(newBook);
            }

            return excelBooks;
            
        }

        public void cleanUpMemory()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }

        

        public static void CreateExcelFile(DataTable tableOfItem, string workSheet, string reportTitle, string reportName)
        {
            Excel.Application excel;
            Excel.Workbook worKbooK;
            Excel.Worksheet worKsheeT;
            Excel.Range celLrangE;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = true;
                excel.DisplayAlerts = true;
                worKbooK = excel.Workbooks.Add(Type.Missing);


                worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = workSheet;
                //worKsheeT.Name = "StudentReportCard";

                worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, 8]].Merge();
                worKsheeT.Cells[1, 1] = reportTitle;
                //worKsheeT.Cells[1, 1] = "Student Report Card";
                worKsheeT.Cells.Font.Size = 15;


                int rowcount = 2;

                foreach (DataRow datarow in tableOfItem.Rows)
                {
                    rowcount += 1;
                    for (int i = 1; i <= tableOfItem.Columns.Count; i++)
                    {

                        if (rowcount == 3)
                        {
                            worKsheeT.Cells[2, i] = tableOfItem.Columns[i - 1].ColumnName;
                            worKsheeT.Cells.Font.Color = Color.Black;

                        }

                        worKsheeT.Cells[rowcount, i] = datarow[i - 1].ToString();

                        if (rowcount > 3)
                        {
                            if (i == tableOfItem.Columns.Count)
                            {
                                if (rowcount % 2 == 0)
                                {
                                    celLrangE = worKsheeT.Range[worKsheeT.Cells[rowcount, 1], worKsheeT.Cells[rowcount, tableOfItem.Columns.Count]];
                                }

                            }
                        }

                    }

                }

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[rowcount, tableOfItem.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Excel.Borders border = celLrangE.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[2, tableOfItem.Columns.Count]];

                worKbooK.SaveAs(reportName);
                //worKbooK.SaveAs("StudentReportCard1");
                //doCleanUp(excel, worKbooK, worKsheeT, celLrangE);

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Console.Write(ex.Message);

            }
            finally
            {
                worKsheeT = null;
                celLrangE = null;
                worKbooK = null;
            }
        }
    }
}
