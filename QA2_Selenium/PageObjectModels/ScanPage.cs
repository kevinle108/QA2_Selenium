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

namespace QA2_Selenium.PageObjectModels
{
    internal class ScanPage
    {
        readonly IWebDriver _driver;
        readonly WebDriverWait _wait;
        public string Url = "https://4qrcode.com/scan-qr-code.php";

        public ScanPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            _driver.Manage().Window.Maximize();
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

        public IWebElement Modal => _wait.Until(ExpectedConditions.ElementIsVisible(By.Id("saveTool")));


        public void EnsurePageLoad()
        {
            _wait.Until(ExpectedConditions.ElementExists(By.Id("saveTool")));
        }

        public void CheckModal()
        {
            if (_driver.FindElement(By.Id("saveTool")).Displayed)
            {
                _driver.ExecuteJavaScript("document.getElementById('saveTool').remove();");
            }
        }

        public async Task DownloadPageSourceAsync()
        {
            string pageSource = _driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
        }
    }
}
