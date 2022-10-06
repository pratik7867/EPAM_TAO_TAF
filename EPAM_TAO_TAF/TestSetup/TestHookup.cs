using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace EPAM_TAO_TAF.TestSetup
{
    public class TestHookup : BasePage
    {
        public enum BrowserType
        {
            CHROME,
            FIREFOX
        };

        public IWebDriver InitBrowser(BrowserType browserType)
        {
            try
            {
                if (browserType == BrowserType.CHROME)
                {
                    driver = new ChromeDriver();
                }
                else if (browserType == BrowserType.FIREFOX)
                {
                    driver = new FirefoxDriver();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return driver;
        }

        public void CloseBrowser()
        {
            try
            {
                if (driver != null)
                {
                    driver.Quit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
