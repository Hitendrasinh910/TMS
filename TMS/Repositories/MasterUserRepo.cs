using Dapper;
using Microsoft.Data.SqlClient;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterUserRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterUserRepo>? _logger;

        public MasterUserRepo(
            IDapperHelper dapperHelper,
            ILogger<MasterUserRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        // ---------------------------------------------------------
        // GET ALL USERS
        // ---------------------------------------------------------
        public async Task<IEnumerable<MasterUser>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterUser>(
                    "usp_Master_User_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.GetAllAsync");
                return Enumerable.Empty<MasterUser>();
            }
        }

        // ---------------------------------------------------------
        // GET USER BY ID
        // ---------------------------------------------------------
        public async Task<MasterUser?> GetByIdAsync(int idUser)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDUser", idUser);

                return await _dapper.QueryFirstOrDefaultAsync<MasterUser>(
                    "usp_Master_User_SelectById",
                    param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.GetByIdAsync | IDUser={IDUser}", idUser);
                return null;
            }
        }

        // --------------------------------------------------------
        // GET USER BY ROLE
        // ----------------------------------------------------------
        public async Task<IEnumerable<MasterUser>> GetByRoleAsync(string roleName)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RoleName", roleName);

                return await _dapper.QueryAsync<MasterUser>(
                    "usp_Master_User_SelectByRole",
                    param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.GetByRoleAsync | RoleName={RoleName}", roleName);
                return Enumerable.Empty<MasterUser>();
            }
        }

        // --------------------------------------------------------
        // GET USER BY ROLE LOCATION COMPANY
        // ----------------------------------------------------------
        public async Task<IEnumerable<MasterUser>> GetByRoleLocationCompanyAsync(string roleName, int idLocation, int idCompany)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@RoleName", roleName);
                param.Add("@IDLocation", idLocation);
                param.Add("@IDCompany", idCompany);

                return await _dapper.QueryAsync<MasterUser>(
                    "usp_Master_User_SelectByRoleLocationCompany",
                    param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.GetByRoleLocationCompanyAsync | RoleName={RoleName}", roleName);
                return Enumerable.Empty<MasterUser>();
            }
        }

        // ---------------------------------------------------------
        // SAVE USER
        // ---------------------------------------------------------
        public async Task<SaveResult> SaveAsync(MasterUser user)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDUser", user.IDUser);
                param.Add("@UserType", user.UserType);
                param.Add("@UserName", user.UserName);
                param.Add("@Password", user.Password);
                param.Add("@FullName", user.FullName);
                param.Add("@Email", user.Email);
                param.Add("@ContactNo", user.ContactNo); // Updated from Mobile

                // Audit Fields (Assumed from AuditFields base class)
                param.Add("@ActionBy", user.E_By);
                param.Add("@ActionById", user.E_ById);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>(
                    "usp_Master_User_Save",
                    param);

                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "SQL Error in MasterUserRepo.SaveAsync | IDUser={IDUser}", user.IDUser);
                return SaveResult.Fail(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.SaveAsync | IDUser={IDUser}", user.IDUser);
                return SaveResult.Fail("Unexpected error occurred.");
            }
        }

        // ---------------------------------------------------------
        // DELETE USER
        // ---------------------------------------------------------
        public async Task<SaveResult> DeleteAsync(int idUser, string deletedBy, int? deletedById = null)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDUser", idUser);
                param.Add("@ActionBy", deletedBy);
                param.Add("@ActionById", deletedById);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>(
                    "usp_Master_User_Delete",
                    param);

                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (SqlException sqlEx)
            {
                _logger?.LogError(sqlEx, "SQL Error in MasterUserRepo.DeleteAsync | IDUser={IDUser}", idUser);
                return SaveResult.Fail(sqlEx.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.DeleteAsync | IDUser={IDUser}", idUser);
                return SaveResult.Fail("Unexpected error occurred.");
            }
        }

        // ---------------------------------------------------------
        // LOGIN VALIDATION
        // ---------------------------------------------------------
        public async Task<MasterUser?> LoginAsync(string userName, string password)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@UserName", userName);
                param.Add("@Password", password);

                return await _dapper.QueryFirstOrDefaultAsync<MasterUser>(
                    "usp_Master_User_Login",
                    param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in MasterUserRepo.LoginAsync | UserName={UserName}", userName);
                return null;
            }
        }
    }
}
