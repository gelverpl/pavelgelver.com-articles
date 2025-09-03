using Microsoft.Extensions.Configuration;

namespace SpOrNotSpPt2;

public static class Configuration
{
    private static readonly IConfigurationRoot s_configurationRoot;

    static Configuration()
    {
        s_configurationRoot = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appSettings.json", false, false)
            .Build();
    }

    public static string ConnectionString
        => s_configurationRoot.GetConnectionString("Default")
           ?? throw new InvalidOperationException("ConnectionString must be set");
}
