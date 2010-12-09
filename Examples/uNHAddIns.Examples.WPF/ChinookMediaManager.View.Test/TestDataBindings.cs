using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Caliburn.Testability;
using NUnit.Framework;

namespace ChinookMediaManager.View.Test
{
	public class DataBindingValidator
	{
		private static BindingValidator ValidatorFor(Type guiElement, Type presenterType)
		{
			var boundType = new BoundType(presenterType);
			object instance = Activator.CreateInstance(guiElement);
			return new BindingValidator(Bound.DependencyObject((DependencyObject)instance, boundType));
		}

		/// <summary>
		/// Validate the bindings of a keyvalue pair 
		/// where the key is the View type and the value is the ViewModel type.
		/// </summary>
		/// <param name="viewViewModelDictionary">IDictionary of View type / ViewModel type</param>
		/// <returns>Enumerable of validations results</returns>
		public IEnumerable<ValidationResult> Validate(IDictionary<Type,Type> viewViewModelDictionary)
		{
			foreach (var viewViewModel in viewViewModelDictionary)
			{
				BindingValidator validator = ValidatorFor(viewViewModel.Key, viewViewModel.Value);
				ValidationResult validatorResult = validator.Validate();
				yield return validatorResult;
			}
		}
	}

	[TestFixture]
	public class TestDataBindings
	{
		private static Type GetViewForViewModel(Type viewModelType)
		{
			string viewName = viewModelType.Name.Replace("ViewModel", "View");
			string viewFullName = string.Format("ChinookMediaManager.GUI.Views.{0}, ChinookMediaManager.GUI", viewName);
			Type viewType = Type.GetType(viewFullName, true);
			return viewType;
		}

		[Test]
		public void AllDatabindingsAreOkay()
		{
			bool fail = false;
			var databindingValidator = new DataBindingValidator();

			Type examplePresenterType = typeof(AlbumManagerViewModel);

			var dictionary = examplePresenterType.Assembly.GetTypes()
												.Where(type => type.Namespace.EndsWith("ViewModels"))
												.ToDictionary(vmType => GetViewForViewModel(vmType), vmType => vmType);

			

			foreach (var validationResult in databindingValidator.Validate(dictionary))
			{
				if(validationResult.HasErrors)
				{
					Console.WriteLine(validationResult.ErrorSummary);
					fail = true;
				}
			}

			fail.Should().Be.False();
		}
	}
}