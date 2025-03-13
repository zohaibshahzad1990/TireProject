using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using static TireProject.NewExcelPage;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = Xamarin.Forms.Color;
using Xamarin.Essentials;

namespace TireProject
{
    public partial class ReportPage : ContentPage
    {
        string FilePath = null;
        string fieldname = "Name";
        GetOnlineData post = new GetOnlineData();
        List<ReportData> ll = new List<ReportData>();
        int fieldtype=0;
        int tireseason = 0;
        string location = "ALL";
        string ssqz = "http://3.133.136.76/api/mains/customexport/02-28-2019/03-09-2019/0/NA/0/ALL/";
        public ReportPage()
        {
            InitializeComponent();
        }

        public async Task RunFirst()
        {
            busymsg.IsVisible = true;
            busyind.IsVisible = true;
            busyind.IsRunning = true;

            var httpClient = new HttpClient();
            try
            {
                var request = "http://3.133.136.76/api/mains/customexport/";
                // +  + "/" +  + "/" +  + "/" +  + "/" +  + "/"



                var json = JsonConvert.SerializeObject(new TempDataPass(datefrom.Date.ToString("yyyy-MM-dd"),
                    dateto.Date.ToString("yyyy-MM-dd"), fieldtype, tireseason, location
                    ));

                HttpContent httpContent = new StringContent(json);

                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var result = await httpClient.PostAsync(request, httpContent);
                if (result.IsSuccessStatusCode)
                {

                    string jsonresult = await result.Content.ReadAsStringAsync();

                    var ll = JsonConvert.DeserializeObject<List<ReportData>>(jsonresult);
                    string date = DateTime.Now.ToShortDateString();
                    date = date.Replace("/", "_");

                    var path = System.IO.Path.Combine(DependencyService.Get<IFileSave>().GetPath(), "xfdevelopers" + date + ".xlsx");
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    FilePath = path;
                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        workbookPart.Workbook = new Workbook();

                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        worksheetPart.Worksheet = new Worksheet();

                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                        Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Tire Inc" };
                        sheets.Append(sheet);
                        workbookPart.Workbook.Save();
                        SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                        Row row = new Row();
                        // Constructing header

                        row.Append(
                            ConstructCell("Report Name", CellValues.String),
                            ConstructCell(fieldname, CellValues.String),
                            ConstructCell("", CellValues.String),
                            ConstructCell("", CellValues.String),
                            ConstructCell("Date From", CellValues.String),
                            ConstructCell(datefrom.Date.ToString("MM-dd-yyyy"), CellValues.String),
                            ConstructCell("Date to", CellValues.String),
                            ConstructCell(dateto.Date.ToString("MM-dd-yyyy"), CellValues.String)
                            );
                        // Insert the header row to the Sheet Data
                        sheetData.AppendChild(row);
                        // Constructing header
                        row = new Row();
                        row.Append(
    ConstructCell("", CellValues.String)
                            );
                        // Insert the header row to the Sheet Data
                        sheetData.AppendChild(row);
                        row = new Row();
                        row.Append(
                            ConstructCell("First Name", CellValues.String),
                            ConstructCell("Last Name", CellValues.String),
                            ConstructCell("Phone No", CellValues.String),
                            ConstructCell("Plate Number", CellValues.String),
                            ConstructCell("Car Make", CellValues.String),
                            ConstructCell("Tire Stored Upto", CellValues.String),
                            ConstructCell("Storage Location", CellValues.String),
                            ConstructCell("Tire Season", CellValues.String)
                            );
                        // Insert the header row to the Sheet Data
                        sheetData.AppendChild(row);

                        // Add each product
                        foreach (var item in ll)
                        {

                            if (item.RefNo == null) item.RefNo = "null";
                            if (item.Date == null) item.Date = "null";
                            if (item.ExtraRefNo == null) item.ExtraRefNo = "null";
                            if (item.ExtraDate == null) item.ExtraDate = "null";

                            //Customer Detail
                            if (item.FName == null) item.FName = "null";
                            if (item.LName == null) item.LName = "null";
                            if (item.MName == null) item.MName = "null";
                            if (item.Address == null) item.Address = "null";
                            if (item.PhoneNo == null) item.PhoneNo = "null";
                            if (item.HomeNo == null) item.HomeNo = "null";
                            if (item.WorkNo == null) item.WorkNo = "null";
                            if (item.Email == null) item.Email = "null";

                            //Tire Storage Detail
                            if (item.DepthLF == null) item.DepthLF = "null";
                            if (item.TireSize1 == null) item.TireSize1 = "null";
                            if (item.TireSize2 == null) item.TireSize2 = "null";
                            if (item.TireSize3 == null) item.TireSize3 = "null";
                            if (item.TireStoredUpto == null) item.TireStoredUpto = "null";
                            if (item.REP == null) item.REP = "null";




                            //Vehicle Detail
                            if (item.PlateNo == null) item.PlateNo = "null";
                            if (item.CarYear == null) item.CarYear = "null";
                            if (item.CarBrand == null) item.CarBrand = "null";
                            if (item.CarModel == null) item.CarModel = "null";
                            if (item.Pic1 == null) item.Pic1 = "null";
                            if (item.Pic2 == null) item.Pic2 = "null";
                            if (item.Pic3 == null) item.Pic3 = "null";
                            if (item.Pic4 == null) item.Pic4 = "null";


                            row = new Row();
                            row.Append(
                                ConstructCell(item.FName, CellValues.String),
                                ConstructCell(item.LName, CellValues.String),
                                ConstructCell(item.PhoneNo, CellValues.String),
                                ConstructCell(item.PlateNo, CellValues.String),
                                ConstructCell(item.CarBrand, CellValues.String),
                                ConstructCell(item.TireStoredUpto, CellValues.String),
                                ConstructCell(item.ExtraRefNo, CellValues.String),
                                ConstructCell(item.TireSeason.ToString(), CellValues.String)
                                );
                            sheetData.AppendChild(row);
                        }

                        worksheetPart.Worksheet.Save();

                        if (System.IO.File.Exists(path))
                        {

                            busymsg.Text = "Successfully Exported!";
                            btnDone.IsVisible = true;
                        }
                        else
                        {
                            busymsg.Text = "Something Wrong With Exporting Data!";
                        }
                    }

                }
                else {
                    busymsg.Text = "Something Wrong With Exporting Data!";
                }

                busyind.IsVisible = false;
                busyind.IsRunning = false;
            }
            catch(Exception ex)
            {

            }

        }
        /* To create cell in Excel */
        private DocumentFormat.OpenXml.Spreadsheet.Cell ConstructCell(string value, CellValues dataType)
        {
            return new DocumentFormat.OpenXml.Spreadsheet.Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var getcomad = (Button)sender;
            getcomad.IsEnabled = false;
            if (System.IO.File.Exists(FilePath))
                await Launcher.OpenAsync
                            (new OpenFileRequest("Excel File", new ReadOnlyFile(FilePath))
                        );
            //await DisplayAlert("File Saved",FilePath,"Okay");


            //DependencyService.Get<IFileSave>().OpenExcel(FilePath);
            getcomad.IsEnabled = true;
        }

