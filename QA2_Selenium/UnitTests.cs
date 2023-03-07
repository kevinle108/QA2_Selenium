using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QA_2_Browser_Testing.PageObjectModels;
using Xunit.Abstractions;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace QA2_Selenium
{
    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // click the event button to display the event qr generator
        [Fact]
        public void LoadEventSection()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                IWebElement eventHeader = homePage.EventHeader;

                eventHeader.Displayed.Should().BeTrue();
                

                Thread.Sleep(2000);



                //string title = driver.Title;
                //IWebElement houseinfo = driver.FindElement(By.Id("house-info"));

                //title.Should().Be("8110 Kellerman Rd, Louisville, KY 40219 | MLS# 1631002 | Redfin");
                //houseinfo.Text.Should().Be("");
                //title.Should().Be("Docs • Svelte");

            }
        }
        
        [Fact]
        public void ClickDatePicker()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                homePage.EventTitle.SendKeys("End of Class Party");
                homePage.EventLocation.SendKeys("Online");
                homePage.EventStartDatePicker.SendKeys("March 31, 2023 1:00 PM");
                homePage.EventEndDatePicker.SendKeys("March 31, 2023 5:00 PM");
                homePage.EventNotes.SendKeys("To celebrate completing the Code Louisville QA track!");


                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                //IWebElement datePickerWidget =
                //    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("div[@class='bootstrap-datetimepicker-widget dropdown-menu top'")));




                Thread.Sleep(10000);



                //string title = driver.Title;
                //IWebElement houseinfo = driver.FindElement(By.Id("house-info"));

                //title.Should().Be("8110 Kellerman Rd, Louisville, KY 40219 | MLS# 1631002 | Redfin");
                //houseinfo.Text.Should().Be("");
                //title.Should().Be("Docs • Svelte");

            }
        }
    }
}