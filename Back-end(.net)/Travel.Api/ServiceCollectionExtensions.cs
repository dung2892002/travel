using System.Reflection;
using Travel.Core.Interfaces.IRepositories;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterServicesAndRepositories(this IServiceCollection services, params Assembly[] assemblies)
        {
            var serviceTypes = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IService)) && t.IsClass);

            foreach (var serviceType in serviceTypes)
            {
                var serviceInterface = serviceType.GetInterfaces().FirstOrDefault(i => i != typeof(IService));
                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, serviceType);
                }
            }

            var repositoryTypes = assemblies.SelectMany(a => a.GetTypes())
                .Where(t => t.GetInterfaces().Contains(typeof(IRepository)) && t.IsClass);

            foreach (var repositoryType in repositoryTypes)
            {
                var repositoryInterface = repositoryType.GetInterfaces().FirstOrDefault(i => i != typeof(IRepository));
                if (repositoryInterface != null)
                {
                    services.AddScoped(repositoryInterface, repositoryType);
                }
            }
        }
    }
}
