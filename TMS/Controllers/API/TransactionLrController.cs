using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Transaction;
using TMS.Services;

namespace TMS.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/transaction/lr")]
    public class TransactionLrController : ControllerBase
    {
        private readonly TransactionLrService _lrService;

        public TransactionLrController(TransactionLrService lrService)
        {
            _lrService = lrService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var lrs = await _lrService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<TransactionLR>>.SuccessResponse("LRs loaded successfully", lrs));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var lrDto = await _lrService.GetByIdAsync(id);
            if (lrDto == null)
                return Ok(ApiResponse<string>.FailResponse("LR not found"));

            return Ok(ApiResponse<TransactionLRDto>.SuccessResponse("LR loaded successfully", lrDto));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] TransactionLRDto dto)
        {
            var userName = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _lrService.SaveAsync(dto, userName);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = dto }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _lrService.DeleteAsync(id, deletedBy);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
