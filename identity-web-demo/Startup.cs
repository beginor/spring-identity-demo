using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NHibernate.AspNetCore.Identity;
using NHibernate.Cfg;
using NHibernate.Mapping.Attributes;
using NHibernate.NetCore;
using IdentityWebDemo.Entities;
using IdentityWebDemo.Authorization;

namespace IdentityWebDemo {

    public class Startup {

        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            config = configuration;
            this.env = env;
        }

        private IConfiguration config { get; }
        private IWebHostEnvironment env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // add nhibernate
            var cfg = new Configuration();
            var configFile = Path.Combine(
                Directory.GetCurrentDirectory(),
                "config",
                "hibernate.config"
            );
            cfg.Configure(configFile);
            var isDev = env.IsDevelopment().ToString();
            cfg.SetProperty(NHibernate.Cfg.Environment.ShowSql, isDev);
            cfg.SetProperty(NHibernate.Cfg.Environment.FormatSql, isDev);
            cfg.AddIdentityMappings();
            cfg.AddAttributeMappingAssembly(typeof(Startup).Assembly);
            services.AddHibernate(cfg);

            // add identity
            services.AddIdentity<AppUser, AppRole>()
                .AddDefaultTokenProviders()
                .AddHibernateStores();
            
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();

            services.AddControllers();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityWebDemo", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityWebDemo v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }

    public static class ConfigurationExtensions {

        public static Configuration AddAttributeMappingAssembly(this Configuration cfg, Assembly assembly) {
            HbmSerializer.Default.Validate = true;
            var stream = HbmSerializer.Default.Serialize(assembly);
            using var reader = new StreamReader(stream);
            var xml = reader.ReadToEnd();
            cfg.AddXml(xml);
            return cfg;
        }

    }
}
