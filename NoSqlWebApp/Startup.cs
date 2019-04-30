using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NoSqlWebApp.Providers;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace NoSqlWebApp
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			services.AddTransient<IRedisDatabaseProvider, RedisDatabaseProvider>();

			services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new Info
				{
					Title = "NoSQL Web App",
					Version = "v1",
					Description = "NoSQL Web App"
				});

				var appPath = PlatformServices.Default.Application.ApplicationBasePath;
				var appName = PlatformServices.Default.Application.ApplicationName;
				var filePath = Path.Combine(appPath, $"{appName}.xml");

				options.IncludeXmlComments(filePath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();

			app.UseSwagger(setupAction: null);

			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "NoSQL Web App");
			});
		}
	}
}
