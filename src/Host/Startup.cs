﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DependencyExtension;
using Microsoft.Extensions.Hosting;

namespace HostApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //添加MVC
            services.AddMvcCore();
            //依赖注入
            services.AddService();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
          
            //使用MVC
            //app.UseMvc();
            app.UseRouting().UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }

       
    }
}
