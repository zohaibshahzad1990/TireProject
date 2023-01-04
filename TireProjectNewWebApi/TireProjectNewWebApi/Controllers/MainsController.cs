using System.Collections.Generic;
using TireProjectNewWebApi.Models;
using TireProjectNewWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace TireProjectNewWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainsController : ControllerBase
    {
        private readonly MainService _mainService;
        public static IWebHostEnvironment _environment;
        
        public MainsController(MainService mainService, IWebHostEnvironment environment)
        {
            _mainService = mainService;
            _environment = environment;
        }

        [HttpGet("backup")]
        public void GetBackupStart()
        {
            _mainService.BackupFunction(_environment.WebRootPath);
        }

        [HttpGet("updatenew")]
        public ActionResult<string> GetUUNew()
        {
            return _mainService.GetUpdateNew();
        }

        [HttpGet("emailsent/{subject}/{body}/")]
        public ActionResult<string> GetEmailSent(string subject,string body)
        {
            return _mainService.SendMail(_environment.WebRootPath,subject,body);
        }

        [HttpGet]
        public ActionResult<List<ReportData>> Get()
        {
            return _mainService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetMain")]
        public ActionResult<ReportData> Get(string id)
        {
            var main = _mainService.Get(id);

            if (main == null)
            {
                return NotFound();
            }

            return main;
        }

        [HttpPost]
        public ActionResult<ReportData> Create(ReportData main)
        {
            _mainService.Create(main);

            return CreatedAtRoute("GetMain", new { id = main.Id.ToString() }, main);
        }

        [HttpPost("{id:length(24)}")]
        public IActionResult Update(string id, ReportData mainIn)
        {
            var main = _mainService.Get(id);

            if (main == null)
            {
                return NotFound();
            }

            _mainService.Update(id, mainIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var main = _mainService.Get(id);

            if (main == null)
            {
                return NotFound();
            }

            _mainService.Remove(main.Id);

            return NoContent();
        }

        [HttpGet("delete/{id:length(24)}")]
        public IActionResult DeleteWithGet(string id)
        {
            var main = _mainService.Get(id);

            if (main == null)
            {
                return NotFound();
            }

            _mainService.Remove(id);

            return NoContent();
        }


        [HttpGet("getalldata/")]
        public ActionResult<List<ReportData>> GetData()
        {
            return _mainService.GetAllData();
        }
        [HttpGet("getalldata/{start}/{end}/{sort}/")]
        public ActionResult<List<ReportData>> GetDataPage(int start,int end,int sort)
        {
            return _mainService.GetAllDataPagination(start,end,sort);
        }
        [HttpGet("getsearchdata/{search}/{sort}/")]
        public ActionResult<List<ReportData>> GetSearch(string search,int sort)
        {
            return _mainService.GetSearchData(search,sort);
        }
        [HttpGet("getsearchdata/{search}/{start}/{end}/")]
        public ActionResult<List<ReportData>> GetSearchPage(string search,int start, int end)
        {
            return _mainService.GetSearchDataPagination(search, start, end);
        }
        [HttpGet("customexport/{datefrom}/{dateto}/{ii}/{eTireSeason}/{warehouse}/")]
        public ActionResult<List<ReportData>> GetCustomExportData(string datefrom, string dateto,int ii, int eTireSeason, string warehouse)
        {
            return _mainService.GetCustomExportData(datefrom,dateto,ii,eTireSeason,warehouse);
        }

        [HttpPost("customexport/")]
        public ActionResult<List<ReportData>> GetCustomExportData1([FromBody] TempDataPass tempDataPass)
        {
            if (tempDataPass != null)
            {
                var resukt = _mainService.GetCustomExportData(tempDataPass.datefrom,
                tempDataPass.dateto,
                tempDataPass.ii, tempDataPass.eTireSeason, tempDataPass.warehouse);
                return resukt;
            }
            else return NotFound();
            
        }

        //Test
        [HttpGet("delall")]
        public string Deletedata()
        {
            return _mainService.DelAll();
        }
        [HttpGet("changedata")]
        public string DateAllGet()
        {
            var ll = _mainService.DateChangeFunc();
            return "success";
        }
    }
}