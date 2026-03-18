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
    [Route("api/master/state")]
    public class MasterStateController : ControllerBase
    {
        private readonly MasterStateService _stateService;

        public MasterStateController(MasterStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var states = await _stateService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MasterState>>.SuccessResponse("States loaded successfully", states));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var state = await _stateService.GetByIdAsync(id);
            if (state == null)
                return Ok(ApiResponse<string>.FailResponse("State not found"));

            return Ok(ApiResponse<MasterState>.SuccessResponse("State loaded successfully", state));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterState state)
        {
            var actionBy = ClaimsHelper.GetCurrentUserFullName(User);
            state.E_By = actionBy;

            var result = await _stateService.SaveAsync(state);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = state }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _stateService.DeleteAsync(id, deletedBy);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
