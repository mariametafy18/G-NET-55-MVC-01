using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class PlanRepository : Repository<Plan>, IPlanRepository
    {
        private readonly GymDbContext _dbContext;
        public PlanRepository(GymDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<IEnumerable<Plan>> GetAllWithMembershipAsync(bool tracking = false)
        {
            return tracking ? await _dbContext.Plans.Include(x => x.Memberships).ToListAsync() : await _dbContext.Plans.Include(x => x.Memberships).ToListAsync();
        }
    }
}
