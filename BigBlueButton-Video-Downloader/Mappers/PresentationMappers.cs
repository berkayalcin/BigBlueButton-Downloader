using System;
using System.Collections.Generic;
using System.Linq;
using BigBlueButton_Video_Downloader.Models;
using OpenQA.Selenium;

namespace BigBlueButton_Video_Downloader.Mappers
{
    public static class PresentationMappers
    {
        public static IEnumerable<PresentationItem> ConvertToPresentationItems(this IWebElement element)
        {
            var imageItems = element.FindElements(By.TagName("image")).ToList();

            foreach (var imageElement in imageItems)
            {
                yield return new PresentationItem()
                {
                    In = Convert.ToDouble(imageElement.GetAttribute("in")),
                    Out = Convert.ToDouble(imageElement.GetAttribute("out")),
                    Source = imageElement.GetAttribute("xlink:href"),
                    Text = imageElement.GetAttribute("text")
                };
            }
        }
    }
}