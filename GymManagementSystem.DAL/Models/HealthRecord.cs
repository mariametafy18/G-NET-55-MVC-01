
namespace GymManagementSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public string Height { get; set; }
        public string Weight { get; set; }
        public string BloodType { get; set; }
        public string? Note { get; set; }
        #region Relationship
        public Member Member { get; set; } = default!;
        public int MemberId { get; set; }

        
        #endregion
    }
}
