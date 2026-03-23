using Dapper;
using System.Data;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Transaction;

namespace TMS.Repositories
{
    public class TransactionChallanRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<TransactionChallanRepo> _logger;

        public TransactionChallanRepo(IDapperHelper dapperHelper, ILogger<TransactionChallanRepo> logger)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }

        // ---------------------------------------------------------
        // GET ALL (For List Page)
        // ---------------------------------------------------------
        public async Task<IEnumerable<TransactionChallan>> GetAllAsync()
        {
            try
            {
                return await _dapper.QueryAsync<TransactionChallan>("usp_Transaction_Challan_Select");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransactionChallanRepo.GetAllAsync");
                return Enumerable.Empty<TransactionChallan>();
            }
        }

        // ---------------------------------------------------------
        // GET BY ID (For Editing)
        // ---------------------------------------------------------
        public async Task<TransactionChallanDto> GetByIdAsync(int idChallan)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDChallan", idChallan);

                var multi = await _dapper.QueryMultipleAsync("usp_Transaction_Challan_SelectById", param);
                {
                    var header = await multi.Reader.ReadSingleOrDefaultAsync<TransactionChallan>();

                    if (header == null)
                    {
                        return null;
                    }

                    var detailsList = await multi.Reader.ReadAsync<TransactionChallanDetail>();

                    var dto = new TransactionChallanDto();
                    dto.Header = header;
                    dto.Details = detailsList.ToList();

                    return dto;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransactionChallanRepo.GetByIdAsync | IDChallan: {IDChallan}", idChallan);
                return null;
            }
        }

        // ---------------------------------------------------------
        // SAVE (Header + Grid via TVP)
        // ---------------------------------------------------------
        public async Task<SaveResult> SaveAsync(TransactionChallanDto dto, string userName)
        {
            try
            {
                var header = dto.Header;

                // 1. Build the DataTable mapping for the TVP exactly as defined in SQL
                var dtDetails = new DataTable();
                dtDetails.Columns.Add("IDLR", typeof(int));
                dtDetails.Columns.Add("MethodOfPacking", typeof(string));
                dtDetails.Columns.Add("Description", typeof(string));
                dtDetails.Columns.Add("Packages", typeof(string));
                dtDetails.Columns.Add("FreightOn", typeof(string));
                dtDetails.Columns.Add("FixAmt", typeof(decimal));
                dtDetails.Columns.Add("Weight", typeof(decimal));
                dtDetails.Columns.Add("Rate", typeof(decimal));
                dtDetails.Columns.Add("Amount", typeof(decimal));

                if (dto.Details != null && dto.Details.Count > 0)
                {
                    foreach (var item in dto.Details)
                    {
                        dtDetails.Rows.Add(
                            item.IDLR,
                            item.MethodOfPacking,
                            item.Description,
                            item.Packages,
                            item.FreightOn,
                            item.FixAmt,
                            item.Weight,
                            item.Rate,
                            item.Amount
                        );
                    }
                }

                // 2. Map all Header Parameters
                var param = new DynamicParameters();
                param.Add("@IDChallan", header.IDChallan);
                param.Add("@VoucherNo", header.VoucherNo);
                param.Add("@ChallanDate", header.ChallanDate);
                param.Add("@IDBill", header.IDBill);
                param.Add("@IDTruck", header.IDTruck);
                param.Add("@DriverName", header.DriverName);
                param.Add("@IDTransporter", header.IDTransporter);
                param.Add("@IDConsignee", header.IDConsignee);
                param.Add("@PanCardHolder", header.PanCardHolder);
                param.Add("@PanCardNo", header.PanCardNo);
                param.Add("@AdvancePayment1", header.AdvancePayment1);
                param.Add("@AdvancePayment2", header.AdvancePayment2);

                // Financials
                param.Add("@FreightAmt", header.FreightAmt);
                param.Add("@ExtraAmt", header.ExtraAmt);
                param.Add("@FinalAmt", header.FinalAmt);
                param.Add("@CashAmt", header.CashAmt);
                param.Add("@Cheque1Amt", header.Cheque1Amt);
                param.Add("@Cheque1No", header.Cheque1No);
                param.Add("@Cheque1Date", header.Cheque1Date);
                param.Add("@Cheque2Amt", header.Cheque2Amt);
                param.Add("@Cheque2No", header.Cheque2No);
                param.Add("@Cheque2Date", header.Cheque2Date);
                param.Add("@AdvanceAmt", header.AdvanceAmt);
                param.Add("@IsPaymentPaidByParty", header.IsPaymentPaidByParty);
                param.Add("@BalanceAmt", header.BalanceAmt);
                param.Add("@PaidByPartyAmt", header.PaidByPartyAmt);

                // Balance Details
                param.Add("@BalanceChequeAmt", header.BalanceChequeAmt);
                param.Add("@BalanceChequeNo", header.BalanceChequeNo);
                param.Add("@BalanceChequeDate", header.BalanceChequeDate);
                param.Add("@Remarks", header.Remarks);
                param.Add("@UserName", userName);

                // 3. Attach the Grid TVP
                param.Add("@ChallanDetails", dtDetails.AsTableValuedParameter("dbo.udt_Challan_Details"));

                // 4. Execute
                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_Challan_Save", param);

                if (result == null)
                {
                    return SaveResult.Fail("No response received from the database.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransactionChallanRepo.SaveAsync");
                return SaveResult.Fail("Failed to save Challan. " + ex.Message);
            }
        }

        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------
        public async Task<SaveResult> DeleteAsync(int idChallan, string deletedBy)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("@IDChallan", idChallan);
                param.Add("@DeletedBy", deletedBy);

                var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_Challan_Delete", param);

                if (result == null)
                {
                    return SaveResult.Fail("No response received from the database.");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TransactionChallanRepo.DeleteAsync | IDChallan: {IDChallan}", idChallan);
                return SaveResult.Fail("Failed to delete Challan. " + ex.Message);
            }
        }
    }
}
