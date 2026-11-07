using GymManagementSystem.DAL.Data.Contexts;
using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Interfaces;

namespace GymManagementSystem.DAL.Repositories.Classes;

public class UnitOfWork : IUnitOfWork
{
    private readonly GymDbContext _dbContext;
    public IMemberRepository memberRepository { get; }

    public ISessionRepository sessionRepository { get; }

    public UnitOfWork(GymDbContext dbContext, IMemberRepository memberRepository, ISessionRepository sessionRepository)
    {
        _dbContext = dbContext;
        this.memberRepository = memberRepository;
        this.sessionRepository = sessionRepository;
    }

    private readonly Dictionary<string, object> Repos = [];
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
    {
        var type = typeof(TEntity).Name;
        if(!string.IsNullOrEmpty(type) && Repos.TryGetValue(type, out object value))
            return (IRepository<TEntity>)Repos[type];

            var repo = new Repository<TEntity>(_dbContext);
            Repos[type] = repo;
            return repo;
    }

    public async Task<int> SaveChanges()
    {
        return await _dbContext.SaveChangesAsync();
    }
}
