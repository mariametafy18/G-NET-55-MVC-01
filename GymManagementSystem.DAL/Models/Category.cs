using System.Collections;

namespace GymManagementSystem.DAL.Models;

public class Category : BaseEntity
{
    public string CategoryName { get; set; }
    #region Relationship
    public ICollection<Session> Sessions { get; set; }

    #endregion    
}