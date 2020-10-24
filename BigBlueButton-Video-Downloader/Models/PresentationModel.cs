using System.Collections.Generic;

namespace BigBlueButton_Video_Downloader.Models
{
    public class PresentationModel : BaseModel
    {
        public IEnumerable<PresentationItem> Items { get; set; }
    }
}