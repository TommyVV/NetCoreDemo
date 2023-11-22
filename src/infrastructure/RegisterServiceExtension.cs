using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Utility
{
    public static class RegisterServiceExtension
    {
        public static void AddService(this IServiceCollection service)
        {
            var serviceDescriptor = ScanServiceDescriptionMetas();

            service.TryAdd(serviceDescriptor);

        }

        /// <summary>
        /// Scans the service description metas.
        /// </summary>
        /// <returns>The service description metas.</returns>
        private static IEnumerable<ServiceDescriptor> ScanServiceDescriptionMetas()
        {
            var serviceDescriptions = new List<ServiceDescriptor>();

            IEnumerable<Assembly> assembiles = AssemblyLoader.LoadAll();
            IEnumerable<Type> services = assembiles
                .SelectMany(assembly => AssemblyTypeLoader.GetTypes(assembly, ContainsDependencyInjectionAttribute));
            var serviceGroup = services
                .SelectMany(service => service.GetCustomAttributes<DependencyInjectionAttribute>().Select(attr => new { Attr = attr, Impl = service }))
                .GroupBy(define => define.Attr.Service);

            foreach (var service in serviceGroup)
            {
                var instance = service.FirstOrDefault();
                serviceDescriptions.Add(new ServiceDescriptor(instance.Attr.Service, instance.Impl, instance.Attr.LifeTime));
            }

            return serviceDescriptions;
        }

        private static bool ContainsDependencyInjectionAttribute(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            return !type.GetCustomAttributes<DependencyInjectionAttribute>().IsNullOrEmpty();
        }
    }
}
