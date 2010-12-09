using System.Collections.Specialized;
using System.Linq;
using NUnit.Framework;
using uNhAddIns.WPF.Collections;

namespace uNhAddIns.WPF.Tests.Collections
{
    [TestFixture]
    public class ObservableSetFixture
    {
        private const string COUNT_PROPERTY_NAME = "Count";
        private const string ISEMPTY_PROPERTY_NAME = "IsEmpty";


        [Test]
        public void add_object_should_raise_collectionchanged()
        {
            int value = 1;
            var observableSet = new ObservableSet<int>();
            bool eventWasRaised = false;

            observableSet.CollectionChanged += (sender, args) =>
                                                   {
                                                       sender.Should().Be.EqualTo(observableSet);
                                                       args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Add);
                                                       args.NewItems[0].Should().Be.EqualTo(value);
                                                       eventWasRaised = true;
                                                   };
            observableSet.Add(value);
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void add_object_should_raise_propertychanged()
        {
            int value = 1;
            var observableSet = new ObservableSet<int>();
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;
            observableSet.PropertyChanged += (sender, args) =>
                                                 {
                                                     sender.Should().Be.EqualTo(observableSet);
                                                     if (args.PropertyName == COUNT_PROPERTY_NAME)
                                                         eventWasRaisedForCount = true;
                                                     if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                                                         eventWasRaisedForIsEmpty = true;
                                                 };

            observableSet.Add(value);
            eventWasRaisedForCount.Should().Be.True();
            eventWasRaisedForIsEmpty.Should().Be.True();
        }

        [Test]
        public void add_object_shouldnot_raise_collectionchanged_when_item_exists()
        {
            int value = 1;
            var observableSet = new ObservableSet<int>();
            bool eventWasRaised = false;
            observableSet.Add(value);
            observableSet.CollectionChanged += (sender, args) => { eventWasRaised = true; };
            observableSet.Add(value);
            eventWasRaised.Should().Be.False();
        }


        [Test]
        public void addall_should_raise_collectionchanged_for_addeditems()
        {
            var valuesToAdd = new[] {1, 2, 3};
            const int valueAlreadyAdded = 1;
            bool eventWasRaised = false;

            var observableSet = new ObservableSet<int> {valueAlreadyAdded};

            observableSet.CollectionChanged += (sender, args) =>
                                                   {
                                                       sender.Should().Be.EqualTo(observableSet);
                                                       args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Add);

                                                       args.NewItems.OfType<int>().Should().Have.SameSequenceAs(new[]
                                                                                                                    {
                                                                                                                        2,
                                                                                                                        3
                                                                                                                    });

                                                       args.NewItems.Contains(valueAlreadyAdded).Should().Be.False();

                                                       eventWasRaised = true;
                                                   };

            observableSet.AddAll(valuesToAdd);
            eventWasRaised.Should().Be.True();
        }


        [Test]
        public void addall_should_raise_propertychanged()
        {
            var valuesToAdd = new[] {1, 2, 3};
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;

            var observableSet = new ObservableSet<int>();

            observableSet.PropertyChanged += (sender, args) =>
                                                 {
                                                     sender.Should().Be.EqualTo(observableSet);
                                                     if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                                                         eventWasRaisedForIsEmpty = true;
                                                     if (args.PropertyName == COUNT_PROPERTY_NAME)
                                                         eventWasRaisedForCount = true;
                                                 };

            observableSet.AddAll(valuesToAdd);
            eventWasRaisedForIsEmpty.Should().Be.True();
            eventWasRaisedForCount.Should().Be.True();
        }

        [Test]
        public void clear_should_raise_collectionchanged()
        {
            bool eventWasRaised = false;


            var observableSet = new ObservableSet<int> {1, 2, 3};

            observableSet.CollectionChanged += (sender, args) =>
                                                   {
                                                       sender.Should().Be.EqualTo(observableSet);
                                                       args.Action.Should().Be.EqualTo(
                                                           NotifyCollectionChangedAction.Reset);
                                                       eventWasRaised = true;
                                                   };

            observableSet.Clear();
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void clear_should_raise_propertychanged()
        {
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;

            var observableSet = new ObservableSet<int> {1, 2, 3};

            observableSet.PropertyChanged += (sender, args) =>
                                                 {
                                                     sender.Should().Be.EqualTo(observableSet);
                                                     if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                                                         eventWasRaisedForIsEmpty = true;
                                                     if (args.PropertyName == COUNT_PROPERTY_NAME)
                                                         eventWasRaisedForCount = true;
                                                 };

            observableSet.Clear();
            eventWasRaisedForIsEmpty.Should().Be.True();
            eventWasRaisedForCount.Should().Be.True();
        }

        [Test]
        public void remove_should_raise_collectionchanged()
        {
            bool eventWasRaised = false;


            var observableSet = new ObservableSet<int> {1, 2, 3};

            observableSet.CollectionChanged += (sender, args) =>
                                                   {
                                                       sender.Should().Be.EqualTo(observableSet);
                                                       args.Action.Should().Be.EqualTo(
                                                           NotifyCollectionChangedAction.Remove);
                                                       args.OldItems[0].Should().Be.EqualTo(2);
                                                       args.OldStartingIndex.Should().Be.EqualTo(1);
                                                       eventWasRaised = true;
                                                   };

            observableSet.Remove(2);
            eventWasRaised.Should().Be.True();
        }

        [Test]
        public void remove_should_raise_propertychanged()
        {
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;

            var observableSet = new ObservableSet<int> {1, 2, 3};

            observableSet.PropertyChanged += (sender, args) =>
                                                 {
                                                     sender.Should().Be.EqualTo(observableSet);
                                                     if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                                                         eventWasRaisedForIsEmpty = true;
                                                     if (args.PropertyName == COUNT_PROPERTY_NAME)
                                                         eventWasRaisedForCount = true;
                                                 };

            observableSet.Remove(1);
            eventWasRaisedForIsEmpty.Should().Be.True();
            eventWasRaisedForCount.Should().Be.True();
        }


        [Test]
        public void removeall_should_raise_collectionchanged_for_removeditems()
        {
            bool eventWasRaised = false;

            var observableSet = new ObservableSet<int> { 1, 2, 3, 4 };

            observableSet.CollectionChanged += (sender, args) =>
            {
                sender.Should().Be.EqualTo(observableSet);
                args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Remove);

                args.OldItems.OfType<int>().Should().Have.SameSequenceAs(new[] { 2, 3 });

                args.OldItems.Count.Should().Be.EqualTo(2);

                eventWasRaised = true;
            };

            observableSet.RemoveAll(new[]{2 , 3});
            eventWasRaised.Should().Be.True();
        }


