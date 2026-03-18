using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterDriverRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterDriverRepo>? _logger;

        public MasterDriverRepo(IDapperHelper dapperHelper, ILogger<MasterDriverRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterDriver>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterDriver>("usp_Master_Driver_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in DriverRepo.GetAllAsync");
                return Enumerable.Empty<MasterDriver>();
            }
        }

        public async Task<MasterDriver?> GetByIdAsync(int idDriver)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDDriver", idDriver);
                return await _dapper.QueryFirstOrDefaultAsync<MasterDriver>("usp_Master_Driver_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in DriverRepo.GetByIdAsync");
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterDriver driver)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDDriver", driver.IDDriver);
                param.Add("@DriverName", driver.DriverName);
                param.Add("@Address", driver.Address);
                param.Add("@ContactNo", driver.ContactNo);
                param.Add("@EmergencyContactNo", driver.EmergencyContactNo);
                param.Add("@DrivingLicenceNo", driver.DrivingLicenceNo);
                param.Add("@DLValidTill", driver.DLValidTill);
                param.Add("@UserName", driver.E_By); // Inherited from AuditFields

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Driver_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in DriverRepo.SaveAsync");
                return SaveResult.Fail("Failed to save driver. " + ex.Message);
            }
        }

        public async Task<SaveResult> DeleteAsync(int idDriver, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDDriver", idDriver);
                param.Add("@DeletedBy", deletedBy);
                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Driver_Delete", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in DriverRepo.DeleteAsync");
                return SaveResult.Fail("Failed to delete driver. " + ex.Message);
            }
        }
    }
}
