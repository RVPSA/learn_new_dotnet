namespace Models.Settings;

public interface IAppSettings
{
    public RedisConfigurations RedisConfigurations { get; set; }
    public ConnectionStrings ConnectionStrings { get; set; }
}