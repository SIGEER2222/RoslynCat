using RoslynCat.Helpers;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using RoslynCat.SQL;
using SqlSugar;

namespace RoslynCat.Rules
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 扩展方法，用于向 IServiceCollection 中添加自定义服务。
        /// </summary>
        /// <param name="services">要添加服务的 IServiceCollection。</param>
        /// <param name="configuration">用于配置的 IConfiguration 实例。</param>
        /// <returns>更新后的 IServiceCollection。</returns>
        public static IServiceCollection AddRoslynCatServices(this IServiceCollection services,IConfiguration configuration) {
            // 使用提供的配置文件，配置 AppSettings 类型的服务。
            services.Configure<AppSettings>(configuration);

            // 将 ChatGPT 类型的服务注册为单例。
            services.AddSingleton<ChatGPT>();

            // 将 IWorkSpaceService、ICompleteProvider、IHoverProvider、ICodeCheckProvider 和 IGistService 类型的服务注册为作用域服务。
            services.AddScoped<IWorkSpaceService,WorkSpaceService>();
            services.AddScoped<ICompleteProvider,CompleteProvider>();
            services.AddScoped<IHoverProvider,HoverProvider>();
            services.AddScoped<ICodeCheckProvider,CodeCheckProvider>();
            services.AddScoped<IGistService,CodeSharing>();

            // 将 CompletionProvider 和 CodeSampleRepository 类型的服务注册为作用域服务。
            services.AddScoped<CompletionProvider>();
            services.AddScoped<CodeSampleRepository>();

            // 使用工厂方法将 ISqlSugarClient 类型的服务注册为单例服务。
            services.AddSingleton<ISqlSugarClient>(provider => new SqlSugarFactory().Create(provider));

            // 添加一个名为 "GithubApi" 的 HttpClient 实例，基地址为 "https://api.github.com"。
            services.AddHttpClient("GithubApi",client =>
            {
                client.BaseAddress = new Uri("https://api.github.com");
            });

            // 返回更新后的 IServiceCollection。
            return services;
        }
    }
}
