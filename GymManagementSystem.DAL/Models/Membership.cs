namespace GymManagementSystem.DAL.Models;

public class Membership : BaseEntity
{
    public Member Member { get; set; } = default!;
    public int MemberId { get; set; }
    public Plan Plan { get; set; } = default!;
    public int PlanId { get; set; }
    // startDate => CreatedAt
    public DateTime EndDate { get; set; }
    public string Status => EndDate > DateTime.Now ? "Active" : "Expired";
    public bool IsActive => EndDate > DateTime.Now;


}
