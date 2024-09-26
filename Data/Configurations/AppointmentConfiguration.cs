using DocLink.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DocLink.Data.Configurations;

internal class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasOne(a => a.Doctor).WithMany(u => u.AppointmentAsDoctor).HasForeignKey(a => a.DoctorId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(a => a.Patient).WithMany(u => u.AppointmentAsPatient).HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.NoAction);
    }
}
