using Noxum.Publishing.ImageConverter.Analyzer;
using Noxum.Publishing.ImageConverter.ImageTypeProperties;
using Noxum.Publishing.ImageConverter.Profiles;

namespace Noxum.Publishing.ImageConverter
{
    /// <summary>
    /// Container facade class for an images
    /// </summary>
    internal class ImageContainer
    {
        private ImageInfoDecorator imageInfoDecorator;
        private readonly ImageAnalyzerDecorator imageAnalyzer;
        private readonly ImageTypeConfiguration imageTypeConfiguration;
        private readonly ProfileManager profileManager;

        private ImageContainer() { }

        /// <summary>
        /// 
        /// </summary>
        internal string FilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal ImageInfoDecorator ImageData
        {
            get
            {
                if (imageInfoDecorator == null || imageInfoDecorator is NullImageInfoDecorator)
                {
                    imageInfoDecorator = imageAnalyzer.GetImageInfo(FilePath);
                }
                return imageInfoDecorator;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ImageType NormalizedImageType { get { return imageTypeConfiguration.GetImageTypeInfo(ImageData.ImageType).ImageType; } }

        /// <summary>
        /// 
        /// </summary>
        internal virtual bool FormatHasResolution { get { return imageTypeConfiguration.GetImageTypeInfo(ImageData.ImageType).HasResolution; } }

        /// <summary>
        /// 
        /// </summary>
        internal virtual bool FormatIsTransparent { get { return imageTypeConfiguration.GetImageTypeInfo(ImageData.ImageType).IsTransparent; } }

        /// <summary>
        /// Gets a value indicating whether [format is vector graphic].
        /// </summary>
        /// <value>
        /// <c>true</c> if [format is vector graphic]; otherwise, <c>false</c>.
        /// </value>
        internal virtual bool FormatIsVectorGraphic { get { return imageTypeConfiguration.GetImageTypeInfo(ImageData.ImageType).IsVector; } }

        /// <summary>
        /// 
        /// </summary>
        internal virtual bool FormatHasPages { get { return imageTypeConfiguration.GetImageTypeInfo(ImageData.ImageType).HasPages; } }

        /// <summary>
        /// Returns path to the extracted iccprofile
        /// </summary>
        internal string GetPathToProfile()
        {
            return profileManager.GetPathToProfile(this);
        }

        /// <summary>
        /// Returns path to fallback-profile for a certain colormodel
        /// </summary>
        internal string GetFallbackProfile(ColorModels colorModel)
        {
            return profileManager.GetFallbackProfile(colorModel);
        }

        /// <summary>
        /// Container facade class for an image
        /// </summary>
        /// <param name="filePath">path to image</param>
        /// <param name="imageAnalyzer">image analyzer</param>
        /// <param name="imageTypeConfiguration">Configuration for imape type specific properties</param>
        /// <param name="profileManager">profile manager</param>
        internal ImageContainer(string filePath, ImageAnalyzerDecorator imageAnalyzer, ImageTypeConfiguration imageTypeConfiguration, ProfileManager profileManager)
        {
            this.profileManager = profileManager;
            this.imageTypeConfiguration = imageTypeConfiguration;
            this.imageAnalyzer = imageAnalyzer;
            this.FilePath = filePath;
        }
    }
}
