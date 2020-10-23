using System;
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
            return wait.Until(drv =>
            {
                var item = drv.FindElement(by);
                if (item != null && item.Displayed)
                    return item;
                return null;
            });

        }
    }
}