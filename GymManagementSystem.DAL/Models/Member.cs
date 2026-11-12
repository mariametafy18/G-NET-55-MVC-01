
namespace GymManagementSystem.DAL.Models
{
    public class Member : GymUser
    {
        // joinDate => CreatedAt
        public string? Photo { get; set; }
        #region Relationship
        public HealthRecord HealthRecord { get; set; } = default!;
        public ICollection<Membership> Memberships { get; set; } = default!;
        public ICollection<MemberSession> MemberSessions { get; set; } = default!;

        #endregion
    }
}
