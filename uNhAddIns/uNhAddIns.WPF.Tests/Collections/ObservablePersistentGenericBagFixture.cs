using System.Collections.Specialized;
using System.ComponentModel;
using NHibernate;
using NUnit.Framework;
using uNhAddIns.WPF.Tests.SampleDomain;

namespace uNhAddIns.WPF.Tests.Collections
{
    [TestFixture]
    public class ObservablePersistentGenericBagFixture : IntegrationBaseTest
    {
        private int CreateNewInvoice()
        {
            int id;
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = new Invoice();
                invoice.AddLine(new InvoiceLine());
                id = (int) session.Save(invoice);
                tx.Commit();
            }
            return id;
        }

        [Test]
        public void add_item_should_raise_collectionchanged_properly()
        {
            int id = CreateNewInvoice();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);
                var newLine = new InvoiceLine();

                ((INotifyCollectionChanged) invoice.Lines).CollectionChanged += (sender, args) =>
                                                                                    {
                                                                                        sender.Should().Be.EqualTo(
                                                                                            invoice.Lines);
                                                                                        args.Action.Should().Be.EqualTo(
                                                                                            NotifyCollectionChangedAction
                                                                                                .Add);
                                                                                        args.NewItems.Count.Should().Be.
                                                                                            EqualTo(1);
                                                                                        args.NewItems[0].Should().Be.
                                                                                            EqualTo(newLine);
                                                                                        eventWasRaised = true;
                                                                                    };

                invoice.AddLine(newLine);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void add_item_should_raise_propertychanged_for_count()
        {
            int id = CreateNewInvoice();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);
                var newLine = new InvoiceLine();

                ((INotifyPropertyChanged) invoice.Lines).PropertyChanged += (sender, args) =>
                                                                                {
                                                                                    if (args.PropertyName == "Count")
                                                                                        eventWasRaised = true;
                                                                                };

                invoice.AddLine(newLine);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void add_item_should_raise_propertychanged_for_item()
        {
            int id = CreateNewInvoice();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);
                var newLine = new InvoiceLine();

                ((INotifyPropertyChanged) invoice.Lines).PropertyChanged += (sender, args) =>
                                                                                {
                                                                                    if (args.PropertyName == "Item[]")
                                                                                        eventWasRaised = true;
                                                                                };

                invoice.AddLine(newLine);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

        [Test]
        public void collection_should_implement_inotifycollectionchanged()
        {
            int id = CreateNewInvoice();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);
                typeof (INotifyCollectionChanged)
                    .Should().Be.AssignableFrom(invoice.Lines.GetType());
                tx.Commit();
            }
        }

        [Test]
        public void collection_should_implement_inotifypropertychanged()
        {
            int id = CreateNewInvoice();
            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);

                typeof (INotifyPropertyChanged)
                    .Should().Be.AssignableFrom(invoice.Lines.GetType());
                tx.Commit();
            }
        }

        [Test]
        public void remove_item_should_raise_collectionchanged_properly()
        {
            int id = CreateNewInvoice();
            bool eventWasRaised = false;

            using (ISession session = sessions.OpenSession())
            using (ITransaction tx = session.BeginTransaction())
            {
                var invoice = session.Get<Invoice>(id);
                InvoiceLine oldLine = invoice.Lines[0];

                ((INotifyCollectionChanged) invoice.Lines).CollectionChanged += (sender, args) =>
                                                                                    {
                                                                                        sender.Should().Be.EqualTo(
                                                                                            invoice.Lines);
                                                                                        args.Action.Should().Be.EqualTo(
                                                                                            NotifyCollectionChangedAction
                                                                                                .Remove);
                                                                                        args.OldItems.Count.Should().Be.
                                                                                            EqualTo(1);
                                                                                        args.OldItems[0].Should().Be.
                                                                                            EqualTo(oldLine);
                                                                                        eventWasRaised = true;
                                                                                    };

                invoice.RemoveLine(oldLine);
                eventWasRaised.Should().Be.True();
                tx.Commit();
            }
        }

		[Test]
		public void can_merge_collection()
		{

			int id = CreateNewInvoice();
			Invoice invoice;

			using (ISession session = sessions.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					invoice = session.Get<Invoice>(id);
					invoice.Lines.Count.Should().Be.EqualTo(1);
					tx.Commit();
				}
			}

			//detached  object - add new line
			invoice.AddLine(new InvoiceLine());


			using (ISession session = sessions.OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{

					var temp_invoice = session.Load<Invoice>(id);

					session.Contains(invoice).Should().Be.False();

					session.Merge(invoice);

					temp_invoice.Lines.Count.Should().Be.EqualTo(invoice.Lines.Count);
					
					tx.Commit();
				}
			}

		}

    }
}