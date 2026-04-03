using TMS.Models.Common;
using TMS.Models.Transaction;
using TMS.Repositories;

namespace TMS.Services
{
    public class TransactionChallanService
    {
        private readonly TransactionChallanRepo _repo;

        public TransactionChallanService(TransactionChallanRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TransactionChallan>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<int> GetVoucherNo() => await _repo.GetVoucherNoAsync();

        public async Task<TransactionChallanDto> GetByIdAsync(int idChallan)
        {
            return await _repo.GetByIdAsync(idChallan);
        }

        public async Task<SaveResult> SaveAsync(TransactionChallanDto dto, string userName)
        {
            return await _repo.SaveAsync(dto, userName);
        }

        public async Task<SaveResult> DeleteAsync(int idChallan, string deletedBy)
        {
            return await _repo.DeleteAsync(idChallan, deletedBy);
        }
    }
}
