using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Xamarin.Forms;
using System.Net.Sockets;

namespace TireProject
{
    public partial class PrintTireLabelZebraPage : ContentPage
    {
        ReportData data = new ReportData();
        private int numberOfCopies = 4; // Default number of copies to print

        public PrintTireLabelZebraPage()
        {
            InitializeComponent();
            stkmain.IsVisible = false;
            printbtn.IsEnabled = true;
            copiesEntry.Text = numberOfCopies.ToString();
        }

        public PrintTireLabelZebraPage(ReportData reportData)
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
                lblbusy.Text = "Loading Info From Server...";
                busy.IsVisible = true;
                busy.IsRunning = true;
                lblbusy.IsVisible = true;
                stkmain.IsVisible = true;

                await Task.Delay(1000);

                // Generate ZPL code for the Zebra printer
                string zplCommand = GenerateZPLCode(reportData);

                // Update status
                lblbusy.Text = "Generating preview...";

                // Display preview using Labelary API
                var previewImage = await GeneratePreviewImage(zplCommand);
                if (previewImage != null)
                {
                    // Make sure previewImageView is defined in your XAML
                    previewImageView.Source = previewImage;
                    previewImageView.IsVisible = true;
                }

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

        private string GenerateZPLCode(ReportData reportData)
        {
            // Zebra ZP450 print width: 4.09 inches (104mm)
            // Typical label size: 4" x 6" (or smaller)

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

            // Text representation of QR code data (space-separated for single line)
            string qrTextData = $"TOTAL TIRE INC {reportData.RefNo} {reportData.TireStoredUpto} {reportData.FName} {reportData.ExtraRefNo}";

            // Create ZPL code for 4" x 6" label
            StringBuilder zpl = new StringBuilder();

            // ZPL Header with proper dimensions
            zpl.AppendLine("^XA");
            zpl.AppendLine("^MD15"); // Medium-dark print darkness
            zpl.AppendLine("^PR2");  // Print speed 2 inches/sec
            zpl.AppendLine("^LH0,0"); // Label home position
            zpl.AppendLine("^PW609"); // Print width (4 inches at 8 dpmm = 609 dots)
            zpl.AppendLine("^LL915"); // Label length (6 inches at 8 dpmm = 915 dots)

            // Company header - centered
            zpl.AppendLine("^FO20,20^A0N,28,28^FB569,1,0,C^FDTOTAL TIRE INC^FS");
            zpl.AppendLine("^FO20,50^A0N,28,28^FB569,1,0,C^FD905-632-3500^FS");
            zpl.AppendLine("^FO20,80^GB569,1,2^FS"); // Horizontal line

            // Plate number (large center text)
            zpl.AppendLine($"^FO20,100^A0N,80,80^FB569,1,0,C^FD{plateNo}^FS");

            // Car info section with horizontal line above and below
            zpl.AppendLine("^FO20,190^GB569,1,2^FS"); // Horizontal line
            zpl.AppendLine($"^FO20,210^A0N,36,36^FB569,1,0,C^FD{carInfo}^FS");
            zpl.AppendLine("^FO20,250^GB569,1,2^FS"); // Horizontal line

            // Make/model centered
            zpl.AppendLine($"^FO50,270^A0N,24,24^FB509,1,0,C^FD{makeModel}^FS");

            // Tire info in three columns
            zpl.AppendLine($"^FO20,300^FB170,1,0,C^A0N,24,24^FD{tireSize}^FS");
            zpl.AppendLine($"^FO190,300^FB150,1,0,C^A0N,24,24^FD{tireQty}^FS");
            zpl.AppendLine($"^FO340,300^FB150,1,0,C^A0N,24,24^FD{seasonText}^FS");

            // Customer name centered
            zpl.AppendLine($"^FO50,330^A0N,24,24^FB509,1,0,C^FD{customerName}^FS");

            // Horizontal line
            zpl.AppendLine("^FO20,360^GB569,1,2^FS");

            // Date and REP info in two columns
            zpl.AppendLine($"^FO20,390^FB250,1,0,C^A0N,24,24^FD{reportData.REP}^FS");
            zpl.AppendLine($"^FO270,390^FB250,1,0,C^A0N,24,24^FD{dateTimeStr}^FS");

            // Location indicators (L, O, C)
            zpl.AppendLine("^FO20,430^A0N,24,24^FDL^FS");
            zpl.AppendLine("^FO20,460^A0N,24,24^FDO^FS");
            zpl.AppendLine("^FO20,490^A0N,24,24^FDC^FS");

            // Add QR code at the center of the label
            zpl.AppendLine("^FO205,520");  // Center position
            zpl.AppendLine("^BQN,2,6,Q");  // QR code: Normal position, Model 2, 6 magnification, Q error correction
            zpl.AppendLine($"^FDMA,{qrCodeData}^FS");  // MA prefix for proper encoding

            // Add QR code data as text below the QR code
            zpl.AppendLine($"^FO20,750^A0N,15,15^FB569,1,0,C^FD{qrTextData}^FS");

            // End ZPL
            zpl.AppendLine("^XZ");

            return zpl.ToString();
        }

