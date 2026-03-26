using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterBillToPartyService
    {
        private readonly MasterBillToPartyRepo _billToPartyRepo;
        public MasterBillToPartyService(MasterBillToPartyRepo billToPartyRepo)
        {
            _billToPartyRepo = billToPartyRepo;
        }
        public async Task<IEnumerable<MasterBillToParty>> GetAllAsync() => await _billToPartyRepo.GetAllAsync();
        public async Task<int> GetSrNo() => await _billToPartyRepo.GetSrNoAsync();
        public async Task<MasterBillToParty> GetByIdAsync(int idPartyBill) => await _billToPartyRepo.GetByIdAsync(idPartyBill);
        public async Task<SaveResult> SaveAsync(MasterBillToParty billToParty) => await _billToPartyRepo.SaveAsync(billToParty);
        public async Task<SaveResult> DeleteAsync(int idPartyBill, string deletedBy) => await _billToPartyRepo.DeleteAsync(idPartyBill, deletedBy);
    }
}
