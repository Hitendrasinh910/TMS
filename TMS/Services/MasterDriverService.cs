using TMS.Models.Common;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterDriverService
    {
        private readonly MasterDriverRepo _driverRepo;
        public MasterDriverService(MasterDriverRepo driverRepo)
        {
            _driverRepo = driverRepo;
        }
        public async Task<IEnumerable<Models.Master.MasterDriver>> GetAllAsync() => await _driverRepo.GetAllAsync();
        public async Task<Models.Master.MasterDriver?> GetByIdAsync(int idDriver) => await _driverRepo.GetByIdAsync(idDriver);
        public async Task<SaveResult> SaveAsync(Models.Master.MasterDriver driver) => await _driverRepo.SaveAsync(driver);
        public async Task<SaveResult> DeleteAsync(int idDriver, string deletedBy) => await _driverRepo.DeleteAsync(idDriver, deletedBy);
    }
}