        private async Task<ImageSource> GeneratePreviewImage(string zplCode)
        {
            try
            {
                // Using Labelary API for preview generation
                using (var httpClient = new HttpClient())
                {
                    // Properly encode ZPL for transmission
                    var content = new StringContent(zplCode, Encoding.UTF8, "application/x-www-form-urlencoded");

                    // Use 8dpmm (203 dpi) which matches ZP450 resolution, and 4x6 inch label size
                    var response = await httpClient.PostAsync("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var imageStream = await response.Content.ReadAsStreamAsync();
                        // Now reset the original stream for barcode scanning
                        imageStream.Position = 0;

                        return ImageSource.FromStream(() => imageStream);
                    }
                    else
                    {
                        // Log the error for debugging
                        Console.WriteLine($"Labelary API error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                        await DisplayAlert("Preview Error",
                            $"Failed to generate preview: {response.StatusCode}", "OK");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating preview: {ex.Message}");
                await DisplayAlert("Preview Error",
                    $"Failed to generate preview: {ex.Message}", "OK");
                return null;
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
                lblbusy.Text = "Sending to printer...";
                busy.IsVisible = true;
                busy.IsRunning = true;
                lblbusy.IsVisible = true;
                stkmain.IsVisible = true;
                printbtn.IsEnabled = false;

                await Task.Run(async () => {
                    try
                    {
                        // Generate ZPL code
                        string zplCommand = GenerateZPLCode(data);

                        // Get printer settings (these might be stored in app settings)
                        string printerIp = "192.168.1.101"; // Default IP - this should be configurable in settings
                        int printerPort = 9100; // Default ZPL port
                        string savedPrinterIp = null;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            // Ask for printer IP if not configured
                             savedPrinterIp = await GetPrinterIpAsync();
                        });
                        if (!string.IsNullOrEmpty(savedPrinterIp))
                        {
                            printerIp = savedPrinterIp;
                        }

                        // Print multiple copies based on user input
                        for (int i = 0; i < numberOfCopies; i++)
                        {
                            using (TcpClient client = new TcpClient())
                            {
                                await client.ConnectAsync(printerIp, printerPort);
                                using (NetworkStream stream = client.GetStream())
                                {
                                    // Convert ZPL command to bytes and send to printer
                                    byte[] zplBytes = Encoding.UTF8.GetBytes(zplCommand);
                                    await stream.WriteAsync(zplBytes, 0, zplBytes.Length);
                                    await stream.FlushAsync();
                                    // Add small delay between prints
                                    await Task.Delay(500);
                                }
                            }

                            // Update progress on UI thread
                            Device.BeginInvokeOnMainThread(() => {
                                lblbusy.Text = $"Printing label {i + 1} of {numberOfCopies}...";
                            });
                        }

                        // Success
                        Device.BeginInvokeOnMainThread(async () => {
                            await DisplayAlert("Success", $"Successfully sent {numberOfCopies} label(s) to the printer", "OK");
                        });
                    }
                    catch (Exception ex)
                    {
                        // Handle error on UI thread
                        Device.BeginInvokeOnMainThread(async () => {
                            await DisplayAlert("Print Error", $"Failed to print: {ex.Message}", "OK");
                        });
                    }
                });
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to print: {ex.Message}", "OK");
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

        private async Task<string> GetPrinterIpAsync()
        {
            // This should check app settings for saved printer IP
            // If not found, you could prompt the user

            // Example implementation - replace with your actual settings implementation
            string savedIp = Application.Current.Properties.ContainsKey("ZebraPrinterIP")
                ? Application.Current.Properties["ZebraPrinterIP"].ToString()
                : "";

            if (string.IsNullOrEmpty(savedIp))
            {

                // Only prompt if we don't have a saved IP
                string result = await DisplayPromptAsync(
                    "Printer Setup",
                    "Enter Zebra Printer IP Address:",
                    initialValue: "192.168.1.101");

                if (!string.IsNullOrEmpty(result))
                {
                    // Save for future use
                    Application.Current.Properties["ZebraPrinterIP"] = result;
                    await Application.Current.SavePropertiesAsync();
                    return result;
                }
                return "192.168.1.101"; // Default if user cancels
            }

            return savedIp;
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