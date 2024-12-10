namespace WebApplication1;

public class RabbitMqOptions
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public TimeSpan RequestedConnectionTimeout { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; }
    public TimeSpan NetworkRecoveryInterval { get; set; }
}
