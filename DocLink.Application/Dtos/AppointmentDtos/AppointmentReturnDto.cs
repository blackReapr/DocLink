namespace DocLink.Application.Dtos.AppointmentDtos;

public class AppointmentReturnDto
{
    public string Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string DoctorName { get; set; }
    public string DoctorSurname { get; set; }
    public string DoctorEmail { get; set; }
    public string PatientName { get; set; }
    public string PatientSurname { get; set; }
    public string PatientEmail { get; set; }
    public decimal Price { get; set; }
}
