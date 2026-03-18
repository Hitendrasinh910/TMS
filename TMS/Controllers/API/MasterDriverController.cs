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
    [Route("api/master/driver")]
    public class MasterDriverController : ControllerBase
    {
        private readonly MasterDriverService _driverService;

        public MasterDriverController(MasterDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var drivers = await _driverService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MasterDriver>>.SuccessResponse("Drivers loaded successfully", drivers));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var driver = await _driverService.GetByIdAsync(id);
            if (driver == null) return Ok(ApiResponse<string>.FailResponse("Driver not found"));

            return Ok(ApiResponse<MasterDriver>.SuccessResponse("Driver loaded successfully", driver));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterDriver driver)
        {
            driver.E_By = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _driverService.SaveAsync(driver);

            if (result.Result != 1) return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = driver }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _driverService.DeleteAsync(id, ClaimsHelper.GetCurrentUserFullName(User));

            if (result.Result != 1) return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
