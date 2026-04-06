using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS.Helpers;
using TMS.Models.Account;
using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Services;

namespace TMS.Controllers.API
{
    //[Authorize]
    [ApiController]
    [Route("api/master/user")]
    public class MasterUserController : ControllerBase
    {
        private readonly MasterUserService _userService;

        public MasterUserController(MasterUserService userService)
        {
            _userService = userService;
        }

        // ---------------------------------------------------------
        // GET ALL USERS
        // ---------------------------------------------------------
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(ApiResponse<IEnumerable<MasterUser>>.SuccessResponse(
                    "Users loaded successfully", users));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to load users. " + ex.Message));
            }
        }

        // UserType dropdown
        [HttpGet("get-all-user-type")]
        public async Task<IActionResult> GetAllUserType()
        {
            var userType = await _userService.GetAllUserTypeAsync();
            return Ok(ApiResponse<IEnumerable<MasterUserType>>.SuccessResponse("UserType loaded successfully", userType));
        }

        // ---------------------------------------------------------
        // GET USER BY ID
        // ---------------------------------------------------------
        [HttpGet("get-by-id/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                    return Ok(ApiResponse<string>.FailResponse("User not found"));

                return Ok(ApiResponse<MasterUser>.SuccessResponse(
                    "User loaded successfully", user));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to load user. " + ex.Message));
            }
        }

        // ---------------------------------------------------------
        // SAVE USER (INSERT / UPDATE)
        // ---------------------------------------------------------
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] MasterUser user)
        {
            try
            {
                // Logic to get the current user performing the action
                var actionBy = ClaimsHelper.GetCurrentUserName(User);
                var actionById = ClaimsHelper.GetCurrentUserId(User);

                var result = await _userService.SaveAsync(user, actionBy, actionById);

                if (!result.IsSuccess)
                    return Ok(ApiResponse<string>.FailResponse(result.Message));

                return Ok(ApiResponse<object>.SuccessResponse(
                    result.Message,
                    new
                    {
                        newId = result.NewId,
                        data = user
                    }));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to save user. " + ex.Message));
            }
        }

        // ---------------------------------------------------------
        // DELETE USER (SOFT DELETE)
        // ---------------------------------------------------------
        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deletedBy = ClaimsHelper.GetCurrentUserName(User);
                var deletedById = ClaimsHelper.GetCurrentUserId(User);

                var result = await _userService.DeleteAsync(id, deletedBy, deletedById);

                if (!result.IsSuccess)
                    return Ok(ApiResponse<string>.FailResponse(result.Message));

                return Ok(ApiResponse<string>.SuccessResponse(result.Message));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Failed to delete user. " + ex.Message));
            }
        }

        // ---------------------------------------------------------
        // LOGIN (PUBLIC ACCESS)
        // ---------------------------------------------------------
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await _userService.LoginAsync(request.UserName, request.Password);

                if (!result.Success)
                    return Ok(ApiResponse<LoginResponse>.FailResponse(result.Message));

                return Ok(ApiResponse<LoginResponse>.SuccessResponse("Login successful", result));
            }
            catch (Exception ex)
            {
                return Ok(ApiResponse<string>.FailResponse("Login failed. " + ex.Message));
            }
        }
    }
}
