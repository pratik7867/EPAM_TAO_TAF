using System.Configuration;
using NUnit.Framework;
using EPAM_TAO_TAF.BaseTestConfig;
using EPAM_TAO_TAF.TestPages;

namespace EPAM_TAO_TAF.Tests
{
    [TestFixture]
    public class ProductCheckoutWorkFlow : BaseTestConfigurations
    {
        [Test]
        public void ProductCheckoutTest()
        {
            //DATA Arrangements
            var productName = ConfigurationManager.AppSettings["ProductName"].ToString();
            var firstName = ConfigurationManager.AppSettings["FirstName"].ToString();
            var lastName = ConfigurationManager.AppSettings["LastName"].ToString();
            var postalCode = ConfigurationManager.AppSettings["PostalCode"].ToString();

            //Log in to application
            ProductsPage productsPage = LogInPage.GetInstance(this).LogIntoApplication(strUserName, strPassword);

            //Add Product to Cart
            var productPrice = productsPage.getProductPrice(productName);
            productsPage.AddToCart(productName);

            //Navigate to Cart Page and Validate Price of the added Product
            CartPage cartPage = productsPage.ClickOnShoppingCart();
            Assert.AreEqual(productPrice, cartPage.GetProductPrice());

            //Navigate to Checkout Page and Fill up mandatory details to conitune
            CheckoutPage checkoutPage = cartPage.ClickOnCheckout();
            checkoutPage.FillUpCheckoutDetailsAndContiue(firstName, lastName, postalCode);

            //Validate Name and Price of the added Product and click on Finish button
            Assert.AreEqual(productName, checkoutPage.GetProductName());
            Assert.AreEqual(productPrice, checkoutPage.GetProductPrice());
            checkoutPage.ClickOnFinish();
        }
    }
}
