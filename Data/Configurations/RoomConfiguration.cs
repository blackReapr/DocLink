using DocLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocLink.Data.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasOne(r => r.ParticipantOneUser).WithMany().HasForeignKey(r => r.ParticipantOne).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(r => r.ParticipantTwoUser).WithMany().HasForeignKey(r => r.ParticipantTwo).OnDelete(DeleteBehavior.NoAction);
    }
}
