using System.Collections.Generic;
using Caliburn.PresentationFramework.Screens;
using ChinookMediaManager.Domain;
using ChinookMediaManager.Domain.AppServicesInterfaces;

namespace ChinookMediaManager.GUI.Albums.Browse
{
	public class AlbumsBrowseViewModel : Screen<IEnumerable<Album>>
	{
		private readonly IAlbumManager albumManager;

		public AlbumsBrowseViewModel(IAlbumManager albumManager)
		{
			this.albumManager = albumManager;
			DisplayName = "Albums";
		}

		public override IEnumerable<Album> Subject
		{
			get
			{
				return albumManager.GetAlbumsToBrowse();
			}
		}


		

	}
}