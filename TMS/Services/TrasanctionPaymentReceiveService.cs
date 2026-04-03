using TMS.Models.Common;
using TMS.Models.Transaction;
using TMS.Repositories;

namespace TMS.Services
{
    public class TransactionPaymentReceiveService
    {
        private readonly TransactionPaymentReceiveRepo _repo;
        public TransactionPaymentReceiveService(TransactionPaymentReceiveRepo repo) => _repo = repo;

        public async Task<IEnumerable<TransactionPaymentReceive>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<int> GetReceiptNo() => await _repo.GetReceiptNoAsync();
        public async Task<TransactionPaymentReceive?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<SaveResult> SaveAsync(TransactionPaymentReceive payment) => await _repo.SaveAsync(payment);
        public async Task<SaveResult> DeleteAsync(int id, string deletedBy) => await _repo.DeleteAsync(id, deletedBy);
    }
}
