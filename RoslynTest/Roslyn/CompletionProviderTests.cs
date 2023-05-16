using Moq;
using NUnit.Framework;
using RoslynCat.Interface;
using RoslynCat.Roslyn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynCat.Roslyn.Tests
{
    [TestFixture()]
    public class CompletionProviderTests
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

        [Test()]
        public async Task RunCodeTest1() {
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