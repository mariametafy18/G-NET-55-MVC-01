using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Data.Configurations;

public class MemberSessionsConfigurations : IEntityTypeConfiguration<MemberSession>
{
    public void Configure(EntityTypeBuilder<MemberSession> builder)
    {
        builder.Ignore(x => x.Id);

        builder.Property(x => x.CreatedAt)
               .HasDefaultValueSql("GETDATE()");

        builder.HasKey(x => new { x.MemberId, x.SessionId });
    }
}
