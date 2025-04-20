using System.Collections.Generic;
using System.Linq;
using TireProjectNewWebApi.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using MongoDB.Bson.IO;

namespace TireProjectNewWebApi.Services
{
    public class MainService
    {
        private readonly IMongoCollection<ReportData> _ReportDatas;
        private readonly IMongoCollection<Settings> _Settings;
        public MainService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MainString"));
            var database = client.GetDatabase("TireDatabase");
            _ReportDatas = database.GetCollection<ReportData>("TireCollection");
            _Settings = database.GetCollection<Settings>("Settings");
            //var database = client.GetDatabase("TireAppMediaDatabase");
            //_ReportDatas = database.GetCollection<ReportData>("TireAppMediaCollection");

        }

        public async Task<Settings> GetSettings()
        {
            var settings = await _Settings.Find(_ => true).FirstOrDefaultAsync();
            return settings;
        }

        public async Task SaveSettings(Settings settings)
        {
            var existingSettings = await _Settings.Find(_ => true).FirstOrDefaultAsync();

            if (existingSettings == null)
            {
                await _Settings.InsertOneAsync(settings);
            }
            else
            {
                settings.Id = existingSettings.Id;
                await _Settings.ReplaceOneAsync(s => s.Id == existingSettings.Id, settings);
            }
        }
        public async Task BackupFunction(string hostpath)
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromDays(1);

