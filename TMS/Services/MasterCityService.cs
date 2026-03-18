using TMS.Models.Common;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterCityService
    {
        private readonly MasterCityRepo _cityRepo;
        public MasterCityService(MasterCityRepo cityRepo)
        {
            _cityRepo = cityRepo;
        }
        public async Task<IEnumerable<Models.Master.MasterCity>> GetAllAsync() => await _cityRepo.GetAllAsync();
        public async Task<Models.Master.MasterCity?> GetByIdAsync(int idCity) => await _cityRepo.GetByIdAsync(idCity);
        public async Task<SaveResult> SaveAsync(Models.Master.MasterCity city) => await _cityRepo.SaveAsync(city);
        public async Task<SaveResult> DeleteAsync(int idCity, string deletedBy) => await _cityRepo.DeleteAsync(idCity, deletedBy);
    }
}
