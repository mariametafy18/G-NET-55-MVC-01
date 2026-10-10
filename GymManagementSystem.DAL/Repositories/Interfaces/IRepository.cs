using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : BaseEntity, new()
{
    public Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false);
    public Task<TEntity?> GetByIdAsync(int id);
    public Task<int> AddAsync(TEntity entity);
    public Task<int> UpdateAsync(TEntity entity);
    public Task<int> DeleteAsync(TEntity entity);
}
