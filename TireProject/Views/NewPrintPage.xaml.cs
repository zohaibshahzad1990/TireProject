using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using Xamarin.Forms;

namespace TireProject
{
    public partial class NewPrintPage : ContentPage
    {
        ReportData reportData;
        string printfilepath;
        string pp;
        GetOnlineData getOnline = new GetOnlineData();
        bool busycheck = false;
        bool disappeUpdate = true;

        public NewPrintPage()
        {
            InitializeComponent();
        }

        public NewPrintPage(ReportData rep)
        {
            InitializeComponent();

            reportData = rep;
            PageRun();
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;

            if (printfilepath != null)
            {
                DependencyService.Get<IPrint>().printpdf(printfilepath);
            }


            getCommand.IsEnabled = true;
        }

        async Task PageRun()
        {
            lblbusy.Text = "Loading Info From Server...";
            await Task.Delay(2000);
            busy.IsVisible = true;
            busy.IsRunning = true;
            lblbusy.IsVisible = true;
            stkbusy.IsVisible = true;
            var systemHelper = DependencyService.Get<ISystemHelper>();
            var document = new PdfDocument();

            var page = document.AddPage();

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

            XImage image = null;


            ////Image
            if (!File.Exists(DependencyService.Get<IFileSave>().GetPicture()))
            {
                var assembly = typeof(MainPage).GetTypeInfo().Assembly;
                var imageName = assembly.GetName().Name + ".logo.png";



                using (var stream = assembly.GetManifestResourceStream(imageName))
                {
                    image = XImage.FromStream(stream);
                }
            }




            gfx.DrawString("Tire Storage Reciept", new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 3) - 10, 20, (page.Width / 3), 13), XStringFormats.TopCenter);

