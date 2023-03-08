using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using QA_2_Browser_Testing.PageObjectModels;
using Xunit.Abstractions;
using FluentAssertions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.ComponentModel.Design;
using OpenQA.Selenium.Interactions;
using FluentAssertions.Execution;

// Generate, then Read
// https://4qrcode.com/ + https://4qrcode.com/scan-qr-code.php

// QR Code Readers
// https://qreader.online/
// https://me-qr.com/
// https://4qrcode.com/scan-qr-code.php                

// Tooltip6
// https://me-qr.com/ + https://me-qr-scanner.com/qr-scanner#scan-using-file

// File Download
// https://www.youtube.com/watch?v=w1QA5-rYELg
// https://www.youtube.com/watch?v=_8fwyB0t5Ac


namespace QA2_Selenium
{
    public class UnitTests
    {
        private readonly ITestOutputHelper output;

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        // Used for exposing DateWidget html structure since inspecting in Chrome Dev Tools would cause the DateWidget to lose focus and disappear from the DOM tree
        public async Task DownloadPageSourceAsync(IWebDriver driver)
        {
            string pageSource = driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
        }

        
        
        
        [Fact]
        public void Show_DatePicker_Widget_Via_Javascript()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                var homePage = new PageModel(driver, wait);

                homePage.EventButton.Click();

                IWebElement startDatePicker = homePage.EventStartDateInput;
                String javascript = "arguments[0].click()";
                IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
                jse.ExecuteScript(javascript, startDatePicker);
                IWebElement dateWidget = driver.FindElement(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']"));
                var columns = dateWidget.FindElements(By.TagName("td"));
                Thread.Sleep(TimeSpan.FromSeconds(5));
                return;

            }
        }

        [Fact]
        public void Generate_And_Download_Event_QR_Code()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                PageModel page = new PageModel(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");
                page.EventButton.Click();
                page.EventTitle.SendKeys("Pants Appreciation Month");
                page.EventLocation.SendKeys("Everywhere");
                page.EventStartDateInput.Click();
                page.EventFirstDayOfMonth.Click();
                page.EventEndDateInput.Click();
                page.EventLastDayOfMonth.Click();                 
                page.EventNotes.SendKeys("Celebrate pants all month long!");
                page.EventSaveButton.Click();
                page.EventSavePngButton.Click();

                Thread.Sleep(TimeSpan.FromSeconds(3));
            }
        }
        
        [Fact]
        public void ToolTip_Should_Appear_On_Hover()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                PageModel page = new PageModel(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");
                page.EventButton.Click();
                page.EventTitle.SendKeys("Pants Appreciation Month");
                page.EventLocation.SendKeys("Everywhere");
                page.EventStartDateInput.Click();
                page.EventFirstDayOfMonth.Click();
                page.EventEndDateInput.Click();
                page.EventLastDayOfMonth.Click();                 
                page.EventNotes.SendKeys("Celebrate pants all month long!");
                page.EventSaveButton.Click();

                // TODO: implement tooltip
                // hover over to display tooltip
                // Create an instance of the Actions class
                Actions actions = new Actions(driver);
                // Move the mouse pointer over the element to hover over
                actions.MoveToElement(page.EventToolTip).Perform();
                
                using (new AssertionScope())
                {
                    page.EventToolTipText.Displayed.Should().BeTrue();
                    page.EventToolTipText.Text.Should().Be("Copy URL");
                }                
            }
        }
    }
}