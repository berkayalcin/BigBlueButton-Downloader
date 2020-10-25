using System.Collections.Generic;
using System.Threading.Tasks;
using BigBlueButton_Video_Downloader.Models;

namespace BigBlueButton_Video_Downloader.Media
{
    public interface IPresentationService
    {
        Task CreatePresentation(List<PresentationItem> presentationItems,
            string directory,
            string outputFileName,
            string audioPath = null,
            bool useMultiThread = false);
    }
}