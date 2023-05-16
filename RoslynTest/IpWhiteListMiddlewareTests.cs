using Microsoft.CodeAnalysis.Emit;
using Moq;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using System.Threading.Tasks;

namespace RoslynTest
{
    [TestFixture]
    public class CompletionProviderTest
    {
        private Mock<WorkSpaceService> _workSpaceMock;
        private Mock<CompleteProvider> _completeProviderMock;
        private Mock<HoverProvider> _hoverProviderMock;
        private Mock<CodeCheckProvider> _codeCheckProviderMock;

        [SetUp]
        public void SetUp() {
            // 创建模拟对象
            _workSpaceMock = new Mock<WorkSpaceService>();
            _completeProviderMock = new Mock<CompleteProvider>();
            _hoverProviderMock = new Mock<HoverProvider>();
            _codeCheckProviderMock = new Mock<CodeCheckProvider>();

            // 设置模拟对象的行为和返回值
            //_workSpaceMock.Setup(w => w.GetEmitResultAsync()).ReturnsAsync(new EmitResult(true,null,null,null));
            // 其他模拟对象的设置根据需要进行
        }

        [Test]
        public void RunCode_Should_Return_Correct_Result_When_Code_Is_Valid() {
            // Arrange
            var code = "Console.WriteLine(Hello World);"; // 有效的代码
            var read = ""; // 空的输入
            var expected = "Hello World\r\n"; // 期望的输出

            // Act
            var completionProvider = new CompletionProvider(_workSpaceMock.Object,_completeProviderMock.Object,_hoverProviderMock.Object,_codeCheckProviderMock.Object);
            var actual = completionProvider.RunCode(code,read).Result; // 实际的输出

            // Assert
            Assert.AreEqual(expected,actual); // 断言输出是否相等
        }

        // 其他测试方法根据需要编写
    }
    [TestFixture]
    public class RunCodeTests
    {
        private CompletionProvider completionProvider;

        [SetUp]
        public void SetUp() {
            var workSpaceService = new Mock<IWorkSpaceService>();
            var completeProvider = new Mock<ICompleteProvider>();
            var hoverProvider = new Mock<IHoverProvider>();
            var codeCheckProvider = new Mock<ICodeCheckProvider>();
            //workSpaceService.Setup(w => w.GetEmitResultAsync()).ReturnsAsync(new EmitResult());
            completionProvider = new CompletionProvider(workSpaceService.Object,completeProvider.Object,hoverProvider.Object,codeCheckProvider.Object);
        }

        [Test]
        public async Task RunCode_Should_Return_Correct_Result_When_Code_Is_Valid() {
            // Arrange
            string code = "using System; class Program { static void Main(string[] args) { Console.WriteLine(\"Hello World!\"); } }";
            string expected = "Hello World!\r\n";

            // Act
            string actual = await completionProvider.RunCode(code);

            // Assert
            Assert.AreEqual(expected,actual);
        }

    }
}