        [Test]
        public void removeall_should_raise_propertychanged()
        {
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;

            var observableSet = new ObservableSet<int> { 1, 2, 3, 4 };
            observableSet.PropertyChanged += (sender, args) =>
            {
                sender.Should().Be.EqualTo(observableSet);
                if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                    eventWasRaisedForIsEmpty = true;
                if (args.PropertyName == COUNT_PROPERTY_NAME)
                    eventWasRaisedForCount = true;
            };
            
            observableSet.RemoveAll(new[] { 2, 3 });
            eventWasRaisedForIsEmpty.Should().Be.True();
            eventWasRaisedForCount.Should().Be.True();
        }

        [Test]
        public void retainall_should_raise_collectionchanged_for_removeditems()
        {
            bool eventWasRaised = false;

            var observableSet = new ObservableSet<int> { 1, 2, 3, 4 };

            observableSet.CollectionChanged += (sender, args) =>
            {
                sender.Should().Be.EqualTo(observableSet);
                args.Action.Should().Be.EqualTo(NotifyCollectionChangedAction.Remove);


                args.OldItems.OfType<int>().Should().Have.SameSequenceAs(new[] {1, 4});

                args.OldItems.Count.Should().Be.EqualTo(2);

                eventWasRaised = true;
            };

            observableSet.RetainAll(new[] { 2, 3 });
            eventWasRaised.Should().Be.True();
        }


        [Test]
        public void retainall_should_raise_propertychanged()
        {
            bool eventWasRaisedForCount = false;
            bool eventWasRaisedForIsEmpty = false;

            var observableSet = new ObservableSet<int> { 1, 2, 3, 4 };
            observableSet.PropertyChanged += (sender, args) =>
            {
                sender.Should().Be.EqualTo(observableSet);
                if (args.PropertyName == ISEMPTY_PROPERTY_NAME)
                    eventWasRaisedForIsEmpty = true;
                if (args.PropertyName == COUNT_PROPERTY_NAME)
                    eventWasRaisedForCount = true;
            };

            observableSet.RetainAll(new[] { 2, 3 });
            eventWasRaisedForIsEmpty.Should().Be.True();
            eventWasRaisedForCount.Should().Be.True();
        }

        [Test]
        public void retainall_should_work()
        {
            ObservableSet<int> observableSet = new ObservableSet<int> { 1, 2, 3, 4, 5 };
            observableSet.RetainAll(new[] {2, 3});
            observableSet.Count.Should().Be.EqualTo(2);
            observableSet.Should().Have.SameSequenceAs(new[] {2, 3});
        }

        [Test]
        public void removeall_should_work()
        {
            var observableSet = new ObservableSet<int> { 1, 2, 3, 4, 5 };
            observableSet.RemoveAll(new[] { 2, 3 });
            observableSet.Count.Should().Be.EqualTo(3);
            observableSet.Should().Have.SameSequenceAs(new[] { 1, 4, 5});
        }

        [Test]
        public void remove_should_work()
        {
            var observableSet = new ObservableSet<int> { 1, 2, 3, 4, 5 };
            observableSet.Remove(1);
            observableSet.Count.Should().Be.EqualTo(4);
            observableSet.Should().Have.SameSequenceAs(new[] { 2, 3, 4, 5 }); 
        }

        [Test]
        public void add_should_work()
        {
            var observableSet = new ObservableSet<int>();
            observableSet.Add(1);
            observableSet.Contains(1).Should().Be.True();
        }

        [Test]
        public void addall_should_work()
        {
            var observableSet = new ObservableSet<int>();
            observableSet.AddAll(new[] {1, 2, 3});
            observableSet.Count.Should().Be.EqualTo(3);
            observableSet.Should().Have.SameSequenceAs(new[] { 1, 2, 3}); 
        }

        [Test]
        public void clear_should_work()
        {
            var observableSet = new ObservableSet<int> { 1, 2, 3 };
            observableSet.Clear();
            observableSet.Count.Should().Be.EqualTo(0);
        }

    }
}