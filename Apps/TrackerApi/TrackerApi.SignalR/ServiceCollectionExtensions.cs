using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerApi.SignalR
{
    public static class ServiceCollectionExtensions
    {
        //public static void AddInfrastructureSignalRClient(this IServiceCollection services)
        //{
        //    services.Scan(scan => scan.FromCallingAssembly()
        //                              .AddClasses(classes => classes.Where(SkipTheseTypes))
        //                              .UsingRegistrationStrategy(RegistrationStrategy.Skip)
        //                              .AsImplementedInterfaces()
        //                              .WithTransientLifetime());
        //}

        //static bool SkipTheseTypes(Type x)
        //{
        //    var skipTheseTypes = typeof(Exception).IsAssignableFrom(x);
        //    return !skipTheseTypes;
        //}

        public static IServiceCollection AddTrackerApiSignalR(this IServiceCollection services)
        {
            services.AddSignalR();
           
            return services;
        }

        public static WebApplication
    }
}
