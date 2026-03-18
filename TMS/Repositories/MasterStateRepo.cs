using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterStateRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterStateRepo>? _logger;

        public MasterStateRepo(IDapperHelper dapperHelper, ILogger<MasterStateRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterState>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterState>("usp_Master_State_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in StateRepo.GetAllAsync");
                return Enumerable.Empty<MasterState>();
            }
        }

        public async Task<MasterState?> GetByIdAsync(int idState)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDState", idState);
                return await _dapper.QueryFirstOrDefaultAsync<MasterState>("usp_Master_State_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in StateRepo.GetByIdAsync | IDState={IDState}", idState);
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterState state)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDState", state.IDState);
                param.Add("@IDCountry", state.IDCountry);
                param.Add("@State", state.State);
                param.Add("@UserName", state.E_By);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_State_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in StateRepo.SaveAsync | IDState={IDState}", state.IDState);
                return SaveResult.Fail("Failed to save state. " + ex.Message);
            }
        }

        public async Task<SaveResult> DeleteAsync(int idState, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDState", idState);
                param.Add("@DeletedBy", deletedBy);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_State_Delete", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in StateRepo.DeleteAsync | IDState={IDState}", idState);
                return SaveResult.Fail("Failed to delete state. " + ex.Message);
            }
        }
    }
}
