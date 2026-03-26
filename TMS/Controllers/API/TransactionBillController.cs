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
    [Route("api/transaction/bill")]
    public class TransactionBillController : ControllerBase
    {
        private readonly TransactionBillService _service;

        public TransactionBillController(TransactionBillService service)
        {
            _service = service;
        }

        // ---------------------------------------------------------
        // GET ALL
        // ---------------------------------------------------------
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<TransactionBill>>.SuccessResponse("Bills loaded successfully", data));
        }

        [HttpGet("get-bill-no")]
        public async Task<IActionResult> GetBillNo()
        {
            int billNo = await _service.GetBillNo();
            return Ok(new { success = true, data = billNo });
        }

        // ---------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------
        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
            {
                return Ok(ApiResponse<string>.FailResponse("Bill not found"));
            }
            return Ok(ApiResponse<TransactionBillDto>.SuccessResponse("Bill loaded successfully", data));
        }

        // ---------------------------------------------------------
        // SAVE
        // ---------------------------------------------------------
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] TransactionBillDto dto)
        {
            var userName = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _service.SaveAsync(dto, userName);

            if (result.Result != 1)
            {
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            }

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId }));
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userName = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _service.DeleteAsync(id, userName);

            if (result.Result != 1)
            {
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));
            }

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
