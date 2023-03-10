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
        private readonly IWebDriver Driver;

        private readonly WebDriverWait Wait;


        public ScanPage(IWebDriver driver, WebDriverWait wait)
        {
            Driver = driver;
            Wait = wait;
        }


        public IWebElement FileUpload => Driver.FindElement(By.XPath("//input[@id='file-selector']"));

        // Explict wait for a matching partial text to appear in the name element
        public IWebElement FileName
        {
            get
            {                  
                //CheckModal();
                Wait.Until(ExpectedConditions.TextToBePresentInElement(Driver.FindElement(By.Id("js-file-name")), "."));
                return Driver.FindElement(By.Id("js-file-name"));
            }
        }

        // Explict wait for a matching partial text to appear in the result element
        public IWebElement ScanResult
        {
            get
            {
                //CheckModal();
                IWebElement scanResult = Driver.FindElement(By.Id("file-qr-result"));
                Wait.Until(
                    ExpectedConditions
                    .TextToBePresentInElementValue(scanResult, "BEGIN:VCALENDAR"));
                return scanResult;
            }
        }

        public IWebElement Modal => Wait.Until(ExpectedConditions.ElementIsVisible(By.Id("saveTool")));

        public void CheckModal()
        {
            if (Driver.FindElement(By.Id("saveTool")).Displayed)
            {
                Driver.ExecuteJavaScript("document.getElementById('saveTool').remove();");
            }
        }

        public async Task DownloadPageSourceAsync()
        {
            string pageSource = Driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
        }
    }
}
