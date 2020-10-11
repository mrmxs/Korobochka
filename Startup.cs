using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Korobochka.Repositories;
using Korobochka.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
// using Korobochka.GoogleSheets;

namespace Korobochka
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GoogleSheets.Settings>(Configuration.GetSection(
                string.Join('.', typeof(GoogleSheets.Settings).FullName.Split('.').TakeLast(2))
            ));
            services.AddSingleton<GoogleSheets.ISettings>(sp =>
                sp.GetRequiredService<IOptions<GoogleSheets.Settings>>().Value);

            services.AddSingleton<GoogleSheets.Client>();


            services
                .AddSingleton<PlacesRepository>();

            services
                .AddSingleton<PlacesCRUDService>();

            services.AddControllers(opt =>  // or AddMvc()
            {
                // remove formatter that turns nulls into 204 - No Content responses
                opt.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Korobochka", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Korobochka v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
