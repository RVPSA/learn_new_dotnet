namespace Models.Settings;

public class AppSettings : IAppSettings
{
    public RedisConfigurations RedisConfigurations { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }

    public AppSettings()
    {
        RedisConfigurations = new RedisConfigurations();
        ConnectionStrings = new ConnectionStrings();
    }
}