using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces
{
    public interface ISessionRepository : IRepository<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategory();
        Task<int> GetCountOfBookedSlotsAync(int sessionId);
    }
}
