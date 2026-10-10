using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.Services.Members.Interfaces
{
    public interface IMemberService
    {
        public Task<IEnumerable<MemberViewModel>> GetMembersAsync();
        public Task<bool> AddMemberAsync(CreateMemberViewModel model);
    }
}
