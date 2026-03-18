using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterCityRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterCityRepo>? _logger;

        public MasterCityRepo(IDapperHelper dapperHelper, ILogger<MasterCityRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterCity>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterCity>("usp_Master_City_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CityRepo.GetAllAsync");
                return Enumerable.Empty<MasterCity>();
            }
        }

        public async Task<MasterCity?> GetByIdAsync(int idCity)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCity", idCity);
                return await _dapper.QueryFirstOrDefaultAsync<MasterCity>("usp_Master_City_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CityRepo.GetByIdAsync");
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterCity city)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCity", city.IDCity);
                param.Add("@IDState", city.IDState);
                param.Add("@City", city.City);
                param.Add("@UserName", city.E_By);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_City_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CityRepo.SaveAsync");
                return SaveResult.Fail("Failed to save city. " + ex.Message);
            }
        }

        public async Task<SaveResult> DeleteAsync(int idCity, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCity", idCity);
                param.Add("@DeletedBy", deletedBy);
                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_City_Delete", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CityRepo.DeleteAsync");
                return SaveResult.Fail("Failed to delete city. " + ex.Message);
            }
        }
    }
}
