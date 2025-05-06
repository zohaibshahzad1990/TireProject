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
        int fieldtype = 0;
        int tireseason = 0;
        string location = "ALL";
        string ssqz = $"{Constants.BASE_URL}/api/mains/customexport/02-28-2019/03-09-2019/0/NA/0/ALL/";

        // Dictionary to store all location buttons for easy access
        private Dictionary<string, Button> locationButtons = new Dictionary<string, Button>();
        private List<string> warehouseCodes = new List<string>();

        public ReportPage()
        {
            InitializeComponent();

            // Setup location buttons after initialization
            SetupLocationButtons();
        }

        private void SetupLocationButtons()
        {
            // Get warehouse codes from application properties
            if (Application.Current.Properties.ContainsKey("WLocations"))
            {
                string warehouseCodesStr = Application.Current.Properties["WLocations"].ToString();
                warehouseCodes = warehouseCodesStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
            }
            else
            {
                // Fallback to default codes if property doesn't exist
                warehouseCodes = new List<string> { "HR", "FV", "DC", "OR", "UT" };
            }

            // Setup the grid for buttons
            locationButtonsGrid.Children.Clear();
            locationButtonsGrid.RowDefinitions.Clear();
            locationButtonsGrid.ColumnDefinitions.Clear();

            // Add column definitions for a 2-column grid
            locationButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            locationButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            // Calculate how many rows we need
            int rowsNeeded = (int)Math.Ceiling((warehouseCodes.Count + 1) / 2.0); // +1 for ALL button

            // Add row definitions
            for (int i = 0; i < rowsNeeded; i++)
            {
                locationButtonsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Add ALL button first
            var allButton = new Button
            {
                Text = "ALL",
                BackgroundColor = Color.FromHex("#e74c3c"),
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            allButton.Clicked += EveBtnlocation;
            locationButtonsGrid.Children.Add(allButton, 0, 0);
            Grid.SetColumnSpan(allButton, 2);
            locationButtons.Add("ALL", allButton);

            // Add location buttons
            int row = 1;
            int column = 0;

            foreach (var code in warehouseCodes)
            {
                var button = new Button
                {
                    Text = code,
                    BackgroundColor = Color.White,
                    TextColor = Color.FromHex("#e74c3c"),
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };

                button.Clicked += EveBtnlocation;
                locationButtonsGrid.Children.Add(button, column, row);
                locationButtons.Add(code, button);

                // Move to next column or row
                column++;
                if (column > 1)
                {
                    column = 0;
                    row++;
                }
            }
        }

        public async Task RunFirst()
        {
            busymsg.IsVisible = true;
            busyind.IsVisible = true;
            busyind.IsRunning = true;

            var httpClient = new HttpClient();
            try
            {
                var request = $"{Constants.BASE_URL}/api/mains/customexport/";
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
                else
                {
                    busymsg.Text = "Something Wrong With Exporting Data!";
                }

                busyind.IsVisible = false;
                busyind.IsRunning = false;
            }
            catch (Exception ex)
            {
                busymsg.Text = "Error: " + ex.Message;
                busyind.IsVisible = false;
                busyind.IsRunning = false;
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

            // Reset all location buttons to default state
            foreach (var btn in locationButtons.Values)
            {
                btn.BackgroundColor = Color.White;
                btn.TextColor = Color.FromHex("#e74c3c");
            }

            // Highlight the selected button
            getcommand.BackgroundColor = Color.FromHex("#e74c3c");
            getcommand.TextColor = Color.White;

            // Set the selected location
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