using TMS.Models.Common;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterTruckService
    {
        private readonly MasterTruckRepo _truckRepo;
        public MasterTruckService(MasterTruckRepo truckRepo)
        {
            _truckRepo = truckRepo;
        }
        public async Task<IEnumerable<Models.Master.MasterTruck>> GetAllAsync() => await _truckRepo.GetAllAsync();
        public async Task<Models.Master.MasterTruck?> GetByIdAsync(int idTruck) => await _truckRepo.GetByIdAsync(idTruck);
        public async Task<SaveResult> SaveAsync(Models.Master.MasterTruck truck) => await _truckRepo.SaveAsync(truck);
        public async Task<SaveResult> DeleteAsync(int idTruck, string deletedBy) => await _truckRepo.DeleteAsync(idTruck, deletedBy);
    }
}
