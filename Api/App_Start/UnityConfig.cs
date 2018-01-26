using System;
using Unity;
using BS = G2.DB.BusinessServices;

namespace G2.DB.Api
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
		#region Unity Container
		private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
		{
			var container = new UnityContainer();
			RegisterTypes(container);
			return container;
		});

		public static IUnityContainer GetConfiguredContainer()
		{
			return container.Value;
		}
		#endregion

		/// <summary>
		/// Registers the type mappings with the Unity container.
		/// </summary>
		/// <param name="container">The unity container to configure.</param>
		/// <remarks>
		/// There is no need to register concrete types such as controllers or
		/// API controllers (unless you want to change the defaults), as Unity
		/// allows resolving a concrete type even if it was not previously
		/// registered.
		/// </remarks>
		public static void RegisterTypes(IUnityContainer container)
        {
			// NOTE: To load from web.config uncomment the line below.
			// Make sure to add a Unity.Configuration to the using statements.
			// container.LoadConfiguration();

			// TODO: Register your type's mappings here.
			// container.RegisterType<IProductRepository, ProductRepository>();
			RegisterControllerDependencies(container);
			RegisterBusinessServices(container);
		}

		public static void RegisterBusinessServices(IUnityContainer container)
		{
			container.RegisterType<BS.Contracts.IAccountService, BS.Factories.AccountService>();
			container.RegisterType<BS.Contracts.IPartyService, BS.Factories.PartyService>();
			container.RegisterType<BS.Contracts.ISaleService, BS.Factories.SaleService>();
			container.RegisterType<BS.Contracts.IDashboardService, BS.Factories.DashboardService>();
		}

		public static void RegisterControllerDependencies(IUnityContainer container)
		{
			//container.RegisterType<Web.Controllers.ReportController>(new InjectionConstructor(new ResolvedParameter<BS.Contracts.IReportService>(),
			//																	 new ResolvedParameter<BS.Contracts.ISaleService>()));
			//container.RegisterType<Web.Controllers.BuyerController>(new InjectionConstructor(new ResolvedParameter<BS.Contracts.IBuyerService>()));
			//container.RegisterType<Web.Controllers.SaleController>(new InjectionConstructor(new ResolvedParameter<BS.Contracts.ISaleService>()));
			//container.RegisterType<Web.Controllers.DashboardController>(new InjectionConstructor(new ResolvedParameter<BS.Contracts.IDashboardService>()));
		}
	}
}