namespace DocLink.Core.Entities;

public class Room : BaseEntity
{
    public string ParticipantOne { get; set; }
    public string ParticipantTwo { get; set; }
    public AppUser ParticipantOneUser { get; set; }
    public AppUser ParticipantTwoUser { get; set; }
}
