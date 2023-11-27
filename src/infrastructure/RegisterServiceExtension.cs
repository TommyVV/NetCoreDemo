using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DependencyExtension
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
            var assembiles = AssemblyLoader.LoadAll();
            
            var services = assembiles
                .SelectMany(assembly => AssemblyLoader.GetTypes(assembly, ContainsDependencyInjectionAttribute));
            
            var serviceGroup = services
                .SelectMany(service => service.GetCustomAttributes<DependencyInjectionAttribute>().Select(attr => new { Attr = attr, Impl = service }))
                .GroupBy(define => define.Attr.Service);

            return serviceGroup.Select(service => service.FirstOrDefault()).Select(instance => new ServiceDescriptor(instance.Attr.Service, instance.Impl, instance.Attr.LifeTime)).ToList();
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
