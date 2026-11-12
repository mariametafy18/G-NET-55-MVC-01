using GymManagementSystem.DAL.Models.Enums;

namespace GymManagementSystem.DAL.Models;

public class Trainer : GymUser
{
    // HireDate => CreatedAt
    public Speciality Speciality { get; set; }
    #region Relationship
    public ICollection<Session> Sessions { get; set; } = default!;
    #endregion    
}