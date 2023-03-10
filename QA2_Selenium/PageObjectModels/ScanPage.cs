using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IWebElement ScanResult => GetScanResult();

        public IWebElement FileUpload => Driver.FindElement(By.XPath("//input[@id='file-selector']"));

        private IWebElement GetScanResult()
        {
            IWebElement scanResult = Driver.FindElement(By.Id("file-qr-result"));
            Wait.Until(ExpectedConditions.TextToBePresentInElementValue(scanResult, "BEGIN:VCALENDAR"));
            return scanResult;
        }

    }
}
