namespace Models.Settings;

public class RedisConfigurations
{
    public string RedisURL { get; set; }
    public Instances Instances { get; set; }
}

public class Instances
{
    public string Employee { get; set; }
}