using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository : IRepository<Plan>
    {
        public Task<IEnumerable<Plan>> GetAllWithMembershipAsync(bool tracking = false);
    }
}
