using Dapper;
using System.Data;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Transaction;

namespace TMS.Repositories
{
    public class TransactionBillRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<TransactionBillRepo>? _logger;

        public TransactionBillRepo(IDapperHelper dapperHelper, ILogger<TransactionBillRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        // ---------------------------------------------------------
        // GET ALL (For List Page)
        // ---------------------------------------------------------
        public async Task<IEnumerable<TransactionBill>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<TransactionBill>("usp_Transaction_Bill_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in TransactionBillRepo.GetAllAsync");
                return Enumerable.Empty<TransactionBill>();
            }
        }

        public async Task<int> GetBillNoAsync()
        {
            try
            {
                // Calls a stored procedure to get Max(SrNo) + 1
                return await _dapper.QueryFirstOrDefaultAsync<int>("usp_Transaction_Bill_SelectBillNo");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in BillRepo.GetBillNoAsync");
                // Return 1 as a fallback (assuming it's the first record)
                return 1;
            }
        }

        // ---------------------------------------------------------
        // GET BY ID (Header + Details for Edit)
        // ---------------------------------------------------------
        public async Task<TransactionBillDto?> GetByIdAsync(int idBill)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDBill", idBill);

                var multi = await _dapper.QueryMultipleAsync("usp_Transaction_Bill_SelectById", param);

                var header = await multi.Reader.ReadSingleOrDefaultAsync<TransactionBill>();
                if (header == null) return null;

                var details = (await multi.Reader.ReadAsync<TransactionBillDetail>()).ToList();

                return new TransactionBillDto
                {
                    Header = header,
                    Details = details
                };
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in TransactionBillRepo.GetByIdAsync | IDBill={IDBill}", idBill);
                return null;
            }
        }

        // ---------------------------------------------------------
        // SAVE (Header + TVP Details)
        // ---------------------------------------------------------
        public async Task<SaveResult> SaveAsync(TransactionBillDto dto, string userName)
        {
            try
            {
                var header = dto.Header;

                // 1. Convert List to DataTable for TVP
                var dtDetails = new DataTable();
                dtDetails.Columns.Add("IDLR", typeof(int));
                dtDetails.Columns.Add("Description", typeof(string));
                dtDetails.Columns.Add("IDFromCity", typeof(int));
                dtDetails.Columns.Add("IDToCity", typeof(int));
                dtDetails.Columns.Add("FreightOn", typeof(string));
                dtDetails.Columns.Add("FixAmt", typeof(decimal));
                dtDetails.Columns.Add("Weight", typeof(decimal));
                dtDetails.Columns.Add("Rate", typeof(decimal));
                dtDetails.Columns.Add("ExtraCharges", typeof(decimal));
                dtDetails.Columns.Add("Amount", typeof(decimal));
                dtDetails.Columns.Add("Remarks", typeof(string));

                if (dto.Details != null)
                {
                    foreach (var item in dto.Details)
                    {
                        dtDetails.Rows.Add(
                            item.IDLR,
                            item.Description,
                            item.IDFromCity,
                            item.IDToCity,
                            item.FreightOn,
                            item.FixAmt,
                            item.Weight,
                            item.Rate,
                            item.ExtraCharges,
                            item.Amount,
                            item.Remarks
                        );
                    }
                }

                // 2. Map Parameters
                var param = new DynamicParameters();
                param.Add("@IDBill", header.IDBill);
                param.Add("@BillNo", header.BillNo);
                param.Add("@BillDate", header.BillDate);
                param.Add("@IDBillToParty", header.IDBillToParty);
                param.Add("@IDTruck", header.IDTruck);
                param.Add("@DriverMobileNo", header.DriverMobileNo);
                param.Add("@FinalAmount", header.FinalAmount);
                param.Add("@ToPayAmount", header.ToPayAmount);
                param.Add("@CommissionAmt", header.CommissionAmt);
                param.Add("@Remarks", header.Remarks);
                param.Add("@UserName", userName);

                // Add the DataTable parameter
                param.Add("@BillDetails", dtDetails.AsTableValuedParameter("dbo.udt_Bill_Details"));

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_Bill_Save", param);

                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in TransactionBillRepo.SaveAsync");
                return SaveResult.Fail("Failed to save Bill. " + ex.Message);
            }
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------
        public async Task<SaveResult> DeleteAsync(int idBill, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDBill", idBill);
                param.Add("@DeletedBy", deletedBy);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_Bill_Delete", param);

                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in TransactionBillRepo.DeleteAsync | IDBill={IDBill}", idBill);
                return SaveResult.Fail("Failed to delete Bill. " + ex.Message);
            }
        }
    }
}
