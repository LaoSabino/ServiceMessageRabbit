using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace WebApplication1;

public class ConnectionFactoryBuilder(IOptions<RabbitMqOptions> rabbitMqOptions)
{
    private string _hostName = rabbitMqOptions.Value.HostName;
    private int _port = rabbitMqOptions.Value.Port;
    private string _userName = rabbitMqOptions.Value.UserName;
    private string _password = rabbitMqOptions.Value.Password;
    private TimeSpan _requestedConnectionTimeout = rabbitMqOptions.Value.RequestedConnectionTimeout;
    private bool _automaticRecoveryEnabled = rabbitMqOptions.Value.AutomaticRecoveryEnabled;
    private TimeSpan _networkRecoveryInterval = rabbitMqOptions.Value.NetworkRecoveryInterval;

    public ConnectionFactoryBuilder SetHostName(string hostName)
    {
        _hostName = hostName; return this;
    }

    public ConnectionFactoryBuilder SetPort(int port)
    {
        _port = port; return this;
    }

    public ConnectionFactoryBuilder SetUserName(string userName)
    {
        _userName = userName; return this;
    }

    public ConnectionFactoryBuilder SetPassword(string password)
    {
        _password = password; return this;
    }
    public ConnectionFactoryBuilder SetRequestedConnectionTimeout(TimeSpan timeout)
    {
        _requestedConnectionTimeout = timeout; return this;
    }

    public ConnectionFactoryBuilder SetAutomaticRecoveryEnabled(bool enabled)
    {
        _automaticRecoveryEnabled = enabled; return this;
    }

    public ConnectionFactoryBuilder SetNetworkRecoveryInterval(TimeSpan interval)
    {
        _networkRecoveryInterval = interval; return this;
    }

    public ConnectionFactory Build()
    {
        return new ConnectionFactory
        {
            HostName = _hostName,
            Port = _port,
            UserName = _userName,
            Password = _password,
            RequestedConnectionTimeout = _requestedConnectionTimeout,
            AutomaticRecoveryEnabled = _automaticRecoveryEnabled,
            NetworkRecoveryInterval = _networkRecoveryInterval
        };
    }
}
