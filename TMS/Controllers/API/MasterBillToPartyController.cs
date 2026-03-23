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
    [Route("api/master/bill-to-party")]
    public class MasterBillToPartyController : ControllerBase
    {
        private readonly MasterBillToPartyService _billService;
        public MasterBillToPartyController(MasterBillToPartyService billService) => _billService = billService;

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll() => Ok(ApiResponse<IEnumerable<dynamic>>.SuccessResponse("Loaded", await _billService.GetAllAsync()));

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id) => Ok(ApiResponse<MasterBillToParty>.SuccessResponse("Loaded", await _billService.GetByIdAsync(id)));

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterBillToParty bill)
        {
            bill.E_By = ClaimsHelper.GetCurrentUserFullName(User);
            var res = await _billService.SaveAsync(bill);
            return res.Result == 1 ? Ok(ApiResponse<object>.SuccessResponse(res.Message, new { newId = res.NewId })) : Ok(ApiResponse<string>.FailResponse(res.Message));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _billService.DeleteAsync(id, ClaimsHelper.GetCurrentUserFullName(User));
            return res.Result == 1 ? Ok(ApiResponse<string>.SuccessResponse(res.Message)) : Ok(ApiResponse<string>.FailResponse(res.Message));
        }
    }
}
