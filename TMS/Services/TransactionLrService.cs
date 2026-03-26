using TMS.Models.Common;
using TMS.Models.Transaction;
using TMS.Repositories;

namespace TMS.Services
{
    public class TransactionLrService
    {
        private readonly TransactionLrRepo _lrRepo;

        public TransactionLrService(TransactionLrRepo lrRepo)
        {
            _lrRepo = lrRepo;
        }

        public async Task<IEnumerable<TransactionLR>> GetAllAsync() => await _lrRepo.GetAllAsync();

        public async Task<int> GetLrNo() => await _lrRepo.GetLrNoAsync();

        public async Task<TransactionLRDto?> GetByIdAsync(int idLr) => await _lrRepo.GetByIdAsync(idLr);

        public async Task<SaveResult> SaveAsync(TransactionLRDto dto, string userName) => await _lrRepo.SaveAsync(dto, userName);

        public async Task<SaveResult> DeleteAsync(int idLr, string deletedBy) => await _lrRepo.DeleteAsync(idLr, deletedBy);
    }
}
