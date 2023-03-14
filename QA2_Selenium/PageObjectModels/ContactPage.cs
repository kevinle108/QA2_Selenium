using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;

namespace QA2_Selenium.PageObjectModels
{
    internal class ContactPage
    {
        readonly IWebDriver _driver;
        readonly WebDriverWait _wait;
        public string Url = "https://4qrcode.com/contact.php";
        public string Title = "Contact";
        public string EmailAddress = "Support@4qrcode.com";

        public ContactPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(Url);
            EnsurePageLoad();
        }

        public IWebElement ContactEmail => _driver.FindElement(By.XPath("//section[@id='contact']//p"));

        public void EnsurePageLoad()
        {
            _wait.Until(ExpectedConditions.ElementIsVisible((By.XPath("//section[@id='contact']//p"))));
        }
    }
}
