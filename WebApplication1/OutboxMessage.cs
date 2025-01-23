namespace WebApplication1;

public class OutboxMessage
{
    public int Id { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime DateCreated { get; set; }
}
