﻿using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using QA2_Selenium.PageObjectModels;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace QA_2_Browser_Testing.PageObjectModels
{
    internal class HomePage
    {
        readonly IWebDriver _driver;
        readonly WebDriverWait _wait;
        public string Url = "https://4qrcode.com/";
        public string Title = "4qrcode - Free online QR Code generator ( create a QR Code ).";

        public HomePage(IWebDriver driver, WebDriverWait wait)
        {
            _driver = driver;
            _wait = wait;
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
            EnsurePageLoad();
        }

        public IWebElement LangChangeButton => _driver.FindElement(By.Id("navbarDropdown"));
        public IWebElement LangSpanishOption => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@aria-labelledby='navbarDropdown']//a[@href='https://4qrcode.com/?lang=es']")));
        public IWebElement UrlTextInput => _driver.FindElement(By.Id("malink"));
        public IWebElement ColorsButton => _driver.FindElement(By.Id("btn1"));
        public IWebElement ColorPicker => _driver.FindElement(By.Id("qrcolorpicker"));
        public IWebElement QrBox => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[name()='svg']/*[name()='rect']")));
        public IWebElement ScannerLink => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("scan")));
        public IWebElement ContactLink => _wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("contact")));               
        public IWebElement DonateModal => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='exampleModal']//button[@aria-label='Close']")));
        public IWebElement Alert => _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@role='alert']//div[contains(@class, 'toast-body')]")));
        public IWebElement LinkButton => _driver.FindElement(By.XPath("//a[@href='#link']"));
        public IWebElement PhoneButton => _driver.FindElement(By.XPath("//a[@href='#tel']"));
        public IWebElement PhoneNumberInput => _driver.FindElement(By.XPath("//div[@id='tel']//input"));

        // Event QR Section Elements
        public IWebElement EventButton => _driver.FindElement(By.XPath("//span[text()='Event']/ancestor::a"));
        public IWebElement EventHeader => _driver.FindElement(By.XPath("//*[@id='event']/h2"));
        public IWebElement EventTitle => _driver.FindElement(By.XPath("//label[text()='Event title']/following-sibling::input"));
        public IWebElement EventLocation => _driver.FindElement(By.XPath("//label[text()='Location']/following-sibling::input"));
        public IWebElement EventStartDateInput => _driver.FindElement(By.XPath("//*[@id='eventstart']"));
        public IWebElement EventEndDateInput => _driver.FindElement(By.XPath("//*[@id='eventend']"));
        public IWebElement EventDateWidget => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[@class='bootstrap-datetimepicker-widget dropdown-menu top']")));
        public IWebElement EventFirstDayOfMonth => EventDateWidget.FindElement(By.XPath("//td[text()='1']"));
        public IWebElement EventLastDayOfMonth => GetLastDay();
        public IWebElement EventReminderDropdown => _driver.FindElement(By.Id("eventreminder"));
        public IWebElement EventReminderNowOption => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//option[@value='PT0M']")));
        public IWebElement EventNotes => _driver.FindElement(By.XPath("//label[text()='Notes']/following-sibling::textarea"));
        public IWebElement EventSaveButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@id='preloadSave']//parent::button")));
        public IWebElement EventSavePngButton => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class, 'linksholder')]//button[contains(@class, 'svgtopng')]")));
        public IWebElement EventPngName => _driver.FindElement(By.XPath("//div[contains(@class, 'linksholder')]//a[@class='serve-png d-none']"));
        public IWebElement EventToolTip => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//span[@class='tooltip2']")));
        public IWebElement EventToolTipText => _wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='myTooltip']")));        

        public IWebElement CheckModal()
        {
            var ele = _driver.FindElement((By.XPath("//div[@id='exampleModal']//button[@aria-label='Close']")));
            ele = _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@id='exampleModal']//button[@aria-label='Close']")));
            ele.Click();
            ele = _driver.FindElement((By.XPath("//div[@id='exampleModal']//button[@aria-label='Close']")));
            return ele;
        }


        public void EnsurePageLoad()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//a[@href='#link']")));
        }

        // Returns the last day element of the current month. This prevents accidentally selecting the last day of the previous month if present in the date widget
        private IWebElement GetLastDay()
        {
            int lastDayOfMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            var dayOptions = EventDateWidget.FindElements(By.XPath($"//td[text()='{lastDayOfMonth}']"));
            return dayOptions[^1];
        }

        // Used for exposing DateWidget html structure since inspecting in Chrome Dev Tools would cause the DateWidget to lose focus and disappear from the DOM tree
        public async Task DownloadPageSourceAsync()
        {
            string pageSource = _driver.PageSource;
            await File.WriteAllTextAsync("PageSource.html", pageSource);
        }
    }
}
