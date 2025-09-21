using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Entry = Xamarin.Forms.Entry;

namespace TireProject
{
    public partial class PrintTireLabelPdfPage : ContentPage
    {
        ReportData data = new ReportData();
        private int numberOfCopies = 4; // Default number of copies to print
        private string pdfFilePath; // Store the generated PDF path

        public PrintTireLabelPdfPage()
        {
            InitializeComponent();
            stkmain.IsVisible = false;
            printbtn.IsEnabled = true;
            copiesEntry.Text = numberOfCopies.ToString();
        }

        public PrintTireLabelPdfPage(ReportData reportData)
        {
            InitializeComponent();
            data = reportData;

            // Make sure the overlay is not visible initially
            stkmain.IsVisible = false;

            // Set copies text field
            copiesEntry.Text = numberOfCopies.ToString();

            // Run page initialization without showing the overlay initially
            Device.BeginInvokeOnMainThread(async () => {
                await PageRun(reportData);
            });
        }

        async Task PageRun(ReportData reportData)
        {
            try
            {
                // Show overlay with loading message
                lblbusy.Text = "Generating preview...";
                busy.IsVisible = true;
                busy.IsRunning = true;
                lblbusy.IsVisible = true;
                stkmain.IsVisible = true;

                await Task.Delay(1000);

                // Generate PDF
                byte[] pdfBytes = await GeneratePdfAsync(reportData, 1);

                // Save PDF to temporary file for preview
                var systemHelper = DependencyService.Get<ISystemHelper>();
                pdfFilePath = Path.Combine(systemHelper.GetTemporaryDirectory(), $"TireLabels_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                File.WriteAllBytes(pdfFilePath, pdfBytes);

                // Create PDF viewer and add it to the layout
                var pdfView = new PdfView() { VerticalOptions = LayoutOptions.FillAndExpand };
                previewContainer.Children.Clear(); // Clear any existing content
                previewContainer.Children.Add(pdfView);
                pdfView.Uri = pdfFilePath;

                lblbusy.Text = "Ready to Print!";
            }
            catch (Exception ex)
            {
                lblbusy.Text = "Error generating preview.";
                await DisplayAlert("Error", $"Could not generate preview: {ex.Message}", "OK");
            }
            finally
            {
                // Hide the overlay when done
                busy.IsVisible = false;
                busy.IsRunning = false;
                lblbusy.IsVisible = false;
                stkmain.IsVisible = false;
                printbtn.IsEnabled = true;
            }
        }

        private async Task<byte[]> GeneratePdfAsync(ReportData reportData, int copies)
        {
            // Create a new PDF document
            PdfDocument document = new PdfDocument();
            document.Info.Title = "Tire Labels";

            // Label dimensions (4 × 6 inches = 288 × 432 points)
            // 1 inch = 72 points in PDF
            double labelWidth = 288;  // 4 inches
            double labelHeight = 432; // 6 inches

            // Calculate number of pages needed (1 label per page)
            int totalLabels = copies;

            for (int labelIndex = 0; labelIndex < totalLabels; labelIndex++)
            {
                // Create new page with exact label size
                PdfPage page = document.AddPage();
                page.Width = labelWidth;
                page.Height = labelHeight;
                XGraphics gfx = XGraphics.FromPdfPage(page);

                // Draw label content
                DrawLabel(gfx, reportData, 0, 0, labelWidth, labelHeight);
            }

            // Save to memory stream and return bytes
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
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
            XFont headerFont = new XFont(fontName, 18, XFontStyleEx.Bold);
            XFont subheaderFont = new XFont(fontName, 14);
            XFont plateFont = new XFont(fontName, 42, XFontStyleEx.Bold);
            XFont carInfoFont = new XFont(fontName, 18, XFontStyleEx.Bold);
            XFont normalFont = new XFont(fontName, 11);
            XFont smallFont = new XFont(fontName, 8);

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
            double margin = 10;
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
            currentY += companyNameSize.Height + 5;

            gfx.DrawString(companyPhone, subheaderFont, XBrushes.Black,
                xStart + (usableWidth - companyPhoneSize.Width) / 2, currentY + companyPhoneSize.Height);
            currentY += companyPhoneSize.Height + 5;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 10;

            // Plate number (large center text)
            XSize plateSize = gfx.MeasureString(plateNo, plateFont);
            gfx.DrawString(plateNo, plateFont, XBrushes.Black,
                xStart + (usableWidth - plateSize.Width) / 2, currentY + plateSize.Height);
            currentY += plateSize.Height + 5;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 5;

            // Car info section
            XSize carInfoSize = gfx.MeasureString(carInfo, carInfoFont);
            gfx.DrawString(carInfo, carInfoFont, XBrushes.Black,
                xStart + (usableWidth - carInfoSize.Width) / 2, currentY + carInfoSize.Height);
            currentY += carInfoSize.Height + 5;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 5;

            // Make/model centered
            XSize makeModelSize = gfx.MeasureString(makeModel, normalFont);
            gfx.DrawString(makeModel, normalFont, XBrushes.Black,
                xStart + (usableWidth - makeModelSize.Width) / 2, currentY + makeModelSize.Height);
            currentY += makeModelSize.Height + 5;

            // Tire info in three columns
            double columnWidth = usableWidth / 3;
            gfx.DrawString(tireSize, normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString(tireQty, normalFont, XBrushes.Black, xStart + columnWidth, currentY + normalFont.Height);
            gfx.DrawString(seasonText, normalFont, XBrushes.Black, xStart + 2 * columnWidth, currentY + normalFont.Height);
            currentY += normalFont.Height + 5;

            // Customer name centered
            XSize customerNameSize = gfx.MeasureString(customerName, normalFont);
            gfx.DrawString(customerName, normalFont, XBrushes.Black,
                xStart + (usableWidth - customerNameSize.Width) / 2, currentY + customerNameSize.Height);
            currentY += customerNameSize.Height + 5;

            // Horizontal line
            gfx.DrawLine(new XPen(XColors.Black, 1), xStart, currentY, xStart + usableWidth, currentY);
            currentY += 5;

            // Date and REP info in two columns
            gfx.DrawString(reportData.REP, normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString(dateTimeStr, normalFont, XBrushes.Black, xStart + usableWidth / 2, currentY + normalFont.Height);
            currentY += normalFont.Height + 5;

            // Location indicators - more compact with horizontal layout
            gfx.DrawString("L", normalFont, XBrushes.Black, xStart, currentY + normalFont.Height);
            gfx.DrawString("O", normalFont, XBrushes.Black, xStart + 30, currentY + normalFont.Height);
            gfx.DrawString("C", normalFont, XBrushes.Black, xStart + 60, currentY + normalFont.Height);

            // Add company code and storage location next to LOC
            string companyStorageText = $"{reportData.CompanyCode} - {reportData.ExtraRefNo ?? ""}";
            gfx.DrawString(companyStorageText, normalFont, XBrushes.Black, xStart + 90, currentY + normalFont.Height);
            currentY += normalFont.Height + 5;

            // Calculate remaining space
            double remainingSpace = height - currentY - margin;
            double qrCodeSize = 130;

            // Check if we have enough space for the QR code
            if (remainingSpace < qrCodeSize + 25)
            {
                // Not enough space, reduce QR code size
                qrCodeSize = Math.Max(100, remainingSpace - 25);
            }

            // Generate and add QR code
            try
            {
                // Use the platform-specific QR code generator through IScanner
                var scanner = DependencyService.Get<IScanner>();
                if (scanner != null)
                {
                    // Generate higher resolution QR code - request larger size
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
                            currentY += qrCodeSize + 5;

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
                        if (currentY + 15 <= height - margin)
                        {
                            // Add QR code data as text below the QR code
                            XSize qrTextSize = gfx.MeasureString(qrTextData, smallFont);
                            // Only print as much text as fits on a single line with some safety margin
                            string displayText = qrTextData;
                            if (qrTextSize.Width > usableWidth - 20)
                            {
                                int charLimit = (int)((usableWidth - 20) / (qrTextSize.Width / qrTextData.Length));
                                displayText = qrTextData.Substring(0, Math.Min(charLimit, qrTextData.Length)) + "...";
                            }
                            gfx.DrawString(displayText, smallFont, XBrushes.Black,
                                xStart + (usableWidth - gfx.MeasureString(displayText, smallFont).Width) / 2,
                                currentY + smallFont.Height);
                        }
                    }
                    else
                    {
                        throw new Exception("QR code generation returned empty result");
                    }
                }
                else
                {
                    throw new Exception("Scanner service not found");
                }
            }
            catch (Exception)
            {
                // QR code generation failed - continue without QR code
            }
        }


        async void EvePrint(object sender, EventArgs e)
        {
            // Validate number of copies
            if (!int.TryParse(copiesEntry.Text, out numberOfCopies) || numberOfCopies <= 0)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid number of copies", "OK");
                copiesEntry.Text = "4"; // Reset to default
                numberOfCopies = 4;
                return;
            }

            try
            {
                lblbusy.Text = "Generating PDF...";
                busy.IsVisible = true;
                busy.IsRunning = true;
                lblbusy.IsVisible = true;
                stkmain.IsVisible = true;
                printbtn.IsEnabled = false;

                // Generate PDF with the requested number of copies
                byte[] pdfBytes = await GeneratePdfAsync(data, numberOfCopies);

                // Save to temporary file for printing
                var systemHelper = DependencyService.Get<ISystemHelper>();
                string filePath = Path.Combine(systemHelper.GetTemporaryDirectory(), $"TireLabels_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                File.WriteAllBytes(filePath, pdfBytes);

                // Send to printer
                DependencyService.Get<IPrint>().printpdf(filePath);

                // Success
                await DisplayAlert("Success", "PDF sent to printer", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to print PDF: {ex.Message}", "OK");
            }
            finally
            {
                // Reset UI
                busy.IsVisible = false;
                busy.IsRunning = false;
                lblbusy.IsVisible = false;
                stkmain.IsVisible = false;
                printbtn.IsEnabled = true;
            }
        }

        private void OnCopiesEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Validate that only numbers are entered
            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                bool isValid = int.TryParse(e.NewTextValue, out int value);
                if (!isValid)
                {
                    ((Entry)sender).Text = e.OldTextValue;
                }
            }
        }
    }
}