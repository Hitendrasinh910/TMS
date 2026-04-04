using TMS.Models;
using TMS.Models.Common;
using TMS.Repositories;

namespace TMS.Services
{
    public class UserRightsService
    {
        private readonly UserRightsRepo _repo;

        public UserRightsService(UserRightsRepo repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UserRights>> GetUserRightsAsync(int idUser)
        {
            return await _repo.GetUserRightsAsync(idUser);
        }

        public async Task<SaveResult> SaveRightsAsync(UserRightSaveDto dto, string userName)
        {
            return await _repo.SaveRightsAsync(dto, userName);
        }
    }
}