            var timer = new System.Threading.Timer((e) =>
            {
                SendMail(hostpath,"Database Backup Update","Updated Backup File");
            }, null, startTimeSpan, periodTimeSpan);
        }

        public string SendMail(string hostpath,string subject,string body)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(new MailAddress("asfandyar.data@gmail.com"));
            //mailMessage.To.Add(new MailAddress("totaltirestorage@gmail.com"));
            
            mailMessage.From = new MailAddress("appmedia1234@gmail.com");
            mailMessage.Subject = subject;
            //mailMessage.IsBodyHtml = true;
            mailMessage.Body = body;

            
            if (!Directory.Exists(Path.Combine(hostpath, "BackupData")))
            {
                Directory.CreateDirectory(Path.Combine(hostpath, "BackupData"));
            }

            var uploadLocation = Path.Combine(hostpath, "BackupData");
            var ffgilename = "Backup_"+DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmss");
            var file = Path.Combine(uploadLocation, ffgilename + ".txt");

            //Directory.CreateDirectory(file);
            var resulyt=_ReportDatas.Find(ReportData => true).ToList();
            var resultinjson=Newtonsoft.Json.JsonConvert.SerializeObject(resulyt);
            File.WriteAllText(file, resultinjson);

            System.Net.Mail.Attachment attachment;
            attachment = new System.Net.Mail.Attachment(file);
            mailMessage.Attachments.Add(attachment);


            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "appmedia1234@gmail.com",
                    Password = "P@ss1234"
                };
                try
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(mailMessage);
                    return "success";
                }
                catch (Exception ex)
                {
                    return "failed" + ex.Message;
                }
            }
        }

        public string GetUpdateNew()
        {
            var rf = _ReportDatas.Find(ReportData => true).ToList();
            foreach (var item in rf)
            {
                if (item.Pic1?.Contains("3.133.136.76")==false || item.Pic2?.Contains("3.133.136.76") == false || item.Pic3?.Contains("3.133.136.76") == false || item.Pic4?.Contains("3.133.136.76") == false || item.CustomerSign?.Contains("3.133.136.76") == false)
                {
                    Debug.WriteLine($"FOUND--------------------------------------------------------------- {Newtonsoft.Json.JsonConvert.SerializeObject(item)}");
                    if (item.Pic1 != null)
                        item.Pic1 = item.Pic1.ReplaceDomain();
                    if (item.Pic2 != null)
                        item.Pic2 = item.Pic2.ReplaceDomain();
                    if (item.Pic3 != null)
                        item.Pic3 = item.Pic3.ReplaceDomain();
                    if (item.Pic4 != null)
                        item.Pic4 = item.Pic4.ReplaceDomain();
                    if (item.CustomerSign != null)
                        item.CustomerSign = item.CustomerSign.ReplaceDomain();
                    Update(item.Id, item);
                }
                else
                {
                    //Debug.WriteLine("Not found");
                }
            }
            return "success";
        }

        public List<ReportData> Get()
        {
            return _ReportDatas.Find(ReportData => true).ToList();
        }

        public List<ReportData> GetAllData()
        {
            return _ReportDatas.Find(ReportData => true).ToList();
        }

        public List<ReportData> GetAllDataPagination(int start,int end,int sort)
        {
            switch (sort)
            {
                case 1:
                    return _ReportDatas.Find(ReportData => true).SortBy(ReportData=>ReportData.FName).Skip(start).Limit(end).ToList();
                    break;
                case 2:
                    return _ReportDatas.Find(ReportData => true).SortByDescending(ReportData => ReportData.FName).Skip(start).Limit(end).ToList();
                    break;
                case 3:
                    return _ReportDatas.Find(ReportData => true).SortBy(ReportData => ReportData.Date).Skip(start).Limit(end).ToList();
                    break;
                case 4:
                    return _ReportDatas.Find(ReportData => true).SortByDescending(ReportData => ReportData.Date).Skip(start).Limit(end).ToList();
                    break;
                case 5:
                    return _ReportDatas.Find(ReportData => true).SortBy(ReportData => ReportData.PlateNo).Skip(start).Limit(end).ToList();
                    break;
                default:
                    return _ReportDatas.Find(ReportData => true).Skip(start).Limit(end).ToList();
                    break;
            }
        }

        public List<ReportData> GetSearchData(string ssdata,int sort)
        {
            ssdata = ssdata.ToLower();
            IQueryable<ReportData> query = _ReportDatas.AsQueryable();
            switch (sort)
            {
                case 1:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).OrderBy(ReportData => ReportData.FName).ToList();
                    break;
                case 2:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).OrderByDescending(ReportData => ReportData.FName).ToList();
                    break;
                case 3:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).OrderBy(ReportData => ReportData.Date).ToList();
                    break;
                case 4:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).OrderByDescending(ReportData => ReportData.Date).ToList();
                    break;
                case 5:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).OrderBy(ReportData => ReportData.PlateNo).ToList();
                    break;
                default:
                    return query.Where(item => item.PhoneNo.ToLower() == ssdata
                     || item.FName.ToLower() == ssdata
                     || item.LName.ToLower() == ssdata
                     || item.Email.ToLower() == ssdata
                     || item.PlateNo.ToLower().Contains(ssdata)
                     || item.TireSize1.ToLower() == ssdata
                     || item.CarYear.ToLower() == ssdata
                     || item.CarBrand.ToLower() == ssdata
                     || item.CarModel.ToLower() == ssdata
                         || item.RefNo.ToLower() == ssdata).ToList();
                    break;
            }
        }

        public List<ReportData> GetSearchDataPagination(string ssdata,int start, int end)
        {
            ssdata = ssdata.ToLower();
            return _ReportDatas.Find(item => item.PhoneNo.ToLower() ==ssdata
                     || item.FName.ToLower()==ssdata
                     || item.LName.ToLower()==ssdata
                     || item.Email.ToLower()==ssdata
                     || item.PlateNo.Contains(ssdata)
                     || item.TireSize1.ToLower()==ssdata
                     || item.CarYear.ToLower()==ssdata
                     || item.CarBrand.ToLower()==ssdata
                     || item.CarModel.ToLower()==ssdata
                         || item.RefNo.ToLower()==ssdata).Skip(start).Limit(end).ToList();
        }

        public List<ReportData> GetCustomExportData(string datefrom, string dateto, int ii,int eTireSeason,string warehouse)
        {
            List<ReportData> ll = new List<ReportData>();
            List<ReportData> l1 = new List<ReportData>();
            switch (ii)
            {
                case 0:
                    if(warehouse=="ALL")
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason)).SortBy(x1 => x1.FName).ToList();
                        ll = ll.OrderBy(zas=>zas.FName,StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    else
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason)&& item.ExtraRefNo.ToLower().Contains(warehouse.ToLower())).SortBy(x1 => x1.FName).ToList();
                        ll = ll.OrderBy(zas => zas.FName, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    break;
                case 1:
                    if (warehouse == "ALL")
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason)).SortBy(x1 => x1.PlateNo).ToList();
                        ll = ll.OrderBy(zas => zas.PlateNo, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    else
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason) && item.ExtraRefNo.ToLower().Contains(warehouse.ToLower())).SortBy(x1 => x1.PlateNo).ToList();
                        ll = ll.OrderBy(zas => zas.PlateNo, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    break;
                case 2:
                    if (warehouse == "ALL")
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason)).SortBy(x1 => x1.CarBrand).ToList();
                        ll = ll.OrderBy(zas => zas.CarBrand, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    else
                    {
                        ll = _ReportDatas.Find(item => item.TireSeason.Equals(eTireSeason) && item.ExtraRefNo.ToLower().Contains(warehouse.ToLower())).SortBy(x1 => x1.CarBrand).ToList();
                        ll = ll.OrderBy(zas => zas.CarBrand, StringComparer.OrdinalIgnoreCase).ToList();
                    }
                    break;
                default:
                    break;
            }
            DateTime dt1 = DateTime.ParseExact(datefrom, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(dateto, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            foreach (var item in ll)
            {
                //var spli = item.Date.Split('-');
                //var dchange = spli[1] + "-" + spli[2] + "-" + spli[0];
                if (!string.IsNullOrEmpty(item.Date)) {
                    DateTime dd = DateTime.ParseExact(item.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    if (dd >= dt1 && dd <= dt2)
                    {
                        l1.Add(item);
                    }
                }
                
            }
            return l1;
        }

        public ReportData Get(string id)
        {
            return _ReportDatas.Find<ReportData>(main => main.Id == id).FirstOrDefault();
        }

        public ReportData Create(ReportData ReportData)
        {
            _ReportDatas.InsertOne(ReportData);
            ReportData.RefNo = "TT1-" + _ReportDatas.Find(rep => true).ToList().Count().ToString("D8");
            _ReportDatas.ReplaceOne(rep1 => rep1.Id == ReportData.Id, ReportData);
            return ReportData;
        }

        public void Update(string id, ReportData ReportDataIn)
        {
            _ReportDatas.ReplaceOne(ReportData => ReportData.Id == id, ReportDataIn);
        }

        public void Remove(ReportData ReportDataIn)
        {
            _ReportDatas.DeleteOne(ReportData => ReportData.Id == ReportDataIn.Id);
        }

        public void Remove(string id)
        {
            _ReportDatas.DeleteOne(ReportData => ReportData.Id == id);
        }


        //Test
        public string DelAll()
        {
            int ii = 1;
            var alldata = _ReportDatas.Find(ReportData => true).SortBy(ReportData => ReportData.Date).ToList();
            foreach (var item in alldata)
            {
                item.RefNo = "TT1-" + ii.ToString("D8");
                _ReportDatas.ReplaceOne(ReportData => ReportData.Id == item.Id, item);
                ii++;
            }

            return alldata.Count.ToString();
        }
        public string DateChangeFunc()
        {
            var alldata = _ReportDatas.Find(ReportData => true).ToList();
            foreach (var item in alldata)
            {
                try
                {
                    if (!string.IsNullOrEmpty(item.Pic1))
                        item.Pic1 = changeHostName(item.Pic1);// item.Pic1.Replace("https://tireproject.", "https://tireprojectwebapi.");
                    if (!string.IsNullOrEmpty(item.Pic2))
                        item.Pic2 = changeHostName(item.Pic2);//item.Pic2.Replace("https://tireproject.", "https://tireprojectwebapi.");
                    if (!string.IsNullOrEmpty(item.Pic3))
                        item.Pic3 = changeHostName(item.Pic3);// item.Pic3.Replace("https://tireproject.", "https://tireprojectwebapi.");
                    if (!string.IsNullOrEmpty(item.Pic4))
                        item.Pic4 = changeHostName(item.Pic4); //item.Pic4.Replace("https://tireproject.", "https://tireprojectwebapi.");
                    if (!string.IsNullOrEmpty(item.CustomerSign))
                        item.CustomerSign = changeHostName(item.CustomerSign);// item.CustomerSign.Replace("https://tireproject.", "https://tireprojectwebapi.");
                    _ReportDatas.ReplaceOne(ReportData => ReportData.Id == item.Id, item);
                }catch(Exception ex)
                {

                }
            }
            return "string";
        }

        string changeHostName(string url)
        {
            try
            {
                var uri = new UriBuilder(url);
                uri.Host = "209.127.116.78";
                uri.Port = 8008;
                uri.Scheme = "http";
                return uri.ToString();
            }catch(Exception ex)
            {
                Debug.WriteLine($" wrng:{url}");
                return url;
            }
        }
    }
}
