using System;
using System.Collections.Concurrent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BigBlueButton_Video_Downloader.Webdriver
{
    public static class WebDriverFactory
    {
        private static readonly ConcurrentDictionary<WebDriverType, IWebDriver> WebDrivers =
            new ConcurrentDictionary<WebDriverType, IWebDriver>();

        static WebDriverFactory()
        {
            InitializeChromeDriver();
        }

        private static void InitializeChromeDriver()
        {
            var chromeOptions = new ChromeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                LeaveBrowserRunning = true,
            };
            chromeOptions.AddArgument("headless");
            chromeOptions.AddArgument("mute-audio");
            chromeOptions.AddArgument("--log-level=3");
            chromeOptions.AddArgument("--disable-logging");
            chromeOptions.AddExcludedArgument("enable-logging");

            var chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, chromeOptions);
            WebDrivers.TryAdd(WebDriverType.Chrome, chromeDriver);
        }

        public static IWebDriver Create(WebDriverType type) => WebDrivers[type];
    }
}