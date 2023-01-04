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

namespace TireProjectNewWebApi.Services
{
    public class MainService
    {
        private readonly IMongoCollection<ReportData> _ReportDatas;
        public MainService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MainString"));
            var database = client.GetDatabase("TireDatabase");
            _ReportDatas = database.GetCollection<ReportData>("TireCollection");
            //var database = client.GetDatabase("TireAppMediaDatabase");
            //_ReportDatas = database.GetCollection<ReportData>("TireAppMediaCollection");

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
                if(item.Pic1!=null)
                    item.Pic1 = item.Pic1.Replace("https://tireprojectwebapi.azurewebsites.net", "https://test1.innovativebuddy.com");
                if (item.Pic2 != null)
                    item.Pic2 = item.Pic2.Replace("https://tireprojectwebapi.azurewebsites.net", "https://test1.innovativebuddy.com");
                if (item.Pic3 != null)
                    item.Pic3 = item.Pic3.Replace("https://tireprojectwebapi.azurewebsites.net", "https://test1.innovativebuddy.com");
                if (item.Pic4 != null)
                    item.Pic4 = item.Pic4.Replace("https://tireprojectwebapi.azurewebsites.net", "https://test1.innovativebuddy.com");
                if (item.CustomerSign != null)
                    item.CustomerSign = item.CustomerSign.Replace("https://tireprojectwebapi.azurewebsites.net", "https://test1.innovativebuddy.com");
                Update(item.Id, item);
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
                if(item.Pic1!=null)
                    item.Pic1 = item.Pic1.Replace("https://tireproject.", "https://tireprojectwebapi.");
                if (item.Pic2 != null)
                    item.Pic2 = item.Pic2.Replace("https://tireproject.", "https://tireprojectwebapi.");
                if (item.Pic3 != null)
                    item.Pic3 = item.Pic3.Replace("https://tireproject.", "https://tireprojectwebapi.");
                if (item.Pic4 != null)
                    item.Pic4 = item.Pic4.Replace("https://tireproject.", "https://tireprojectwebapi.");
				if (item.CustomerSign != null)
					item.CustomerSign = item.CustomerSign.Replace("https://tireproject.", "https://tireprojectwebapi.");
				_ReportDatas.ReplaceOne(ReportData => ReportData.Id == item.Id, item);
            }
            return "string";
        }
    }
}
