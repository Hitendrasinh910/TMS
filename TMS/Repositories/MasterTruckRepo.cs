using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Master;

namespace TMS.Repositories
{
    public class MasterTruckRepo
    {
        private readonly IDapperHelper _dapper;
        public MasterTruckRepo(IDapperHelper dapperHelper) => _dapper = dapperHelper;

        public async Task<IEnumerable<MasterTruck>> GetAllAsync() => await _dapper.QueryAsync<MasterTruck>("usp_Master_Truck_Select");

        public async Task<MasterTruck?> GetByIdAsync(int idTruck)
        {
            var param = new DynamicParameters();
            param.Add("@IDTruck", idTruck);
            return await _dapper.QueryFirstOrDefaultAsync<MasterTruck>("usp_Master_Truck_SelectById", param);
        }

        public async Task<SaveResult> SaveAsync(MasterTruck truck)
        {
            var param = new DynamicParameters();
            param.Add("@IDTruck", truck.IDTruck);
            param.Add("@TruckNumber", truck.TruckNumber);
            param.Add("@PanCardHolder", truck.PanCardHolder);
            param.Add("@PanCardNo", truck.PanCardNo);
            param.Add("@Remarks", truck.Remarks);
            param.Add("@UserName", truck.E_By);
            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Truck_Save", param);
            return result ?? SaveResult.Fail("No response");
        }

        public async Task<SaveResult> DeleteAsync(int idTruck, string deletedBy)
        {
            var param = new DynamicParameters();
            param.Add("@IDTruck", idTruck);
            param.Add("@DeletedBy", deletedBy);
            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Master_Truck_Delete", param);
            return result ?? SaveResult.Fail("No response");
        }
    }
}
