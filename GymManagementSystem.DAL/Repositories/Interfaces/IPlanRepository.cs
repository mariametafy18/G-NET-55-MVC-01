using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface IPlanRepository
    {
        public Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false);
        public Task<Plan?> GetByIdAsync(int id);
        public Task<int> AddAsync(Plan plan);
        public Task<int> UpdateAsync(Plan plan);
        public Task<int> DeleteAsync(Plan plan);
    }
}
