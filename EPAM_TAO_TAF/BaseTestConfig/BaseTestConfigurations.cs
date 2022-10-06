using System;
using System.Configuration;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using EPAM_TAO_TAF.TestSetup;
using EPAM_TAO_TAF.UI_Helpers;

namespace EPAM_TAO_TAF.BaseTestConfig
{
    [TestFixture]
    public abstract class BaseTestConfigurations : TestHookup
    {
        public string strBrowser { get { return ConfigurationManager.AppSettings["Browser"].ToString(); } }
        public string strSiteURL { get { return ConfigurationManager.AppSettings["SiteURL"].ToString(); } }
        public string strAUT { get { return ConfigurationManager.AppSettings["AUT"].ToString(); } }
        public string strUserName { get { return ConfigurationManager.AppSettings["UserName"].ToString(); } }
        public string strPassword { get { return ConfigurationManager.AppSettings["Password"].ToString(); } }
        

        [SetUp]
        public void setupDriverSession()
        {
            try
            {
                driver = InitBrowser((BrowserType)Enum.Parse(typeof(BrowserType), strBrowser.ToUpper()));
                CommonUtilities.commonUtilities.NavigateToURL(driver, strSiteURL);
                CommonUtilities.commonUtilities.MaximizeWindow(driver);

                ExtentReportHelper.GetInstance(strAUT, driver).CreateTest(TestContext.CurrentContext.Test.Name);
            }
            catch(Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }
        }

        [TearDown]
        public void closeDriverSession()
        {
            try
            {
                var status = TestContext.CurrentContext.Result.Outcome.Status;
                var stacktrace = TestContext.CurrentContext.Result.StackTrace;
                var errorMessage = "<pre>" + TestContext.CurrentContext.Result.Message + "</pre>";

                switch (status)
                {
                    case TestStatus.Failed:
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{errorMessage}<br>Stack Trace: <br>{stacktrace}<br>", CommonUtilities.commonUtilities.TakeScreenshot(driver, TestContext.CurrentContext.Test.Name));
                        break;
                    case TestStatus.Skipped:
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusSkipped();
                        break;
                    default:
                        ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusPass();
                        break;
                }                
            }
            catch(Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }
            finally
            {
                CloseBrowser();
            }
        }

        [OneTimeTearDown]
        public void CloseAll()
        {
            try
            {
                ExtentReportHelper.GetInstance(strAUT, driver).CloseExtentReport();
            }
            catch(Exception ex)
            {
                ExtentReportHelper.GetInstance(strAUT, driver).SetTestStatusFail($"<br>{ex.Message}<br>Stack Trace: <br>{ex.StackTrace}<br>");
            }
        }
    }
}
