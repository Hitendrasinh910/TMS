using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterPartyAccountRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterPartyAccountRepo>? _logger;

        public MasterPartyAccountRepo(IDapperHelper dapperHelper, ILogger<MasterPartyAccountRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterPartyAccount>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterPartyAccount>("usp_Master_PartyAccount_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PartyAccountRepo.GetAllAsync");
                return Enumerable.Empty<MasterPartyAccount>();
            }
        }

        public async Task<MasterPartyAccount?> GetByIdAsync(int idPartyAccount)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDPartyAccount", idPartyAccount);
                return await _dapper.QueryFirstOrDefaultAsync<MasterPartyAccount>("usp_Master_PartyAccount_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PartyAccountRepo.GetByIdAsync | IDPartyAccount={IDPartyAccount}", idPartyAccount);
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterPartyAccount account)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDPartyAccount", account.IDPartyAccount);
                param.Add("@AccountSrNo", account.AccountSrNo);
                param.Add("@PartyCode", account.PartyCode);
                param.Add("@IDAccountType", account.IDAccountType);
                param.Add("@PartyName", account.PartyName);
                param.Add("@Address", account.Address);
                param.Add("@IDState", account.IDState);
                param.Add("@IDCity", account.IDCity);
                param.Add("@ContactNo1", account.ContactNo1);
                param.Add("@ContactNo2", account.ContactNo2);
                param.Add("@Email", account.Email);
                param.Add("@OpeningBalance", account.OpeningBalance);
                param.Add("@IDBalanceType", account.IDBalanceType);
                param.Add("@GSTNo", account.GSTNo);
                param.Add("@PanNo", account.PanNo);
                param.Add("@UserName", account.E_By);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_PartyAccount_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PartyAccountRepo.SaveAsync | IDPartyAccount={IDPartyAccount}", account.IDPartyAccount);
                return SaveResult.Fail("Failed to save party account. " + ex.Message);
            }
        }

        public async Task<SaveResult> DeleteAsync(int idPartyAccount, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDPartyAccount", idPartyAccount);
                param.Add("@DeletedBy", deletedBy);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_PartyAccount_Delete", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PartyAccountRepo.DeleteAsync | IDPartyAccount={IDPartyAccount}", idPartyAccount);
                return SaveResult.Fail("Failed to delete party account. " + ex.Message);
            }
        }
    }
}
