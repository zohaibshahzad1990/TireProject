using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Xamarin.Forms;

namespace TireProject
{
    public partial class PrintTirelabelPageA6 : ContentPage
    {
        ReportData data = new ReportData();
        string printfilepath;
        public PrintTirelabelPageA6()
        {
            InitializeComponent();
            stkmain.IsVisible = false;
            printbtn.IsEnabled = true;

        }

        public PrintTirelabelPageA6(ReportData reportData)
        {
            InitializeComponent();
            data = reportData;
            PageRun(reportData);
        }

        async Task PageRun(ReportData reportData)
        {
            lblbusy.Text = "Loading Info From Server...";
            await Task.Delay(2000);
            busy.IsVisible = true;
            busy.IsRunning = true;
            lblbusy.IsVisible = true;
            stkmain.IsVisible = true;
            var systemHelper = DependencyService.Get<ISystemHelper>();
            var document = new PdfDocument();

            var page = document.AddPage();
            page.Size = PdfSharp.PageSize.Legal;
            page.TrimMargins.Top = 75;
            page.TrimMargins.Bottom = 75;
            //page.TrimMargins = PdfSharp.Pdf.TrimMargins(;
            //var paw1 = page.Width / 2;
            //var paw2 = page.Height / 2;
            //page.Width = paw1;
            //page.Height = paw2;
            var gfx = XGraphics.FromPdfPage(page);
            XTextFormatter left = new XTextFormatter(gfx);
            left.Alignment = XParagraphAlignment.Left;
            XTextFormatter right = new XTextFormatter(gfx);
            right.Alignment = XParagraphAlignment.Right;
            XTextFormatter justify = new XTextFormatter(gfx);
            justify.Alignment = XParagraphAlignment.Justify;
            XTextFormatter center = new XTextFormatter(gfx);
            center.Alignment = XParagraphAlignment.Center;

            var fontName = systemHelper.GetDefaultSystemFont();

            var font = new XFont(fontName, 20);
            var fontBold = new XFont(fontName, 20, XFontStyle.Regular);
            var fontItalic = new XFont(fontName, 20, XFontStyle.Italic);

            data.RefNo = data.RefNo != null ? data.RefNo : "null";
            data.TireStoredUpto = data.TireStoredUpto != null ? data.TireStoredUpto : "null";
            data.FName = data.FName != null ? data.FName : "null";
            data.ExtraRefNo = data.ExtraRefNo != null ? data.ExtraRefNo : "null";

            string value = "TOTAL TIRE INC" + "\n" +
                data.RefNo + "\n" +
                data.TireStoredUpto + "\n" +
                            data.FName + "\n" +
                            data.ExtraRefNo;
            int ii = value.Length;

            string DateTimeStr = DateTime.Now.ToLocalTime().ToString("dd-MM-yyyy");
            string sst = "";
            //Checked Box Season
            switch (data.TireSeason)
            {
                case ETireSeason.AllSeason:
                    sst = "All Seasons";
                    break;
                case ETireSeason.Summer:
                    sst = ETireSeason.Summer.ToString();
                    break;
                case ETireSeason.Winter:
                    sst = ETireSeason.Winter.ToString();
                    break;
                case ETireSeason.Other:
                    sst = ETireSeason.Other.ToString();
                    break;
                default:
                    break;
            }

            //var tempPath = Path.Combine(Path.GetTempPath(), "imgdownload4.png");
            //var br = DependencyService.Get<IScanner>().GenerateBarcode(value, ZXing.BarcodeFormat.CODE_128);
            //File.WriteAllBytes(tempPath, br);

            //gfx.DrawImage(XImage.FromFile(tempPath),new XRect(100,20,page.Width-200, (page.Height - 100) / 4));
            //gfx.DrawImage(XImage.FromFile(tempPath), new XRect(100, ((page.Height - 100) / 4)+40, page.Width - 200, (page.Height - 100) / 4));
            //gfx.DrawImage(XImage.FromFile(tempPath), new XRect(100, ((page.Height - 100) / 4)*2 + 60, page.Width - 200, (page.Height - 100) / 4));
            //gfx.DrawImage(XImage.FromFile(tempPath), new XRect(100, ((page.Height - 100) / 4) * 3 + 80, page.Width - 200, (page.Height - 100) / 4));



            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0), 2), 0, page.Height / 2, page.Width, page.Height / 2);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0), 2), page.Width / 2, 0, page.Width / 2, page.Height);
            var platnotemp = data.PlateNo.Replace(" ", "");

            //1
            int pagewidthstart = 0;
            int pagewidth = ((int)(page.Width / 2));
            var pagewidthend = page.Width;
            var fontsizeb = 7;

            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart+5, 5, pagewidth - 5, 7), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, 5, pagewidth - 5, 7), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 20, pagewidthend, 20);
            gfx.DrawString(platnotemp, new XFont(fontName, 75, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 25, pagewidth - 20, 75), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 70+35, pagewidthend, 70 + 35);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 20, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 75 + 35, pagewidth - 20, 20), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 100 + 35, pagewidthend, 100 + 35);

            gfx.DrawString(data.MakeModel, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 105 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopRight);

            gfx.DrawString(data.FName + " " + data.LName, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 125 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 135 + 35, pagewidthend, 135 + 35);

            gfx.DrawString(DateTimeStr, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, ((page.Height) / 2) - 10, pagewidth - 20, 7), XStringFormats.CenterRight);
            gfx.DrawString(reportData.REP, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, ((page.Height) / 2) - 10, pagewidth - 20, 7), XStringFormats.Center);
            
            gfx.DrawString("L", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 145 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 160 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 175 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            //2
            pagewidthstart = ((int)(page.Width/2));
            pagewidth = pagewidthstart;

            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, 5, pagewidth - 5, 7), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, 5, pagewidth - 5, 7), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, 20, pagewidth, 20);
            gfx.DrawString(platnotemp, new XFont(fontName, 75, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 25, pagewidth - 20, 75), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, 70+35, pagewidth, 70 + 35);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 20, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 75 + 35, pagewidth - 20, 20), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, 100 + 35, pagewidth, 100 + 35);

            gfx.DrawString(data.MakeModel, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 105 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 115 + 35, pagewidth - 20, 7), XStringFormats.TopRight);

            gfx.DrawString(data.FName + " " + data.LName, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 125 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, 135 + 35, pagewidth, 135);

            gfx.DrawString(DateTimeStr, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, ((page.Height) / 2) - 10, pagewidth - 20, 7), XStringFormats.CenterRight);
            gfx.DrawString(reportData.REP, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, ((page.Height) / 2) - 10, pagewidth - 20, 7), XStringFormats.Center);
            
            gfx.DrawString("L", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 145 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 160 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, 175 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);



            //3
            pagewidthstart = 0;
            pagewidth = ((int)(page.Width / 2));
            int pageheightstart = ((int)(page.Height / 2));

            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, pageheightstart+5, pagewidth - 5, 7), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, pageheightstart + 5, pagewidth - 5, 7), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, pageheightstart + 20, pagewidth, pageheightstart + 20);
            gfx.DrawString(platnotemp, new XFont(fontName, 75, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 25, pagewidth - 20, 75), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, pageheightstart + 70+35, pagewidth, pageheightstart + 70 + 35);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 20, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 75 + 35, pagewidth - 20, 20), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, pageheightstart + 100 + 35, pagewidth, pageheightstart + 100 + 35);

            gfx.DrawString(data.MakeModel, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 105 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopRight);

            gfx.DrawString(data.FName + " " + data.LName, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 125 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, pageheightstart + 135 + 35, pagewidth,pageheightstart+ 135 + 35);

            gfx.DrawString(DateTimeStr, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, page.Height - 10, pagewidth - 20, 7), XStringFormats.CenterRight);

            gfx.DrawString(reportData.REP, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, page.Height - 10, pagewidth - 20, 7), XStringFormats.Center);

            gfx.DrawString("L", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 145 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 160 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 175 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            //4
            pagewidthstart = ((int)(page.Width / 2));
            pagewidth = pagewidthstart;
            pagewidthend = page.Width;
            pageheightstart = ((int)(page.Height / 2));

            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, pageheightstart + 5, pagewidth - 5, 7), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 5, pageheightstart + 5, pagewidth - 5, 7), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, pageheightstart + 20, pagewidthend, pageheightstart + 20);
            gfx.DrawString(platnotemp, new XFont(fontName, 75, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 25, pagewidth - 20, 75), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, pageheightstart + 70+35, pagewidthend, pageheightstart + 70 + 35);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 20, XFontStyle.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 75 + 35, pagewidth - 20, 20), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, pageheightstart + 100 + 35, pagewidthend, pageheightstart + 100 + 35);

            gfx.DrawString(data.MakeModel, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 105 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 115 + 35, pagewidth - 20, 7), XStringFormats.TopRight);

            gfx.DrawString(data.FName + " " + data.LName, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 125 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), pagewidthstart, pageheightstart + 135+35, pagewidthend, pageheightstart+ 135 + 35);

            gfx.DrawString(DateTimeStr, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, page.Height - 10, pagewidth - 20, 7), XStringFormats.CenterRight);

            gfx.DrawString(reportData.REP, new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, page.Height - 10, pagewidth - 20, 7), XStringFormats.Center);
            
            gfx.DrawString("L", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 145 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 160 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, fontsizeb, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(pagewidthstart + 10, pageheightstart + 175 + 35, pagewidth - 20, 7), XStringFormats.TopLeft);

            lblbusy.Text = "Successfully Loaded!";
            //////////////////////////
            printfilepath = Path.Combine(systemHelper.GetTemporaryDirectory(), "test.pdf");
            document.Save(printfilepath);

            var customWebView = new PdfView() { VerticalOptions = LayoutOptions.FillAndExpand };

            stk.Children.Add(customWebView);
            customWebView.Uri = printfilepath;
            printbtn.IsEnabled = true;
            busy.IsVisible = false;
            busy.IsRunning = false;
            lblbusy.IsVisible = false;
            stkmain.IsVisible = false;
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