            gfx.DrawString("Ref No.", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 3) - 10, 38, (page.Width / 4), 13), XStringFormats.TopLeft);


            gfx.DrawString("Date: ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 3) - 10, 53, (page.Width / 4), 13), XStringFormats.TopLeft);
            gfx.DrawString("Storage Location: ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 3) - 10, 68, (page.Width / 4), 13), XStringFormats.TopLeft);


            gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 10, 115, page.Width - 10, 115);


            //gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 10, 120, page.Width - 10, 120);
            //A

            gfx.DrawString("Customer Details", new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 115, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);

            //gfx.DrawString("Ref No : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 150, (page.Width / 4) - 10, 15), XStringFormats.TopLeft);
            //gfx.DrawString("Date : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 4) + 10, 150, (page.Width / 4) - 10, 15), XStringFormats.TopLeft);

            //gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 10, 170, (page.Width / 2) - 10, 170);

            gfx.DrawString("Name : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 150, (page.Width / 4) - 10, 15), XStringFormats.TopLeft);

            gfx.DrawString("Address : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 170, (page.Width / 2) - 10, 25), XStringFormats.TopLeft);
            gfx.DrawString("Phone No : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 220, (page.Width / 2) - 10, 15), XStringFormats.TopLeft);
            gfx.DrawString("Home : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 240, (page.Width / 4) - 10, 15), XStringFormats.TopLeft);
            gfx.DrawString("Work : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 4) + 10, 240, (page.Width / 4) - 10, 15), XStringFormats.TopLeft);
            gfx.DrawString("Email : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 260, (page.Width / 2) - 10, 15), XStringFormats.TopLeft);

            //gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), page.Width/2, 120, page.Width /2, (page.Height - 250 + 120) / 2);
           
            gfx.DrawString("Tire Storage Detail", new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 115, (page.Width / 2) - 20, 13), XStringFormats.TopCenter);

            gfx.DrawString("Types  of Tire", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 160);

            gfx.DrawString("Tire Size", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 200);

            gfx.DrawString("RIM Attached", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 240);
            gfx.DrawString("Yes", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 240);
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 170, 230, 15, 15);
            gfx.DrawString("No", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 200, 240);
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 260, 230, 15, 15);

            gfx.DrawString("Type of RIM", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 260);



            gfx.DrawString("Tire Depth", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 280);
            gfx.DrawString("Tire Brand", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 300);


            gfx.DrawString("Tire Stored Upto", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 10, 320);



            //gfx.DrawLine(new XPen(XColor.FromArgb(0, 0, 0)), 10, (page.Height - 250+120)/2, page.Width - 10, (page.Height - 250 + 120) / 2);
            //C
            gfx.DrawString("Vehicle Details", new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, (page.Height - 100) / 2, (page.Width / 2) - 10, 13), XStringFormats.TopCenter);
            gfx.DrawString("Plate Number : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 170 + 120) / 2, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);
            gfx.DrawString("Car Year : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 130 + 120) / 2, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);
            gfx.DrawString("Car Brand : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 90 + 120) / 2, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);
            gfx.DrawString("Car Model : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 50 + 120) / 2, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);



            gfx.DrawString("Remarks", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height + 110) / 2, page.Width / 2 - 65, 15), XStringFormats.TopLeft);

            //Terms& Condition
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 0, 0)), 10, page.Height - 270, page.Width - 20, 170);
            gfx.DrawString("Terms & Conditions", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, page.Height - 260, page.Width / 2 - 65, 15), XStringFormats.TopLeft);
            //Authorize Signature
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 0, 0)), 10, page.Height - 90, (page.Width / 2) - 75, 80);
            gfx.DrawString("Authorized Representative", new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, page.Height - 95, page.Width / 2 - 75, 80), XStringFormats.BottomCenter);

            gfx.DrawString(reportData.REP, new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, page.Height - 95, page.Width / 2 - 75, 80), XStringFormats.Center);

            //gfx.DrawString(reportData.REP, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20,page.Height - 95, (page.Width / 2) - 65, 65), XStringFormats.Center);

            //Customer Signature
            gfx.DrawRectangle(new XPen(XColor.FromArgb(0, 0, 0)), page.Width - ((page.Width / 2) - 75) - 10, page.Height - 90, (page.Width / 2) - 75, 80);
            gfx.DrawString("Customer Signature", new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - ((page.Width / 2) - 75), page.Height - 95, page.Width / 2 - 75, 80), XStringFormats.BottomCenter);
            //gfx.DrawString("Date:", new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - ((page.Width / 2) - 180), page.Height - 95, page.Width / 2 - 75, 80), XStringFormats.BottomLeft);


            //////////////////////////////////
            //Header
            if (reportData.CompanyLogo == null) reportData.CompanyLogo = "";
            if (reportData.CompanyName == null) reportData.CompanyName = "";
            if (reportData.CompanyAddress == null) reportData.CompanyAddress = "";
            if (reportData.RefNo == null) reportData.RefNo = "";
            if (reportData.Date == null) reportData.Date = "";
            if (reportData.ExtraRefNo == null) reportData.ExtraRefNo = "";
            if (reportData.ExtraDate == null) reportData.ExtraDate = "";

            //Customer Detail
            if (reportData.RefNo == null) reportData.RefNo = "";
            if (reportData.Date == null) reportData.Date = "";
            if (reportData.FName == null) reportData.FName = "";
            if (reportData.LName == null) reportData.LName = "";
            if (reportData.MName == null) reportData.MName = "";
            if (reportData.Address == null) reportData.Address = "";
            if (reportData.PhoneNo == null) reportData.PhoneNo = "";
            if (reportData.HomeNo == null) reportData.HomeNo = "";
            if (reportData.WorkNo == null) reportData.WorkNo = "";
            if (reportData.Email == null) reportData.Email = "";

            //Tire Storage Detail
            if (reportData.TireSize1 == null) reportData.TireSize1 = "";
            if (reportData.DepthLF == null) reportData.DepthLF = "";
            if (reportData.TireStoredUpto == null) reportData.TireStoredUpto = "";
            if (reportData.TermCondition == null) reportData.TermCondition = "";



            //Vehicle Detail
            if (reportData.PlateNo == null) reportData.PlateNo = "";
            if (reportData.CarYear == null) reportData.CarYear = "";
            if (reportData.CarBrand == null) reportData.CarBrand = "";
            if (reportData.CarModel == null) reportData.CarModel = "";
            if (reportData.CusSignDate == null) reportData.CusSignDate = "";

            if (Application.Current.Properties.ContainsKey("CName"))
            {
                reportData.CompanyName = Application.Current.Properties["CName"].ToString();
            }
            else reportData.CompanyName = "";

            if (Application.Current.Properties.ContainsKey("CAddress"))
            {
                reportData.CompanyAddress = Application.Current.Properties["CAddress"].ToString();
            }
            else reportData.CompanyAddress = "";

            if (Application.Current.Properties.ContainsKey("CTerms"))
            {
                reportData.TermCondition = Application.Current.Properties["CTerms"].ToString();
            }
            else reportData.TermCondition = "";


            lblterms.Text = reportData.TermCondition;
            //////////////////////////////////

            //Header
            if (File.Exists(DependencyService.Get<IFileSave>().GetPicture()))
            {
                reportData.CompanyLogo = DependencyService.Get<IFileSave>().GetPicture();
                gfx.DrawImage(XImage.FromFile(reportData.CompanyLogo), new XRect(10, 30, 50, 50));
            }
            else
            {
                gfx.DrawImage(image, new XRect(10, 30, 50, 50));
            }

            left.DrawString(reportData.ExtraDate, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(85, (page.Height + 110) / 2, (page.Width / 2) - 65, 50), XStringFormats.TopLeft);


            gfx.DrawString(reportData.CompanyName, new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), 70, 30);
            left.DrawString(reportData.CompanyAddress, new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(70, 38, 250, 78), XStringFormats.TopLeft);
            gfx.DrawString(reportData.RefNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 4) - 10, 38, (page.Width / 4), 13), XStringFormats.TopRight);
            gfx.DrawString(reportData.Date, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - (page.Width / 4) - 10, 53, (page.Width / 4), 13), XStringFormats.TopRight);
            gfx.DrawString(reportData.ExtraRefNo, new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - 110, 85, 100, 30), XStringFormats.TopRight);

            //Customer Detail
            //gfx.DrawString(reportData.RefNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 150, (page.Width / 4) - 10, 15), XStringFormats.TopRight);
            //gfx.DrawString(reportData.Date, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 4) + 10, 150, (page.Width / 4) - 20, 15), XStringFormats.TopRight);


            var ssting = reportData.FName + " " + reportData.MName + " " + reportData.LName;

            gfx.DrawString(ssting, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 150, (page.Width / 2) - 20, 15), XStringFormats.TopRight);
            right.DrawString(reportData.Address, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(110, 170, (page.Width / 2) - 120, 50), XStringFormats.TopLeft);
            gfx.DrawString(reportData.PhoneNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 220, (page.Width / 2) - 20, 15), XStringFormats.TopRight);
            gfx.DrawString(reportData.HomeNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 240, (page.Width / 4) - 10, 15), XStringFormats.TopRight);
            gfx.DrawString(reportData.WorkNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 4) + 10, 240, (page.Width / 4) - 20, 15), XStringFormats.TopRight);
            gfx.DrawString(reportData.Email, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(10, 260, (page.Width / 2) - 20, 15), XStringFormats.TopRight);

            //Tire Storage Detail

            switch (reportData.IfStaggered)
            {
                case EIfStag.Yes:
                    reportData.TireSize1 = reportData.TireSize1;
                    reportData.TireSize2 = reportData.TireSize2;
                    reportData.TireSize3 = reportData.TireSize3;
                    reportData.TireSize4 = reportData.TireSize4;
                    break;
                case EIfStag.No:
                    reportData.TireSize1 = reportData.TireSize1;
                    reportData.TireSize2 = "Same";
                    reportData.TireSize3 = "Same";
                    reportData.TireSize4 = "Same";
                    break;
                default:
                    break;
            }

            gfx.DrawString(reportData.TireSize1, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 200);
            //gfx.DrawString(reportData.TireSize2, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 200, 200);


            if (!reportData.TireSize2.Contains("Same"))
            {
                gfx.DrawString("Rear : " + reportData.TireSize2, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 220);
                gfx.DrawString("Front : " + reportData.TireSize3, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 200, 220);

            }

            gfx.DrawString(reportData.DepthLF, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 280);
            gfx.DrawString(reportData.TireStoredUpto, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 110, 320);

            string sst = "";
            //Checked Box Season
            switch (reportData.TireSeason)
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


            var rimtypestr = "";

            if (!string.IsNullOrWhiteSpace(reportData.NoOfTires))
            {
                //Checked Box Rim Attached
                switch (reportData.RimAttached)
                {
                    case ERimAttached.Yes:
                        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 170, 230, 15, 15);

                        //Checked Box Rim Types
                        switch (reportData.TypeRim)
                        {
                            case ERimTypes.Steel:
                                rimtypestr = ERimTypes.Steel.ToString();
                                break;
                            case ERimTypes.Alloy:
                                rimtypestr = ERimTypes.Alloy.ToString();
                                break;
                            case ERimTypes.OEM:
                                rimtypestr = ERimTypes.OEM.ToString();
                                break;
                            case ERimTypes.Other:
                                rimtypestr = ERimTypes.Other.ToString();
                                break;
                            default:

                                break;
                        }

                        break;
                    case ERimAttached.No:
                        gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 260, 230, 15, 15);
                        rimtypestr = "No Rims Attached";
                        break;
                    default:
                        break;
                }

            }

            //gfx.DrawString("No of Tires : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 170, 170, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);
            gfx.DrawString("No of Tires : ", new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 170, (page.Width / 2) - 40, 13), XStringFormats.TopLeft);

            if (!string.IsNullOrWhiteSpace(reportData.NoOfTires))
                gfx.DrawString(sst, new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 150, (page.Width / 2) - 40, 13), XStringFormats.TopRight);
            //gfx.DrawString(reportData.NoOfTires, new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 170, (page.Width / 2) - 40, 13), XStringFormats.TopRight);

            gfx.DrawString(reportData.NoOfTires, new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 180);
            gfx.DrawString(reportData.MakeModel, new XFont(fontName, 13, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), (page.Width / 2) + 100, 300);


            gfx.DrawString(rimtypestr, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect((page.Width / 2) + 10, 250, (page.Width / 2) - 40, 13), XStringFormats.TopRight);
            //Vehicle Detail
            gfx.DrawString(reportData.PlateNo, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 170 + 120) / 2, (page.Width / 2) - 30, 13), XStringFormats.TopRight);
            gfx.DrawString(reportData.CarYear, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 130 + 120) / 2, (page.Width / 2) - 30, 13), XStringFormats.TopRight);
            gfx.DrawString(reportData.CarBrand, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 90 + 120) / 2, (page.Width / 2) - 30, 13), XStringFormats.TopRight);
            gfx.DrawString(reportData.CarModel, new XFont(fontName, 12, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, (page.Height - 50 + 120) / 2, (page.Width / 2) - 30, 13), XStringFormats.TopRight);

            ////Four Images 
            if (reportData.Pic1 != null && !reportData.Pic1.Contains("http://209.127.116.78:8008"))
            {
                lblbusy.Text = "Downloading Image 1";
                var tempPath = Path.Combine(Path.GetTempPath(), "imgdownload1.png");
                await Task.Run(async () =>
                {
                    using (var web = new System.Net.WebClient())
                        await FileDownloader.DownloadFile(reportData.Pic1, tempPath, web);
                });
                if (File.Exists(tempPath))
                    gfx.DrawImage(XImage.FromFile(tempPath), new XRect((page.Width / 2) + 10, (page.Height - 250 + 130) / 2, page.Width / 4 - 20, ((page.Height - 290) - ((page.Height - 130) / 2)) / 2));
            }
            if (reportData.Pic2 != null && !reportData.Pic2.Contains("http://209.127.116.78:8008"))
            {
                lblbusy.Text = "Downloading Image 2";
                var tempPath = Path.Combine(Path.GetTempPath(), "imgdownload2.png");
                await Task.Run(async () =>
                {
                    using (var web = new System.Net.WebClient())
                        await FileDownloader.DownloadFile(reportData.Pic2, tempPath, web);
                });
                if (File.Exists(tempPath))
                    gfx.DrawImage(XImage.FromFile(tempPath), new XRect(page.Width - (page.Width / 4), (page.Height - 250 + 130) / 2, page.Width / 4 - 20, ((page.Height - 290) - ((page.Height - 250 + 120) / 2)) / 2));
            }
            if (reportData.Pic3 != null && !reportData.Pic3.Contains("http://209.127.116.78:8008"))
            {
                lblbusy.Text = "Downloading Image 3";
                var tempPath = Path.Combine(Path.GetTempPath(), "imgdownload3.png");
                await Task.Run(async () =>
                {
                    using (var web = new System.Net.WebClient())
                        await FileDownloader.DownloadFile(reportData.Pic3, tempPath, web);
                });
                if (File.Exists(tempPath))
                    gfx.DrawImage(XImage.FromFile(tempPath), new XRect((page.Width / 2) + 10, ((page.Height - 260) + ((page.Height - 250 + 120) / 2)) / 2, page.Width / 4 - 20, ((page.Height - 290) - ((page.Height - 250 + 120) / 2)) / 2));
            }
            if (reportData.Pic4 != null && !reportData.Pic4.Contains("http://209.127.116.78:8008"))
            {
                lblbusy.Text = "Downloading Image 4";
                var tempPath = Path.Combine(Path.GetTempPath(), "imgdownload4.png");
                await Task.Run(async () =>
                {
                    using (var web = new System.Net.WebClient())
                        await FileDownloader.DownloadFile(reportData.Pic4, tempPath, web);
                });
                if (File.Exists(tempPath))
                    gfx.DrawImage(XImage.FromFile(tempPath), new XRect(page.Width - (page.Width / 4), ((page.Height - 260) + ((page.Height - 250 + 120) / 2)) / 2, page.Width / 4 - 20, ((page.Height - 290) - ((page.Height - 250 + 120) / 2)) / 2));
            }

            if (reportData.CustomerSign != null && !reportData.CustomerSign.Contains("http://209.127.116.78:8008"))
            {
                lblbusy.Text = "Downloading Sign Image";
                var tempPath = Path.Combine(Path.GetTempPath(), "custsign.png");
                await Task.Run(async () =>
                {
                    using (var web = new System.Net.WebClient())
                        await FileDownloader.DownloadFile(reportData.CustomerSign, tempPath, web);
                });
                if (File.Exists(tempPath))
                    pp = tempPath;
                else
                    pp = null;
            }


            if (pp != null)
            {
                gfx.DrawImage(XImage.FromFile(pp), page.Width - ((page.Width / 2) - 75), page.Height - 85, (page.Width / 2) - 100, 55);
            }


            //Term And Condition

            if (Device.RuntimePlatform == Device.Android)
                left.DrawString(reportData.TermCondition, new XFont(fontName, 7, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, page.Height - 240, page.Width - 40, 150), XStringFormats.TopLeft);
            else
                left.DrawString(reportData.TermCondition, new XFont(fontName, 8, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(20, page.Height - 240, page.Width - 40, 150), XStringFormats.TopLeft);



            //Footer
            //gfx.DrawImage(XImage.FromFile(reportData.AuthorizeSign), 10, page.Height - 90, (page.Width / 2) - 75, 65);
            //
            //gfx.DrawString(reportData.CusSignDate, new XFont(fontName, 10, XFontStyle.Regular), new XSolidBrush(XColor.FromArgb(0, 0, 0)), new XRect(page.Width - ((page.Width / 2) - 210), page.Height - 95, page.Width / 2 - 75, 80), XStringFormats.BottomLeft);


            lblbusy.Text = "Successfully Loaded!";




            //////////////////////////
            printfilepath = Path.Combine(systemHelper.GetTemporaryDirectory(), "test.pdf");
            document.Save(printfilepath);




            var customWebView = new PdfView() { VerticalOptions = LayoutOptions.FillAndExpand };

            stk.Children.Add(customWebView);
            customWebView.Uri = printfilepath;
            busy.IsVisible = false;
            busy.IsRunning = false;
            lblbusy.IsVisible = false;
            stkbusy.IsVisible = false;
        }

        void EveSms(object sender, System.EventArgs e)
        {
            var geCommand = (Button)sender;
            geCommand.IsEnabled = false;
            if (printfilepath != null)
            {
                //DependencyService.Get<InterfaceEmail>().SendSMS("Total Tire Inc.");
            }
            geCommand.IsEnabled = true;
        }
        async void EveEmail(object sender, System.EventArgs e)
        {
            var geCommand = (Button)sender;
            geCommand.IsEnabled = false;
            disappeUpdate = false;
            if (printfilepath != null)
            {

                var res = await DependencyService.Get<IConvertToPng>().Convert(printfilepath);
                if (res != null)
                {
                    await DependencyService.Get<IShare>().Show("Total Tire Inc.", "Your Tire Storage Reciept", reportData.Email, reportData.PhoneNo, res);
                }

            }
            disappeUpdate = true;
            geCommand.IsEnabled = true;
        }
        void EvePrint(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            disappeUpdate = false;
            if (printfilepath != null)
            {
                DependencyService.Get<IPrint>().printpdf(printfilepath);
            }

            disappeUpdate = true;
            getCommand.IsEnabled = true;
        }
        void EveBack(object sender, System.EventArgs e)
        {
            var geCommand = (StackLayout)sender;
            geCommand.IsEnabled = false;

            if (busycheck)
            {
                stksign.IsVisible = false;
                stkBack.IsVisible = false;
            }

            geCommand.IsEnabled = true;

        }

        void EveSign(object sender, System.EventArgs e)
        {
            var geCommand = (Button)sender;
            geCommand.IsEnabled = false;

            stkBack.IsVisible = true;
            stksign.IsVisible = true;

            geCommand.IsEnabled = true;

        }

        async void SignDoneEve(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            busycheck = true;
            getCommand.Text = "DONE";

            await GetSignatureImage();
            if (pp != null)
            {
                stk.Children.RemoveAt(0);



                if (pp != null && File.Exists(pp))
                {
                    getCommand.Text = "Saving Sign Image to Server...";
                    var ss = await UploadImage(reportData.RefNo + DateTime.Now.ToString("ssmmmmhhddmmyy") + ".png", pp);
                    if (ss != null)
                    {
                        reportData.CustomerSign = "https://" + ss;
                        reportData.CustomerSign = reportData.CustomerSign.Replace('"', ' ');
                        reportData.CustomerSign = reportData.CustomerSign.Replace(" ", "");

                        var res = await getOnline.PutAsync(reportData.Id, reportData);
                        if (res)
                        {
                            getCommand.Text = "Successfully Saved!";
                        }
                        else
                        {
                            getCommand.Text = "Something Wrong!";
                        }



                    }
                }


                PageRun();

            }
            stksign.IsVisible = false;
            stkBack.IsVisible = false;

            busycheck = false;
            getCommand.IsEnabled = true;

        }

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    //if (disappeUpdate) 
        //    //{
        //    //    MessagingCenter.Send<PrintPage>(this, "PrintUpdate");
        //    //    Task.Run(async()=> 
        //    //    {
        //    //        await Navigation.PopAsync();
        //    //     });
        //    //}

        //}

        public async Task GetSignatureImage()
        {
            byte[] bytedata;
            using (var ms = new System.IO.MemoryStream())
            {
                var streamdata = await signatureView.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);
                if (streamdata != null)
                {
                    streamdata.CopyTo(ms);
                    bytedata = ms.ToArray();

                }
                else bytedata = null;
            }

            if (bytedata != null)
            {
                pp = Path.Combine(Path.GetTempPath(), "sigImage.png");
                File.WriteAllBytes(pp, bytedata);
            }
            else
            {
                pp = null;
            }


        }
        public async Task<string> UploadImage(string filename, string path)
        {
            var ff = File.Open(path, FileMode.Open);

            var content = new MultipartFormDataContent
            {
                { new StreamContent(ff), "\"files\"", filename }
            };

            var httpClient = new HttpClient();
            var uploadservicebasedurl = "http://209.127.116.78:8008/api/UploadPics/";
            var httpresponseMessage = await httpClient.PostAsync(uploadservicebasedurl, content);
            string getname = await httpresponseMessage.Content.ReadAsStringAsync();

            ff.Close();
            ff.Dispose();

            if (getname.Contains("jpg") || getname.Contains("png"))
            {
                return getname;
            }
            else return null;
        }
    }
}
