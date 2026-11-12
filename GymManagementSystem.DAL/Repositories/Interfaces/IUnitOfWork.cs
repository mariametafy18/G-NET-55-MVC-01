using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    public IMemberRepository memberRepository { get; }
    public ISessionRepository sessionRepository { get; }
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();

    Task<int> SaveChanges();
}
