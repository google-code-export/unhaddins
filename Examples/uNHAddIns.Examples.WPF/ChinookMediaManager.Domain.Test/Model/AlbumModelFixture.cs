using System;
using System.Linq;
using ChinookMediaManager.Domain.Impl;
using ChinookMediaManager.Domain.Test.Util;
using Moq;
using NUnit.Framework;
using SharpTestsEx;
using uNhAddIns.Adapters;

namespace ChinookMediaManager.Domain.Test.Model
{
	[TestFixture]
	public class AlbumModelFixture
	{
		[Test]		
		public void CanGetAlbumById()
		{
			var album = new Album().SetId(123);
			var daoAlbum = new DaoStub<Album>().AddEntity(album);
			var model = new AlbumManager(daoAlbum, new Mock<IEntityValidator>().Object);
			model.GetAlbumById(123).Should().Be.EqualTo(album);
		}
		
		[Test]
		public void WhenEntityIsInvalidThenThrowExceptionAndDontSave()
		{
			//Arrange
			var album = new Album();
			var daoAlbum = new DaoStub<Album>().AddEntity(album);
			var validator = new Mock<IEntityValidator>();
			validator.Setup(v => v.IsValid(album)).Returns(false);
			var model = new AlbumManager(daoAlbum, validator.Object);

			//Act
			model.Executing(am => am.Save(album))
				.Throws<InvalidOperationException>(); //assert

			//assert
			daoAlbum.Mock.Verify(da => da.MakePersistent(album), Times.Never());
		}

		[Test]
		public void GetAlbumsShouldReturnOrderedAlbums()
		{
			//Arrange
			var daoAlbum = new DaoStub<Album>().AddEntities(new Album{Title = "za"}, new Album{Title = "aa"});

			var model = new AlbumManager(daoAlbum, new Mock<IEntityValidator>().Object);

			//Act
			var result = model.GetAlbumsToBrowse();

			//assert
			result.Should().Have.SameSequenceAs(daoAlbum.RetrieveAll().OrderBy(a => a.Title).ToArray());
		}


		[Test]
		public void WhenEntityIsNotValidThenIsValidReturnsFalse()
		{
			var album = new Album();
			var entityValidator = new Mock<IEntityValidator>();
			var model = new AlbumManager(new DaoStub<Album>(), entityValidator.Object);
			entityValidator.Setup(ev => ev.IsValid(album)).Returns(false);
			model.IsValid(album).Should().Be.False();
		}

		[Test]
		public void WhenEntityIsValidThenIsValidReturnsTrue()
		{
			var album = new Album();
			var entityValidator = new Mock<IEntityValidator>();
			var model = new AlbumManager(new DaoStub<Album>(), entityValidator.Object);
			entityValidator.Setup(ev => ev.IsValid(album)).Returns(true);
			model.IsValid(album).Should().Be.True();
		}
	}
}