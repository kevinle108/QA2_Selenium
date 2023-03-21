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
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

// Project Requirements: https://docs.google.com/document/d/1YSXJaFg-Am6vQNyrQI8wuJMaX9g0VAs_f7byq2izn-I/edit

// Generate, then Read
// https://4qrcode.com/ + https://4qrcode.com/scan-qr-code.php

// Questions for Class
// test run order

namespace QA2_Selenium
{
    public class UnitTests
    {
        readonly ITestOutputHelper _output;
        readonly string pantsUrl = "https://en.wikipedia.org/wiki/Trousers";
        readonly string eventTitle = "Pants Appreciation Month";
        readonly string eventLocation = "Everywhere";
        readonly string eventDescription = "Celebrate pants all month long!";
        readonly string sampleFileName = "sample.png";

        public UnitTests(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public void HomePage_Loads_Successfully()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);
            driver.Title.Should().Be(homePage.Title);
        }

        [Fact]
        public void ScanPage_Loads_Successfully()
        {
            using IWebDriver driver = new ChromeDriver();
            var scanPage = new ScanPage(driver);
            driver.Navigate().GoToUrl(scanPage.Url);
            driver.Title.Should().Be(scanPage.Title);
        }

        [Fact]
        public void ContactPage_Loads_Successfully()
        {
            using IWebDriver driver = new ChromeDriver();
            ContactPage contactPage = new ContactPage(driver);
            driver.Title.Should().Be(contactPage.Title);
        }

        [Fact]
        public void ScannerLink_Routes_To_ScanPage()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);
            homePage.ScannerLink.Click();
            ScanPage scanPage = new ScanPage(driver);
            driver.Title.Should().Be(scanPage.Title);
        }

        [Fact]
        public void ContactLink_Routes_To_ContactPage()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);
            homePage.ContactLink.Click();
            ContactPage contactPage = new ContactPage(driver);
            driver.Title.Should().Be(contactPage.Title);
        }

        [Fact]
        public void ScannerLink_Opens_In_NewTab()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            // Opens Scanner in new tab
            Actions openInNewTab = new Actions(driver);
            openInNewTab
                .KeyDown(Keys.LeftControl)
                .Click(homePage.ScannerLink)
                .KeyUp(Keys.LeftControl)
                .Build()
                .Perform();
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            ScanPage scanPage = new ScanPage(driver);
            using (new AssertionScope())
            {
                driver.WindowHandles.Should().HaveCount(2);
                driver.Title.Should().Be(scanPage.Title);
            }
        }

        [Fact]
        public void Change_Language_To_Spanish()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);
            homePage.LangChangeButton.Click();
            homePage.LangSpanishOption.Click();
            homePage.EnsurePageLoad();

            using (new AssertionScope())
            {
                driver.Url.Should().Be("https://4qrcode.com/?lang=es");
                driver.Title.Should().Be("4qrcode - Crea tu código QR gratis en línea (Generador de código Qr)");
            }

        }

        [Fact]
        public void Color_Picker_Changes_QR_Background()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            // Scrolls page to better view tooltip
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, (document.body.scrollHeight)/2)");

            homePage.UrlTextInput.SendKeys(pantsUrl);
            homePage.ColorsButton.Click();
            homePage.ColorPicker.Clear();
            homePage.ColorPicker.SendKeys("#fb0404");
            var color = homePage.QrBox.GetCssValue("fill");

            using (new AssertionScope())
            {
                homePage.QrBox.Displayed.Should().BeTrue();
                color.Should().Be("rgb(251, 4, 4)");
            }
        }

        [Fact]
        public void EventButton_Displays_Event_Section()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            homePage.EventButton.Click();            

            using (new AssertionScope())
            {
                homePage.EventHeader.Displayed.Should().BeTrue();
                homePage.EventHeader.Text.Should().Be("Event QR Code");
            }
        }
        
        [Fact]
        public void Reminder_Dropdown_Selects_Event_Start()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            homePage.EventButton.Click();
            homePage.EventStartDateInput.Click();
            homePage.EventFirstDayOfMonth.Click();

            // Scrolls page to better view tooltip
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            homePage.EventReminderDropdown.Click();
            homePage.EventReminderNowOption.Click();

            homePage.EventReminderDropdown.GetDomProperty("selectedIndex").Should().Be("1");
        }

        [Fact]
        public void Correct_Contact_Email()
        {
            using IWebDriver driver = new ChromeDriver();
            ContactPage contactPage = new ContactPage(driver);
            contactPage.ContactEmail.Text.Should().Be(contactPage.EmailAddress);
        }

        [Fact]
        public void File_Uploads_Successfully()
        {
            using IWebDriver driver = new ChromeDriver();            
            var scanPage = new ScanPage(driver);
            driver.Navigate().GoToUrl(scanPage.Url);

            // Get the file name and path for file upload
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), sampleFileName);
            _output.WriteLine(Directory.GetCurrentDirectory());
            _output.WriteLine(filePath);

            // upload file
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
            HomePage homePage = new HomePage(driver);
            homePage.EventButton.Click();
            homePage.EventStartDateInput.Click();
            homePage.EventFirstDayOfMonth.Click();
            string dateText = homePage.EventStartDateInput.GetAttribute("value");

            dateText.Should().Contain(" 1, ");
        }

        [Fact]
        public void Tooltip_Appears_On_Hover()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            // Scrolls page to better view tooltip
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

            homePage.UrlTextInput.SendKeys(pantsUrl);
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
        
        [Fact]
        public void Negative_Test_Incomplete_Event()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            homePage.EventButton.Click();
            homePage.EventTitle.SendKeys(eventTitle);
            
            homePage.Alert.Text.Should().Be("Please provide more data");                       
        }
        
        [Fact]
        public void Negative_Test_Phone_Number_Rejects_Letters()
        {
            using IWebDriver driver = new ChromeDriver();
            HomePage homePage = new HomePage(driver);

            homePage.PhoneButton.Click();
            homePage.PhoneNumberInput.SendKeys("abc123");
            
            homePage.PhoneNumberInput.GetAttribute("value").Should().Be("123");                       
        }

        [Fact]
        public void Generate_Download_And_Verify_QR_Code()
        {
            // Set up the ChromeOptions to download the file to a specific folder
            ChromeOptions options = new ChromeOptions();
            string downloadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "MyDownloads"); // Set custom download directory
            Directory.CreateDirectory(downloadDirectory);
            options.AddUserProfilePreference("download.default_directory", downloadDirectory);

            using IWebDriver driver = new ChromeDriver(options);
            HomePage homePage = new HomePage(driver);

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

            // check for download completion by looking for donate modal
            homePage.CheckModal();
            //homePage.DonateModal.Click();           

            // Get the file name and path for file upload
            string fileName = homePage.EventPngName.GetAttribute("download");
            string filePath = Path.Combine(downloadDirectory, fileName);
            _output.WriteLine(fileName);
            _output.WriteLine(Directory.GetCurrentDirectory());
            _output.WriteLine(filePath);

            var scanPage = new ScanPage(driver);
            driver.SwitchTo().Window(driver.WindowHandles.Last());

            // Upload file
            IWebElement saveModal = driver.FindElement(By.XPath("//div[@id='saveTool']//button[@aria-label='Close']"));
            _output.WriteLine("saveModal " + saveModal.Displayed.ToString());
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
}