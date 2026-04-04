using Dapper;
using System.Data;
using TMS.Helpers;
using TMS.Models;
using TMS.Models.Common;

namespace TMS.Repositories
{
    public class UserRightsRepo
    {
        private readonly IDapperHelper _dapper;

        public UserRightsRepo(IDapperHelper dapperHelper) => _dapper = dapperHelper;

        public async Task<IEnumerable<UserRights>> GetUserRightsAsync(int idUser)
        {
            var param = new DynamicParameters();
            param.Add("@IDUser", idUser);
            return await _dapper.QueryAsync<UserRights>("usp_Extra_UserRights_GetByUser", param);
        }

        public async Task<SaveResult> SaveRightsAsync(UserRightSaveDto dto, string userName)
        {
            var dt = new DataTable();
            dt.Columns.Add("IDForms", typeof(int));
            dt.Columns.Add("AllowToView", typeof(bool));
            dt.Columns.Add("AllowToAdd", typeof(bool));
            dt.Columns.Add("AllowToUpdate", typeof(bool));
            dt.Columns.Add("AllowToDelete", typeof(bool));

            if (dto.Rights != null)
            {
                foreach (var r in dto.Rights)
                {
                    dt.Rows.Add(r.IDForms, r.AllowToView, r.AllowToAdd, r.AllowToUpdate, r.AllowToDelete);
                }
            }

            var param = new DynamicParameters();
            param.Add("@IDUser", dto.IDUser);
            param.Add("@UserName", userName);
            param.Add("@Rights", dt.AsTableValuedParameter("dbo.udt_UserRights"));

            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Extra_UserRights_Save", param);
            return result ?? SaveResult.Fail("No response");
        }
    }
}
