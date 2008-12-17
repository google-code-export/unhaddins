﻿using System;
using System.Windows.Forms;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using SessionManagement.Infrastructure.InversionOfControl;
using SessionManagement.GUI.Views;

namespace SessionManagement.GUI
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			ConfigureIoC();
			Application.Run(new MainForm());

		}

		private static void ConfigureIoC()
		{
			IWindsorContainer container = new WindsorContainer(new XmlInterpreter("castle.config"));
			IoC.RegisterResolver(new WindsorDependencyResolver(container));
			IoC.RegisterImplementationOf("CreateProduct", typeof(AddProductView), LifeStyle.Transient);
		}
	}
}
