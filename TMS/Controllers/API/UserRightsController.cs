using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models;
using TMS.Models.Common;
using TMS.Services;

namespace TMS.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/auth/user-rights")]
    public class UserRightsController : ControllerBase
    {
        private readonly UserRightsService _service;

        public UserRightsController(UserRightsService service)
        {
            _service = service;
        }

        // ---------------------------------------------------------
        // GET RIGHTS FOR A SPECIFIC USER
        // ---------------------------------------------------------
        [HttpGet("get-by-user/{idUser:int}")]
        public async Task<IActionResult> GetByUser(int idUser)
        {
            var rightsData = await _service.GetUserRightsAsync(idUser);

            if (rightsData == null)
            {
                return Ok(ApiResponse<string>.FailResponse("Could not retrieve user rights."));
            }

            return Ok(ApiResponse<IEnumerable<UserRights>>.SuccessResponse("Rights loaded successfully", rightsData));
        }

        // ---------------------------------------------------------
        // SAVE ALL RIGHTS FOR A USER
        // ---------------------------------------------------------
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] UserRightSaveDto dto)
        {
            var actionByUserName = ClaimsHelper.GetCurrentUserFullName(User);

            var result = await _service.SaveRightsAsync(dto, actionByUserName);

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
