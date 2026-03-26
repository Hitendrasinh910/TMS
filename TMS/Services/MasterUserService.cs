using DRSPortal.Models.Account;
using TMS.Helpers;
using TMS.Models.Account;
using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterUserService
    {
        private readonly MasterUserRepo _repo;
        private readonly JwtHelper _jwtHelper;

        public MasterUserService(
            MasterUserRepo repo,
            JwtHelper jwtHelper)
        {
            _repo = repo;
            _jwtHelper = jwtHelper;
        }

        // ---------------------------------------------------------
        // GET ALL USERS
        // ---------------------------------------------------------
        public Task<IEnumerable<MasterUser>> GetAllAsync()
            => _repo.GetAllAsync();

        // GET UserType
        public async Task<IEnumerable<MasterUserType>> GetAllUserTypeAsync() => await _repo.GetAllUserTypeAsync();  // User Type

        // ---------------------------------------------------------
        // GET USER BY ID
        // ---------------------------------------------------------
        public Task<MasterUser?> GetByIdAsync(int idUser)
            => _repo.GetByIdAsync(idUser);

        // --------------------------------------------------------
        // GET USER BY ROLE
        // -------------------------------------------------------
        public Task<IEnumerable<MasterUser>> GetByRoleAsync(string roleName)
            => _repo.GetByRoleAsync(roleName);

        // --------------------------------------------------------
        // GET USER BY ROLE LOCATION COMPANY
        // --------------------------------------------------------
        public Task<IEnumerable<MasterUser>> GetByRoleLocationCompanyAsync(string roleName, int idLocation, int idCompany)
            => _repo.GetByRoleLocationCompanyAsync(roleName, idLocation, idCompany);

        // ---------------------------------------------------------
        // SAVE USER
        // ---------------------------------------------------------
        public async Task<SaveResult> SaveAsync(MasterUser user, string actionBy, int? actionById = null)
        {
            // 🔒 Basic validations
            if (string.IsNullOrWhiteSpace(user.UserName))
                return SaveResult.Fail("Username is required");

            if (user.IDUser == 0 && string.IsNullOrWhiteSpace(user.Password))
                return SaveResult.Fail("Password is required");

            // Assign Audit Info
            user.E_By = actionBy;
            user.E_ById = actionById;

            return await _repo.SaveAsync(user);
        }

        // ---------------------------------------------------------
        // DELETE USER
        // ---------------------------------------------------------
        public Task<SaveResult> DeleteAsync(int idUser, string deletedBy, int? deletedById = null)
            => _repo.DeleteAsync(idUser, deletedBy, deletedById);

        // ---------------------------------------------------------
        // LOGIN
        // ---------------------------------------------------------
        public async Task<LoginResponse> LoginAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Username and password required"
                };
            }

            var user = await _repo.LoginAsync(userName, password);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // 🔑 Generate JWT
            // Note: Uses null-coalescing to ensure IDs are passed correctly to the helper
            var token = _jwtHelper.GenerateToken(
                user.IDUser,
                user.UserName,
                user.FullName,
                user.UserType, // Assuming UserType acts as the Role in your Master_User table
                0,             // Default Company if not in table
                0);            // Default Location if not in table

            return new LoginResponse
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(4),
                User = MapToJwtUser(user)
            };
        }

        // ---------------------------------------------------------
        // MAP DB USER → JWT USER
        // ---------------------------------------------------------
        private static JwtUser MapToJwtUser(MasterUser user)
        {
            return new JwtUser
            {
                IDUser = user.IDUser,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email ?? "",
                Mobile = user.ContactNo ?? "", // Mapping ContactNo to the Mobile field in JwtUser
                Role = user.UserType           // Mapping UserType to the Role field in JwtUser
            };
        }
    }
}
