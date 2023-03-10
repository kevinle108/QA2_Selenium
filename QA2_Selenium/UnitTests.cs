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
        ITestOutputHelper output;
        string homeUrl = "https://4qrcode.com/";
        string scanUrl = "https://4qrcode.com/scan-qr-code.php";
        string eventTitle = "Pants Appreciation Month";
        string eventLocation = "Everywhere";
        string eventDescription = "Celebrate pants all month long!";
        string sampleFileName = "sample.png";

        public UnitTests(ITestOutputHelper output)
        {
            this.output = output;
        }


       [Fact]
        public void File_Uploads_Successfully()
        {
            using IWebDriver driver = new ChromeDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(scanUrl);
            ScanPage scanPage = new ScanPage(driver, wait);

            // Get the file name and path for file upload
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), sampleFileName);
            output.WriteLine(Directory.GetCurrentDirectory());
            output.WriteLine(filePath);

            scanPage.FileUpload.SendKeys(filePath);
            string resultFileName = scanPage.FileName.Text;
            string resultText = scanPage.ScanResult.GetAttribute("value");

            // Check the QR text for correct text information
            using (new AssertionScope())
            {
                resultFileName.Should().Be(sampleFileName);
                resultText.Should().Contain("BEGIN:VCALENDAR");
            }

        }

        [Fact]
        public void Date_Picker_Widget_Input()
        {
            using IWebDriver driver = new ChromeDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeUrl);
            HomePage homePage = new HomePage(driver, wait);
            homePage.EventButton.Click();
            homePage.EventStartDateInput.Click();
            homePage.EventFirstDayOfMonth.Click();
            string dateText = homePage.EventStartDateInput.GetAttribute("value");
            output.WriteLine(dateText);
            dateText.Should().Contain(" 1, ");
        }

        [Fact]
        public void Tooltip_Appears_On_Hover()
        {
            using IWebDriver driver = new ChromeDriver();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(homeUrl);
            HomePage homePage = new HomePage(driver, wait);

            // Scrolls page to better view tooltip
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            homePage.UrlTextInput.SendKeys("https://www.dictionary.com/browse/pants");
            homePage.EventSaveButton.Click();

            var tooltip = homePage.EventToolTip;
            Actions actions = new Actions(driver);

            actions.MoveToElement(homePage.EventToolTip).Perform();


            using (new AssertionScope())
            {
                homePage.EventToolTipText.Displayed.Should().BeTrue();
                homePage.EventToolTipText.Text.Should().Be("Copy URL");
            }
        }




        //[Fact]
        //public void Show_DatePicker_Widget_Via_Javascript()
        //{
        //    using (IWebDriver driver = new ChromeDriver())
        //    {
        //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //        driver.Manage().Window.Maximize();
        //        driver.Navigate().GoToUrl(homeUrl);

        //        var homePage = new HomePage(driver, wait);

        //        homePage.EventButton.Click();

        //        IWebElement startDatePicker = homePage.EventStartDateInput;
        //        String javascript = "arguments[0].click()";
        //        IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
        //        jse.ExecuteScript(javascript, startDatePicker);
        //        IWebElement dateWidget = driver.FindElement(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']"));
        //        var columns = dateWidget.FindElements(By.TagName("td"));
        //        Thread.Sleep(TimeSpan.FromSeconds(5));
        //        return;

        //    }
        //}

        //[Fact]
        //public void Generate_And_Download_Event_QR_Code()
        //{
        //    using (IWebDriver driver = new ChromeDriver())
        //    {
        //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //        HomePage homePage = new HomePage(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
        //        driver.Manage().Window.Maximize();
        //        driver.Navigate().GoToUrl(homeUrl);
        //        homePage.EventButton.Click();
        //        homePage.EventTitle.SendKeys("Pants Appreciation Month");
        //        homePage.EventLocation.SendKeys("Everywhere");
        //        homePage.EventStartDateInput.Click();
        //        homePage.EventFirstDayOfMonth.Click();
        //        homePage.EventEndDateInput.Click();
        //        homePage.EventLastDayOfMonth.Click();                 
        //        homePage.EventNotes.SendKeys("Celebrate pants all month long!");
        //        homePage.EventSaveButton.Click();
        //        homePage.EventSavePngButton.Click();

        //        Thread.Sleep(TimeSpan.FromSeconds(3));
        //    }
        //}
        
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
                HomePage homePage = new HomePage(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(homeUrl);

                // Opens Scanner in new tab
                Actions openInNewTab = new Actions(driver);
                openInNewTab
                    .KeyDown(Keys.LeftControl)
                    .Click(homePage.ScannerLink)
                    .KeyUp(Keys.LeftControl)
                    .Build()
                    .Perform();
                driver.SwitchTo().Window(driver.WindowHandles.First());

                // Fill out Event form
                

                homePage.EventButton.Click();
                homePage.EventTitle.SendKeys(eventTitle);
                homePage.EventLocation.SendKeys(eventLocation);
                homePage.EventStartDateInput.Click();
                homePage.EventFirstDayOfMonth.Click();
                homePage.EventEndDateInput.Click();
                homePage.EventLastDayOfMonth.Click();                 
                homePage.EventNotes.SendKeys(eventDescription);
                homePage.EventSaveButton.Click();

                // Optional: Show tooltip
                Actions actions = new Actions(driver);
                actions.MoveToElement(homePage.EventToolTip).Perform();

                // Download QR code png file
                homePage.EventSavePngButton.Click();
                
                // Get the file name and path for file upload
                string fileName = homePage.EventPngName.GetAttribute("download");
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
                scanPage.FileUpload.SendKeys(filePath);

                // Get the result
                string resultText = scanPage.ScanResult.GetAttribute("value");

                // Check the QR text for correct text information
                using (new AssertionScope())
                {
                    resultText.Should().Contain($"SUMMARY:{eventTitle}");
                    resultText.Should().Contain($"LOCATION:{eventLocation}");
                    resultText.Should().Contain($"DESCRIPTION:{eventDescription}");
                }
            }
        }
        
        // TODO: remove old tooltip test
        //[Fact]
        //public void ToolTip_Should_Appear_On_Hover()
        //{
        //    using (IWebDriver driver = new ChromeDriver())
        //    {
        //        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        //        HomePage homePage = new HomePage(driver, new WebDriverWait(driver, TimeSpan.FromSeconds(5)));
                
        //        driver.Manage().Window.Maximize();
        //        driver.Navigate().GoToUrl(homeUrl);
        //        homePage.EventButton.Click();
        //        homePage.EventTitle.SendKeys("Pants Appreciation Month");
        //        homePage.EventLocation.SendKeys("Everywhere");
        //        homePage.EventStartDateInput.Click();
        //        homePage.EventFirstDayOfMonth.Click();
        //        homePage.EventEndDateInput.Click();
        //        homePage.EventLastDayOfMonth.Click();                 
        //        homePage.EventNotes.SendKeys("Celebrate pants all month long!");
        //        homePage.EventSaveButton.Click();

        //        Actions actions = new Actions(driver);
        //        actions.MoveToElement(homePage.EventToolTip).Perform();
                
        //        using (new AssertionScope())
        //        {
        //            homePage.EventToolTipText.Displayed.Should().BeTrue();
        //            homePage.EventToolTipText.Text.Should().Be("Copy URL");
        //        }                
        //    }
        //}
    }
}