using System;
using System.Collections.Concurrent;
using BigBlueButton_Video_Downloader.Enums;

namespace BigBlueButton_Video_Downloader.Specifications
{
    public static class VideoFileExtensionSpecificationFactory
    {
        private static readonly ConcurrentDictionary<VideoType, Type>
            VideoFileExtensionSpecifications = new ConcurrentDictionary<VideoType, Type>();

        static VideoFileExtensionSpecificationFactory()
        {
            VideoFileExtensionSpecifications.TryAdd(VideoType.Mp4, typeof(Mp4VideoFileExtensionSpecification));
            VideoFileExtensionSpecifications.TryAdd(VideoType.WebM, typeof(WebmVideoFileExtensionSpecification));
        }

        public static IVideoFileExtensionSpecification Create(VideoType type)
        {
            return Activator.CreateInstance(VideoFileExtensionSpecifications[type]) as IVideoFileExtensionSpecification;
        }
    }
}