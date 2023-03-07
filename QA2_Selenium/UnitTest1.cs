using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QA_2_Browser_Testing.PageObjectModels;
using Xunit.Abstractions;
using FluentAssertions;

namespace QA2_Selenium
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;

        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void LoadPage_()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                IWebElement eventHeader = homePage.EventHeader;
                

                Thread.Sleep(2000);



                //string title = driver.Title;
                //IWebElement houseinfo = driver.FindElement(By.Id("house-info"));

                //title.Should().Be("8110 Kellerman Rd, Louisville, KY 40219 | MLS# 1631002 | Redfin");
                //houseinfo.Text.Should().Be("");
                //title.Should().Be("Docs • Svelte");

            }
        }
    }
}