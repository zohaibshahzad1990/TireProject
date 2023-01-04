using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TireProjectNewWebApi.Models;
using TireProjectNewWebApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TireProjectNewWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DataUploadController : Controller
    {
        public DataUploadController(MainService mainService)
        {
            _mainService = mainService;
        }
        private readonly MainService _mainService;
        // GET: api/values
        [HttpGet]
        public async Task<string> Get()
        {
            var http = new HttpClient();
            var json = await http.GetStringAsync("https://tireprojectwebapi.azurewebsites.net/api/mains/");
            var ll = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ReportData>>(json);
            var listdata = new List<ReportData>();
            foreach (var item in ll)
            {
                listdata.Add(new ReportData
                {
                    Id = null,
                    CompanyLogo = item.CompanyLogo,
                    CompanyName = item.CompanyName,
                    CompanyAddress = item.CompanyAddress,
                    TermCondition = item.TermCondition,
                    RefNo = item.RefNo,
                    Date = item.Date,
                    ExtraRefNo = item.ExtraRefNo,
                    ExtraDate = item.ExtraDate,
                    FName = item.FName,
                    MName = item.MName,
                    LName = item.LName,
                    Address = item.Address,
                    PhoneNo = item.PhoneNo,
                    HomeNo = item.HomeNo,
                    WorkNo = item.WorkNo,
                    Email = item.Email,
                    PlateNo = item.PlateNo,
                    CarYear = item.CarYear,
                    CarBrand = item.CarBrand,
                    CarModel = item.CarModel,
                    TireSeason = item.TireSeason,
                    NoOfTires = item.NoOfTires,
                    TireSize1 = item.TireSize1,
                    TireSize2 = item.TireSize2,
                    TireSize3 = item.TireSize3,
                    TireSize4 = item.TireSize4,
                    IfStaggered = item.IfStaggered,
                    MakeModel = item.MakeModel,
                    RimAttached = item.RimAttached,
                    TypeRim = item.TypeRim,
                    DepthLF = item.DepthLF,
                    DepthLR = item.DepthLR,
                    DepthRF = item.DepthRF,
                    DepthRR = item.DepthRR,
                    BarcodeLF = item.BarcodeLF,
                    BarcodeLR = item.BarcodeLR,
                    BarcodeRF = item.BarcodeRF,
                    BarcodeRR = item.BarcodeRR,
                    TireStoredUpto = item.TireStoredUpto,
                    REP = item.REP,
                    Pic1 = item.Pic1,
                    Pic2 = item.Pic2,
                    Pic3 = item.Pic3,
                    Pic4 = item.Pic4,
                    CustomerSign = item.CustomerSign,
                    CusSignDate = item.CusSignDate
                });
            }

            foreach (var item in listdata)
            {
                _mainService.Create(item);
            }

            return "success";
        }
    }
}
