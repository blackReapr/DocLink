namespace DocLink.Core.Entities;

public class Message : BaseEntity
{
    public string Content { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
    public AppUser Sender { get; set; }
    public AppUser Receiver { get; set; }
}
