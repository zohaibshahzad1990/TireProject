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

namespace TireProject
{
    public partial class NewExcelPage : ContentPage
    {
        GetOnlineData post = new GetOnlineData();
        List<ReportData> ll = new List<ReportData>();

        public NewExcelPage()
        {
            InitializeComponent();
            RunPage();
        }

        public NewExcelPage(Uri uri)
        {
            InitializeComponent();


            Uri myUri = new Uri(uri.OriginalString);
            string code = System.Web.HttpUtility.ParseQueryString(myUri.Query).Get("code");

            string ClientId = "";
            string redirectUri = "";
            if (Device.RuntimePlatform == Device.Android)
            {
                ClientId = @"976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr.apps.googleusercontent.com";
                redirectUri = @"com.googleusercontent.apps.976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr:oauth2redirect";
            }
            else
            {
                ClientId = @"976132508244-8vjvj9sboohdd1gq07ftb7vkus3srfg5.apps.googleusercontent.com";
                redirectUri = @"com.googleusercontent.apps.976132508244-8vjvj9sboohdd1gq07ftb7vkus3srfg5:oauth2redirect";
            }

            GetTokenReport(code, redirectUri, ClientId);
        }


        public async Task RunPage()
        {
            if (Application.Current.Properties.ContainsKey("Token"))
            {
                await Task.Run(async () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        busymsg.IsVisible = true;
                        busyind.IsVisible = true;
                        busyind.IsRunning = true;
                    });
                    string ss = Application.Current.Properties["Token"].ToString();
                    var sd = JsonConvert.DeserializeObject<TokenData>(ss);

                    var ll = await post.GetAllData();
                    post.DataList = ll;
                    ll = post.DataList.OrderBy(x => x.Date).ToList();


                    List<string> sigledata = new List<string>();
                    List<List<string>> alldata = new List<List<string>>();

                    sigledata.Add("ID");
                    sigledata.Add("Ref No");
                    sigledata.Add("Date");
                    sigledata.Add("First Name");
                    sigledata.Add("Middle Name");
                    sigledata.Add("Last Name");
                    sigledata.Add("Address");
                    sigledata.Add("Phone No");
                    sigledata.Add("Home No");
                    sigledata.Add("Work No");
                    sigledata.Add("Email");
                    sigledata.Add("Plate Number");
                    sigledata.Add("Car Year");
                    sigledata.Add("Car Brand");
                    sigledata.Add("Car Model");
                    sigledata.Add("Storage Location");
                    sigledata.Add("Tire Season");
                    sigledata.Add("Number of Tire");
                    sigledata.Add("Tire Size");
                    sigledata.Add("Front Tire");
                    sigledata.Add("Rear Tire");
                    sigledata.Add("Tire Make");
                    sigledata.Add("Rim Type");
                    sigledata.Add("Tire Depth");
                    sigledata.Add("REP");
                    sigledata.Add("Tire Stored");
                    sigledata.Add("Remarks");



                    alldata.Add(new List<string>(sigledata));
                    sigledata.Clear();



                    //stitle = stitle.TrimEnd(',');
                    int io = 0;
                    foreach (var item in ll)
                    {
                        if (item.RefNo == null) item.RefNo = "";
                        if (item.Date == null) item.Date = "";
                        if (item.ExtraRefNo == null) item.ExtraRefNo = "";
                        if (item.ExtraDate == null) item.ExtraDate = "";

                        //Customer Detail
                        if (item.FName == null) item.FName = "";
                        if (item.LName == null) item.LName = "";
                        if (item.MName == null) item.MName = "";
                        if (item.Address == null) item.Address = "";
                        if (item.PhoneNo == null) item.PhoneNo = "";
                        if (item.HomeNo == null) item.HomeNo = "";
                        if (item.WorkNo == null) item.WorkNo = "";
                        if (item.Email == null) item.Email = "";

                        //Tire Storage Detail
                        if (item.DepthLF == null) item.DepthLF = "";
                        if (item.TireSize1 == null) item.TireSize1 = "";
                        if (item.TireSize2 == null) item.TireSize2 = "";
                        if (item.TireSize3 == null) item.TireSize3 = "";
                        if (item.TireStoredUpto == null) item.TireStoredUpto = "";
                        if (item.REP == null) item.REP = "";




                        //Vehicle Detail
                        if (item.PlateNo == null) item.PlateNo = "";
                        if (item.CarYear == null) item.CarYear = "";
                        if (item.CarBrand == null) item.CarBrand = "";
                        if (item.CarModel == null) item.CarModel = "";
                        if (item.Pic1 == null) item.Pic1 = "";
                        if (item.Pic2 == null) item.Pic2 = "";
                        if (item.Pic3 == null) item.Pic3 = "";
                        if (item.Pic4 == null) item.Pic4 = "";



                        sigledata.Add(item.Id.ToString());
                        sigledata.Add(item.RefNo.ToString());
                        sigledata.Add(item.Date.ToString());
                        sigledata.Add(item.FName.ToString());
                        sigledata.Add(item.MName.ToString());
                        sigledata.Add(item.LName.ToString());
                        sigledata.Add(item.Address.ToString());
                        sigledata.Add(item.PhoneNo.ToString());
                        sigledata.Add(item.HomeNo.ToString());
                        sigledata.Add(item.WorkNo.ToString());
                        sigledata.Add(item.Email.ToString());
                        sigledata.Add(item.PlateNo.ToString());
                        sigledata.Add(item.CarYear.ToString());
                        sigledata.Add(item.CarBrand.ToString());
                        sigledata.Add(item.CarModel.ToString());
                        sigledata.Add(item.ExtraRefNo.ToString());
                        sigledata.Add(item.TireSeason.ToString());
                        sigledata.Add(item.NoOfTires.ToString());
                        sigledata.Add(item.TireSize1.ToString());
                        sigledata.Add(item.TireSize2.ToString());
                        sigledata.Add(item.TireSize3.ToString());
                        sigledata.Add(item.MakeModel.ToString());
                        sigledata.Add(item.TypeRim.ToString());
                        sigledata.Add(item.DepthLF.ToString());
                        sigledata.Add(item.REP.ToString());
                        sigledata.Add(item.TireStoredUpto.ToString());
                        sigledata.Add(item.ExtraDate.ToString());



                        alldata.Add(new List<string>(sigledata));
                        sigledata.Clear();
                    }


                    var strUrl = @"https://sheets.googleapis.com/v4/spreadsheets/1RgYsVkrJL599ahbubBWxBhQMvYQEymK05Rv9bvmdw5E/values/Sheet1!A1?valueInputOption=RAW&access_token=" + sd.access_token;

                    var httpClient = new HttpClient();

                    InputData input = new InputData() { values = alldata };


                    var json = JsonConvert.SerializeObject(input);

                    HttpContent httpContent = new StringContent(json);

                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var result = await httpClient.PutAsync(strUrl, httpContent);

                    if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        Application.Current.Properties.Remove("Token");
                        RunPage();
                        return;
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (result.IsSuccessStatusCode)
                        {
                            busymsg.Text = "Successfully Exported!";
                            btnDone.IsVisible = true;
                        }
                        else
                        {
                            busymsg.Text = "Something Wrong With Exporting Data!";
                        }



                        busyind.IsVisible = false;
                        busyind.IsRunning = false;
                    });



                });
            }

            else
            {
                string mainUrl = @"https://accounts.google.com/o/oauth2/v2/auth?scope=https://www.googleapis.com/auth/spreadsheets&access_type=offline&response_type=code&state=security_token%3D138r5719ru3e1%26url%3Dhttps://oauth2.example.com/token&";
                string ClientId = @"";
                string redirectUri = @"";
                if (Device.RuntimePlatform == Device.Android)
                {
                    //ClientId = @"&redirect_uri=com.googleusercontent.apps.976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr:oauth2redirect";
                    redirectUri = @"&redirect_uri=com.googleusercontent.apps.976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr:oauth2redirect";
                    ClientId = @"&client_id=976132508244-rsm3j0u6udqg5bea3k31tdi5aiv34ktr.apps.googleusercontent.com";
                }
                else
                {
                    redirectUri = @"&redirect_uri=com.googleusercontent.apps.976132508244-8vjvj9sboohdd1gq07ftb7vkus3srfg5:oauth2redirect";
                    ClientId = @"&client_id=976132508244-8vjvj9sboohdd1gq07ftb7vkus3srfg5.apps.googleusercontent.com";
                }
                Device.OpenUri(new Uri(mainUrl + redirectUri + ClientId));
            }





        }

        void Handle_Clicked(object sender, System.EventArgs e)
        {
            Device.OpenUri(new Uri("https://docs.google.com/spreadsheets/d/1RgYsVkrJL599ahbubBWxBhQMvYQEymK05Rv9bvmdw5E/edit#gid=0"));
        }

        public async Task GetTokenReport(string code, string redirectUri, string ClientId)
        {
            busymsg.Text = "Getting User Profile Info...";
            var dict = new Dictionary<string, string>();
            dict.Add("code", code);
            dict.Add("grant_type", "authorization_code");
            dict.Add("redirect_uri", redirectUri);
            dict.Add("client_id", ClientId);

            var httpClient = new HttpClient();


            var httpContent = new FormUrlEncodedContent(dict);
            httpContent.Headers.Clear();
            httpContent.Headers.Add("Content-Type", "application/x-www-form-urlencoded");


            var uploadservicebasedurl = "https://www.googleapis.com/oauth2/v4/token";
            var httpresponseMessage = await httpClient.PostAsync(uploadservicebasedurl, httpContent);

            if (httpresponseMessage.IsSuccessStatusCode)
            {
                string getname = await httpresponseMessage.Content.ReadAsStringAsync();

                var ddd = JsonConvert.DeserializeObject<TokenData>(getname);
                var sss = JsonConvert.SerializeObject(ddd);

                Application.Current.Properties["Token"] = sss;
                RunPage();
            }

            else
            {
                busymsg.Text = "Something Wrong With Your Session Token!";
                busyind.IsVisible = false;
            }




        }


        public class InputData
        {
            public List<List<string>> values { get; set; }
        }
        public class TokenData
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
            public string scope { get; set; }
            public string token_type { get; set; }
        }


    }

}
