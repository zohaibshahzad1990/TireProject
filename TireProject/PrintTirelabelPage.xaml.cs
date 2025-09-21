using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Button = Xamarin.Forms.Button;

namespace TireProject
{
    public partial class PrintTirelabelPage : ContentPage
    {
        ReportData data = new ReportData();
        string printfilepath;

        public PrintTirelabelPage()
        {
            InitializeComponent();
            stkmain.IsVisible = false;
            printbtn.IsEnabled = true;
        }

        public PrintTirelabelPage(ReportData reportData)
        {
            InitializeComponent();
            data = reportData;
            PageRun(reportData);
        }

        async Task PageRun(ReportData reportData)
        {
            try
            {
                lblbusy.Text = "Loading Info From Server...";
                await Task.Delay(2000);
                busy.IsVisible = true;
                busy.IsRunning = true;
                lblbusy.IsVisible = true;
                stkmain.IsVisible = true;

                // Set default company code if not provided
                if (reportData.CompanyCode == null || string.IsNullOrEmpty(reportData.CompanyCode))
                    reportData.CompanyCode = "TTI";

                var systemHelper = DependencyService.Get<ISystemHelper>();
                var document = new PdfDocument();
                document.Info.Title = "Tire Labels";

                var page = document.AddPage();
                page.Size = PdfSharp.PageSize.A4;
                var gfx = XGraphics.FromPdfPage(page);

                // Calculate dimensions for 4 labels on A4 page
                double pageWidth = page.Width;
                double pageHeight = page.Height;
                double labelWidth = pageWidth / 2;
                double labelHeight = pageHeight / 2;

                // Draw 4 labels in a 2x2 grid
                DrawLabel(gfx, reportData, 0, 0, labelWidth, labelHeight);                    // Top-left
                DrawLabel(gfx, reportData, labelWidth, 0, labelWidth, labelHeight);          // Top-right
                DrawLabel(gfx, reportData, 0, labelHeight, labelWidth, labelHeight);         // Bottom-left
                DrawLabel(gfx, reportData, labelWidth, labelHeight, labelWidth, labelHeight); // Bottom-right

                lblbusy.Text = "Successfully Loaded!";

                // Save the PDF
                printfilepath = Path.Combine(systemHelper.GetTemporaryDirectory(), "test.pdf");
                document.Save(printfilepath);

                // Display the PDF
                var customWebView = new PdfView() { VerticalOptions = LayoutOptions.FillAndExpand };
                stk.Children.Add(customWebView);
                customWebView.Uri = printfilepath;

                printbtn.IsEnabled = true;
            }
            catch (Exception ex)
            {
                lblbusy.Text = "Error generating labels.";
                await DisplayAlert("Error", $"Could not generate labels: {ex.Message}", "OK");
            }
            finally
            {
                busy.IsVisible = false;
                busy.IsRunning = false;
                lblbusy.IsVisible = false;
                stkmain.IsVisible = false;
            }
        }

