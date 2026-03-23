using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterPartyAccountService
    {
        private readonly MasterPartyAccountRepo _partyAccountRepo;

        public MasterPartyAccountService(MasterPartyAccountRepo partyAccountRepo)
        {
            _partyAccountRepo = partyAccountRepo;
        }

        public async Task<IEnumerable<MasterPartyAccount>> GetAllAsync() => await _partyAccountRepo.GetAllAsync();
        public async Task<IEnumerable<MasterAccountType>> GetAllAccountTypeAsync() => await _partyAccountRepo.GetAllAccountTypeAsync();  // Account Type
        public async Task<IEnumerable<MasterBalanceType>> GetAllBalanceTypeAsync() => await _partyAccountRepo.GetAllBalanceTypeAsync(); // Balance Type

        public async Task<MasterPartyAccount?> GetByIdAsync(int idPartyAccount) => await _partyAccountRepo.GetByIdAsync(idPartyAccount);
        public async Task<SaveResult> SaveAsync(MasterPartyAccount account) => await _partyAccountRepo.SaveAsync(account);
        public async Task<SaveResult> DeleteAsync(int idPartyAccount, string deletedBy) => await _partyAccountRepo.DeleteAsync(idPartyAccount, deletedBy);
    }
}
