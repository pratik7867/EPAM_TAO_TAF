using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using EPAM_TAO_TAF.BaseTestConfig;
using EPAM_TAO_TAF.TestSetup;
using EPAM_TAO_TAF.UI_Helpers;

namespace EPAM_TAO_TAF.TestPages
{
    public class ProductsPage : DriverPageContext
    {
        private static readonly object syncLock = new object();
        private static ProductsPage _productsPage = null;

        BaseTestConfigurations baseTestConfigurations { get; set; }

        ProductsPage(BaseTestConfigurations _baseTestConfigurations) : base(_baseTestConfigurations.driver)
        {
            baseTestConfigurations = _baseTestConfigurations;
        }

        public static ProductsPage GetInstance(BaseTestConfigurations _baseTestConfigurations)
        {
            lock (syncLock)
            {
                if (_productsPage == null)
                {
                    _productsPage = new ProductsPage(_baseTestConfigurations);
                }
                return _productsPage;
            }
        }

        #region Elements/Locators
        
        string strDivProductPriceLocator = ".//button[@id='add-to-cart-{0}']/preceding::div[@class='inventory_item_price']";
        string strBtnAddToCartLocator = "add-to-cart-{0}";

        [FindsBy(How = How.Id, Using= "shopping_cart_container")]
        IWebElement btnShoppingCart { get; set; }        

        #endregion

        #region Action Methods

        public string getProductPrice(string strProductName)
        {
            CommonUtilities.commonUtilities.WaitForPageLoad(baseTestConfigurations.driver, 10);            

            return CommonUtilities.commonUtilities.WaitForElementToBeVisible(baseTestConfigurations.driver, By.XPath(String.Format(strDivProductPriceLocator, strProductName.Replace(" ", "-").ToLower())), 5).Text;
        }

        public void AddToCart(string strProductName)
        {
            CommonUtilities.commonUtilities.WaitForElementToBeVisible(baseTestConfigurations.driver, By.Id(String.Format(strBtnAddToCartLocator, strProductName.Replace(" ", "-").ToLower())), 5).Click();
        }

        public CartPage ClickOnShoppingCart()
        {
            btnShoppingCart.Click();

            return CartPage.GetInstance(baseTestConfigurations);
        }

        #endregion
    }
}
