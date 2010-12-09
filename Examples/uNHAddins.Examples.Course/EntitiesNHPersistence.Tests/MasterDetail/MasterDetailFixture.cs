using System.Collections;
using NHibernate;
using NUnit.Framework;
using YourPrjDomain.MasterDetail;
using System.Collections.Generic;

namespace uNHAddins.Examples.Course.Tests.MasterDetail
{
	[TestFixture]
	public class MasterDetailFixture : TestCase
	{
		protected override System.Collections.IList Mappings
		{
			get { return new string[] { "MasterDetail.MasterDetail.hbm.xml" }; }
		}

		object savedId;

		[Test]
		public void Create()
		{
			CreateMaster();
			log.Debug("------ Master added ------");

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Master m;
				using (new TemporaryOffLog("NHibernate.SQL"))
					m = s.Get<Master>(savedId);

				m.Name = "NewMasterName";

				IList<Detail> dl = new List<Detail>(m.Details);
				bool deleted = false;
				foreach (Detail detail in dl)
				{
					if (!deleted)
					{
						m.RemoveDetail(detail);
						deleted = true;
					}
					else
						detail.Description = "NewDesc";
				}

				s.Save(m);
				t.Commit();
			}
			log.Debug("------ Masters Update ------");

			CleanUp();
			log.Debug("------ Masters Deleted ------");
		}

		private void CreateMaster()
		{
			Master m = new Master();
			m.Name = "Pepe";
			for (int i = 1; i <= 3; i++)
			{
				Detail d = new Detail("d" + i, m);
				m.AddDetail(d);
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Save(m);
				savedId = m.Id;
				t.Commit();
			}
		}

		public void CleanUp()
		{
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				s.Delete("from System.Object o");
				t.Commit();
			}
		}

		[Test]
		public void FechingDetails()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
				CreateMaster();

			using (ISession s = OpenSession())
			{
				Master m;
				using (new TemporaryOffLog("NHibernate.SQL"))
					m = s.Get<Master>(savedId);
				s.Evict(m);

				IList<Detail> dl = s.CreateQuery("from Detail d where d.Master = :pMaster")
					.SetParameter("pMaster", m)
					.List<Detail>();

				log.Debug("------ Having Details list ------");
				foreach (Detail detail in dl)
				{
					log.DebugFormat("Detail-{0} from Master-{1}", detail.Description, detail.Master.Name);
				}
			}

			using (new TemporaryOffLog("NHibernate.SQL"))
				CleanUp();
		}

		[Test]
		public void CheckCollectionConcreteType()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
				CreateMaster();

			using (ISession s = OpenSession())
			{
				Master mg = s.Get<Master>(savedId);
				log.DebugFormat("The collection type is:{0}", mg.Details.GetType());
			}

			using (new TemporaryOffLog("NHibernate.SQL"))
				CleanUp();
		}

		[Test]
		public void DeferencingDetail()
		{
			using (new TemporaryOffLog("NHibernate.SQL"))
				CreateMaster();

			IList<Detail> dl;
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				Master m = s.Get<Master>(savedId);

				dl = new List<Detail>(m.Details);
				foreach (Detail detail in dl)
				{
					m.RemoveDetail(detail);
				}

				s.Save(m);
				t.Commit();
			}

			log.Debug("--- We still having details ---");
			foreach (Detail detail in dl)
			{
				log.DebugFormat("Detail-{0} from Master-{1}", detail.Description, detail.Master.Name);
			}

			using (new TemporaryOffLog("NHibernate.SQL"))
				CleanUp();

		}
	}
}
