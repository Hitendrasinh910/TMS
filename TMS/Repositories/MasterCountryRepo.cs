using Dapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterCountryRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterCountryRepo>? _logger;

        public MasterCountryRepo(IDapperHelper dapperHelper, ILogger<MasterCountryRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterCountry>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterCountry>("usp_Master_Country_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CountryRepo.GetAllAsync");
                return Enumerable.Empty<MasterCountry>();
            }
        }

        public async Task<MasterCountry?> GetByIdAsync(int idCountry)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCountry", idCountry);
                return await _dapper.QueryFirstOrDefaultAsync<MasterCountry>("usp_Master_Country_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CountryRepo.GetByIdAsync | IDCountry={IDCountry}", idCountry);
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterCountry country)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCountry", country.IDCountry);
                param.Add("@Country", country.Country);
                param.Add("@UserName", country.E_By); // Inherited from AuditFields

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Country_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CountryRepo.SaveAsync | IDCountry={IDCountry}", country.IDCountry);
                return SaveResult.Fail("Failed to save country. " + ex.Message);
            }
        }

        public async Task<SaveResult> DeleteAsync(int idCountry, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDCountry", idCountry);
                param.Add("@DeletedBy", deletedBy);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Country_Delete", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in CountryRepo.DeleteAsync | IDCountry={IDCountry}", idCountry);
                return SaveResult.Fail("Failed to delete country. " + ex.Message);
            }
        }
    }
}
