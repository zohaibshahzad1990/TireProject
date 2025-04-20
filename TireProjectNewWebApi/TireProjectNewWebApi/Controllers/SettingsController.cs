using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Threading.Tasks;
using TireProjectNewWebApi.Models;
using TireProjectNewWebApi.Services;

namespace TireProjectNewWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly MainService _mainService;

        public SettingsController(MainService mainService)
        {
            _mainService = mainService;
            
        }

        [HttpGet]
        public async Task<ActionResult<Settings>> GetSettings()
        {
            var settings = await _mainService.GetSettings();
            if (settings == null)
                return NotFound();

            return settings;
        }

        [HttpPost]
        public async Task<IActionResult> SaveSettings(Settings settings)
        {
            await _mainService.SaveSettings(settings);

            return Ok();
        }
    }
}
