using System;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Utility;

namespace HostApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC
            services.AddMvcCore().AddJsonFormatters();
            //依赖注入
            AddService(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //使用MVC
            app.UseMvc();

        }

        public void AddService(IServiceCollection service)
        {
            var assembiles = AssemblyLoader.LoadAll();

            var services = assembiles
                .SelectMany(assembly => AssemblyTypeLoader.GetTypes(assembly, ContainsDependencyInjectionAttribute));

            var serviceGroup = services
                .SelectMany(s => s.GetCustomAttributes<DependencyInjectionAttribute>().Select(attr => new { Attr = attr, Impl = s })).GroupBy(define => define.Attr.Service);

            foreach (var s in serviceGroup)
            {
                var instance = s.FirstOrDefault();

                if (instance == null) continue;

                //注入
                service.AddSingleton(instance.Attr.Service, instance.Impl);
            }
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
