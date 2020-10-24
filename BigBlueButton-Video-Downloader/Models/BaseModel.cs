namespace BigBlueButton_Video_Downloader.Models
{
    public abstract class BaseModel
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorCode);
    }
}