using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        public MemberRepository(GymDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<Member> GetMemberWithRelatedData(int id)
        {
            return _dbContext.Members.Where(x => x.Id == id).Include(x => x.Memberships).ThenInclude(x => x.Plan);
        }
    }
}
