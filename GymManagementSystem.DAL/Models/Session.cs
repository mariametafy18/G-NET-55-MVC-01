namespace GymManagementSystem.DAL.Models
{
    public class Session : BaseEntity
    {
        public string Description { get; set; }
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        #region Relationship
        public Trainer Trainer { get; set; } = default!;
        public int TrainerId { get; set; }
        public Category Category { get; set; } = default!;
        public int CategoryId { get; set; }
        public ICollection<MemberSession> MemberSessions { get; set; } = default!; 
        #endregion
    }
}
