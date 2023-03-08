using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA_2_Browser_Testing.PageObjectModels
{
    internal class PageModel
    {
        private readonly IWebDriver Driver;

        private readonly WebDriverWait Wait;

        public PageModel(IWebDriver driver, WebDriverWait wait)
        {
            Driver = driver;
            Wait = wait;
        }
        

        public IWebElement EventButton => Driver.FindElement(By.XPath("//span[text()='Event']/ancestor::a"));
        public IWebElement EventHeader => Driver.FindElement(By.XPath("//*[@id='event']/h2"));
        public IWebElement EventTitle => Driver.FindElement(By.XPath("//label[text()='Event title']/following-sibling::input"));        
        public IWebElement EventLocation => Driver.FindElement(By.XPath("//label[text()='Location']/following-sibling::input"));        
        public IWebElement EventStartDateInput => Driver.FindElement(By.XPath("//*[@id='eventstart']"));
        public IWebElement EventEndDateInput => Driver.FindElement(By.XPath("//*[@id='eventend']"));        
        public IWebElement EventDateWidget => Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']")));
        public IWebElement EventFirstDayOfMonth => EventDateWidget.FindElement(By.XPath("//td[text()='1']"));
        public IWebElement EventLastDayOfMonth => GetLastDay();
        public IWebElement EventNotes => Driver.FindElement(By.XPath("//label[text()='Notes']/following-sibling::textarea"));
        public IWebElement EventSaveButton => Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@id='preloadSave']//parent::button")));
        public IWebElement EventSavePngButton => Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class, 'linksholder')]//button[contains(@class, 'svgtopng')]")));
        
        public IWebElement EventPngName => Driver.FindElement(By.XPath("//div[contains(@class, 'linksholder')]//a[@class='serve-png d-none']"));

        public IWebElement EventToolTip => Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='tooltip2']")));
        public IWebElement EventToolTipText => Wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='myTooltip']")));

        public IWebElement ScannerLink => Driver.FindElement(By.XPath("//a[@id='scan']"));

        // Returns the last day element of the current month. This prevents accidentally selecting the last day of the previous month if present in the date widget
        private IWebElement GetLastDay()
        {
            int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var dayOptions = EventDateWidget.FindElements(By.XPath($"//td[text()='{lastDayOfMonth}']"));
            return dayOptions[dayOptions.Count - 1];
        }
    }
}
