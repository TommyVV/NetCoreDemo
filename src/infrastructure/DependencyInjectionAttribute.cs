using Microsoft.Extensions.DependencyInjection;
using System;

namespace DependencyExtension
{
    public class DependencyInjectionAttribute:Attribute
    {
        public Type Service { get; set; }

        public ServiceLifetime LifeTime { get; set; }

        public DependencyInjectionAttribute(Type service, ServiceLifetime lifeTime = ServiceLifetime.Singleton)
        {
            Service = service;
            LifeTime = lifeTime;
        }
    }

   
}
