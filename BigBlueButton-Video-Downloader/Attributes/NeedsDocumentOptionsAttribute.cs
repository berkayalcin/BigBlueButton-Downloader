using System;

namespace BigBlueButton_Video_Downloader.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NeedsDocumentOptionsAttribute : Attribute
    {
    }
}