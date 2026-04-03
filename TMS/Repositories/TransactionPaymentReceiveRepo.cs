using Dapper;
using TMS.Helpers;
using TMS.Models.Common;
using TMS.Models.Transaction;

namespace TMS.Repositories
{
    public class TransactionPaymentReceiveRepo
    {
        private readonly IDapperHelper _dapper;
        private readonly ILogger<TransactionPaymentReceive>? _logger;

        public TransactionPaymentReceiveRepo(IDapperHelper dapperHelper, ILogger<TransactionPaymentReceive>? logger = null)
        {
            _dapper = dapperHelper;
            _logger = logger;
        }
        public async Task<IEnumerable<TransactionPaymentReceive>> GetAllAsync() => await _dapper.QueryAsync<TransactionPaymentReceive>("usp_Transaction_PaymentReceive_Select");

        public async Task<int> GetReceiptNoAsync()
        {
            try
            {
                // Calls a stored procedure to get Max(SrNo) + 1
                return await _dapper.QueryFirstOrDefaultAsync<int>("usp_Transaction_PaymentReceive_SelectReceiptNo");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PaymentReceive.GetReceiptNoAsync");
                // Return 1 as a fallback (assuming it's the first record)
                return 1;
            }
        }

        public async Task<IEnumerable<TransactionPaymentType>> GetAllPaymentTypeAsync()
        {
            try
            {
                return await _dapper.QueryAsync<TransactionPaymentType>("usp_Transaction_PaymentType_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PaymentReceive.GetAllPaymentTypeAsync");
                return Enumerable.Empty<TransactionPaymentType>();
            }
        }

        public async Task<IEnumerable<TransactionPaymentMode>> GetAllPaymentModeAsync()
        {
            try
            {
                return await _dapper.QueryAsync<TransactionPaymentMode>("usp_Transaction_PaymentMode_Select");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in PaymentReceive.GetAllPaymentModeAsync");
                return Enumerable.Empty<TransactionPaymentMode>();
            }
        }

        public async Task<TransactionPaymentReceive?> GetByIdAsync(int idPayment)
        {
            var param = new DynamicParameters();
            param.Add("@IDPayment", idPayment);
            return await _dapper.QueryFirstOrDefaultAsync<TransactionPaymentReceive>("usp_Transaction_PaymentReceive_SelectById", param);
        }

        public async Task<SaveResult> SaveAsync(TransactionPaymentReceive payment)
        {
            var param = new DynamicParameters();
            param.Add("@IDPayment", payment.IDPayment);
            param.Add("@ReceiptNo", payment.ReceiptNo);
            param.Add("@PaymentDate", payment.PaymentDate);
            param.Add("@IDPaymentType", payment.IDPaymentType);
            param.Add("@IDParty", payment.IDParty);
            param.Add("@IDBill", payment.IDBill);
            param.Add("@BillAmount", payment.BillAmount);
            param.Add("@OutstandingAmount", payment.OutstandingAmount);
            param.Add("@IDPaymentMode", payment.IDPaymentMode);
            param.Add("@AmountReceived", payment.AmountReceived);
            param.Add("@TDSAmt", payment.TDSAmt);
            param.Add("@BalanceAmt", payment.BalanceAmt);
            param.Add("@Remarks", payment.Remarks);
            param.Add("@UserName", payment.E_By);

            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_PaymentReceive_Save", param);
            return result ?? SaveResult.Fail("No response");
        }

        public async Task<SaveResult> DeleteAsync(int idPayment, string deletedBy)
        {
            var param = new DynamicParameters();
            param.Add("@IDPayment", idPayment);
            param.Add("@DeletedBy", deletedBy);
            var result = await _dapper.QueryFirstOrDefaultAsync<SaveResult>("usp_Transaction_PaymentReceive_Delete", param);
            return result ?? SaveResult.Fail("No response");
        }
    }
}