        private void DrawLabel(XGraphics gfx, ReportData reportData, double x, double y, double width, double height)
        {
            var systemHelper = DependencyService.Get<ISystemHelper>();
            var fontName = systemHelper.GetDefaultSystemFont();

            // Set default company code if not provided
            if (reportData.CompanyCode == null || string.IsNullOrEmpty(reportData.CompanyCode))
                reportData.CompanyCode = "TTI";

            // Fonts for different sections
            XFont headerFont = new XFont(fontName, 14, XFontStyleEx.Bold);
            XFont subheaderFont = new XFont(fontName, 10);
            XFont plateFont = new XFont(fontName, 32, XFontStyleEx.Bold);
            XFont carInfoFont = new XFont(fontName, 14, XFontStyleEx.Bold);
            XFont normalFont = new XFont(fontName, 8);
            XFont smallFont = new XFont(fontName, 6);

            // Clean data inputs
            string plateNo = reportData.PlateNo?.Replace(" ", "") ?? "";
            string carInfo = $"{reportData.CarBrand} {reportData.CarModel}";
            string makeModel = reportData.MakeModel ?? "";
            string tireSize = reportData.TireSize1 ?? "";
            string tireQty = $"Qty: {reportData.NoOfTires}";
            string customerName = $"{reportData.FName} {reportData.LName}";
            string dateTimeStr = DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy");

            // Determine tire season text
            string seasonText = "";
            switch (reportData.TireSeason)
            {
                case ETireSeason.AllSeason:
                    seasonText = "All Seasons";
                    break;
                case ETireSeason.Summer:
                    seasonText = "Summer";
                    break;
                case ETireSeason.Winter:
                    seasonText = "Winter";
                    break;
                case ETireSeason.Other:
                    seasonText = "Other";
                    break;
            }

            // Create QR code data with pipe separators
            string qrCodeData = $"TOTAL TIRE INC|{reportData.RefNo}|{reportData.TireStoredUpto}|{reportData.FName}|{reportData.ExtraRefNo}";

            // Text representation of QR code data
            string qrTextData = $"TOTAL TIRE INC {reportData.RefNo} {reportData.TireStoredUpto} {reportData.FName} {reportData.ExtraRefNo}";

            // Margins inside the label
            double margin = 5;
            double xStart = x + margin;
            double yStart = y + margin;
            double usableWidth = width - (2 * margin);

            // Start drawing from top
            double currentY = yStart;

            // Company header - centered
            string companyName = "TOTAL TIRE INC";
            string companyPhone = "905-632-3500";
            XSize companyNameSize = gfx.MeasureString(companyName, headerFont);
            XSize companyPhoneSize = gfx.MeasureString(companyPhone, subheaderFont);

            gfx.DrawString(companyName, headerFont, XBrushes.Black,
                xStart + (usableWidth - companyNameSize.Width) / 2, currentY + companyNameSize.Height);
            currentY += companyNameSize.Height + 2;

            gfx.DrawString(companyPhone, subheaderFont, XBrushes.Black,
                xStart + (usableWidth - companyPhoneSize.Width) / 2, currentY + companyPhoneSize.Height);
            currentY += companyPhoneSize.Height + 2;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 5;

            // Plate number (large center text)
            XSize plateSize = gfx.MeasureString(plateNo, plateFont);
            gfx.DrawString(plateNo, plateFont, XBrushes.Black,
                xStart + (usableWidth - plateSize.Width) / 2, currentY + plateSize.Height);
            currentY += plateSize.Height + 3;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 3;

            // Car info section
            XSize carInfoSize = gfx.MeasureString(carInfo, carInfoFont);
            gfx.DrawString(carInfo, carInfoFont, XBrushes.Black,
                xStart + (usableWidth - carInfoSize.Width) / 2, currentY + carInfoSize.Height);
            currentY += carInfoSize.Height + 3;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 3;

            // Make/model centered
            XSize makeModelSize = gfx.MeasureString(makeModel, normalFont);
            gfx.DrawString(makeModel, normalFont, XBrushes.Black,
                xStart + (usableWidth - makeModelSize.Width) / 2, currentY + makeModelSize.Height);
            currentY += makeModelSize.Height + 3;

            // Tire info in three columns
            double columnWidth = usableWidth / 3;
            gfx.DrawString(tireSize, normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString(tireQty, normalFont, XBrushes.Black, xStart + columnWidth, currentY + normalFont.Height);
            gfx.DrawString(seasonText, normalFont, XBrushes.Black, xStart + 2 * columnWidth, currentY + normalFont.Height);
            currentY += normalFont.Height + 3;

            // Customer name centered
            XSize customerNameSize = gfx.MeasureString(customerName, normalFont);
            gfx.DrawString(customerName, normalFont, XBrushes.Black,
                xStart + (usableWidth - customerNameSize.Width) / 2, currentY + customerNameSize.Height);
            currentY += customerNameSize.Height + 3;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 3;

            // Date and REP info in two columns
            gfx.DrawString(reportData.REP, normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString(dateTimeStr, normalFont, XBrushes.Black, xStart + usableWidth / 2, currentY + normalFont.Height);
            currentY += normalFont.Height + 3;

            // Location indicators with company code and storage location
            gfx.DrawString("L", normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString("O", normalFont, XBrushes.Black, xStart + 15, currentY + normalFont.Height);
            gfx.DrawString("C", normalFont, XBrushes.Black, xStart + 30, currentY + normalFont.Height);

            // Add company code and storage location next to LOC
            string companyStorageText = $"{reportData.CompanyCode} - {reportData.ExtraRefNo ?? ""}";
            gfx.DrawString(companyStorageText, normalFont, XBrushes.Black, xStart + 45, currentY + normalFont.Height);
            currentY += normalFont.Height + 3;

            // Calculate remaining space for QR code
            double remainingSpace = (y + height) - currentY - margin;
            double qrCodeSize = Math.Min(remainingSpace - 10, 80);

            // Generate and add QR code
            try
            {
                // Use the platform-specific QR code generator through IScanner
                var scanner = DependencyService.Get<IScanner>();
                if (scanner != null)
                {
                    byte[] qrCodeBytes = scanner.GenerateBarcode(qrCodeData, ZXing.BarcodeFormat.QR_CODE);

                    if (qrCodeBytes != null && qrCodeBytes.Length > 0)
                    {
                        // Save QR code to temporary file and load as XImage
                        var tempQrPath = Path.Combine(systemHelper.GetTemporaryDirectory(), $"qr_{Guid.NewGuid()}.png");
                        try
                        {
                            File.WriteAllBytes(tempQrPath, qrCodeBytes);
                            XImage xImage = XImage.FromFile(tempQrPath);

                            // Calculate position for QR code to center it
                            double qrX = xStart + (usableWidth - qrCodeSize) / 2;

                            // Draw QR code
                            gfx.DrawImage(xImage, qrX, currentY, qrCodeSize, qrCodeSize);
                            currentY += qrCodeSize + 2;

                            xImage.Dispose();
                        }
                        finally
                        {
                            // Clean up temporary file
                            if (File.Exists(tempQrPath))
                            {
                                try { File.Delete(tempQrPath); } catch { }
                            }
                        }

                        // Check if we have room for the text
                        if (currentY + 8 <= (y + height) - margin)
                        {
                            // Add QR code data as text below the QR code
                            XSize qrTextSize = gfx.MeasureString(qrTextData, smallFont);
                            string displayText = qrTextData;
                            if (qrTextSize.Width > usableWidth - 10)
                            {
                                int charLimit = (int)((usableWidth - 10) / (qrTextSize.Width / qrTextData.Length));
                                displayText = qrTextData.Substring(0, Math.Min(charLimit, qrTextData.Length)) + "...";
                            }
                            gfx.DrawString(displayText, smallFont, XBrushes.Black,
                                xStart + (usableWidth - gfx.MeasureString(displayText, smallFont).Width) / 2,
                                currentY + smallFont.Height);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // QR code generation failed - continue without QR code
            }
        }


        async void EvePrint(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            if (printfilepath != null)
            {
                DependencyService.Get<IPrint>().printpdf(printfilepath);
            }
            getCommand.IsEnabled = true;
        }
    }
}