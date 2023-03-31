using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using OpenQA.Selenium.Support.Extensions;
using Xunit.Abstractions;

namespace QA2_Selenium.PageObjectModels
{
    internal class ScanPage
    {
        readonly IWebDriver _driver;
        readonly WebDriverWait _wait;
        public string Url = "https://4qrcode.com/scan-qr-code.php";
        public string Title = "4qrcode - Free online QR Code reader camera or with image";

        public ScanPage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _driver.Manage().Window.Maximize();
            EnsurePageLoad();
        }


        public IWebElement FileUpload => _driver.FindElement(By.XPath("//input[@id='file-selector']"));

        // Explict wait for a matching partial text to appear in the name element
        public IWebElement FileName
        {
            get
            {                  
                //CheckModal();
                _wait.Until(ExpectedConditions.TextToBePresentInElement(_driver.FindElement(By.Id("js-file-name")), "."));
                return _driver.FindElement(By.Id("js-file-name"));
            }
        }

        // Explict wait for a matching partial text to appear in the result element
        public IWebElement ScanResult
        {
            get
            {
                //CheckModal();
                IWebElement scanResult = _driver.FindElement(By.Id("file-qr-result"));
                _wait.Until(
                    ExpectedConditions
                    .TextToBePresentInElementValue(scanResult, "BEGIN:VCALENDAR"));
                return scanResult;
            }
        }

        public IWebElement CloseModalButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@id='saveTool']//button[@aria-label='Close']")));

        public void EnsurePageLoad()
        {
            _wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[@id='saveTool']")));
        }

        public IWebElement CheckModal()
        {
            string xpath = "//div[@id='saveTool']//button[@aria-label='Close']";
            var ele = _driver.FindElement((By.XPath(xpath)));
            ele = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xpath)));
            ele.Click();
            ele = _driver.FindElement((By.XPath(xpath)));
            return ele;
        }

        public async Task DownloadPageSourceAsync()
        {
            string pageSource = _driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
        }
    }
}
