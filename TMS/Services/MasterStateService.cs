using TMS.Models.Common;
using TMS.Models.Master;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterStateService
    {
        private readonly MasterStateRepo _stateRepo;

        public MasterStateService(MasterStateRepo stateRepo)
        {
            _stateRepo = stateRepo;
        }

        public async Task<IEnumerable<MasterState>> GetAllAsync() => await _stateRepo.GetAllAsync();
        public async Task<MasterState?> GetByIdAsync(int idState) => await _stateRepo.GetByIdAsync(idState);
        public async Task<SaveResult> SaveAsync(MasterState state) => await _stateRepo.SaveAsync(state);
        public async Task<SaveResult> DeleteAsync(int idState, string deletedBy) => await _stateRepo.DeleteAsync(idState, deletedBy);
    }
}
