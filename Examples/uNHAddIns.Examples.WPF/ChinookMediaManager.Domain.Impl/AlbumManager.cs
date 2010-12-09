using System;
using System.Collections.Generic;
using System.Linq;
using ChinookMediaManager.Data;
using ChinookMediaManager.Domain.AppServicesInterfaces;
using uNhAddIns.Adapters;

namespace ChinookMediaManager.Domain.Impl
{
	[PersistenceConversational]
	public class AlbumManager : IAlbumManager
	{
		private readonly IDao<Album> daoAlbum;
		private readonly IEntityValidator entityValidator;

		public AlbumManager(IDao<Album> daoAlbum, IEntityValidator entityValidator)
		{
			this.daoAlbum = daoAlbum;
			this.entityValidator = entityValidator;
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public IEnumerable<Album> GetAlbumsToBrowse()
		{
			return daoAlbum.RetrieveAll().OrderBy(a => a.Title).ToArray();
		}

		//The default [PersistenceConversation(ConversationEndMode = EndMode.Continue)]
		public Album GetAlbumById(int id)
		{
			return daoAlbum.Get(id);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.End)]
		public void Save(Album album)
		{
			if(!IsValid(album))
			{
				throw new InvalidOperationException("The album is invalid. You should verify is valid with IsValid before calling save.");	
			}	
			daoAlbum.MakePersistent(album);
		}

		[PersistenceConversation(ConversationEndMode = EndMode.Abort)]
		public void Cancel(Album album)
		{}

		[PersistenceConversation(Exclude = true)]
		public bool IsValid(Album album)
		{
			return entityValidator.IsValid(album);
		}
	}
}