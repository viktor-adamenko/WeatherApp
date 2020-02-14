using System.Web.Configuration;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;
using WeatherApp.BusinessLogic;

namespace WeatherApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            var apiKey = WebConfigurationManager.AppSettings["ApiKey"];
            container.RegisterType<WeatherApiProcessor>(new InjectionConstructor(apiKey));
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}