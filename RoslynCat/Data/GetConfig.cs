
namespace RoslynCat.Data
{
    ///// <summary>
    ///// 获取配置信息
    ///// </summary>
    //public class GetConfig
    //{
    //    private IConfigurationRoot _configuration;
    //    public GetConfig() {
    //        var configurationBuilder = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    //                                        .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
    //                                        .Build();
    //        _configuration = configurationBuilder;

    //    }

    //    public string GistId { get => _configuration["gist"]; }
    //    public string OpenAI { get => _configuration["OpneAI"]; }
    //    public string ConnectionString { get => _configuration.GetConnectionString("DbConnect"); }
    //}

    public class AppSettings
    {
        public string AdminSafeList { get; set; }
        public Logging Logging { get; set; }
        public string AllowedHosts { get; set; }
        public string GistId { get; set; }
        public string OpneAI { get; set; }
        public bool DetailedErrors { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
    }

    public class Logging
    {
        public LogLevel LogLevel { get; set; }
    }

    public class LogLevel
    {
        public string Default { get; set; }
        public string Microsoft_AspNetCore { get; set; }
    }

    public class ConnectionStrings
    {
        public string DbConnect { get; set; }
        public string SqliteDb { get; set; }
    }
}
