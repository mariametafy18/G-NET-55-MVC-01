using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class PlanRepository : IPlanRepository
    {
        private readonly GymDbContext _dbContext; // dependency
        public PlanRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;    
        }
        public async Task<int> AddAsync(Plan plan)
        {
            await _dbContext.Plans.AddAsync(plan);
            return await _dbContext.SaveChangesAsync();
        }

        public Task<int> DeleteAsync(Plan plan)
        {
            _dbContext.Plans.Remove(plan);
            return _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false)
        {
            return tracking ?await _dbContext.Plans.ToListAsync() : await _dbContext.Plans.AsNoTracking().ToListAsync();
        }

        public async Task<Plan?> GetByIdAsync(int id)
        {
            return await _dbContext.Plans.FindAsync(id);
        }

        public async Task<int> UpdateAsync(Plan plan)
        {
            _dbContext.Plans.Update(plan);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
