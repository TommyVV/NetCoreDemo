using System;

namespace Utility
{
    public class DependencyInjectionAttribute:Attribute
    {
        public Type Service { get; set; }

        public LifeTime LifeTime { get; set; }

        public DependencyInjectionAttribute(Type service, LifeTime lifeTime=LifeTime.Singleton)
        {
            Service = service;
            LifeTime = lifeTime;
        }
    }

    public enum LifeTime
    {
        Singleton,
        Scoped,
        Transient
    }
}
