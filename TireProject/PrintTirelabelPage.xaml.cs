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
            lblbusy.Text = "Loading Info From Server...";
            await Task.Delay(2000);
            busy.IsVisible = true;
            busy.IsRunning = true;
            lblbusy.IsVisible = true;
            stkmain.IsVisible = true;
            var systemHelper = DependencyService.Get<ISystemHelper>();
            var document = new PdfDocument();

            var page = document.AddPage();
            page.Size= PdfSharp.PageSize.Letter;
            page.TrimMargins.Top = 35;
            page.TrimMargins.Bottom = 35;
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
            var fontBold = new XFont(fontName, 20, XFontStyleEx.Regular);
            var fontItalic = new XFont(fontName, 20, XFontStyleEx.Italic);

            data.RefNo = data.RefNo != null ? data.RefNo : "null";
            data.TireStoredUpto = data.TireStoredUpto != null ? data.TireStoredUpto : "null";
            data.FName = data.FName != null ? data.FName : "null";
            data.ExtraRefNo = data.ExtraRefNo != null ? data.ExtraRefNo : "null";

            string value = "TOTAL TIRE INC" + "\n" +
                data.RefNo + "\n"+
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



            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0),2), 0, page.Height/2, page.Width, page.Height / 2);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0), 2), page.Width / 2, 0, page.Width/2, page.Height);


            var platnotemp = data.PlateNo.Replace(" ","");

            //1
            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 10, (page.Width / 2) - 10, 13), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 10, (page.Width / 2) - 20, 13), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 35, page.Width / 2, 35);
            gfx.DrawString(platnotemp, new XFont(fontName, 70, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 45, (page.Width / 2) - 20, 70), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 130, page.Width / 2, 130);
            gfx.DrawString(data.CarBrand+" "+ data.CarModel, new XFont(fontName, 30, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 140, (page.Width / 2) - 20, 30), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 180, page.Width / 2, 180);

            gfx.DrawString(data.MakeModel, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 190, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 215, (page.Width / 2)-20, 13), XStringFormats.TopLeft);
            gfx.DrawString("Qty: "+data.NoOfTires, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 215, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 215, (page.Width / 2) - 20, 13), XStringFormats.TopRight);

            gfx.DrawString(data.FName+" " + data.LName, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 240, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, 270, page.Width / 2, 270);

            gfx.DrawString(DateTimeStr, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, ((page.Height)/2)-30, (page.Width / 2) - 20, 13), XStringFormats.CenterRight);


            gfx.DrawString(reportData.REP, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, ((page.Height) / 2) - 30, (page.Width / 2) - 20, 13), XStringFormats.Center);

            gfx.DrawString("L", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 280, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 310, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 340, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            //2
            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, 10, (page.Width / 2) - 10, 13), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, 10, (page.Width / 2) - 20, 13), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2), 35, page.Width, 35);
            gfx.DrawString(platnotemp, new XFont(fontName, 70, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, 45, (page.Width / 2) - 20, 70), XStringFormats.Center);


            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), page.Width / 2, 130, page.Width , 130);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 30, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, 140, (page.Width / 2) - 20, 30), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), page.Width / 2, 180, page.Width , 180);

            gfx.DrawString(data.MakeModel, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, 190, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 215, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 215, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 215, (page.Width / 2) - 20, 13), XStringFormats.TopRight);

            gfx.DrawString(data.FName +" " + data.LName, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 240, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2), 270, page.Width, 270);


            
            gfx.DrawString(DateTimeStr, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, ((page.Height) / 2) - 30, (page.Width / 2) - 20, 13), XStringFormats.CenterRight);


            gfx.DrawString(reportData.REP, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, ((page.Height) / 2) - 30, (page.Width / 2) - 20, 13), XStringFormats.Center);

            gfx.DrawString("L", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 280, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 310, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 340, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);


            //3
            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 10, (page.Width / 2) - 10, 13), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 10, (page.Width / 2) - 20, 13), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, (page.Height / 2)+35, page.Width / 2, (page.Height / 2) + 35);
            gfx.DrawString(platnotemp, new XFont(fontName, 70, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2)+45, (page.Width / 2) - 20, 70), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, (page.Height / 2) +130, page.Width / 2, (page.Height / 2)+ 130);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 30, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 140, (page.Width / 2) - 20, 30), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, (page.Height / 2) + 180, page.Width / 2, (page.Height / 2)+ 180);

            gfx.DrawString(data.MakeModel, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 190, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopRight);

            gfx.DrawString(data.FName + " " + data.LName, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height / 2) + 240, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 0, (page.Height / 2) + 270, page.Width / 2, (page.Height / 2) + 270);


            gfx.DrawString(DateTimeStr, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, ((page.Height)) - 30, (page.Width / 2) - 20, 13), XStringFormats.CenterRight);

            gfx.DrawString(reportData.REP, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, ((page.Height)) - 30, (page.Width / 2) - 20, 13), XStringFormats.Center);
            
            gfx.DrawString("L", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height/2)+ 280, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height/2) + 310, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height/2) + 340, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            //4
            gfx.DrawString("TOTAL TIRE INC", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 10, (page.Width / 2) - 10, 13), XStringFormats.TopLeft);
            gfx.DrawString("905-632-3500", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 10, (page.Width / 2) - 20, 13), XStringFormats.TopRight);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2), (page.Height / 2) + 35, page.Width, (page.Height / 2) + 35);
            gfx.DrawString(platnotemp, new XFont(fontName, 70, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, (page.Height / 2)+45, (page.Width / 2) - 20, 70), XStringFormats.Center);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), page.Width / 2, (page.Height / 2) + 130, page.Width , (page.Height / 2) + 130);
            gfx.DrawString(data.CarBrand + " " + data.CarModel, new XFont(fontName, 30, XFontStyleEx.Bold), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 140, (page.Width / 2) - 20, 30), XStringFormats.Center);
            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), page.Width / 2, (page.Height / 2) + 180, page.Width , (page.Height / 2) + 180);

            gfx.DrawString(data.MakeModel, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 190, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawString(data.TireSize1, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("Qty: " + data.NoOfTires, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);
            gfx.DrawString(sst, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 215, (page.Width / 2) - 20, 13), XStringFormats.TopRight);

            gfx.DrawString(data.FName+" "+ data.LName, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 240, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2), (page.Height / 2) + 270, page.Width, (page.Height / 2) + 270);

            gfx.DrawString(DateTimeStr, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2)+10, ((page.Height)) - 30, (page.Width / 2) - 20, 13), XStringFormats.CenterRight);

            gfx.DrawString(reportData.REP, new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, ((page.Height)) - 30, (page.Width / 2) - 20, 13), XStringFormats.Center);

            
            gfx.DrawString("L", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 280, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("O", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 310, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);
            gfx.DrawString("C", new XFont(fontName, 13, XFontStyleEx.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, (page.Height / 2) + 340, (page.Width / 2) - 20, 13), XStringFormats.TopLeft);

            lblbusy.Text = "Successfully Loaded!";
            //////////////////////////
            printfilepath = Path.Combine(systemHelper.GetTemporaryDirectory(), "test.pdf");
            document.Save(printfilepath);

            var customWebView = new PdfView() { VerticalOptions = LayoutOptions.FillAndExpand
            };
            stk.Children.Add(customWebView);

            customWebView.Uri = printfilepath;

            //App.pdfpath = printfilepath;
            //stk.Uri = Path.GetFileName(printfilepath);
            //stk.On<Xamarin.Forms.PlatformConfiguration.Android>().EnableZoomControls(true);
            //stk.On<Xamarin.Forms.PlatformConfiguration.Android>().DisplayZoomControls(false);



            //customWebView.Path = printfilepath;
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
