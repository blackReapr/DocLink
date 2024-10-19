namespace DocLink.Core.Entities;

public class Appointment : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DoctorId { get; set; }
    public string PatientId { get; set; }
    public Status Status { get; set; }
    public AppUser Doctor { get; set; }
    public AppUser Patient { get; set; }
    public decimal Price { get; set; }
    public string Note { get; set; }
}


public enum Status
{
    REJECTED,
    PENDING,
    ACCEPTED
}