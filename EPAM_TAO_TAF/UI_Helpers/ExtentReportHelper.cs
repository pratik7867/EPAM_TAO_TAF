using System;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace EPAM_TAO_TAF.UI_Helpers
{
    public class ExtentReportHelper
    {
        private static readonly object syncLock = new object();
        private static ExtentReportHelper _extentReportHelper = null;

        public ExtentReports extent { get; set; }
        public ExtentHtmlReporter reporter { get; set; }
        public ExtentTest test { get; set; }

        ExtentReportHelper(string strAUT, IWebDriver driver)
        {
            extent = new ExtentReports();

            reporter = new ExtentHtmlReporter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), DateTime.Now.ToString("dd-MM-yyyy")) + @"\" + "TAF_Report.html");
            reporter.Config.DocumentTitle = "Automation Testing Report";
            reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            extent.AttachReporter(reporter);

            extent.AddSystemInfo("Application Under Test", strAUT);
            extent.AddSystemInfo("Environment", "QA");
            extent.AddSystemInfo("Machine", Environment.MachineName);
            extent.AddSystemInfo("OS", Environment.OSVersion.VersionString);

            if(driver != null)
            {
                ICapabilities browserCap = ((RemoteWebDriver)driver).Capabilities;

                extent.AddSystemInfo("Browser", browserCap.BrowserName);
                extent.AddSystemInfo("Browser Version", browserCap.Version);                
            }
        }        

        public static ExtentReportHelper GetInstance(string strAUT, IWebDriver driver = null)
        {
            lock (syncLock)
            {
                if (_extentReportHelper == null)
                {
                    _extentReportHelper = new ExtentReportHelper(strAUT, driver);
                }
                return _extentReportHelper;
            }
        }

        public void CreateTest(string testName)
        {
            test = extent.CreateTest(testName);
        }
        public void SetStepStatusPass(string stepDescription)
        {
            test.Log(Status.Pass, stepDescription);
        }
        public void SetStepStatusWarning(string stepDescription)
        {
            test.Log(Status.Warning, stepDescription);
        }
        public void SetTestStatusPass()
        {
            test.Pass("Test Executed Sucessfully!");
        }
        public void SetTestStatusFail(string message, string strPathToSSFile = null)
        {
            var printMessage = "<p><b>Test FAILED!</b></p>" + $"Message: <br>{message}<br>";
            test.Fail(printMessage);

            if(!string.IsNullOrEmpty(strPathToSSFile))
            {
                test.AddScreenCaptureFromPath(strPathToSSFile);
            }            
        }        
        public void SetTestStatusSkipped()
        {
            test.Skip("Test skipped!");
        }
        public void CloseExtentReport()
        {
            extent.Flush();         
        }
    }
}
