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
    [Route("api/transaction/challan")]
    public class TransactionChallanController : ControllerBase
    {
        private readonly TransactionChallanService _service;

        public TransactionChallanController(TransactionChallanService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<TransactionChallan>>.SuccessResponse("Challans loaded successfully", data));
        }

        [HttpGet("get-voucher-no")]
        public async Task<IActionResult> GetVoucherNo()
        {
            int voucherNo = await _service.GetVoucherNo();
            return Ok(new { success = true, data = voucherNo });
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return Ok(ApiResponse<string>.FailResponse("Challan not found"));
            }

            return Ok(ApiResponse<TransactionChallanDto>.SuccessResponse("Challan loaded successfully", data));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] TransactionChallanDto dto)
        {
            var userName = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _service.SaveAsync(dto, userName);

            if (result.Result == 1)
            {
                return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId }));
            }
            else
            {
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            }
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userName = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _service.DeleteAsync(id, userName);

            if (result.Result == 1)
            {
                return Ok(ApiResponse<string>.SuccessResponse(result.Message));
            }
            else
            {
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            }
        }
    }
}
