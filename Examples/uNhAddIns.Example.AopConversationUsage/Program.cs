using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using uNhAddIns.Example.AopConversationUsage.Entities;
using uNhAddIns.Example.AopConversationUsage.BusinessLogic;
using Iesi.Collections.Generic;

namespace uNhAddIns.Example.AopConversationUsage
{
	class Program
	{
		private static void Main(string[] args)
		{
			AppConfigure();
			CreateNewCrocodileFamilyInOneForm();

			WorkWithHumanFamilyInTwoForms();

			Console.ReadLine();
		}

		private static void AppConfigure()
		{
			ServiceLocatorProvider.Initialize();
		}

		private static void CreateNewCrocodileFamilyInOneForm()
		{
			// Instatiation of the model: you can use it from the Form or better from presenter (MVP)
			var familyCrudModel = ServiceLocator.Current.GetInstance<IFamilyCrudModel<Reptile>>();

			// work outside the model
			Family<Reptile> rfamily = CreateReptileFamily();

			familyCrudModel.Save(rfamily); // The user press Apply
			// At this point we have a persistent Family<Reptile> instance but...

			familyCrudModel.CancelAll(); // The user press Cancel
			// At this point all our instance are in a inconsistent state
			// CancelAll is the abort of conversation but...
			// we can continue working in a clear conversation
			// a possible "Refresh" of all existing Family<Reptile>
			IList<Family<Reptile>> l = familyCrudModel.GetEntirelyList();
			Console.WriteLine("Saved Family<Reptile> after Abort; count:" + l.Count);

			// work outside the model
			rfamily = CreateReptileFamily();

			familyCrudModel.Save(rfamily); // At this point we have a persistent Family<Reptile> instance and...
			familyCrudModel.AcceptAll(); // This time the user press the Ok button
			// AcceptAll is the End of conversation but...
			// we can continue working in a clear conversation
			// a possible "Refresh" of all existing Family<Reptile>
			l = familyCrudModel.GetEntirelyList();
			Console.WriteLine("Saved Family<Reptile> after Accept; count:" + l.Count);

			ShowTheReptileFamily(l[0]); // we can access to the lazy relationship/collections

			familyCrudModel.CancelAll(); // This is like the close of the Form
		}

		private static Family<Reptile> CreateReptileFamily()
		{
			var rf = new Reptile { Description = "Crocodile Father" };
			var rm = new Reptile { Description = "Crocodile Mother" };
			var rc1 = new Reptile { Description = "Crocodile Child1" };
			var rc2 = new Reptile { Description = "Crocodile Child2" };
			return new Family<Reptile> { Father = rf, Mother = rm, Childs = new HashedSet<Reptile> { rc1, rc2 } };
		}

		private static void ShowTheReptileFamily(Family<Reptile> family)
		{
			Console.WriteLine();
			Console.WriteLine("The Reptile family is :");
			Console.WriteLine("Father=> " + family.Father.Description);
			Console.WriteLine("Mother=> " + family.Mother.Description);
			foreach (Reptile reptile in family.Childs)
			{
				Console.WriteLine("Child=> " + reptile.Description);
			}
			Console.WriteLine("-----------------------");
		}


		private static void WorkWithHumanFamilyInTwoForms()
		{
			var familyCrudModel1 = ServiceLocator.Current.GetInstance<IFamilyCrudModel<Human>>();
			var familyCrudModel2 = ServiceLocator.Current.GetInstance<IFamilyCrudModel<Human>>();

			// work for form 1
			Family<Human> hfamily1 = CreateHumanFamily("Flinstone");
			familyCrudModel1.Save(hfamily1);

			// work for form 2
			Family<Human> hfamily2 = CreateHumanFamily("Stoneflin");
			familyCrudModel2.Save(hfamily2);

			// work for form 1
			familyCrudModel1.AcceptAll();
			IList<Family<Human>> l = familyCrudModel1.GetEntirelyList();
			Console.WriteLine("Saved Family<Human> after Accept in Form1; count:" + l.Count);
			ShowTheHumanFamily(l[0]);

			// work in form 2
			familyCrudModel2.AcceptAll();

			// Refresh the list of saved families from Form1
			l = familyCrudModel1.GetEntirelyList();
			Console.WriteLine("Saved Family<Human> after Accept in Form2; count:" + l.Count);
			foreach (var family in l)
			{
				ShowTheHumanFamily(family);
			}
		}

		private static Family<Human> CreateHumanFamily(string surname)
		{
			var hf = new Human { Description = surname, Name = "Fred" };
			var hm = new Human { Description = surname, Name = "Wilma" };
			var hc1 = new Human { Description = surname, Name = "Pebbles" };
			return new Family<Human> { Father = hf, Mother = hm, Childs = new HashedSet<Human> { hc1 } };
		}

		private static void ShowTheHumanFamily(Family<Human> family)
		{
			Console.WriteLine();
			Console.WriteLine("The Reptile family is :");
			Console.WriteLine("Father=> " + family.Father.Description + " " + family.Father.Name);
			Console.WriteLine("Mother=> " + family.Mother.Description + " " + family.Mother.Name);
			foreach (Human human in family.Childs)
			{
				Console.WriteLine("Child=> " + human.Description + " " + human.Name);
			}
			Console.WriteLine("-----------------------");
		}

	}
}
