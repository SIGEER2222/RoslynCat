using BootstrapBlazor.Components;
using Bunit;
using Microsoft.Extensions.Options;
using Moq;
using RoslynCat.Helpers;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using RoslynCat.Shared;
using RoslynCat.SQL;
using SqlSugar;
using System;
using TestContext = Bunit.TestContext;

namespace RoslynTest.Pages
{
    public class IndexTests : BunitTestContext
    {
        // 声明一个context字段
        public TestContext context;

        [SetUp]
        public void SetUp() {
            // Arrange: create a test context and render the Index component
            // 初始化context字段
            context = new Bunit.TestContext();
            // 使用提供的配置文件，配置 AppSettings 类型的服务。
            var services = context.Services;
            // 将 ChatGPT 类型的服务注册为单例。
            services.AddSingleton<ChatGPT>();

            // 将 IWorkSpaceService、ICompleteProvider、IHoverProvider、ICodeCheckProvider 和 IGistService 类型的服务注册为作用域服务。
            services.AddTransient<IWorkSpaceService,WorkSpaceService>();
            services.AddTransient<ICompleteProvider,CompleteProvider>();
            services.AddTransient<IHoverProvider,HoverProvider>();
            services.AddTransient<ICodeCheckProvider,CodeCheckProvider>();
            services.AddTransient<IGistService,CodeSharing>();

            // 将 CompletionProvider 和 CodeSampleRepository 类型的服务注册为作用域服务。
            services.AddTransient<CompletionProvider>();
            services.AddTransient<CodeSampleRepository>();
            services.AddBootstrapBlazor();
            // 使用工厂方法将 ISqlSugarClient 类型的服务注册为单例服务。
            services.AddSingleton<ISqlSugarClient>(provider => new SqlSugarFactory().Create(provider));

            // 添加一个名为 "GithubApi" 的 HttpClient 实例，基地址为 "https://api.github.com"。
            services.AddHttpClient("GithubApi",client => {
                client.BaseAddress = new Uri("https://api.github.com");
            });
        }


        [Test]
        public void IndexPageShouldRenderCorrectly() {
            // 使用context字段
            var ctx = context;
            //var cut = ctx.RenderComponent<Counter>();
            var buttons = new [] {"运行","分享代码","控制台输入","问问GPT？",};
            var cut = ctx.RenderComponent<RoslynCat.Pages.Index>();

            var buttonElements = cut.FindAll("button");
            // Assert: verify that the component has the expected elements and content
            for (int i = 0 ; i < buttons.Length ; i++) {
                buttonElements[i].MarkupMatches($"<button>{buttons[i]}</button>");
            }
        }

        [Test]
        public void RunCode_Should_Return_Correct_Result_When_Code_Is_Valid()
        {
            // Arrange
            var code = "Console.WriteLine(Hello World);"; // 有效的代码
            var read = ""; // 空的输入
            var expected = "Hello World\r\n"; // 期望的输出
            var workSpaceService = new Mock<WorkSpaceService>();
            var completeProvider = new Mock<CompleteProvider>();
            var hoverProvider = new Mock<HoverProvider>();
            var codeCheckProvider = new Mock<CodeCheckProvider>();
            // Act
            var completionProvider = new CompletionProvider(workSpaceService.Object, completeProvider.Object, 
                hoverProvider.Object, codeCheckProvider.Object);
            var actual = completionProvider.RunCode(code, read).Result; // 实际的输出
            // Assert
            Assert.AreEqual(expected, actual); // 断言输出是否相等
        }
    }
}
