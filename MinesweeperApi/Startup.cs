using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MinesweeperApi.Service;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;

namespace Minesweeper
{
   public class Startup
   {
      public Startup(IConfiguration configuration) => Configuration = configuration;

      public IConfiguration Configuration { get; }

      public void ConfigureServices(IServiceCollection services)
      {
         services
            .AddCors(options =>
            {
               options.AddDefaultPolicy(
                  builder =>
                  {
                     builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                  }
               );
            })
            .AddControllers()
            .AddNewtonsoftJson(
               options => { options.SerializerSettings.Converters.Add(new StringEnumConverter()); }
            );

         services.AddHealthChecks();
         services.AddSingleton<GameService>();
         services.AddSwaggerGen(c =>
         {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Minesweeper API", Version = "v1" });
         }).AddSwaggerGenNewtonsoftSupport();
      }

      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minesweeper API v1");           
         });

         app.UseSwagger();
         app.UseHttpsRedirection();
         app.UseDefaultFiles();
         app.UseStaticFiles();
         app.UseRouting();
         app.UseCors();
         app.UseAuthorization();
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
         });
      }
   }
}