        async void EveExport(object sender, System.EventArgs e)
        {
            var getcomad = (Button)sender;
            getcomad.IsEnabled = false;
            stkdatainput.IsVisible = false;
            stkwork.IsVisible = true;
            await RunFirst();
            getcomad.IsEnabled = true;
        }

        
        void EveBtnRimType(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            b1.BackgroundColor = Color.White;
            b1.TextColor = Color.FromHex("#e74c3c");
            b2.BackgroundColor = Color.White;
            b2.TextColor = Color.FromHex("#e74c3c");
            b3.BackgroundColor = Color.White;
            b3.TextColor = Color.FromHex("#e74c3c");
            b4.BackgroundColor = Color.White;
            b4.TextColor = Color.FromHex("#e74c3c");

            getcommand.BackgroundColor = Color.FromHex("#e74c3c");
            getcommand.TextColor = Color.White;

            if (getcommand.Text.Contains("AllSeason"))
                tireseason = 0;
            else if (getcommand.Text.Contains("Winter"))
                tireseason = 1;
            else if (getcommand.Text.Contains("Summer"))
                tireseason = 2;
            else
                tireseason = 3;

            getcommand.IsEnabled = true;
        }

        void EveBtnlocation(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            a1.BackgroundColor = Color.White;
            a1.TextColor = Color.FromHex("#e74c3c");
            a2.BackgroundColor = Color.White;
            a2.TextColor = Color.FromHex("#e74c3c");
            a3.BackgroundColor = Color.White;
            a3.TextColor = Color.FromHex("#e74c3c");
            a4.BackgroundColor = Color.White;
            a4.TextColor = Color.FromHex("#e74c3c");
			a5.BackgroundColor = Color.White;
			a5.TextColor = Color.FromHex("#e74c3c");
			getcommand.BackgroundColor = Color.FromHex("#e74c3c");
            getcommand.TextColor = Color.White;
            location = getcommand.Text;
            getcommand.IsEnabled = true;
        }

        void EveBtnChecked1(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            c1.IsChecked = true;
            c2.IsChecked = false;
            c3.IsChecked = false;
            fieldname = "Name";
            fieldtype = 0;
            getcommand.IsEnabled = true;
        }
        void EveBtnChecked2(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            c2.IsChecked = true;
            c1.IsChecked = false;
            c3.IsChecked = false;
            fieldname = "Plate Number";
            fieldtype = 1;
            getcommand.IsEnabled = true;
        }
        void EveBtnChecked3(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            c3.IsChecked = true;
            c2.IsChecked = false;
            c1.IsChecked = false;
            fieldname = "Make/Model";
            fieldtype = 2;
            getcommand.IsEnabled = true;
        }
    }
}
