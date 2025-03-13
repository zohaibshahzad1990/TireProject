using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FFImageLoading.Forms;
using Plugin.Media;
using Xamarin.Forms;

namespace TireProject
{
    public partial class NewDetailPage : ContentPage
    {
        string pp;
        ReportData reportData = new ReportData();
        GetOnlineData getOnline = new GetOnlineData();
        string im1;
        string im2;
        string im3;
        string im4;
        bool post = true;
        bool busycheck = false;
        string warehh = null;
        string enrimtype = null;
        string enstorebtn = null;

        public NewDetailPage()
        {
            InitializeComponent();
            gr.Padding = new Thickness(10, Device.RuntimePlatform == Device.iOS ? App.StatusBarHeight + 10 : 10, 10, 10);
            Task.Run(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    post = true;
                    reportData.TireSeason = ETireSeason.AllSeason;
                    reportData.RimAttached = ERimAttached.Yes;
                    reportData.IfStaggered = EIfStag.No;
                    StkStagHide();
                    StkRimShow();
                    listware.ItemsSource = new List<string> { "HR", "FV", "DC", "OR", "UT" };
                });
            });
        }

        public NewDetailPage(ReportData report, int cloneornot)
        {
            InitializeComponent();
            gr.Padding = new Thickness(0, Device.RuntimePlatform == Device.iOS ? App.StatusBarHeight : 0, 0, 0);

            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (cloneornot == 0)
                    {
                        post = true;
                        //reportData = report;
                        enmi.Text = report.MName;
                        enfirst.Text = report.FName;
                        ensecond.Text = report.LName;
                        enaddress.Text = report.Address;
                        enphone.Text = report.PhoneNo;
                        enhome.Text = report.HomeNo;
                        enwork.Text = report.WorkNo;
                        enemail.Text = report.Email;
                        if (report.ExtraRefNo.StartsWith("HR", StringComparison.Ordinal))
                        {
                            var ssd = report.ExtraRefNo;
                            ssd = ssd.Replace("HR", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "HR";
                            enstorageloc.Text = ssd;
                            warehh = "HR";
                        }
                        else if (report.ExtraRefNo.StartsWith("FV", StringComparison.Ordinal))
                        {
                            var ssd = report.ExtraRefNo;
                            ssd = ssd.Replace("FV", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "FV";
                            enstorageloc.Text = ssd;
                            warehh = "FV";
                        }
                        else if (report.ExtraRefNo.StartsWith("DC", StringComparison.Ordinal))
                        {
                            var ssd = report.ExtraRefNo;
                            ssd = ssd.Replace("DC", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "DC";
                            enstorageloc.Text = ssd;
                            warehh = "DC";
                        }
                        else if (report.ExtraRefNo.StartsWith("OR", StringComparison.Ordinal))
                        {
                            var ssd = report.ExtraRefNo;
                            ssd = ssd.Replace("OR", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "OR";
                            enstorageloc.Text = ssd;
                            warehh = "OR";
                        }
                        else if (report.ExtraRefNo.StartsWith("UT", StringComparison.Ordinal))
                        {
                            var ssd = report.ExtraRefNo;
                            ssd = ssd.Replace("UT", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "UT";
                            enstorageloc.Text = ssd;
                            warehh = "UT";
                        }
                        else
                        {
                        }
                        enplateno.Text = report.PlateNo;
                        encartyear.Text = report.CarYear;
                        encarbrand.Text = report.CarBrand;
                        encarmodel.Text = report.CarModel;

                        reportData.TireSeason = ETireSeason.AllSeason;
                        reportData.RimAttached = ERimAttached.Yes;
                        reportData.IfStaggered = EIfStag.No;
                        StkStagHide();
                        StkRimShow();
                        listware.ItemsSource = new List<string> { "HR", "FV", "DC", "OR", "UT" };
                    }
                    else
                    {
                        post = false;
                        reportData = report;
                        endate.Date = DateTime.Parse(reportData.Date);
                        enrefno.Text = reportData.RefNo;
                        enmi.Text = reportData.MName;
                        enfirst.Text = reportData.FName;
                        ensecond.Text = reportData.LName;
                        enaddress.Text = reportData.Address;
                        enphone.Text = reportData.PhoneNo;
                        enhome.Text = reportData.HomeNo;
                        enwork.Text = reportData.WorkNo;
                        enemail.Text = reportData.Email;
                        if (report.ExtraRefNo.StartsWith("HR", StringComparison.Ordinal))
                        {
                            var ssd = reportData.ExtraRefNo;
                            ssd = ssd.Replace("HR", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "HR";
                            enstorageloc.Text = ssd;
                            warehh = "HR";
                        }
                        else if (report.ExtraRefNo.StartsWith("FV", StringComparison.Ordinal))
                        {
                            var ssd = reportData.ExtraRefNo;
                            ssd = ssd.Replace("FV", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "FV";
                            enstorageloc.Text = ssd;
                            warehh = "FV";
                        }
                        else if (report.ExtraRefNo.StartsWith("DC", StringComparison.Ordinal))
                        {
                            var ssd = reportData.ExtraRefNo;
                            ssd = ssd.Replace("DC", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "DC";
                            enstorageloc.Text = ssd;
                            warehh = "DC";
                        }
                        else if (report.ExtraRefNo.StartsWith("OR", StringComparison.Ordinal))
                        {
                            var ssd = reportData.ExtraRefNo;
                            ssd = ssd.Replace("OR", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "OR";
                            enstorageloc.Text = ssd;
                            warehh = "OR";
                        }
                        else if (report.ExtraRefNo.StartsWith("UT", StringComparison.Ordinal))
                        {
                            var ssd = reportData.ExtraRefNo;
                            ssd = ssd.Replace("UT", "");
                            ssd = ssd.Trim();
                            enwarehouse.Text = "UT";
                            enstorageloc.Text = ssd;
                            warehh = "UT";
                        }
                        else
                        {
                        }
                        enremark.Text = reportData.ExtraDate;
                        enrep.Text = reportData.REP;
                        enstorebtn = reportData.TireStoredUpto;
                        if (!string.IsNullOrWhiteSpace(reportData.TireStoredUpto))
                        {
                            if (reportData.TireStoredUpto.ToUpper() == "NEXT SUMMER")
                            {
                                btseason1.TextColor = Color.White;
                                btseason1.BackgroundColor = Color.FromHex("#e74c3c");
                                btseason2.TextColor = Color.FromHex("#e74c3c");
                                btseason2.BackgroundColor = Color.White;
                            }
                            else
                            {
                                btseason2.BackgroundColor = Color.White;
                                btseason2.TextColor = Color.FromHex("#e74c3c");
                                btseason1.TextColor = Color.FromHex("#e74c3c");
                                btseason1.BackgroundColor = Color.White;
                            }
                        }
                        switch (reportData.TireSeason)
                        {
                            case ETireSeason.AllSeason:
                                rad.SelectedIndex = 0;
                                break;
                            case ETireSeason.Summer:
                                rad.SelectedIndex = 1;
                                break;
                            case ETireSeason.Winter:
                                rad.SelectedIndex = 2;
                                break;
                            case ETireSeason.Other:
                                rad.SelectedIndex = 3;
                                break;
                            default:
                                break;
                        }
                        switch (reportData.RimAttached)
                        {
                            case ERimAttached.Yes:
                                radrimatt.SelectedIndex = 0;
                                StkRimShow();
                                break;
                            case ERimAttached.No:
                                radrimatt.SelectedIndex = 1;
                                StkRimHide();
                                break;
                            default:
                                break;
                        }
                        switch (reportData.IfStaggered)
                        {
                            case EIfStag.Yes:
                                radstag.SelectedIndex = 0;
                                StkStagShow();
                                entiresize11.Text = "";
                                entiresize12.Text = "";
                                entiresize13.Text = "";

                                break;
                            case EIfStag.No:
                                radstag.SelectedIndex = 1;
                                StkStagHide();
                                break;
                            default:
                                break;
                        }
                        switch (reportData.DepthLF)
                        {
                            case "10%-30%":
                                b4.BorderColor = Color.Black;
                                break;
                            case "30%-50%":
                                b3.BorderColor = Color.Black;
                                break;
                            case "50%-70%":
                                b2.BorderColor = Color.Black;
                                break;
                            case "70%-80%":
                                b1.BorderColor = Color.Black;
                                break;
                            default:
                                break;
                        }
                        switch (reportData.TypeRim.ToString())
                        {
                            case "STEEL":
                                break;
                            case "ALLOY":
                                break;
                            case "OEM":
                                break;
                            case "OTHER":
                                break;
                            default:
                                break;
                        }
                        enrimtype = reportData.TypeRim.ToString();
                        foreach (var item in grbtnsave.Children)
                        {
                            var itemrr = item as Button;
                            if (itemrr.Text.ToUpper() == enrimtype.ToUpper())
                            {
                                itemrr.TextColor = Color.White;
                                itemrr.BackgroundColor = Color.FromHex("#e74c3c");
                            }
                            else
                            {
                                itemrr.BackgroundColor = Color.White;
                                itemrr.TextColor = Color.FromHex("#e74c3c");
                            }
                        }
                        ennumtyre.Text = reportData.NoOfTires;

                        if (!string.IsNullOrWhiteSpace(reportData.TireSize1))
                        {
                            var ss = reportData.TireSize1.Split('-');
                            if (!string.IsNullOrWhiteSpace(ss[0]))
                                entiresize11.Text = ss[0];
                            if (ss.Length > 1)
                                entiresize12.Text = ss[1];
                            if (ss.Length > 2)
                                entiresize13.Text = ss[2];
                        }
                        if (!string.IsNullOrWhiteSpace(reportData.TireSize2))
                        {
                            var ss = reportData.TireSize2.Split('-');
                            if (!string.IsNullOrWhiteSpace(ss[0]))
                                entiresize21.Text = ss[0];
                            if (ss.Length > 1)
                                entiresize22.Text = ss[1];
                            if (ss.Length > 2)
                                entiresize23.Text = ss[2];
                        }
                        if (!string.IsNullOrWhiteSpace(reportData.TireSize3))
                        {
                            var ss = reportData.TireSize3.Split('-');
                            if (!string.IsNullOrWhiteSpace(ss[0]))
                                entiresize31.Text = ss[0];
                            if (ss.Length > 1)
                                entiresize32.Text = ss[1];
                            if (ss.Length > 2)
                                entiresize33.Text = ss[2];
                        }
                        entiremake.Text = reportData.MakeModel;
                        reportData.DepthLF = reportData.DepthLF;
                        enplateno.Text = reportData.PlateNo;
                        encartyear.Text = reportData.CarYear;
                        encarbrand.Text = reportData.CarBrand;
                        encarmodel.Text = reportData.CarModel;
                        if (reportData.Pic1 != null)
                        {
                            var imgchild = new CachedImage();
                            imgchild.Source = reportData.Pic1;
                            imgchild.HeightRequest = 100;
                            stkimage.Children.Add(imgchild);
                            im1 = reportData.Pic1;
                        }
                        if (reportData.Pic2 != null)
                        {
                            var imgchild = new CachedImage();
                            imgchild.Source = reportData.Pic2;
                            imgchild.HeightRequest = 100;
                            stkimage.Children.Add(imgchild);
                            im2 = reportData.Pic2;
                        }
                        if (reportData.Pic3 != null)
                        {
                            var imgchild = new CachedImage();
                            imgchild.Source = reportData.Pic3;
                            imgchild.HeightRequest = 100;
                            stkimage.Children.Add(imgchild);
                            im3 = reportData.Pic3;
                        }
                        if (reportData.Pic4 != null)
                        {
                            var imgchild = new CachedImage();
                            imgchild.Source = reportData.Pic4;
                            imgchild.HeightRequest = 100;
                            stkimage.Children.Add(imgchild);
                            im4 = reportData.Pic4;
                        }
                        listware.ItemsSource = new List<string> { "HR", "FV", "DC", "OR", "UT" };
                    }

                });
            });
        }

        void EveBack(object sender, System.EventArgs e)
        {
            if (busycheck)
            {
                stkBack.IsVisible = false;
            }
        }
        void EveBtnRimType(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            foreach (var item in grbtnsave.Children)
            {
                var itemrr = item as Button;
                itemrr.BackgroundColor = Color.White;
                itemrr.TextColor = Color.FromHex("#e74c3c");
            }
            getcommand.TextColor = Color.White;
            getcommand.BackgroundColor = Color.FromHex("#e74c3c");
            enrimtype = getcommand.Text;
            switch (enrimtype)
            {
                case "ALLOY":
                    reportData.TypeRim = ERimTypes.Alloy;
                    break;
                case "OEM":
                    reportData.TypeRim = ERimTypes.OEM;
                    break;
                case "OTHER":
                    reportData.TypeRim = ERimTypes.Other;
                    break;
                case "STEEL":
                    reportData.TypeRim = ERimTypes.Steel;
                    break;
                default:
                    break;
            }

            getcommand.IsEnabled = true;
        }

        void EveBtnSeason(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;

            foreach (var item in sttt1.Children)
            {
                var itemrr = item as Button;
                itemrr.BackgroundColor = Color.White;
                itemrr.TextColor = Color.FromHex("#e74c3c");
            }
            getcommand.TextColor = Color.White;
            getcommand.BackgroundColor = Color.FromHex("#e74c3c");
            enstorebtn = getcommand.Text;
            reportData.TireStoredUpto = enstorebtn;
            getcommand.IsEnabled = true;
        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            stkBack.IsVisible = true;
        }
        void EveButtonNext(object sender, System.EventArgs e)
        {
            btnNext.IsEnabled = false;

            if (frpersonal.IsVisible)
            {
                var sd = NullConditionPersonal();
                if (sd)
                {
                    btnDone.IsVisible = false;
                    frpersonal.IsVisible = false;
                    frstoragedetail.IsVisible = false;
                    frtiredetail.IsVisible = true;
                    frvehicledetail.IsVisible = false;
                    frphotos.IsVisible = false;
                    btnNext.IsVisible = false;
                    tooitem.Text = "NEXT";
                }
            }
            btnNext.IsEnabled = true;
        }
        async void EveButtonDone(object sender, System.EventArgs e)
        {
            btnDone.IsEnabled = false;
            frdeleterec.IsVisible = true;
            btnDone.IsEnabled = true;
        }

        public async Task<string> UploadImage(string filename, string path)
        {
            var ff = File.Open(path, FileMode.Open);

            var content = new MultipartFormDataContent
            {
                { new StreamContent(ff), "\"files\"", filename }
            };


            var httpClient = new HttpClient();
            var uploadservicebasedurl = "http://3.133.136.76/api/UploadPics/";
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
        void Handle_Clicked_1(object sender, System.EventArgs e)
        {
            stkBack.IsVisible = true;
        }
        void Handle_Clicked_2(object sender, System.EventArgs e)
        {
            stkBack.IsVisible = true;
        }
        async void Handle_Clicked_4(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;

            if (stkimage.Children.Count >= 4)
            {
                lblpopmsg.Text = "You can only take 4 Pictures!";
                frmsg.IsVisible = true;
                await Task.Delay(2500);
                frmsg.IsVisible = false;
                getCommand.IsEnabled = true;
                return;
            }
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                DisplayAlert("No Camera", ":( No camera available.", "OK");
                getCommand.IsEnabled = true;
                return;
            }

            getCommand.IsEnabled = true;
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Custom,
                CustomPhotoSize = 30,
                Directory = "Sample",
                Name = "test.png"
            });
            if (file == null)
            {
                getCommand.IsEnabled = true;
                return;
            }
            string sfilename = null;
            if (im1 == null) sfilename = "tempres1.png";
            else if (im2 == null) sfilename = "tempres2.png";
            else if (im3 == null) sfilename = "tempres3.png";
            else sfilename = "tempres4.png";
            var editImage = await DependencyService.Get<IImageResize>().ResizeImage(file.Path, sfilename, 400, 400);
            var imgchild = new Image { HeightRequest = 100, Source = editImage };
            stkimage.Children.Add(imgchild);
            if (im1 == null) im1 = editImage;
            else if (im2 == null) im2 = editImage;
            else if (im3 == null) im3 = editImage;
            else im4 = editImage;
            getCommand.IsEnabled = true;
        }

        void Handle_Clicked_8(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            if (stkimage.Children.Count > 0)
            {
                stkimage.Children.RemoveAt(stkimage.Children.Count - 1);
                frdelete.IsVisible = false;
                if (im4 != null)
                { im4 = null; reportData.Pic4 = null; }
                else if (im3 != null) { im3 = null; reportData.Pic3 = null; }
                else if (im2 != null) { im2 = null; reportData.Pic2 = null; }
                else { im1 = null; reportData.Pic1 = null; }
            }
            else frdelete.IsVisible = false;
            getCommand.IsEnabled = true;
        }
        void Handle_Clicked_9(object sender, System.EventArgs e)
        {
            frdelete.IsVisible = false;
        }
        void Handle_Clicked_10(object sender, System.EventArgs e)
        {
            if (stkimage.Children.Count > 0)
            {
                frdelete.IsVisible = true;
            }
        }
        void Handle_SelectedItemChanged_1(object sender, System.EventArgs e)
        {
            switch (radstag.SelectedIndex)
            {
                case 0:
                    reportData.IfStaggered = EIfStag.Yes;
                    StkStagShow();
                    entiresize11.IsEnabled = false;
                    entiresize12.IsEnabled = false;
                    entiresize13.IsEnabled = false;

                    break;
                case 1:
                    reportData.IfStaggered = EIfStag.No;
                    StkStagHide();
                    entiresize11.IsEnabled = true;
                    entiresize12.IsEnabled = true;
                    entiresize13.IsEnabled = true;

                    break;
                default:
                    break;
            }
        }
        void Handle_SelectedItemChanged_2(object sender, System.EventArgs e)
        {
            switch (radrimatt.SelectedIndex)
            {
                case 0:
                    reportData.RimAttached = ERimAttached.Yes;
                    StkRimShow();
                    break;
                case 1:
                    reportData.RimAttached = ERimAttached.No;
                    StkRimHide();
                    break;
                default:
                    break;
            }
        }
        async void Handle_Clicked_11(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;
            if (frphotos.IsVisible)
            {
                btnDone.IsVisible = false;
                frpersonal.IsVisible = false;
                frstoragedetail.IsVisible = false;
                frtiredetail.IsVisible = true;
                frvehicledetail.IsVisible = false;
                frphotos.IsVisible = false;
                btnNext.IsVisible = false;
                tooitem.Text = "NEXT";
            }
            else if (frtiredetail.IsVisible)
            {
                btnDone.IsVisible = false;
                frvehicledetail.IsVisible = true;
                frphotos.IsVisible = false;
                frpersonal.IsVisible = true;
                frstoragedetail.IsVisible = true;
                frtiredetail.IsVisible = false;
                btnNext.IsVisible = true;
                tooitem.Text = "SAVE";
            }
            else
            {
                await Navigation.PopAsync();
            }
            getCommand.IsEnabled = true;
        }

        protected override bool OnBackButtonPressed()
        {
            if (frphotos.IsVisible)
            {
                btnDone.IsVisible = false;
                frpersonal.IsVisible = false;
                frstoragedetail.IsVisible = false;
                frtiredetail.IsVisible = true;
                frvehicledetail.IsVisible = false;
                frphotos.IsVisible = false;
                btnNext.IsVisible = true;
                tooitem.Text = "NEXT";
                return true;

            }
            else if (frtiredetail.IsVisible)
            {
                btnDone.IsVisible = false;
                frvehicledetail.IsVisible = true;
                frphotos.IsVisible = false;
                frpersonal.IsVisible = true;
                frstoragedetail.IsVisible = true;
                frtiredetail.IsVisible = false;
                btnNext.IsVisible = true;
                tooitem.Text = "SAVE";
                return true;
            }
            else
            {
                return false;
            }
        }
        async void Handle_Tapped(object sender, System.EventArgs e)
        {
            var getCommand = (Label)sender;
            getCommand.IsEnabled = false;

            if (tooitem.Text == "SAVE")
            {
                if (frphotos.IsVisible)
                {
                    var sd1 = NullConditionPersonal();
                    var sd2 = NullConditionDetail();
                    var sd3 = NullConditionPhoto();
                    if (sd1 && sd2 && sd3)
                    {
                        busycheck = true;
                        stkBack.IsVisible = true;
                        var reportDataAfterSave = await SaveData();
                        //Check
                        frmsg.IsVisible = true;
                        lblpopmsg.Text = "Getting Data For Printing...";
                        if (getOnline.DataList == null)
                        {
                            getOnline.DataList = new List<ReportData>();
                        }
                        var oldRecord = getOnline.DataList.Where(x => x.Id == reportDataAfterSave.Id).FirstOrDefault();
                        if (oldRecord != null)
                        {
                            getOnline.DataList.RemoveAll(x => x.Id == reportDataAfterSave.Id);
                        }
                        getOnline.DataList.Add(reportDataAfterSave);

                        reportData = reportDataAfterSave;
                        //Check
                        stkBack.IsVisible = false;
                        busycheck = false;
                        frmsg.IsVisible = false;
                        //Navigation.InsertPageBefore(new NewPrintPage(reportData), this);
                        //await Navigation.PopAsync();
                        ShowPrint();
                    }
                }
                else if (frpersonal.IsVisible)
                {
                    var sd = NullConditionPersonal();
                    if (sd)
                    {
                        busycheck = true;
                        stkBack.IsVisible = true;

                        var reportDataAfterSave = await SaveData();
                        //Check
                        frmsg.IsVisible = true;
                        lblpopmsg.Text = "Getting Data For Printing...";
                        if (getOnline.DataList == null)
                        {
                            getOnline.DataList = new List<ReportData>();
                        }
                        var oldRecord = getOnline.DataList.Where(x => x.Id == reportDataAfterSave.Id).FirstOrDefault();
                        if (oldRecord != null)
                        {
                            getOnline.DataList.RemoveAll(x => x.Id == reportDataAfterSave.Id);
                        }
                        getOnline.DataList.Add(reportDataAfterSave);

                        reportData = reportDataAfterSave;
                        //Check

                        ShowPrint();
                        frmsg.IsVisible = false;
                        stkBack.IsVisible = false;
                        busycheck = false;
                    }
                }
                else
                {
                }
            }
            else if (tooitem.Text == "NEXT")
            {
                var sd2 = NullConditionDetail();
                if (sd2)
                {
                    if (frtiredetail.IsVisible)
                    {
                        btnDone.IsVisible = true;
                        frvehicledetail.IsVisible = false;
                        frpersonal.IsVisible = false;
                        frstoragedetail.IsVisible = false;
                        frtiredetail.IsVisible = false;
                        frphotos.IsVisible = true;
                        tooitem.Text = "SAVE";
                    }
                }
            }
            else
            {
            }
            getCommand.IsEnabled = true;
        }

        async void Handle_Clicked_12(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            var httpClient = new HttpClient();
            var response = await httpClient.DeleteAsync("http://3.133.136.76/api/mains/" + reportData.Id);
            if (response.IsSuccessStatusCode)
            {
                lblpopmsg.Text = "Success";
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            getCommand.IsEnabled = true;
        }

        void Handle_Clicked_13(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            frdeleterec.IsVisible = false;
            getCommand.IsEnabled = true;
        }
        bool NullConditionDetail()
        {
            if (string.IsNullOrWhiteSpace(ennumtyre.Text.Trim())) { NullMsg("No of Tire Should not be Empty!"); return false; }
            else if (reportData.IfStaggered == EIfStag.No && (string.IsNullOrWhiteSpace(entiresize11.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize12.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize13.Text.Trim()))) { NullMsg("Tire Size Should not be Empty!"); return false; }
            else if (reportData.IfStaggered == EIfStag.Yes && (string.IsNullOrWhiteSpace(entiresize21.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize22.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize23.Text.Trim()))) { NullMsg("Front Tire Size Should not be Empty!"); return false; }
            else if (reportData.IfStaggered == EIfStag.Yes && (string.IsNullOrWhiteSpace(entiresize31.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize32.Text.Trim()) || string.IsNullOrWhiteSpace(entiresize33.Text.Trim()))) { NullMsg("Rear Tire Size Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(entiremake.Text.Trim())) { NullMsg("Tire Brand Should not be Empty!"); return false; }
            else if (string.IsNullOrEmpty(enrimtype) && reportData.RimAttached == ERimAttached.Yes)
            {
                NullMsg("Type of Rim Should be Selected!"); return false;
            }
            else if (string.IsNullOrWhiteSpace(enstorebtn))
            {
                NullMsg("Tire Store Upto Should be Selected!"); return false;
            }
            else if (string.IsNullOrWhiteSpace(reportData.DepthLF)) { NullMsg("Tire Depth Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(enplateno.Text.Trim())) { NullMsg("Vehicle Plate Number Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(enrep.Text.Trim())) { NullMsg("REP Should not be Empty!"); return false; }
            else
            {
                return true;
            }
        }
        bool NullConditionPhoto()
        {
            if (im1 == null) { NullMsg("Select Atleast One Picture!"); return false; }
            else
            {
                return true;
            }
        }
        bool NullConditionPersonal()
        {
            if (string.IsNullOrWhiteSpace(enfirst.Text.Trim())) { NullMsg("First Name Should not be Empty!"); return false; }
            //else if (string.IsNullOrWhiteSpace(ensecond.Text.Trim())) { NullMsg("Second Name Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(enphone.Text.Trim())) { NullMsg("Phone Number Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(enplateno.Text.Trim())) { NullMsg("Plate Number Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(encarbrand.Text.Trim())) { NullMsg("Car Make Should not be Empty!"); return false; }
            else if (string.IsNullOrWhiteSpace(encarmodel.Text.Trim())) { NullMsg("Car Model Should not be Empty!"); return false; }
            else if (!string.IsNullOrWhiteSpace(enstorageloc.Text.Trim()) && enwarehouse.Text.Equals("Warehouse")) { NullMsg("Select Warehouse Location!"); return false; }
            else
            {
                return true;
            }
        }
        async Task<ReportData> SaveData()
        {
            frmsg.IsVisible = true;
            reportData.MName = enmi.Text.Trim();
            reportData.FName = enfirst.Text.Trim();
            reportData.LName = ensecond.Text.Trim();
            reportData.Address = enaddress.Text.Trim();
            reportData.PhoneNo = enphone.Text.Trim();
            reportData.HomeNo = enhome.Text.Trim();
            reportData.WorkNo = enwork.Text.Trim();
            reportData.Email = enemail.Text.Trim();
            reportData.NoOfTires = ennumtyre.Text.Trim();
            if (!string.IsNullOrWhiteSpace(warehh))
                reportData.ExtraRefNo = warehh + " " + enstorageloc.Text.Trim();
            else
                reportData.ExtraRefNo = "";
            reportData.TireSize1 = entiresize11.Text.Trim() + "-" + entiresize12.Text.Trim() + "-" + entiresize13.Text.Trim();
            reportData.TireSize2 = entiresize21.Text.Trim() + "-" + entiresize22.Text.Trim() + "-" + entiresize23.Text.Trim();
            reportData.TireSize3 = entiresize31.Text.Trim() + "-" + entiresize32.Text.Trim() + "-" + entiresize33.Text.Trim();
            reportData.MakeModel = entiremake.Text.Trim();
            reportData.DepthLF = reportData.DepthLF;
            reportData.PlateNo = enplateno.Text.Trim();
            reportData.CarBrand = encarbrand.Text.Trim();
            reportData.CarYear = encartyear.Text.Trim();
            reportData.CarModel = encarmodel.Text.Trim();
            reportData.REP = enrep.Text.Trim();
            if (!string.IsNullOrWhiteSpace(enremark.Text))
                reportData.ExtraDate = enremark.Text.Trim();
            reportData.Date = endate.Date.ToString("yyyy-MM-dd");
            reportData.TireStoredUpto = reportData.TireStoredUpto;
            ReportData reportDateAfterSave = null;
            if (im1 != null)
            {
                if (!im1.Contains("http"))
                {
                    lblpopmsg.Text = "Uploading Tyre Image 1";
                    var ss = await UploadImage(reportData.RefNo + DateTime.Now.ToString("ssmmmmhhddmmyy") + ".png", im1);
                    if (ss != null)
                    {
                        reportData.Pic1 = "http://" + ss;
                        reportData.Pic1 = reportData.Pic1.Replace('"', ' ');
                        reportData.Pic1 = reportData.Pic1.Replace(" ", "");
                    }
                    else
                    {
                        reportData.Pic1 = null;
                    }
                }
            }
            if (im2 != null)
            {
                if (!im2.Contains("http"))
                {
                    lblpopmsg.Text = "Uploading Tyre Image 2";
                    var ss = await UploadImage(reportData.RefNo + DateTime.Now.ToString("ssmmmmhhddmmyy") + ".png", im2);
                    if (ss != null)
                    {
                        reportData.Pic2 = "http://" + ss;
                        reportData.Pic2 = reportData.Pic2.Replace('"', ' ');
                        reportData.Pic2 = reportData.Pic2.Replace(" ", "");
                    }
                    else
                    {
                        reportData.Pic2 = null;
                    }
                }
            }
            if (im3 != null)
            {
                if (!im3.Contains("http"))
                {
                    lblpopmsg.Text = "Uploading Tyre Image 3";
                    var ss = await UploadImage(reportData.RefNo + DateTime.Now.ToString("ssmmmmhhddmmyy") + ".png", im3);
                    if (ss != null)
                    {
                        reportData.Pic3 = "http://" + ss;
                        reportData.Pic3 = reportData.Pic3.Replace('"', ' ');
                        reportData.Pic3 = reportData.Pic3.Replace(" ", "");
                    }
                    else
                    {
                        reportData.Pic3 = null;
                    }
                }
            }
            if (im4 != null)
            {
                if (!im4.Contains("http"))
                {
                    lblpopmsg.Text = "Uploading Tyre Image 4";
                    var ss = await UploadImage(reportData.RefNo + DateTime.Now.ToString("ssmmmmhhddmmyy") + ".png", im4);
                    if (ss != null)
                    {
                        reportData.Pic4 = "http://" + ss;
                        reportData.Pic4 = reportData.Pic4.Replace('"', ' ');
                        reportData.Pic4 = reportData.Pic4.Replace(" ", "");
                    }
                    else
                    {
                        reportData.Pic4 = null;
                    }
                }
            }
            lblpopmsg.Text = "Saving Data...";
            if (!post)
            {
                var res = await getOnline.PutAsync(reportData.Id, reportData);
                if (res)
                {
                    reportDateAfterSave = await getOnline.GetAsync(reportData.Id);
                    if (reportDateAfterSave != null)
                    {
                        lblpopmsg.Text = "Success";


                        frmsg.IsVisible = false;
                    }
                    //Application.Current.MainPage = new NavigationPage(new MainPage());
                    //return;
                }
                else
                {
                    lblpopmsg.Text = "Something Wrong!";
                }
            }
            else
            {
                reportDateAfterSave = await getOnline.PostAsync(reportData);
                if (reportDateAfterSave != null)
                {
                    lblpopmsg.Text = "Success";
                    //Application.Current.MainPage = new NavigationPage(new MainPage());
                    //return;
                }
                else
                {
                    lblpopmsg.Text = "Something Wrong!";
                }
            }
            frmsg.IsVisible = false;
            return reportDateAfterSave;
        }
        void NullMsg(string msg)
        {
            Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    lblpopmsg.Text = msg;
                    frmsg.IsVisible = true;
                    await Task.Delay(5000);
                    frmsg.IsVisible = false;
                });
            });

        }
        void StkStagShow()
        {
            if (!stkstag.IsVisible)
            {
                stkstag.Scale = 0;
                stkstag.IsVisible = true;
                stkstag.ScaleTo(1, 100, Easing.Linear);


                stkstagtire.Scale = 1;
                stkstagtire.ScaleTo(0, 100, Easing.Linear);
                stkstagtire.IsVisible = false;
            }
        }
        void StkStagHide()
        {
            if (stkstag.IsVisible)
            {
                stkstag.Scale = 1;
                stkstag.ScaleTo(0, 100, Easing.Linear);
                stkstag.IsVisible = false;

                stkstagtire.Scale = 0;
                stkstagtire.IsVisible = true;
                stkstagtire.ScaleTo(1, 100, Easing.Linear);
            }
        }
        void StkRimShow()
        {
            if (!stkrimst.IsVisible)
            {
                stkrimst.Scale = 0;
                stkrimst.IsVisible = true;
                stkrimst.ScaleTo(1, 100, Easing.Linear);
            }
        }
        void StkRimHide()
        {
            if (stkrimst.IsVisible)
            {
                stkrimst.Scale = 1;
                stkrimst.ScaleTo(0, 100, Easing.Linear);
                stkrimst.IsVisible = false;
            }
        }
        void Handle_Clicked_14(object sender, System.EventArgs e)
        {
            b1.IsEnabled = false;
            b1.BorderColor = Color.FromHex("#92D050");
            b2.BorderColor = Color.FromHex("#FFFF02");
            b3.BorderColor = Color.FromHex("#FDC000");
            b4.BorderColor = Color.FromHex("#FA1100");

            b1.BorderColor = Color.Black;
            reportData.DepthLF = "70%-80%";
            b1.IsEnabled = true;
        }
        void Handle_Clicked_15(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            b1.BorderColor = Color.FromHex("#92D050");
            b2.BorderColor = Color.FromHex("#FFFF02");
            b3.BorderColor = Color.FromHex("#FDC000");
            b4.BorderColor = Color.FromHex("#FA1100");

            b2.BorderColor = Color.Black;
            reportData.DepthLF = "50%-70%";
            getCommand.IsEnabled = true;
        }
        void Handle_Clicked_16(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            b1.BorderColor = Color.FromHex("#92D050");
            b2.BorderColor = Color.FromHex("#FFFF02");
            b3.BorderColor = Color.FromHex("#FDC000");
            b4.BorderColor = Color.FromHex("#FA1100");
            b3.BorderColor = Color.Black;
            reportData.DepthLF = "30%-50%";
            getCommand.IsEnabled = true;
        }
        void Handle_Clicked_17(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            b1.BorderColor = Color.FromHex("#92D050");
            b2.BorderColor = Color.FromHex("#FFFF02");
            b3.BorderColor = Color.FromHex("#FDC000");
            b4.BorderColor = Color.FromHex("#FA1100");
            b4.BorderColor = Color.Black;
            reportData.DepthLF = "10%-30%";

            getCommand.IsEnabled = true;
        }
        void Handle_Clicked_18(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            var eTireSeason = (string)getCommand.BindingContext;
            warehh = eTireSeason;
            enwarehouse.Text = eTireSeason;
            selware.IsVisible = false;
            stkBack.IsVisible = false;
            getCommand.IsEnabled = true;
        }
        void Handle_Clicked_19(object sender, System.EventArgs e)
        {
            stkBack.IsVisible = true;
            selware.IsVisible = true;
        }
        void Tire11(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize11.Text.Length == 3)
                entiresize12.Focus();
        }
        void Tire12(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize12.Text.Length == 2)
                entiresize13.Focus();
        }
        void Tire13(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize13.Text.Length == 2)
                entiresize13.Unfocus();
        }
        void Tire21(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize21.Text.Length == 3)
                entiresize22.Focus();
        }
        void Tire22(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize22.Text.Length == 2)
                entiresize23.Focus();
        }
        void Tire23(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize23.Text.Length == 2)
                entiresize31.Focus();
        }
        void Tire31(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize31.Text.Length == 3)
                entiresize32.Focus();
        }
        void Tire32(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize32.Text.Length == 2)
                entiresize33.Focus();
        }
        void Tire33(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            if (entiresize33.Text.Length == 2)
                entiremake.Focus();
        }

        void Handle_SelectedItemChanged(object sender, System.EventArgs e)
        {
            switch (rad.SelectedIndex)
            {
                case 0:
                    reportData.TireSeason = ETireSeason.AllSeason;
                    break;
                case 1:
                    reportData.TireSeason = ETireSeason.Summer;
                    break;
                case 2:
                    reportData.TireSeason = ETireSeason.Winter;
                    break;
                case 3:
                    reportData.TireSeason = ETireSeason.Other;
                    break;
                default:
                    break;
            }

        }

        async void EveMainPage(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            await Navigation.PopAsync();
            HidePrint();
            getCommand.IsEnabled = true;
        }
        async void EvePrintA4(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            Navigation.InsertPageBefore(new PrintTirelabelPage(reportData), this);
            await Navigation.PopAsync();
            HidePrint();
            getCommand.IsEnabled = true;
        }
        async void EvePrintA6(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            //await Navigation.PushAsync(new PrintTirelabelPage(report));
            //await Navigation.PushAsync(new PrintTirelabelPageA6(reportData));
            Navigation.InsertPageBefore(new PrintTirelabelPageA6(reportData), this);
            await Navigation.PopAsync();
            //var action= await DisplayActionSheet((("Select Page Size", "A4", "A6");
            HidePrint();
            getCommand.IsEnabled = true;
        }

        async void EvePrintPdf(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            //await Navigation.PushAsync(new PrintTirelabelPage(report));
            //await Navigation.PushAsync(new PrintTirelabelPageA6(reportData));
            Navigation.InsertPageBefore(new NewPrintPage(reportData), this);
            await Navigation.PopAsync();
            //var action= await DisplayActionSheet((("Select Page Size", "A4", "A6");
            HidePrint();
            getCommand.IsEnabled = true;
        }



        async void ShowPrint()
        {
            printtype.IsVisible = true;
            await printtype.ScaleTo(1, 150);
        }
        async void HidePrint()
        {
            await printtype.ScaleTo(0, 150);
            printtype.IsVisible = false;
        }
    }
}