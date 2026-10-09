using GymManagementSystem.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.DAL.Models;

public class GymUser : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public Address Address { get; set; }
}
[Owned]
public class Address
{
    public string Street { get; set; }
    public string City { get; set; }
    public int BuildingNumber { get; set; }
}
