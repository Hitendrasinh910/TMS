using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Models.Transaction;
using TMS.Services;

namespace TMS.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/transaction/payment-receive")]
    public class TransactionPaymentReceiveController : ControllerBase
    {
        private readonly TransactionPaymentReceiveService _service;
        public TransactionPaymentReceiveController(TransactionPaymentReceiveService service) => _service = service;

        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAll() => Ok(ApiResponse<IEnumerable<dynamic>>.SuccessResponse("Loaded", await _service.GetAllAsync()));

        //[HttpGet("get-by-id/{id:int}")]
        //public async Task<IActionResult> GetById(int id) => Ok(ApiResponse<TransactionPaymentReceive>.SuccessResponse("Loaded", await _service.GetByIdAsync(id)));

        // GET ALL Payments
        // ---------------------------------------------------------
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var payments = await _service.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<TransactionPaymentReceive>>.SuccessResponse(
                    "Payments loaded successfully", payments));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to load payments. " + ex.Message));
            }
        }

        [HttpGet("get-receipt-no")]
        public async Task<IActionResult> GetReceiptNo()
        {
            int receiptNo = await _service.GetReceiptNo();
            return Ok(new { success = true, data = receiptNo });
        }

        // ---------------------------------------------------------
        // GET Payments BY ID
        // ---------------------------------------------------------
        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var payment = await _service.GetByIdAsync(id);
                if (payment == null)
                    return Ok(ApiResponse<string>.FailResponse("Payment not found"));

                return Ok(ApiResponse<TransactionPaymentReceive>.SuccessResponse(
                    "Payment loaded successfully", payment));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to load user. " + ex.Message));
            }
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] TransactionPaymentReceive payment)
        {
            payment.E_By = ClaimsHelper.GetCurrentUserFullName(User);
            var res = await _service.SaveAsync(payment);
            return res.Result == 1 ? Ok(ApiResponse<object>.SuccessResponse(res.Message, new { newId = res.NewId })) : Ok(ApiResponse<string>.FailResponse(res.Message));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteAsync(id, ClaimsHelper.GetCurrentUserFullName(User));
            return res.Result == 1 ? Ok(ApiResponse<string>.SuccessResponse(res.Message)) : Ok(ApiResponse<string>.FailResponse(res.Message));
        }
    }
}
