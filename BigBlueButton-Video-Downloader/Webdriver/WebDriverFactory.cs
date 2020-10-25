using System;
using System.Collections.Concurrent;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace BigBlueButton_Video_Downloader.Webdriver
{
    public static class WebDriverFactory
    {
        private static readonly ConcurrentDictionary<WebDriverType, Func<IWebDriver>> WebDrivers =
            new ConcurrentDictionary<WebDriverType, Func<IWebDriver>>();

        static WebDriverFactory()
        {
            WebDrivers.TryAdd(WebDriverType.Chrome, InitializeChromeDriver);
        }

        private static IWebDriver InitializeChromeDriver()
        {
            var chromeOptions = new ChromeOptions()
            {
                PageLoadStrategy = PageLoadStrategy.Normal,
                LeaveBrowserRunning = false,
            };
            var userAgent =
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.50 Safari/537.36";

            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--mute-audio");
            chromeOptions.AddArgument("--window-size=1920,1080");
            chromeOptions.AddArgument("--disable-extensions");
            chromeOptions.AddArgument("--proxy-server='direct://'");
            chromeOptions.AddArgument("--proxy-bypass-list=*");
            chromeOptions.AddArgument("--start-maximized");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArgument("--allow-running-insecure-content");
            chromeOptions.AddArgument($"--user-agent={userAgent}");

            var chromeDriver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory, chromeOptions);
            chromeDriver.Manage().Window.Maximize();
            return chromeDriver;
        }

        public static IWebDriver Create(WebDriverType type)
        {
            return WebDrivers[type].Invoke();
        }

        public static IWebDriver Create(WebDriverType type, string url)
        {
            var driver = Create(type);
            driver.Url = url;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            return driver;
        }
    }
}