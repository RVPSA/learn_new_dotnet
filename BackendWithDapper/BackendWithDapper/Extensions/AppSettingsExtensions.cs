using Models.Settings;

namespace BackendWithDapper.Extensions;

public static class AppSettingsExtensions
{
    public static void AddAppSettings(this WebApplicationBuilder builder)
    {
        JsonConfigurationExtensions.AddJsonFile((IConfigurationBuilder)(object)builder.Configuration, "appsettings.json",false, true);
    }

    public static IAppSettings AddConfigurations(this IConfiguration configuration)
    {
        var appSettings = new AppSettings();
        ConfigurationBinder.Bind(configuration,(object)appSettings);
        return appSettings;
    }
}