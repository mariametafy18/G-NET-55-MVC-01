using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        

        public SessionRepository(GymDbContext dbContext): base(dbContext)
        {
        }

        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategory()
        {
            return await _dbContext.Sessions.AsNoTracking().Include(x => x.Trainer).Include(x => x.Category).ToListAsync();
        }

        public async Task<int> GetCountOfBookedSlotsAync(int sessionId)
        {
            return await _dbContext.MemberSessions.AsNoTracking().Where(x => x.SessionId == sessionId).CountAsync();
        }
    }
}
