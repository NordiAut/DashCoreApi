using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CoreApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CoreApi
{
    public class Startup
    {
        private string _connectionString = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>{
                opt.AddPolicy("CorsPolicy",
                c => c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            //services.AddMvc();
            services.AddControllers();   

            _connectionString = Configuration["secretConnectionString"];
            
            services.AddEntityFrameworkNpgsql().AddDbContext<ApiContext>(
                opt => opt.UseNpgsql(_connectionString));

            services.AddTransient<DataSeed>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }            

            seed.SeedData(20, 1000);

            //app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            // app.UseMvc(routes =>
            //     routes.MapRoute("default", "api/{controller}/{action}/{id?}")
            // );            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapControllerRoute("default", "{controller=Home}/{action}/{id?}");
                //endpoints.MapControllerRoute("default", "api/{controller}/{action}/{id?}");
                //Todo Check Why order is not shown
                

            });

        }
    }
}
