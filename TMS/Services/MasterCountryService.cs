using Microsoft.AspNetCore.Mvc.RazorPages;
using TMS.Models.Common;
using TMS.Repositories;

namespace TMS.Services
{
    public class MasterCountryService
    {
        private readonly MasterCountryRepo _countryRepo;
        public MasterCountryService(MasterCountryRepo countryRepo)
        {
            _countryRepo = countryRepo;
        }
        public async Task<IEnumerable<Models.Master.MasterCountry>> GetAllAsync() => await _countryRepo.GetAllAsync();
        public async Task<Models.Master.MasterCountry?> GetByIdAsync(int idCountry) => await _countryRepo.GetByIdAsync(idCountry);
        public async Task<SaveResult> SaveAsync(Models.Master.MasterCountry country) => await _countryRepo.SaveAsync(country);
        public async Task<SaveResult> DeleteAsync(int idCountry, string deletedBy) => await _countryRepo.DeleteAsync(idCountry, deletedBy);
    }
}
