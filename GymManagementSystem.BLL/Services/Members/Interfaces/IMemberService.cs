using GymManagementSystem.BLL.Common;
using GymManagementSystem.BLL.Services.Members.ViewModels;
using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.BLL.Services.Members.Interfaces;

public interface IMemberService
{
    public Result<IEnumerable<MemberViewModel>> GetMembersAsync();
    public Task<Result> AddMemberAsync(CreateMemberViewModel model);
    public Task<Result<MemberDetailsViewModel?>> GetMemberAsync(int? Id);
    public Task<Result<HealthRecordViewModel?>> GetHealthRecordDetailsAsync(int? Id);
    public Task<Result<MemberToUpdateViewModel?>> GetMemberToUpdateAsync(int? Id);
    public Task<Result> UpdateMemberAsync(MemberToUpdateViewModel model);
    public Task<Result> DeleteMemberAsync(int? id);
}