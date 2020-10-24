using System;
using BigBlueButton_Video_Downloader.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BigBlueButton_Video_Downloader.Webdriver
{
    public static class WebDriverExtensions
    {
        public static IWebElement ExactlyFindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds <= 0) return driver.FindElement(by);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                return wait.Until(drv =>
                {
                    var item = drv.FindElement(by);
                    if (item != null && item.Displayed)
                        return item;
                    return null;
                });
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static IWebElement GetByCssSelector(this IWebDriver webDriver, string selector, int timeOutInSeconds)
        {
            return webDriver.ExactlyFindElement(By.CssSelector(selector), timeOutInSeconds);
        }
    }
}