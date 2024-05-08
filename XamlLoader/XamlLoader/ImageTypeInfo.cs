namespace Noxum.Publishing.ImageConverter.ImageTypeProperties
{
	/// <summary>
	/// General information for an image type
	/// </summary>
	public class ImageTypeInfo
	{
		/// <summary>
		/// Indicates if this image type can have pages or multiple frames
		/// </summary>
		public bool HasPages { get; set; }

		/// <summary>
		/// Indicates if this image type can have layers
		/// </summary>
		public bool HasLayers { get; set; }

		/// <summary>
		/// Indicates if this image type can have a Resolution
		/// </summary>
		public bool HasResolution { get; set; }

		/// <summary>
		/// Indicates if this image type can be transparent
		/// </summary>
		public bool IsTransparent { get; set; }

		/// <summary>
		/// Indicates if this image type is a vector graphic
		/// </summary>
		public bool IsVector { get; set; }

		/// <summary>
		/// Indicates if this image type can have clip path
		/// </summary>
		public bool HasClippingPaths { get; set; }

		/// <summary>
		/// Normalized ImageType
		/// </summary>
		public ImageType ImageType { get; set; }

		//This prevent return null, if alias list is empty
		private AliasContainer aliasCOntainer = new AliasContainer();

		/// <summary>
		/// similar image types for this 'normalized' image type
		/// </summary>
		public AliasContainer Alias { get { return aliasCOntainer; } set { aliasCOntainer = value; } }

		/// <summary>
		/// Constructor
		/// </summary>
		public ImageTypeInfo()
		{
		}
	}
}
