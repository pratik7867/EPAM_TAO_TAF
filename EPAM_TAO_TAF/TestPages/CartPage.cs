using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF.BaseTestConfig;
using EPAM_TAO_TAF.TestSetup;
using EPAM_TAO_TAF.UI_Helpers;

namespace EPAM_TAO_TAF.TestPages
{
    public class CartPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static CartPage _cartPage = null;

        BaseTestConfigurations baseTestConfigurations { get; set; }

        CartPage(BaseTestConfigurations _baseTestConfigurations) : base(_baseTestConfigurations.driver)
        {
            baseTestConfigurations = _baseTestConfigurations;
        }

        public static CartPage GetInstance(BaseTestConfigurations _baseTestConfigurations)
        {
            lock (syncLock)
            {
                if (_cartPage == null)
                {
                    _cartPage = new CartPage(_baseTestConfigurations);
                }
                return _cartPage;
            }
        }

        #region Elements/Locators        

        [FindsBy(How = How.ClassName, Using = "inventory_item_price")]
        IWebElement divProductPrice { get; set; }

        [FindsBy(How = How.Id, Using = "checkout")]
        IWebElement btnCheckout { get; set; }

        #endregion

        public string GetProductPrice()
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(baseTestConfigurations.driver, 10);

            return divProductPrice.Text;
        }

        public CheckoutPage ClickOnCheckout()
        {
            btnCheckout.Click();

            return CheckoutPage.GetInstance(baseTestConfigurations);
        }
    }
}
