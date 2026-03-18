using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Services;

namespace TMS.Controllers.API
{
    //[Authorize]
    [ApiController]
    [Route("api/master/city")]
    public class MasterCityController : ControllerBase
    {
        private readonly MasterCityService _cityService;

        public MasterCityController(MasterCityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var cities = await _cityService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MasterCity>>.SuccessResponse("Cities loaded successfully", cities));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var city = await _cityService.GetByIdAsync(id);
            if (city == null) return Ok(ApiResponse<string>.FailResponse("City not found"));
            return Ok(ApiResponse<MasterCity>.SuccessResponse("City loaded successfully", city));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterCity city)
        {
            city.E_By = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _cityService.SaveAsync(city);

            if (result.Result != 1) return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = city }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _cityService.DeleteAsync(id, ClaimsHelper.GetCurrentUserFullName(User));
            if (result.Result != 1) return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
