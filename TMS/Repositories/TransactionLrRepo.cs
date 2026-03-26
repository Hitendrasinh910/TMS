using Dapper;
using System.Data;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Transaction;

namespace TMS.Repositories
{
    public class TransactionLrRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<TransactionLrRepo>? _logger;

        public TransactionLrRepo(IDapperHelper dapperHelper, ILogger<TransactionLrRepo>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        // ---------------------------------------------------------
        // GET ALL (Header Only for Grid)
        // ---------------------------------------------------------
        public async Task<IEnumerable<TransactionLR>> GetAllAsync()
        {
            try
            {
                // Assuming you'll create a simple usp_Transaction_LR_Select
                return await _dapper.QueryAsync<TransactionLR>("usp_Transaction_LR_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in LRRepo.GetAllAsync");
                return Enumerable.Empty<TransactionLR>();
            }
        }

        public async Task<int> GetLrNoAsync()
        {
            try
            {
                // Calls a stored procedure to get Max(SrNo) + 1
                return await _dapper.QueryFirstOrDefaultAsync<int>("usp_Transaction_LR_SelectLrNo");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in LRRepo.GetLrNoAsync");
                // Return 1 as a fallback (assuming it's the first record)
                return 1;
            }
        }

        // ---------------------------------------------------------
        // GET BY ID (Header + Details)
        // ---------------------------------------------------------
        //public async Task<TransactionLRDto?> GetByIdAsync(int idLr)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("@IDLR", idLr);

        //        // Assuming usp_Transaction_LR_SelectById returns TWO select statements:
        //        // 1. SELECT * FROM Transaction_LR WHERE IDLR = @IDLR
        //        // 2. SELECT * FROM Transaction_LR_Details WHERE IDLR = @IDLR
        //        using var multi = await _dapper.QueryMultipleAsync("usp_Transaction_LR_SelectById", param);

        //        var header = await multi.ReadSingleOrDefaultAsync<TransactionLR>();
        //        if (header == null) return null;

        //        var details = (await multi.ReadAsync<TransactionLRDetail>()).ToList();

        //        return new TransactionLRDto
        //        {
        //            Header = header,
        //            Details = details
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger?.LogError(ex, "Error in LRRepo.GetByIdAsync");
        //        return null;
        //    }
        //}

        public async Task<TransactionLRDto?> GetByIdAsync(int idLr)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDLR", idLr);

                // 'multi' is a tuple: (GridReader Reader, SqlConnection Conn)
                var multi = await _dapper.QueryMultipleAsync("usp_Transaction_LR_SelectById", param);

                try
                {
                    // Fix: Add .Reader here 👇
                    var header = await multi.Reader.ReadSingleOrDefaultAsync<TransactionLR>();
                    if (header == null) return null;

                    // Fix: Add .Reader here 👇
                    var details = (await multi.Reader.ReadAsync<TransactionLRDetail>()).ToList();

                    return new TransactionLRDto
                    {
                        Header = header,
                        Details = details
                    };
                }
                finally
                {
                    // Clean up: Dispose the reader and connection since your wrapper returns them
                    multi.Reader?.Dispose();
                    multi.Conn?.Dispose();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in LRRepo.GetByIdAsync");
                return null;
            }
        }

        // ---------------------------------------------------------
        // SAVE (Header + TVP Details)
        // ---------------------------------------------------------
        public async Task<SaveResult> SaveAsync(TransactionLRDto dto, string userName)
        {
            try
            {
                var header = dto.Header;

                // 1. Convert List<TransactionLRDetail> to a SQL DataTable
                var dtDetails = new DataTable();
                dtDetails.Columns.Add("MethodOfPacking", typeof(string));
                dtDetails.Columns.Add("Description", typeof(string));
                dtDetails.Columns.Add("Packages", typeof(string));
                dtDetails.Columns.Add("FreightOn", typeof(string));
                dtDetails.Columns.Add("Weight", typeof(decimal));
                dtDetails.Columns.Add("Rate", typeof(decimal));
                dtDetails.Columns.Add("Amount", typeof(decimal));

                if (dto.Details != null)
                {
                    foreach (var item in dto.Details)
                    {
                        dtDetails.Rows.Add(
                            item.MethodOfPacking, item.Description, item.Packages,
                            item.FreightOn, item.Weight, item.Rate, item.Amount
                        );
                    }
                }

                // 2. Map Parameters
                var param = new DynamicParameters();
                param.Add("@IDLR", header.IDLR);
                param.Add("@LRNo", header.LRNo);
                param.Add("@LRDate", header.LRDate);
                param.Add("@IDConsignor", header.IDConsignor);
                param.Add("@ConsignorAddress", header.ConsignorAddress);
                param.Add("@IDFromState", header.IDFromState);
                param.Add("@IDFromCity", header.IDFromCity);
                param.Add("@InvoiceNo", header.InvoiceNo);
                param.Add("@IDTruck", header.IDTruck);
                param.Add("@IDConsignee", header.IDConsignee);
                param.Add("@ConsigneeAddress", header.ConsigneeAddress);
                param.Add("@IDToState", header.IDToState);
                param.Add("@IDToCity", header.IDToCity);
                param.Add("@InvoiceDate", header.InvoiceDate);
                param.Add("@SelectTo", header.SelectTo);
                param.Add("@EWayBillNo", header.EWayBillNo);
                param.Add("@GSTPaidBy", header.GSTPaidBy);
                param.Add("@BilledTo", header.BilledTo);
                param.Add("@ChangeBillToParty", header.ChangeBillToParty);
                param.Add("@DeclaredValue", header.DeclaredValue);
                param.Add("@Remarks", header.Remarks);
                param.Add("@FreightAmt", header.FreightAmt);
                param.Add("@HamaliAmt", header.HamaliAmt);
                param.Add("@OtherChargeAmt", header.OtherChargeAmt);
                param.Add("@TotalAmt", header.TotalAmt);
                param.Add("@UserName", userName);

                // Pass the DataTable as a Table-Valued Parameter
                param.Add("@LRDetails", dtDetails.AsTableValuedParameter("dbo.udt_LR_Details"));

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_LR_Save", param);
                return result ?? SaveResult.Fail("No response from database.");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in LRRepo.SaveAsync");
                return SaveResult.Fail("Failed to save LR. " + ex.Message);
            }
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------
        public async Task<SaveResult> DeleteAsync(int idLr, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDLR", idLr);
                param.Add("@DeletedBy", deletedBy);

                // Assuming you create a standard Delete SP
                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_LR_Delete", param);
                return result ?? SaveResult.Fail("No response");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in LRRepo.DeleteAsync");
                return SaveResult.Fail("Failed to delete LR. " + ex.Message);
            }
        }
    }
}
