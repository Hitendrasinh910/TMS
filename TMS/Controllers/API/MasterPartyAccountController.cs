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
    [Route("api/master/party-account")]
    public class MasterPartyAccountController : ControllerBase
    {
        private readonly MasterPartyAccountService _partyAccountService;

        public MasterPartyAccountController(MasterPartyAccountService partyAccountService)
        {
            _partyAccountService = partyAccountService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _partyAccountService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<MasterPartyAccount>>.SuccessResponse("Party accounts loaded successfully", accounts));
        }

        [HttpGet("get-all-consignor")]
        public async Task<IActionResult> GetAllConsignor()
        {
            var accounts = await _partyAccountService.GetByAccountTypeAsync("consignor");
            return Ok(ApiResponse<IEnumerable<MasterPartyAccount>>.SuccessResponse("Party accounts loaded successfully", accounts));
        }

        [HttpGet("get-all-consignee")]
        public async Task<IActionResult> GetAllConsignee()
        {
            var accounts = await _partyAccountService.GetByAccountTypeAsync("consignee");
            return Ok(ApiResponse<IEnumerable<MasterPartyAccount>>.SuccessResponse("Party accounts loaded successfully", accounts));
        }

        [HttpGet("get-all-account-type")]
        public async Task<IActionResult> GetAllAccountType()
        {
            var accountTypes = await _partyAccountService.GetAllAccountTypeAsync();
            return Ok(ApiResponse<IEnumerable<MasterAccountType>>.SuccessResponse("AccountType loaded successfully", accountTypes));
        }

        [HttpGet("get-sr-no")]
        public async Task<IActionResult> GetSrNo()
        {
            int nextSrNo = await _partyAccountService.GetSrNo();
            return Ok(new { success = true, data = nextSrNo });
        }

        [HttpGet("get-all-balance-type")]
        public async Task<IActionResult> GetAllBalanceType()
        {
            var balanceTypes = await _partyAccountService.GetAllBalanceTypeAsync();
            return Ok(ApiResponse<IEnumerable<MasterBalanceType>>.SuccessResponse("BalanceType loaded successfully", balanceTypes));
        }

        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var account = await _partyAccountService.GetByIdAsync(id);
            if (account == null)
                return Ok(ApiResponse<string>.FailResponse("Party account not found"));

            return Ok(ApiResponse<MasterPartyAccount>.SuccessResponse("Party account loaded successfully", account));
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterPartyAccount account)
        {
            var actionBy = ClaimsHelper.GetCurrentUserFullName(User);
            account.E_By = actionBy;

            var result = await _partyAccountService.SaveAsync(account);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<object>.SuccessResponse(result.Message, new { newId = result.NewId, data = account }));
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedBy = ClaimsHelper.GetCurrentUserFullName(User);
            var result = await _partyAccountService.DeleteAsync(id, deletedBy);

            if (result.Result != 1)
                return Ok(ApiResponse<string>.FailResponse(result.Message, result.ErrorCode));

            return Ok(ApiResponse<string>.SuccessResponse(result.Message));
        }
    }
}
