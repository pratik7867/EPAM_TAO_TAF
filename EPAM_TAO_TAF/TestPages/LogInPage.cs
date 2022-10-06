using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF.BaseTestConfig;
using EPAM_TAO_TAF.TestSetup;
using EPAM_TAO_TAF.UI_Helpers;

namespace EPAM_TAO_TAF.TestPages
{
    public class LogInPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static LogInPage _loginPage = null;

        BaseTestConfigurations baseTestConfigurations { get; set; }

        LogInPage(BaseTestConfigurations _baseTestConfigurations) : base(_baseTestConfigurations.driver)
        {
            baseTestConfigurations = _baseTestConfigurations;
        }

        public static LogInPage GetInstance(BaseTestConfigurations _baseTestConfigurations)
        {
            lock (syncLock)
            {
                if (_loginPage == null)
                {
                    _loginPage = new LogInPage(_baseTestConfigurations);
                }
                return _loginPage;
            }
        }

        #region Elements/Locators

        [FindsBy(How=How.Id, Using="user-name")]
        IWebElement txtUserName { get; set; }

        [FindsBy(How = How.Id, Using = "password")]
        IWebElement txtPassword { get; set; }

        [FindsBy(How = How.Id, Using = "login-button")]
        IWebElement btnLogin { get; set; }

        #endregion

        #region Action Methods

        public ProductsPage LogIntoApplication(string strUserName, string strPassword)
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(baseTestConfigurations.driver, 10);

            txtUserName.Clear();
            txtUserName.SendKeys(strUserName);

            txtPassword.Clear();
            txtPassword.SendKeys(strPassword);

            btnLogin.Click();

            return ProductsPage.GetInstance(baseTestConfigurations);
        }

        #endregion
    }
}
