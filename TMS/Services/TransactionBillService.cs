using TMS.Models.Common;
using TMS.Models.Transaction;
using TMS.Repositories;

namespace TMS.Services
{
    public class TransactionBillService
    {
        private readonly TransactionBillRepo _repo;

        public TransactionBillService(TransactionBillRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TransactionBill>> GetAllAsync() => await _repo.GetAllAsync();

        public async Task<int> GetBillNo() => await _repo.GetBillNoAsync();

        public async Task<TransactionBillDto?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);

        public async Task<SaveResult> SaveAsync(TransactionBillDto dto, string userName) => await _repo.SaveAsync(dto, userName);

        public async Task<SaveResult> DeleteAsync(int id, string deletedBy) => await _repo.DeleteAsync(id, deletedBy);
    }
}
