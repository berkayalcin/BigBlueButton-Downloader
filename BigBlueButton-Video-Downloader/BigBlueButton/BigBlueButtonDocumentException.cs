using System;

namespace BigBlueButton_Video_Downloader.BigBlueButton
{
    public class BigBlueButtonDocumentException : Exception
    {
        public BigBlueButtonDocumentException(string message) : base(message)
        {
        }

        public BigBlueButtonDocumentException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}