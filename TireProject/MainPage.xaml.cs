using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MongoDB.Driver.GeoJsonObjectModel;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TireProject
{
    public partial class MainPage : ContentPage
    {
        //List Properties
        GetOnlineData post = new GetOnlineData();
        ObservableCollection<ReportData> blogpost = new ObservableCollection<ReportData>();
        int totallistcount = 0;
        int getlistcount = 0;
        public int start = 1;
        public int end = 20;
        public bool reachedend = false;
        //*List Properties
        int selectedstate = 4;
        ReportData report = new ReportData();

        public MainPage()
        {
            InitializeComponent();
            resetBtn.IsVisible = false;
            menubox.TranslationX = -App.ScreenWidth;
            RefList(selectedstate);
            listmain.Scrolled += async (sender, e) =>
            {
                if ((totallistcount == 0 && getlistcount >= totallistcount) || busy.IsVisible)
                    return;
                //hit bottom!
                if (e.LastVisibleItemIndex == blogpost.Count - 1)
                {
                    busy.IsVisible = true;
                    busy.IsRunning = true;
                    await LoadItems();
                    busy.IsVisible = false;
                    busy.IsRunning = false;
                }
            };

            if (Device.RuntimePlatform == Device.Android)
            {
                if (Preferences.ContainsKey("backup1"))
                {
                    var milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    var ms = milliseconds - 10;
                    var mad1 = Preferences.Get("backup1", ms);
                    //4320000
                    if ((milliseconds - mad1) > 60000)
                    {
                        Preferences.Set("backup1", milliseconds);
                        RunBackupData();
                    }
                    
                }
                else
                {
                    var milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                    Preferences.Set("backup1", milliseconds);
                    RunBackupData();
                }
                    
            }
        }

        private async Task RunBackupData()
        {
            var result = await post.GetAllDataForBackup();
            await DependencyService.Get<IFileSave>().SaveFileTxt(result);
        }

        private async Task LoadItems()
        {
            if (getlistcount < totallistcount && totallistcount != 0)
            {
                //simulator delayed load
                Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(0.5), () =>
                {
                    for (int index = 0; index < 5; index++)
                    {
                        if (getlistcount != totallistcount && getlistcount < totallistcount)
                        {
                            var sd = post.DataList[getlistcount];
                            blogpost.Add(sd);
                            getlistcount++;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    //stop timer
                    return false;
                });
            }
            if (getlistcount == totallistcount && totallistcount != 0 && string.IsNullOrEmpty(ensearch.Text.Trim()))
            {
                await LoadNextList();
            }
        }

        public async Task LoadNextList()
        {
            if (totallistcount != 0 && getlistcount != 0)
            {
                totallistcount = 0;
                getlistcount = 0;
                start = start + end;

                await Task.Run(async () =>
                {
                    var llres = await post.GetAllData(start, end,selectedstate);
                    post.DataList = llres;
                    totallistcount = post.DataList.Count;
                });

                if (totallistcount != 0)
                {
                    await LoadItems();
                }
                else
                {
                    reachedend = true;
                    busy.IsVisible = false;
                    busy.IsRunning = false;
                }
            }

        }

        public async Task RefList(int ii)
        {
            busy.IsVisible = true;
            busy.IsRunning = true;
            lblempty.IsVisible = false;
            blogpost.Clear();
            totallistcount = 0;
            getlistcount = 0;
            start = 0;
            end = 20;
            reachedend = false;

            await Task.Run(async () =>
            {
                var llres = await post.GetAllData(start, end,selectedstate);
                post.DataList = llres;
            });

            //switch (ii)
            //{
            //    case 1:
            //        post.DataList = post.DataList.OrderBy(x => x.FName).ToList();
            //        break;
            //    case 2:
            //        post.DataList = post.DataList.OrderByDescending(x => x.FName).ToList();
            //        break;
            //    case 3:
            //        post.DataList = post.DataList.OrderBy(x => x.Date).ToList();
            //        break;
            //    case 4:
            //        post.DataList = post.DataList.OrderByDescending(x => x.Date).ToList();
            //        break;
            //    case 5:
            //        post.DataList = post.DataList.OrderBy(x => x.PlateNo).ToList();
            //        break;
            //    default:
            //        break;
            //}
            totallistcount = post.DataList.Count;
            listmain.ItemsSource = blogpost;
            if (totallistcount != 0)
            {
                await LoadItems();
            }
            else
            {
                lblempty.IsVisible = true;
            }
            busy.IsVisible = false;
            busy.IsRunning = false;
        }


        public async Task RefSearchList(string ss, int ii)
        {
            busy.IsVisible = true;
            busy.IsRunning = true;
            lblempty.IsVisible = false;
            blogpost.Clear();
            totallistcount = 0;
            getlistcount = 0;
            start = 0;
            end = 20;
            reachedend = false;

            await Task.Run(async () =>
            {
                await post.GetSearchData(ss, selectedstate);
            });
            //switch (ii)
            //{
            //    case 1:
            //        post.DataList = post.DataList.OrderBy(x => x.FName).ToList();
            //        break;
            //    case 2:
            //        post.DataList = post.DataList.OrderByDescending(x => x.FName).ToList();
            //        break;
            //    case 3:
            //        post.DataList = post.DataList.OrderBy(x => x.Date).ToList();
            //        break;
            //    case 4:
            //        post.DataList = post.DataList.OrderByDescending(x => x.Date).ToList();
            //        break;
            //    case 5:
            //        post.DataList = post.DataList.OrderBy(x => x.PlateNo).ToList();
            //        break;
            //    default:
            //        break;
            //}
            totallistcount = post.DataList.Count;
            listmain.ItemsSource = blogpost;
            if (totallistcount != 0)
            {
                await LoadItems();
            }
            else
            {
                lblempty.IsVisible = true;
            }
            busy.IsVisible = false;
            busy.IsRunning = false;
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            var getCommand = (Frame)sender;
            getCommand.IsEnabled = false;
            if (DependencyService.Get<ICloseApplication>().storagepermission())
            {
                await Navigation.PushAsync(new NewDetailPage());
            }
            getCommand.IsEnabled = true;
        }

        async void Handle_ItemTapped(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            if (DependencyService.Get<ICloseApplication>().storagepermission())
            {
                clonepop.IsVisible = true;
                report = getcommand.BindingContext as ReportData;
            }
            //await Navigation.PushAsync(new NewDetailPage(data));
            getcommand.IsEnabled = true;
        }
        async void EveYes(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            DependencyService.Get<ICloseApplication>().closeApplication();
            getcommand.IsEnabled = true;
        }
        async void EveNo(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            exit.IsVisible = false;
            getcommand.IsEnabled = true;
        }

        async void EveClone(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            clonepop.IsVisible = false;
            await Navigation.PushAsync(new NewDetailPage(report,0));
            getcommand.IsEnabled = true;
        }
        void EveClonePopClose(object sender, System.EventArgs e)
        {
            var getcommand = (StackLayout)sender;
            getcommand.IsEnabled = false;
            clonepop.IsVisible = false;
            getcommand.IsEnabled = true;
        }
        async void EveEdit(object sender, System.EventArgs e)
        {
            var getcommand = (Button)sender;
            getcommand.IsEnabled = false;
            clonepop.IsVisible = false;
            await Navigation.PushAsync(new NewDetailPage(report,1));
            getcommand.IsEnabled = true;
        }
        async void EveShowPrint(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;
            if (DependencyService.Get<ICloseApplication>().storagepermission())
            {
                report = getCommand.BindingContext as ReportData;
                ShowPrint();
            }
            getCommand.IsEnabled = true;
        }
        async void EvePrintPDF(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            await Navigation.PushAsync(new NewPrintPage(report));
            HidePrint();
            getCommand.IsEnabled = true;
        }
        void EvePrintBarcode(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            //await Navigation.PushAsync(new PrintTirelabelPage(report));
            //var action= await DisplayActionSheet((("Select Page Size", "A4", "A6");
            HidePrint();
            getCommand.IsEnabled = true;
        }


        async void EvePrintA4(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            await Navigation.PushAsync(new PrintTirelabelPage(report));
            HidePrint();
            getCommand.IsEnabled = true;
        }
        async void EvePrintA6(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            //await Navigation.PushAsync(new PrintTirelabelPage(report));
            await Navigation.PushAsync(new PrintTireLabelZebraPage(report));
            //var action= await DisplayActionSheet((("Select Page Size", "A4", "A6");
            HidePrint();
            getCommand.IsEnabled = true;
        }

        async void EveSort1(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            HideSort();
            selectedstate = 1;
            await RefList(1);

            getCommand.IsEnabled = true;
        }
        async void EveSort2(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            HideSort();
            selectedstate = 2;
            await RefList(2);

            getCommand.IsEnabled = true;
        }
        async void EveSort3(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            HideSort();
            selectedstate = 3;
            await RefList(3);

            getCommand.IsEnabled = true;
        }
        async void EveSort4(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            HideSort();
            selectedstate = 4;
            await RefList(4);

            getCommand.IsEnabled = true;
        }
        async void EveSort5(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;

            HideSort();
            selectedstate = 5;
            await RefList(5);

            getCommand.IsEnabled = true;
        }

        async void EveSort(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;
            ShowSort();
            getCommand.IsEnabled = true;
        }
        async void EveMenu(object sender, System.EventArgs e)
        {
            var getCommand = (ImageButton)sender;
            getCommand.IsEnabled = false;

            ShowMenu();

            getCommand.IsEnabled = true;
        }

        async void EveBtnMenu1(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            HideMenu();
            RefList(selectedstate);
            getCommand.IsEnabled = true;
        }
        async void EveBtnMenu2(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            await Navigation.PushAsync(new SettingPage());
            getCommand.IsEnabled = true;
        }
        async void EveBtnMenu3(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            await Navigation.PushAsync(new NewExcelPage());
            getCommand.IsEnabled = true;
        }
        async void EveBtnMenu4(object sender, System.EventArgs e)
        {
            var getCommand = (Button)sender;
            getCommand.IsEnabled = false;
            HideMenu();
            await Navigation.PushAsync(new ReportPage());
            getCommand.IsEnabled = true;
        }

        async void EveNew(object sender, System.EventArgs e)
        {
            var getCommand = (Frame)sender;
            getCommand.IsEnabled = false;
            await Navigation.PushAsync(new NewDetailPage());
            getCommand.IsEnabled = true;
        }

        async void EveClose(object sender, System.EventArgs e)
        {
            menubox.TranslateTo(-App.ScreenWidth, 0, 150);
            await sortstk.ScaleTo(0, 150);
            sortstk.IsVisible = false;
            HidePrint();
        }

        async void EveSearch(object sender, System.EventArgs e)
        {
            searchBtn.IsEnabled = false;
            if (!string.IsNullOrWhiteSpace(ensearch.Text))
            {
                busy.IsVisible = true;
                busy.IsRunning = true;
                blogpost.Clear();
                totallistcount = 0;
                getlistcount = 0;

                var ssdata = ensearch.Text.Trim();
                if (ssdata.Length >= 3)
                {
                    await RefSearchList(ssdata, selectedstate);
                }
            }
            busy.IsVisible = false;
            busy.IsRunning = false;
            searchBtn.IsEnabled = true;
        }

        async void Handle_Completed(object sender, System.EventArgs e)
        {
            searchBtn.IsEnabled = false;
            if (!string.IsNullOrWhiteSpace(ensearch.Text))
            {
                busy.IsVisible = true;
                busy.IsRunning = true;
                blogpost.Clear();
                totallistcount = 0;
                getlistcount = 0;

                var ssdata = ensearch.Text.Trim();

                if (ssdata.Length >= 3)
                {
                    await RefSearchList(ssdata, selectedstate);
                }
            }
            busy.IsVisible = false;
            busy.IsRunning = false;
            searchBtn.IsEnabled = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            if(menubox.TranslationX>=0)
            {
                HideMenu();
            }
            else if(sortstk.Scale>=1)
            {
                HideSort();
            }
            else if(clonepop.IsVisible)
                clonepop.IsVisible = false;
            else
            {
                exit.IsVisible = true;
            }
            return true;
        }

        void ShowMenu()
        {
            menubox.TranslateTo(0, 0, 150);
        }
        void HideMenu()
        {
            menubox.TranslateTo(-App.ScreenWidth, 0, 150);
        }
        async void ShowSort()
        {
            sortstk.IsVisible = true;
            await sortstk.ScaleTo(1,150);
        }
        async void HideSort()
        {
            await sortstk.ScaleTo(0, 150);
            sortstk.IsVisible = false;
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

        private async void scanBtn_Clicked(object sender, EventArgs e)
        {
            var scanButton = (ImageButton)sender;
            scanButton.IsEnabled = false;

            try
            {
                busy.IsVisible = true;
                busy.IsRunning = true;

                // Check for storage permission which might be needed for camera
                if (DependencyService.Get<ICloseApplication>().storagepermission())
                {
                    // Call QR code scanner
                    var scanResult = await DependencyService.Get<IScanner>().Scan();

                    // If scan was successful and returned a value
                    if (!string.IsNullOrEmpty(scanResult))
                    {
                        string searchText = scanResult;

                        // Check if it contains a separator (in case the QR has a format like "Type|Value")
                        if (scanResult.Contains("|") && scanResult.Split('|').Length > 1)
                        {
                            searchText = scanResult.Split('|')[1];
                        }

                        // Set the search text to the scan result
                        ensearch.Text = searchText;

                        // Perform search with the scanned text
                        if (searchText.Length >= 3)
                        {
                            blogpost.Clear();
                            totallistcount = 0;
                            getlistcount = 0;

                            await RefSearchList(searchText, selectedstate);
                        }
                        else
                        {
                            await DisplayAlert("Search", "Scanned text too short. Minimum 3 characters required.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error scanning QR code: {ex.Message}", "OK");
            }
            finally
            {
                busy.IsVisible = false;
                busy.IsRunning = false;
                scanButton.IsEnabled = true;
            }
        }

        private async void resetBtn_Clicked(object sender, EventArgs e)
        {
            var resetButton = (ImageButton)sender;
            resetButton.IsEnabled = false;

            try
            {
                busy.IsVisible = true;
                busy.IsRunning = true;

                // Clear the search text
                ensearch.Text = string.Empty;

                // Reset to default state and load all records
                blogpost.Clear();
                totallistcount = 0;
                getlistcount = 0;
                start = 0;
                end = 20;
                reachedend = false;

                // Refresh the list with the current selected state/sort order
                await RefList(selectedstate);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error resetting search: {ex.Message}", "OK");
            }
            finally
            {
                busy.IsVisible = false;
                busy.IsRunning = false;
                resetButton.IsEnabled = true;
            }
        }
     
        private void ensearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            resetBtn.IsVisible = !string.IsNullOrEmpty(ensearch.Text);
        }
    }

    public class GetOnlineData
    {
        private List<ReportData> _datalist;
        public bool _isbusy = false;
        public string WebServiceUrl = "http://3.133.136.76/api/mains";
        public List<ReportData> DataList
        {
            get
            {
                return _datalist;
            }
            set
            {
                _datalist = value;
                OnPropertyChanged();
            }
        }
        public bool IsBusy
        {
            get
            {
                return _isbusy;
            }
            set
            {
                _isbusy = value;
                OnPropertyChanged();
            }
        }

        public async Task<List<ReportData>> GetAllData()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://3.133.136.76/api/mains");

            return JsonConvert.DeserializeObject<List<ReportData>>(json);
        }

        public async Task<string> GetAllDataForBackup()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://3.133.136.76/api/mains/getalldata/");

            return json;
        }
        public async Task<List<ReportData>> GetAllData(int start, int end,int sort)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://3.133.136.76/api/mains/getalldata/" + start + "/" + end+ "/" + sort + "/");

            return JsonConvert.DeserializeObject<List<ReportData>>(json);
        }

        public async Task GetSearchData(string search, int sort)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://3.133.136.76/api/mains/getsearchdata/" + search + "/" + sort + "/");

            DataList = JsonConvert.DeserializeObject<List<ReportData>>(json);
        }

        public async Task GetSearchData(string search, int start, int end)
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync("http://3.133.136.76/api/mains/getsearchdata/" + search + "/" + start + "/" + end);

            DataList = JsonConvert.DeserializeObject<List<ReportData>>(json);
        }


        public async Task<ReportData> PostAsync(ReportData t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await httpClient.PostAsync(WebServiceUrl, httpContent);
            var resJson = await result.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ReportData>(resJson);
            //return result.IsSuccessStatusCode;
        }

        public async Task<bool> PutAsync(string id, ReportData t)
        {
            var httpClient = new HttpClient();

            var json = JsonConvert.SerializeObject(t);

            HttpContent httpContent = new StringContent(json);

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await httpClient.PostAsync(WebServiceUrl +"/"+ id, httpContent);
            return result.IsSuccessStatusCode;

        }

        public async Task<ReportData> GetAsync(string id)
        {
          
            var httpClient = new HttpClient();
     
            var result = await httpClient.GetStringAsync(WebServiceUrl + "/" + id);
            return JsonConvert.DeserializeObject<ReportData>(result);
            

        }
        //INotifier
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      
    }



    //public class TiffImage
    //{
    //    private string myPath;
    //    private Guid myGuid;
    //    private FrameDimension myDimension;
    //    public string[] myImages;
    //    private int myPageCount;
    //    private Bitmap myBMP;

    //    public TiffImage(string path)
    //    {
    //        MemoryStream ms;
    //        Image myImage;

    //        myPath = path;
    //        FileStream fs = new FileStream(myPath, FileMode.Open);
    //        myImage = Image.FromStream(fs);
    //        myGuid = myImage.FrameDimensionsList[0];
    //        myDimension = new FrameDimension(myGuid);
    //        myPageCount = myImage.GetFrameCount(myDimension);
    //        for (int i = 0; i < myPageCount; i++)
    //        {
    //            ms = new MemoryStream();
    //            myImage.SelectActiveFrame(myDimension, i);
    //            myImage.Save(ms, ImageFormat.Bmp);
    //            myBMP = new Bitmap(ms);
    //            myImages.Add(myBMP);
    //            ms.Close();
    //        }
    //        fs.Close();
    //    }
    //}
}

