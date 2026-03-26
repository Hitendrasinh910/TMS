using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterBillToPartyRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<MasterBillToPartyRepo>? _logger;


        public MasterBillToPartyRepo(IDapperHelper dapperHelper, ILogger<MasterBillToPartyRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        public async Task<IEnumerable<MasterBillToParty>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<MasterBillToParty>("usp_Master_BillToParty_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BillToParty.GetAllAsync");
                return Enumerable.Empty<MasterBillToParty>();
            }
        }

        public async Task<int> GetSrNoAsync()
        {
            try
            {
                // Calls a stored procedure to get Max(SrNo) + 1
                return await _dapper.QueryFirstOrDefaultAsync<int>("usp_Master_BillToParty_SelectSrNo");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BillToParty.GetSrNoAsync");
                // Return 1 as a fallback (assuming it's the first record)
                return 1;
            }
        }

        public async Task<MasterBillToParty> GetByIdAsync(int idBillToParty)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDPartyBill", idBillToParty);
                return await _dapper.QueryFirstOrDefaultAsync<MasterBillToParty>("usp_Master_BillToParty_SelectById", param);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BillToParty.GetByIdAsync | IDPartyBill={IDPartyBill}", idBillToParty);
                return null;
            }
        }

        public async Task<SaveResult> SaveAsync(MasterBillToParty billToParty)
        {
            var param = new DynamicParameters();
            param.Add("@IDPartyBill", billToParty.IDPartyBill);
            param.Add("@SrNo", billToParty.SrNo);
            param.Add("@IDConsignor", billToParty.IDConsignor);
            param.Add("@IDConsignee", billToParty.IDConsignee);
            param.Add("@BillTo", billToParty.BillTo);
            param.Add("@Remarks", billToParty.Remarks);
            param.Add("@UserName", billToParty.E_By);

            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_BillToParty_Save", param);
            return result ?? SaveResult.Fail("No response");
        }

        public async Task<SaveResult> DeleteAsync(int idPartyBill, string deletedBy)
        {
            var param = new DynamicParameters();
            param.Add("@IDPartyBill", idPartyBill);
            param.Add("@DeletedBy", deletedBy);

            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_BillToParty_Delete", param);
            return result ?? SaveResult.Fail("No response");
        }
    }
}
