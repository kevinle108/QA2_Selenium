using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA_2_Browser_Testing.PageObjectModels
{
    internal class HomePage
    {
        private readonly IWebDriver Driver;

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }
        
        // TODO: Remove this
        public IWebElement SampleElement 
        {
            get
            {
                return Driver.FindElement(By.Id("house-info"));
            }
        }

        //public IWebElement EventButton => Driver.FindElement(By.XPath("//a[.//span[text()='Event']]"));
        public IWebElement EventButton => Driver.FindElement(By.XPath("//span[text()='Event']/ancestor::a"));

        //public IWebElement EventHeader => Driver.FindElement(By.XPath("//h2[text()='Event QR Code']"));
        public IWebElement EventHeader => Driver.FindElement(By.XPath("//*[@id=\"event\"]/h2"));

    }
}
