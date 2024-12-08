namespace WebApplication1;

using RabbitMQ.Client;
using System;

public class ConnectionFactoryBuilder
{
    private string _hostName = "172.17.0.3";
    private int _port = 5672;
    private string _userName = "guest";
    private string _password = "guest";
    private TimeSpan _requestedConnectionTimeout = TimeSpan.FromSeconds(10);
    private bool _automaticRecoveryEnabled = true;
    private TimeSpan _networkRecoveryInterval = TimeSpan.FromSeconds(5);

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
