using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Services;

namespace TMS.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/master/country")]
    public class MasterCountryController : ControllerBase
    {
        private readonly MasterCountryService _countryService;

        public MasterCountryController(MasterCountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _countryService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MasterCountry>>.SuccessResponse("Countries loaded successfully", countries));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _countryService.GetByIdAsync(id);
            if (country == null)
                return Ok(ApiResponse<string>.FailResponse("Country not found"));

            return Ok(ApiResponse<MasterCountry>.SuccessResponse("Country loaded successfully", country));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterCountry country)
        {
            var actionBy = ClaimsHelper.GetCurrentUserFullName(User);
            country.E_By = actionBy;

            var result = await _countryService.SaveAsync(country);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = country }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _countryService.DeleteAsync(id, deletedBy);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
 