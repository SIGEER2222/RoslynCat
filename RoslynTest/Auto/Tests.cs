using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Playwright.NUnit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoslynTest.Auto
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class Tests : PageTest
    {
        [Test]
        public async Task HomepageHasPlaywrightInTitle()
        {
            await Page.GotoAsync("https://www.baidu.com/");
            // Expect a title "to contain" a substring.
            //await Expect(Page).ToHaveTitleAsync("Playwright");
            // 截图并保存为example.png
            await Page.ScreenshotAsync(new() { Path = "example.png" });
        }
    }
}
