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
using QA2_Selenium.PageObjectModels;

// Project Requirements: https://docs.google.com/document/d/1YSXJaFg-Am6vQNyrQI8wuJMaX9g0VAs_f7byq2izn-I/edit

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
        public void Generate_And_Download_Event_QR_Code_Custom_Folder()
        {
            // Set up the ChromeOptions to download the file to a specific folder
            ChromeOptions options = new ChromeOptions();
            string downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "MyDownloads"); // Set custom download directory
            Directory.CreateDirectory(downloadDirectory);
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);


            using (IWebDriver driver = new ChromeDriver(options))
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                PageModel page = new PageModel(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://4qrcode.com/");

                // Opens Scanner in new tab
                Actions openInNewTab = new Actions(driver);
                openInNewTab
                    .KeyDown(Keys.LeftControl)
                    .Click(page.ScannerLink)
                    .KeyUp(Keys.LeftControl)
                    .Build()
                    .Perform();
                driver.SwitchTo().Window(driver.WindowHandles.First());

                // Fill out Event form
                string title = "Pants Appreciation Month";
                string location = "Everywhere";
                string description = "Celebrate pants all month long!";

                page.EventButton.Click();
                page.EventTitle.SendKeys(title);
                page.EventLocation.SendKeys(location);
                page.EventStartDateInput.Click();
                page.EventFirstDayOfMonth.Click();
                page.EventEndDateInput.Click();
                page.EventLastDayOfMonth.Click();                 
                page.EventNotes.SendKeys(description);
                page.EventSaveButton.Click();

                // Optional: Show tooltip
                Actions actions = new Actions(driver);
                actions.MoveToElement(page.EventToolTip).Perform();

                // Download QR code png file
                page.EventSavePngButton.Click();
                
                // Get the file name and path for file upload
                string fileName = page.EventPngName.GetAttribute("download");
                string filePath = Path.Combine(downloadDirectory, fileName).Replace("\\", "\\\\");
                output.WriteLine(fileName);
                output.WriteLine(Directory.GetCurrentDirectory());
                output.WriteLine(filePath);

                //////////////////////////////////////////////////////////////////////////////////////////////////////////

                // TODO: Refactor file upload code
                // use anchor link instead of navigate
                //driver.Navigate().GoToUrl("https://4qrcode.com/scan-qr-code.php");
                var scanPage = new ScanPage(driver, wait);

                Thread.Sleep(TimeSpan.FromSeconds(5));
                driver.SwitchTo().Window(driver.WindowHandles.Last());

                // Upload file
                IWebElement fileUpload = driver.FindElement(By.XPath("//input[@id='file-selector']"));
                scanPage.FileUpload.SendKeys(filePath);
                //Thread.Sleep(TimeSpan.FromSeconds(3));

                // Get the result
                string resultText = scanPage.ScanResult.GetAttribute("value");

                // Check the QR text for correct text information
                using (new AssertionScope())
                {
                    resultText.Should().Contain($"SUMMARY:{title}");
                    resultText.Should().Contain($"LOCATION:{location}");
                    resultText.Should().Contain($"DESCRIPTION:{description}");
                }
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

                Actions actions = new Actions(driver);
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