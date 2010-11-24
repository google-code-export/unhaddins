using NHibernate;
using NUnit.Framework;
using YourPrjDomain.Inheritance;

namespace uNHAddins.Examples.Course.Tests.Inheritance
{

	public class InheritanceTestCase : TestCase
	{
		[SetUp]
		public void Init()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction transaction = session.BeginTransaction())
				{
					// Create a frog
					Frog frog = new Frog("Bernard", 15);
					session.Save(frog);

					// Create a dog
					Dog dog = new Dog("Beethoven", "San Bernardo");
					session.Save(dog);

					transaction.Commit();
				}
			}
		}

		[Test]
		public void GetAllAnimals()
		{
			using (ISession s = OpenSession())
			{
				s.CreateCriteria(typeof(Animal)).List();
			}
		}

		[Test]
		public void GetAllDogs()
		{
			using (ISession s = OpenSession())
			{
				s.CreateCriteria(typeof(Dog)).List();
			}
		}

		[Test]
		public void GetAllFrogs()
		{
			using (ISession s = OpenSession())
			{
				s.CreateCriteria(typeof(Frog)).List();
			}
		}

		protected override void OnTearDown()
		{
			using (ISession s = OpenSession())
			{
				s.Delete("from System.Object o");
				s.Flush();
			}
		}
	}
}