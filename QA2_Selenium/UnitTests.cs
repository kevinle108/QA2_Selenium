using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QA_2_Browser_Testing.PageObjectModels;
using Xunit.Abstractions;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.ComponentModel.Design;

namespace QA2_Selenium
{
    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        public async Task DownloadPageSourceAsync(IWebDriver driver)
        {
            string pageSource = driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
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
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                homePage.EventTitle.SendKeys("End of Class Party");
                homePage.EventLocation.SendKeys("Online");
                homePage.EventStartDatePicker.SendKeys("March 31, 2023 1:00 PM");
                homePage.EventEndDatePicker.SendKeys("March 31, 2023 5:00 PM");
                homePage.EventNotes.SendKeys("To celebrate completing the Code Louisville QA track!");
                //bool canSave = homePage.EventSaveButton.Enabled;

                IWebElement saveBtn =
                    wait.Until(ExpectedConditions.ElementToBeClickable(homePage.EventSaveButton));

                saveBtn.Click();

                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                //IWebElement datePickerWidget =
                //    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("div[@class='bootstrap-datetimepicker-widget dropdown-menu top'")));




                Thread.Sleep(TimeSpan.FromSeconds(2));



                //string title = driver.Title;
                //IWebElement houseinfo = driver.FindElement(By.Id("house-info"));

                //title.Should().Be("8110 Kellerman Rd, Louisville, KY 40219 | MLS# 1631002 | Redfin");
                //houseinfo.Text.Should().Be("");
                //title.Should().Be("Docs • Svelte");

            }
        }
        
        [Fact]
        public void Show_DatePicker_Widget_Via_Javascript()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                IWebElement startDatePicker = homePage.EventStartDatePicker;
                String javascript = "arguments[0].click()";
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript(javascript, startDatePicker);
                IWebElement dateWidget = driver.FindElement(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']"));
                var columns = dateWidget.FindElements(By.TagName("td"));
                Thread.Sleep(TimeSpan.FromSeconds(5));
                return;
                homePage.EventStartDatePicker.SendKeys("March 31, 2023 1:00 PM");
                homePage.EventEndDatePicker.SendKeys("March 31, 2023 5:00 PM");
                homePage.EventNotes.SendKeys("To celebrate completing the Code Louisville QA track!");
                //bool canSave = homePage.EventSaveButton.Enabled;

                IWebElement saveBtn =
                    wait.Until(ExpectedConditions.ElementToBeClickable(homePage.EventSaveButton));

                saveBtn.Click();

                //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                //IWebElement datePickerWidget =
                //    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("div[@class='bootstrap-datetimepicker-widget dropdown-menu top'")));




                Thread.Sleep(TimeSpan.FromSeconds(2));



                //string title = driver.Title;
                //IWebElement houseinfo = driver.FindElement(By.Id("house-info"));

                //title.Should().Be("8110 Kellerman Rd, Louisville, KY 40219 | MLS# 1631002 | Redfin");
                //houseinfo.Text.Should().Be("");
                //title.Should().Be("Docs • Svelte");

            }
        }
        [Fact]
        public async Task Show_DatePicker_Widget_Via_Wait()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new HomePage(driver);

                homePage.EventButton.Click();

                homePage.EventTitle.SendKeys("Pants Appreciation Month");
                homePage.EventLocation.SendKeys("Everywhere");

                homePage.EventStartDatePicker.Click();

                IWebElement dateWidget =
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']")));

                List<IWebElement> days = dateWidget.FindElements(By.XPath("//td[@data-action='selectDay']")).ToList();

                // Get the current month
                var month = DateTime.Now.Month;
                var year = DateTime.Now.Year;
                var lastDayOfMonth = DateTime.DaysInMonth(year, month);                

                IWebElement first = dateWidget.FindElement(By.XPath("//td[text()='1']"));
                //IWebElement fifth = dateWidget.FindElement(By.XPath("//td[text()='5']"));
                first.Click();
                //Thread.Sleep(TimeSpan.FromSeconds(10));
                //fifth.Click();
                wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//td[text()='1']"))).Click();

                homePage.EventEndDatePicker.Click();
                dateWidget = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']")));
                var dayOptions = dateWidget.FindElements(By.XPath($"//td[text()='{lastDayOfMonth}']"));
                dayOptions[dayOptions.Count - 1].Click();                 

                homePage.EventNotes.SendKeys("Celebrate pants all month long!");

                IWebElement saveBtn =
                    wait.Until(ExpectedConditions.ElementToBeClickable(homePage.EventSaveButton));

                saveBtn.Click();

                Thread.Sleep(TimeSpan.FromSeconds(3));

                //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class, 'linksholder')]//button[contains(@class, 'svgtopng')]"))).Click();
                homePage.ClickSavePngButton();

                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }
    }
}