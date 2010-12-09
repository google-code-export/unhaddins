using System.Collections.Generic;

namespace ChinookMediaManager.Domain.AppServicesInterfaces
{

	public interface IAlbumManager
	{
		/// <summary>
		/// Return a list of albums to browse.
		/// </summary>
		/// <returns></returns>
		IEnumerable<Album> GetAlbumsToBrowse();
       
		/// <summary>
		/// Return an album for a given id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Album GetAlbumById(int id);

		/// <summary>
		/// Persist an album.
		/// </summary>
		/// <param name="album"></param>
		void Save(Album album);

		/// <summary>
		/// Revert changes of a given album to his current persisted state.
		/// </summary>
		/// <param name="album"></param>
		void Cancel(Album album);

		/// <summary>
		/// Return true if the album is valid.
		/// </summary>
		/// <param name="album"></param>
		/// <returns></returns>
		bool IsValid(Album album);
	}
}